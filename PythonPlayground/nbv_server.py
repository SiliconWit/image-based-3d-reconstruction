#! /opt/miniconda3/envs/zoe/bin/python
from concurrent import futures
from common import( compute_depth )
import v3r_pb2_grpc, v3r_pb2
import grpc
import numpy as np
from PIL import Image
import io
import typing
from zoedepth.models.builder import build_model
from zoedepth.utils.config import get_config
from NBV_Poser import NBVPoser
import logging

logging.basicConfig(level=logging.INFO)

depth_maps = list()

class V3RrelayServicer(v3r_pb2_grpc.V3RrelayServicer):
    def __init__(self):
        self.index = 0
        self.gt_deltas = list()
        self.positions = list()
        self.depth_maps = list()
        self.zoe = None
        self.midas = True
        if not self.midas:
            if 'zoe' not in globals():
                conf = get_config("zoedepth", "infer")
                self.zoe = build_model(conf)
        

    def Image_from_Bytes(self, byteArr: bytearray):
        numpy_array = np.frombuffer(byteArr, dtype=np.uint8)
        img = np.asarray( Image.open(io.BytesIO(numpy_array)) )
        return img

    def Calibration(self, request_iterator, context):
        for calibration_input in request_iterator:
            _vec3 = calibration_input.position
            position = np.array([_vec3.x, _vec3.y, _vec3.z])
            self.positions.append(position)
            
            # Perform processing with received data (position and image)
            print(f"Received position: ({position})")
            
            image = calibration_input.image.byteArr  # Assuming image data is bytes
            numpy_array = np.frombuffer(image, dtype=np.uint8)
            img = np.asarray( Image.open(io.BytesIO(numpy_array)) )
            self.depth_maps.append( compute_depth(img, self.zoe) )
        
        self.determine_factor()
        return v3r_pb2.DoneResponse(done=True)
    
    def FindDepths(self, request, context):
        image = request.byteArr
        numpy_array = np.frombuffer(image, dtype=np.uint8)
        img = np.asarray( Image.open(io.BytesIO(numpy_array)) )

        dep = compute_depth(img, self.zoe, "cuda")

        depths = v3r_pb2.DepthData(
            min=np.min(dep),
            max=np.max(dep),
            median=np.median(dep),
            mean=np.mean(dep),
            center=dep[int(dep.shape[0]/2), int(dep.shape[1]/2)]
        )

        return depths
    
    def DerivePoses(self, request:v3r_pb2.NBVInput, context):
        _image = self.Image_from_Bytes(request.image.byteArr)
        _pose = request.refPose
        

        nbv_poser = NBVPoser( _image, _pose, self.zoe )
        if(nbv_poser == None):
            logging.info("NBV Poser Initialised")

        nbv_poser.CalcIntrinsics()
        nbv_poser.RegionsofInterest()
        nbv_poser.Structuring()
        Poses = nbv_poser.retransform_poses()
        
        logging.info(f"{len(Poses.Poses)} poses determined.")
        return Poses

    
    def determine_factor(self):
        _viewpoints = len(self.positions)
        for i in range(_viewpoints-1):
            gt_delta = np.linalg.norm( self.positions[i+1] - self.positions[i] )
            dep_delta = np.abs( self.depth_maps[i+1] - self.depth_maps[i] ) 
            print(f"Factor: { np.mean(dep_delta)/gt_delta }")
    
    def CalibrateDepth(self, request:v3r_pb2.CalibrationInput, context):
        image = self.Image_from_Bytes(request.image.byteArr)


def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    v3r_pb2_grpc.add_V3RrelayServicer_to_server(V3RrelayServicer(), server)
    host = '[::]:49990'
    server.add_insecure_port(host)  # Define server port
    # server.add_insecure_port('0.0.0.0:49990')  # Define server port
    server.start()
    logging.info(f"gRPC Server running: {host}")
    server.wait_for_termination()

if __name__ == '__main__':
    serve()