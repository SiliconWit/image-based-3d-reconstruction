#algorithm 

In the context of this study, blobs refer to features of interest that will be a basis for camera poses.
The feature detection phase of the methodoogy results in a list of blobs from the edge and depthj maps. Both of these lists need to be consolidated and harmonised to avoid redundancy to this end, there are 2 ways of going about this:
	1. **Clustering and Merging**: Apply clustering algorithms like DBSCAN or hierarchical clustering to group nearby blobs. Once clustered, you can either merge blobs in the same cluster into a single blob or represent them by a central or representative blob. This approach is effective when multiple detected blobs actually belong to the same object.
	2. **Spatial Consistency Check**: Analyze the spatial distribution of blobs. Blobs that are too close to each other and have similar characteristics can be merged or one of them can be discarded.
## Clustering and Merging
These methods aim to group similar blobs together and then either merge them into a single representative blob or choose the most representative one from each group. Here are some popular clustering and merging techniques that could be applied:

1. **DBSCAN (Density-Based Spatial Clustering of Applications with Noise)**:    
    - **Principle**: DBSCAN groups together points that are closely packed together, marking as outliers points that lie alone in low-density regions.
    - **Application**: This method is suitable for blob detection scenarios because it can find arbitrarily shaped clusters and is robust to outliers. You can use it to group nearby blobs and then treat each cluster as a single entity.
    - **Parameters**: It mainly requires two parameters: `eps`, the maximum distance between two samples for them to be considered in the same neighborhood, and `min_samples`, the number of samples in a neighborhood for a point to be considered as a core point.
2. **Hierarchical Clustering**:    
    - **Principle**: This method builds a hierarchy of clusters either by a divisive method (splitting) or agglomerative method (merging).
    - **Application**: In your case, an agglomerative approach can be used where each blob starts in its own cluster, and pairs of clusters are merged as one moves up the hierarchy.
    - **Dendrogram**: The results of hierarchical clustering can be visualized using a dendrogram, which can help in deciding the number of clusters by cutting the dendrogram at a suitable level.
3. **K-Means Clustering**:
    - **Principle**: K-means clustering partitions the blobs into K clusters in which each blob belongs to the cluster with the nearest mean.
    - **Application**: Though K-means is a popular method, it may not be as effective if the blobs have irregular shapes or vary greatly in size. However, it can be useful if you have an idea of the number of clusters (or representative blobs) you want to end up with.
4. **Mean Shift Clustering**:    
    - **Principle**: Mean shift clustering aims to discover blobs in a smooth density of samples. It is a centroid-based algorithm, which works by updating candidates for centroids to be the mean of the points within a given region.
    - **Application**: This method is useful for blob detection since it does not require specifying the number of clusters in advance and can handle varied shapes and sizes of clusters.
5. **Merging Strategy**:
    - **Post-Clustering**: After clustering, you can merge blobs in each cluster. Merging can be done by taking the mean position of all blobs in the cluster or by creating a new blob that encompasses all the blobs in the cluster.
    - **Representative Selection**: Alternatively, instead of merging, you can select a representative blob from each cluster (e.g., the largest blob, the most central blob, etc.).
6. **Custom Clustering Approaches**:    
    - **Adaptation**: Depending on the specific characteristics of your data, you might need to adapt or develop a custom clustering method. This could involve modifying existing algorithms to better suit the spatial properties of your blobs.

When applying these techniques, it's essential to consider the specific characteristics of your blobs (such as size, shape, and distribution) and the requirements of your downstream tasks. The choice of parameters (like `eps` in DBSCAN or the number of clusters in K-means) is crucial and may require experimentation or domain-specific knowledge. Additionally, visualizing the clustering results can be very helpful in assessing the effectiveness of your chosen method.

## Mean Shift Clustering
Mean Shift clustering is a non-parametric clustering technique that is particularly effective in handling cases where clusters are not of uniform shapes and sizes, as it does not assume any prior shape (like circles in K-means). It's based on the concept of kernel density estimation.
### Overview of Mean Shift Clustering
1. **Kernel Density Estimation**: Mean Shift uses kernel density estimation (KDE) to create a density surface. Essentially, it places a kernel (like a Gaussian) on each data point, and the sum of these kernels gives a density surface.    
2. **Shifting to the Mean**: The algorithm iteratively shifts each point towards the region of the highest density (peak of the kernel) by updating the location to the mean of the points within its neighborhood.    
3. **Convergence to Cluster Centers**: This shifting process continues until convergence, where data points no longer move significantly. The points that converge to the same peak are assigned to the same cluster.    
4. **Bandwidth Parameter**: The only crucial parameter is the bandwidth, which determines the size of the neighborhood to consider for each data point. A smaller bandwidth can lead to over-clustering (too many clusters), while a larger bandwidth might merge distinct clusters.