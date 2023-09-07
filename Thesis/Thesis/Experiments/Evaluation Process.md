#cloudcompare 
1. The model cloud(mesh) and the data cloud are both loaded into Cloud Compare.
2. The Data Cloud is segmented to retain the volume of interest.
3. The segmented cloud can then be transformed in an attempt to roughly align the clouds together.
4. On Selection of the model and data clouds, perform fine registration of the Point Clouds which will create a transformation matrix and apply it to the data cloud in order to align it to the model cloud.
	- Parameters for Fine Registration with ICP include: Either RMS Difference Threshold or Number of Iterations and Scale Adjust.
5. 