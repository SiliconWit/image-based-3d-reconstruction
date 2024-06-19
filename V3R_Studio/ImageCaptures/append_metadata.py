import sys
from io import BytesIO
import json
import os
import time
import piexif
from PIL import Image
import numpy as np


def appendMetadata(image_file, metadata):
    zeroth_ifd = {
        piexif.ImageIFD.Make: metadata["Make"],
        piexif.ImageIFD.Model: metadata["Model"],
        piexif.ImageIFD.ImageWidth: metadata["ImageWidth"],
        piexif.ImageIFD.ImageLength: metadata["ImageHeight"]
    }

    exif_ifd = {
        piexif.ExifIFD.FocalLength: (int(metadata["FocalLength"] * 100), 100),
        piexif.ExifIFD.FocalLengthIn35mmFilm: metadata["FocalLengthIn35mmFilm"],
        piexif.ExifIFD.FNumber: (int(metadata["FNumber"] * 100), 100),
        piexif.ExifIFD.ExposureTime: (1, metadata["ExposureTime"]),
        piexif.ExifIFD.ISOSpeedRatings: metadata["ISOSpeedRatings"],
        piexif.ExifIFD.DateTimeOriginal: metadata["DateTimeOriginal"],
        piexif.ExifIFD.DateTimeDigitized: metadata["DateTimeDigitized"],
        piexif.ExifIFD.Flash: metadata["Flash"],
        piexif.ExifIFD.PixelXDimension: metadata["PixelXDimension"],
        piexif.ExifIFD.PixelYDimension: metadata["PixelYDimension"],
    }

    thumbnail_img = image_file.copy()  # Make a copy of the image
    thumbnail_img.thumbnail((50, 50), 3)

    # Save the thumbnail image to a BytesIO object
    thumbnail_io = BytesIO()
    thumbnail_img.save(thumbnail_io, format='JPEG')
    thumbnail_data = thumbnail_io.getvalue()

    exif_dict = {
        "0th": zeroth_ifd,
        "Exif": exif_ifd,
        "thumbnail": thumbnail_data
    }
    return piexif.dump(exif_dict)

def get_creation_date(path):
    timestamp = os.path.getctime(path)
    return time.strftime('%Y:%m:%d %H:%M:%S', time.localtime(timestamp))

# Creates masks for a given image
def process_image(filename, img_bytes, output_folder):
    # Remove the background
    output_bytes = remove(img_bytes)
    img_no_bg = Image.open(BytesIO(output_bytes))
    
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
    file_name = filename + ".png" # to save it as .png
    output_path = os.path.join(output_folder, file_name)
    black_white_img.save(output_path, "PNG")

def main(photo_set_dir):
    BASE_DIR = os.path.dirname(os.path.abspath(__file__))
    folder_path = f"{BASE_DIR}/{photo_set_dir}/"
    output_dir = folder_path + "photos/"

    if not os.path.exists( output_dir ):
        os.mkdir( output_dir )


    # Open the JSON file and load the data into a dictionary
    with open(folder_path + 'metadata.json', 'r') as file:
        metadata = json.load(file)

    # Iterate through all the images in the folder
    for filename in os.listdir(folder_path):
        if filename.endswith(".jpg") or filename.endswith(".jpeg"):
            image_path = os.path.join(folder_path, filename)

            with open(image_path, "rb") as img_file:
                input_bytes = img_file.read()
            # img = Image.open(image_path)
            
            # process_image(filename, input_bytes, output_dir)

            # Convert bytes to a Pillow Image object
            img = Image.open(BytesIO(input_bytes))

            creation_date = get_creation_date(image_path)
            metadata["DateTimeOriginal"] = creation_date
            metadata["DateTimeDigitized"] = creation_date
            ExifData = appendMetadata(img, metadata)
            img.save(folder_path + "photos/" + filename, exif=ExifData)
            # os.remove(image_path) # Delete the photos without MetaData

if __name__ == "__main__":
    # Get the photo_set_dir from command line argument
    photo_set_dir_arg = sys.argv[1]
    main(photo_set_dir_arg)
