#tutorial #First-principles #computervision 

In the video [Overview | SIFT Detector](https://www.youtube.com/@firstprinciplesofcomputerv3258)  the presenter talks of thresholding, what is that?

## SIFT
Scale invariant Feature Transform.
What is an interest point? Edges and corners are insufficient, blobs? maybe?
Removing sources of Variation  makes it easier to match.
What makes a feature of interest:
	![[Pasted image 20231029002333.png]]

Edges cannot be localized as they slide along boundaries of objects-not unique enough then.
Blobs make good interest points as they have a fixed position and definite size.
FOr a blob to be useful:
	- locate the blob
	- It must have a size, determine size
	- Determine its principle orientation
	- Formulate a description or signature that is independent of size or orientation.

For a 1D image, the second derivate of Gaussian is convolved with the Signal. The $\sigma$  value is to be changed iteratively in order to find the peak. Remember the zero-crossings detected edges. The peaks locations(in the x) are locations of the blobs. The $\sigma$ widths are proportional to the Blob widths too. The $\sigma$ which gives the extrema correspond to the scale of the blob-the characteristic scale.
![[Pasted image 20231029004232.png]]

For a 2D blob detect, a Normalised Laplacian of Gaussian(NLoG) is used as the 2D equivalent for blob detection.
A higher characteristic scale corresponds to a lower image resolution.
![[Pasted image 20231029191230.png]]
### The Sift Detector
It is an approximation of the NLoG operator.
![[Pasted image 20231029191448.png]]![[Pasted image 20231029191519.png]]

The Difference of Gaussian(DoG) is pretty much a scale of the NLoG.![[Pasted image 20231029191633.png]]![[Pasted image 20231029191748.png]]
Create a stack of images with different gaussian scales.
Compute the difference between successive images-DoG.
For each image, find interest points by moving a sliding window(grid) across the image.
With multiple interest points for each scale, you can use some thresholding by filtering out those with weak extrema.
For images with the same feature but different scales ($\sigma$), once a feature is detected to match them, the ratio of the $\sigma$s can be found(normalised) and used to adjust one of the images for scale invariance.
![[Pasted image 20231029193104.png]]
To take care of orientation, apply a grid on the area the blob occupies, then compute the gradient using the gradient operator to obtain the orientations for each grid. Create  a histogram for each direction. The peak is bar will show the Principle orientation.

### Sift Descriptor
Sift descriptors make use of the hostograms for orientaion only that this time, the blobs orientations are grouped into quadrants. ![[Pasted image 20231029194346.png]]

The concatenated histograms will serve as the SIFT descriptor.
Therefore matching will involve comparing these descriptors: They can be compared through:
	1. L2 Distance between to hostograms![[Pasted image 20231029194537.png]]
	2. Normalized Correlation![[Pasted image 20231029194601.png]]
	3. Intersection - mean for means of eah pair of bins.![[Pasted image 20231029194647.png]]
SIFT is nor robust enough for 3D object detection. It is reliable for small changes in the viewpoint.