from PIL import Image
import numpy as np
from rembg import remove
import os
import io
import time

def process_image(img_path, output_folder):
    # Load the image
    with open(img_path, "rb") as img_file:
        input_bytes = img_file.read()
    
    # Remove the background
    output_bytes = remove(input_bytes)
    img_no_bg = Image.open(io.BytesIO(output_bytes))
    
    # Create a binary mask of pixels that are transparent (i.e., the areas previously occupied by the object)
    data = np.array(img_no_bg)
    red, green, blue, alpha = data.T
    
    # Areas with transparency are white
    white_areas = (alpha > 0)

    data[..., :3][white_areas.T] = [255, 255, 255] # Change these pixels to white
    data[..., :3][~white_areas.T] = [0, 0, 0] # The rest of the pixels are black
    data[..., 3][~white_areas.T] = 255  # Setting alpha to 255 (opaque) for background

    black_white_img = Image.fromarray(data)
    
    # Save the image to the output folder
    file_name = os.path.basename(img_path) + ".png" # to save it as .png
    output_path = os.path.join(output_folder, file_name)
    black_white_img.save(output_path, "PNG")

# Process all images in the folder
input_folder = 'test'
output_folder = 'result'

if not os.path.exists(output_folder):
    os.makedirs(output_folder)

start = time.time()
for img_file in os.listdir(input_folder):
    if img_file.lower().endswith(('png', 'jpg', 'jpeg')):
        process_image(os.path.join(input_folder, img_file), output_folder)
print(time.time() - start)

print("All images processed!")
