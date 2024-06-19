import grpc
from concurrent import futures
import io
import qrcode
import qrcreate_pb2
import qrcreate_pb2_grpc
import numpy as np
from PIL import Image
from qreader import QReader

class QRManageServicer(qrcreate_pb2_grpc.QRManageServicer):
    def Image_from_Bytes(self, byteArr: bytearray):
        numpy_array = np.frombuffer(byteArr, dtype=np.uint8)
        img = np.asarray( Image.open(io.BytesIO(numpy_array)) )
        return img
    
    def GenerateQRCode(self, request, context):
        # Get the data from the gRPC request
        data = request.qrtext

        # Create instance of QRCode
        qr = qrcode.QRCode(
            version=2,
            error_correction=qrcode.constants.ERROR_CORRECT_Q,
            box_size=10,
            border=2,
        )

        # Add data to the QRCode instance
        qr.add_data(data)
        qr.make(fit=True)

        # Create an image from the QRCode instance
        img = qr.make_image(fill_color="black", back_color="white")

        # Save the image to a bytes buffer
        image_buffer = io.BytesIO()
        img.save(image_buffer)
        image_buffer.seek(0)

        # Read the bytes from the buffer and create a bytearray
        byte_array = bytes(bytearray(image_buffer.read()))

        # Return the bytearray as a response
        # return qrcreate_pb2.QRCodeResponse(qr_code=byte_array)
        return qrcreate_pb2.ImgByteArr(byteArr=byte_array)
        
    
    def DecodeQRCode(self, request, context):
        img = self.Image_from_Bytes(request.byteArr)
        qreader = QReader()
        res = qreader.detect_and_decode(image=img,return_detections = False )
        print(res)
        qrTexts = qrcreate_pb2.QRTexts()
        for text in res:
            qr = qrcreate_pb2.QRText(qrtext=text)
            qrTexts.QRTexts.append(qr)
        
        return qrTexts




def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    qrcreate_pb2_grpc.add_QRManageServicer_to_server(QRManageServicer(), server)
    server.add_insecure_port('[::]:50051')
    server.start()
    print("LISTENING")
    server.wait_for_termination()

if __name__ == '__main__':
    serve()
