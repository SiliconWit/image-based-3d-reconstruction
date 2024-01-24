import grpc
from v3r_pb2 import ImgByteArr, NBVInput, CamPose,Vec3, Quat
from v3r_pb2_grpc import V3RrelayStub

def read_image_from_disk(image_path):
    with open(image_path, 'rb') as file:
        return file.read()

def generate_client():
    # Create a gRPC channel and stub
    channel = grpc.insecure_channel('localhost:49990')  # Replace with your server address
    stub = V3RrelayStub(channel)
    return stub

def derive_poses_client(image_path):
    try:
        # Read the image from disk
        image_bytes = read_image_from_disk(image_path)

         # Create NBVInput message
        nbv_input = NBVInput(
            refPose=CamPose(
                location=Vec3(x=1,y=2,z=3), 
                orientation=Quat(w=1, x=.4, y=.2, z=.2)
                ),
            image=ImgByteArr(byteArr=image_bytes)
        )
        # Initialize gRPC client
        client = generate_client()

        # Make gRPC call to DerivePoses
        responses = client.DerivePoses(nbv_input)

        # Iterate through the stream of poses
        for response in responses.Poses:  # Assuming 'Poses' is the repeated field in 'CamPoses'
            print(f"Received pose: Location - {response.location}, Orientation - {response.orientation}")

    except grpc.RpcError as e:
        print(f" gRPC call failed: {e}")

if __name__ == "__main__":
    image_path = "testImages/33.jpg"  # Replace with your image file path
    derive_poses_client(image_path)
