import matplotlib.pyplot as plt
from PIL import Image

def load_image(img_name, img_dir="testImages/"):
    return Image.open(img_dir+img_name).convert("RGB")  # load the image

def show_image(image, label, colorbar=True):
    _, ax = plt.subplots(layout="constrained")
    imgPlot = ax.imshow(image)
    ax.set_title(label)
    if colorbar: plt.colorbar( imgPlot, ax=ax )

def compare_against_orig(image, _map, label=""):
    fig, axs = plt.subplots(1, 2, layout="constrained")
    
    for ax, im, title in zip(axs, [image, _map], ['Base Image', label]):
      ax.imshow(im)
      ax.axis("off")
      ax.set_title(title)
    
    plt.show()
