import piexif
from datetime import datetime
import json

# Load the JSON data from the file
with open('ExifSample.json') as f:
    json_metadata = json.load(f)

# Load the image
filename = 'img2.jpg'
exif_dict = piexif.load(filename)

for ifd in ("0th", "Exif", "GPS", "1st"):
    for tag in exif_dict[ifd]:
        print(piexif.TAGS[ifd][tag]["name"], exif_dict[ifd][tag])



