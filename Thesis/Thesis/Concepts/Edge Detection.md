#theory #tutorial #algorithm #computervision #First-principles

 Source: https://www.youtube.com/playlist?list=PL2zRqk16wsdqXEMpHrc4Qnb5rA1Cylrhx
Edge- location/region with rapid change in image intensity.
- There are edge detectors that use the first derivative of the image, using gradients(convolutions)
- Edge detectors using the second derivative of the image use Laplace methods
- The Canny Edge Detector is popular than the other 2
- Casuses for images:
	- Depth discontinuity
	- Surface normals causing differences
	- Surface reflectance due to material differences
	![[Pasted image 20231016145417.png]]
	An edge detector needs the model of an edge and that can be based on any of the types above.
## Edge Detector
An edge operator that produces the position, magnitude and orientation of the edge.
Performance metrics of an edge detector are detection rate(high), Localistions(good) and Noise Sensitivity(Low).
For a 1D image, the location of the peak aswers the localistion question while the height answers the strength question.
![[Pasted image 20231016154554.png]]
### The Gradient Operator 
For a 2D image, the partial derivation of the continuous function(image) represents tha amount of change along each dimension.![[Pasted image 20231016154952.png]]
The Del l operator produces 2 numbers.![[Pasted image 20231016155023.png]]From the 2 numbers produced one can get the strength
![[Pasted image 20231016155123.png]]
The above notations apply to continous domains.
But for a discrete domain(image), finite difference are used.

At least 4 pixels are used, 2 for the X and 2 for the Y.
Let the physical difference between 2 adjacent pixels be represented by _eps_
The derivative in the X direction is the sum of the difference between 2 adjacent pixels along X divided by 2 _eps_.
![[Pasted image 20231016155421.png]]
![[Pasted image 20231016155654.png]]
The above computation can also be achieved by convolution
![[Pasted image 20231016155812.png]]

### Gradient Operators
There are plenty of gradient operators as shown below:
![[Pasted image 20231016160652.png]]
To declare whether a pixel is an edge or not, edge thresholding is performed and this simply comparing the magnitude of a given pixel with a threshold value.

![[Pasted image 20231016163209.png]]
## Edge Detection using a 2nd Derivative
At the edges, there are zero-crossings.
![[Pasted image 20231016163459.png]]
The **Laplacian operator** is called the *Del Square* operator.![[Pasted image 20231016163635.png]]
Laplacian: Sum of Pure Second Derivatives. Notice that the Del Operator had no sum but just produced 2 numbers(a derivative with respect to X and Y).

- Edges in an image are zero-crossing in the Laplacian of the same image
- The Laplacian operators do not provide detection of edges.
#### Discrete Laplacian operator 
Finite difference approximations for 2nd derivatives are the difference of 2 differences.
They require at least 3 pixels for each direction a 3 by 3 grid.
![[Pasted image 20231016164445.png]]
![[Pasted image 20231016164822.png]]The first convolution mask cannot account for edges that appear at 45 degrees, note that the corner values are 0. The mask can be adjusted to the second image which takes care of the pitfalls of the first.
### Taking care of Noise
For a noisy signal(image), the Gaussian operator can be convolved with the image and the derivative found. 
Or better yet, the derivative of the Gaussian operator can be found and convolved with the signal
![[Pasted image 20231016170045.png]]![[Pasted image 20231016170216.png]]

### Comparison of the Gradient & Laplacian Operators 
- The gradient operator gives location, magnitude and direction of the edge.
- Detection is done using maxima thresholding-use of a threshold value while the Laplacian makes use of zero-crossing.
- The Gradient operator requires 2 convolutions while the Laplacian 1.

## Canny Edge Detector
Makes use of the best of both Gradient and Laplacian operator.
One of the most widely used.

1.  You first smoothen the image with a by convolving it iwhtwith a 2D Gaussian operator.
2. Compute the image gradient using the Sobel operator.
3. From the image gradients in both X and Y, compute the magnitude of each pixel.
4. Find gradient orientation at each pixel 
5. Compute a 1D Laplacian along the direction of the edge from the previous step.
6. Find  zero crossing in Laplacian to find the edge locations.
The _sigma_ of the Gaussian smoothing affects the edge result. Smaller _sigma_ gives finer resolutions. Therefore, the _sigma_ value is a parameter that controls the resolution of edges


## Corner Detection
- Corner detection can be detected also through derivatives.
- Detection of the corners considers the distribution of the edges detected.
- The distribution is then quantified by fitting an Ellipse centered at the origin.
- The semi major( $\lambda_1$ ) and minor( $\lambda_2$ ) axes of the fitted ellipse are used to perform the classifications.
- For a flat region, $\lambda_1$ and $\lambda_2$  are both small. For an edge $\lambda_1$ is larger and $\lambda_2$ is small. For a corner, both $\lambda_1$ and $\lambda_2$ are large.
- The Harris Corner Response function is an empirical formula used to detect corners.
- 