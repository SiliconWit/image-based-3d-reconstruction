from concurrent import futures
from common import( compute_depth )
import v3r_pb2_grpc, v3r_pb2
import grpc
import numpy as np
from PIL import Image
import io
import typing
from common import compute_depth
from zoedepth.models.builder import build_model
from zoedepth.utils.config import get_config

depth_maps = list()

class V3RrelayServicer(v3r_pb2_grpc.V3RrelayServicer):
    def __init__(self) -> None:
        super().__init__()
        self.index = 0
        self.gt_deltas = list()
        self.positions = list()
        self.depth_maps = list()

        if 'zoe' not in globals():
            conf = get_config("zoedepth", "infer")
            self.zoe = build_model(conf)

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

            # print(f"Received image data: {image}")
        # Return DoneResponse indicating completion
        
        self.determine_factor()
        return v3r_pb2.DoneResponse(done=True)
    
    def determine_factor(self):
        _viewpoints = len(self.positions)
        for i in range(_viewpoints-1):
            gt_delta = np.linalg.norm( self.positions[i+1] - self.positions[i] )
            dep_delta = np.abs( self.depth_maps[i+1] - self.depth_maps[i] ) 
            print(f"Factor: { np.mean(dep_delta)/gt_delta }")
            

    


def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    v3r_pb2_grpc.add_V3RrelayServicer_to_server(V3RrelayServicer(), server)
    server.add_insecure_port('[::]:50051')  # Define server port
    server.start()
    server.wait_for_termination()

if __name__ == '__main__':
    serve()