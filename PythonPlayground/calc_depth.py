import torch
import numpy as np
from midas.model_loader import load_model
from common import (
    load_image, display_image
)
first_execution = True
def process(image, model, model_type, device, target_size, optimize, use_camera):
    """
    Run the inference and interpolate.

    Args:
        device (torch.device): the torch device used
        model: the model used for inference
        model_type: the type of the model
        image: the image fed into the neural network
        input_size: the size (width, height) of the neural network input (for OpenVINO)
        target_size: the size (width, height) the neural network output is interpolated to
        optimize: optimize the model to half-floats on CUDA?
        use_camera: is the camera used?

    Returns:
        the prediction
    """
    global first_execution
    sample = torch.from_numpy(image).to(device).unsqueeze(0)

    if optimize and device == torch.device("cuda"):
        if first_execution:
            print("  Optimization to half-floats activated. Use with caution, because models like Swin require\n"
                    "  float precision to work properly and may yield non-finite depth values to some extent for\n"
                    "  half-floats.")
        sample = sample.to(memory_format=torch.channels_last)
        sample = sample.half()

    if first_execution or not use_camera:
        height, width = sample.shape[2:]
        print(f"Input resized to {width}x{height} before entering the encoder")
        first_execution = False

    prediction = model.forward(sample)
    prediction = (
        torch.nn.functional.interpolate(
            prediction.unsqueeze(1),
            size=target_size[::-1],
            mode="bicubic",
            align_corners=False,
        )
        .squeeze()
        .cpu()
        .numpy()
    )

    return prediction

def compute_depth_relative(image):
    device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
    print("Device: %s" % device)
    model_type = "dpt_swin2_large_384"
    model_path = f"Models/{model_type}.pt"
    model, transform, net_w, net_h = load_model(
        device, 
        model_path, 
        model_type, 
        optimize=True, 
        height=None, 
        square=False
    )

    original_image_rgb = image/255.0
    image = transform({"image": original_image_rgb})["image"]

    with torch.no_grad():
        prediction = process(image,
                             model, 
                             model_type, 
                             device,  
                             original_image_rgb.shape[1::-1],
                             True, 
                             False
                        )
    display_image(prediction)

if __name__ == "__main__":
    image = load_image("33.jpg")
    compute_depth_relative(image)
