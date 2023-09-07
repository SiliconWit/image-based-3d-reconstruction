- Meshes can be considered as clouds in Cloud Compare. This possible by considering the vertices or sampling points on the Mesh Surface.
- During alignment, the reference 3D model is referred to as the **Model Cloud** while the 3D model being evaluated will be called the **Data Cloud**.
- Ways in which alignment can be done is described [here](https://www.cloudcompare.org/doc/wiki/index.php/Alignment_and_Registration).
- The last model is the most automated means where the 2 clouds have to be aligned roughly using the translation/rotation tool then applying the "Fine Registration" tool. The result is a transformation matrix which can be copied and applied to an object from the **Edit Menu**.
### Cloud Registration
 ![[Pasted image 20230813180648.png]]
 When aligning 2 models, criteria for alignment can be based on the number of iterations or RMS-difference as well as Final Overlap

## Results

- The mean tells you about the overall bias in the differences.
- The standard deviation informs you about the variability in the differences.
- The histogram, characterized by the classes count, provides a visual representation of how discrepancies are distributed.
## Classes Count
From a quantitative standpoint, the classes count in a histogram primarily affects the granularity of the representation of your data distribution. The significance of the classes count in evaluation depends on the nature of your analysis and what you are trying to discern from the data.

Here's why classes count might be significant:

1. **Resolution of Features**: Sometimes, the discrepancies between datasets might have specific patterns or features that are only discernible at certain levels of granularity. For instance, a peak or trough in the error distribution might be evident with a higher classes count but gets averaged out in a coarser histogram.
    
2. **Statistical Significance**: If you're looking to identify statistically significant patterns or outliers, having an appropriate classes count can help. Too few classes might miss subtle patterns, while too many might make the histogram noisy and harder to interpret from a broader perspective.
    
3. **Understanding Distribution Types**: For advanced analyses, the shape and features of a histogram can provide hints about the underlying type of distribution. For example, a bimodal distribution (with two peaks) might indicate two distinct processes or sources of error in the data. Such features can be more evident with an appropriate granularity.
    
4. **Comparison with Other Datasets**: If you're comparing multiple datasets or results from various reconstruction methods, having a consistent classes count can be essential for a fair comparison.
    

However, it's crucial to approach the classes count with caution:

- **Overinterpretation**: A very high classes count, while providing finer detail, might lead to overinterpretation of random fluctuations as significant patterns.
    
- **Statistical Robustness**: Fewer data points in each class (due to a high classes count) might lead to individual classes being less statistically robust.
    

Ultimately, while the classes count can offer insights and nuances in the data distribution, its significance should be weighed alongside other metrics and qualitative evaluations. For most evaluations, it's beneficial to view histograms at multiple class counts to get both a detailed view and a broader overview of the data distribution.