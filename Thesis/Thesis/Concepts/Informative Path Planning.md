![[Pasted image 20231120154843.png]]

![[Pasted image 20231120154451.png]]
This is like the planning objective function.

## Mapping 
- How will the environment be represented:
	- Dimensionality: 2D, 3D
	- Target Variable: continuous or discrete?
	- Prabablisitc(accounting for noise and uncertainity ), multiresolution(group areas to save on resources)
	- Computational and memory efficiencies.
	![[Pasted image 20231120155524.png]]
- Ocotomap - 3D occupancy structure. Discrete representation. Good for occupancy related problems.
- Gaussian Process - 
## Utility Function
- How to measure "usefulness"
	- Information theoretic measures, e.g. entropy
	- Adaptivity - do we need an adaptive algorithm?
	- Predictive Sensor model
- ![[Pasted image 20231120160203.png]]
- The algorithm above was adaptive and it was specified in the utility function that there was an interest in the "yellower" regions. The adaptive criteria can be defined in a variety of ways.
## Planning Algorithm
- How to plan informative paths?
	- Discrete vs Continuous
	- Lookahead: myopic(greedy, short-term, can be efficient but suffers from local minima) vs non-myopic(long-term)
	- Online replanning
	- Computation and memory efficiency
	- 
# Problem Variations
1. **Robotic Exploration** → Create a map of an unknown environment
2. **Data Collection Planning** → Collecting measures using a sensor.
3. **Inspection Planning** → Determine a cost-efficient path to inspect a known environment
4. **Persistent Monitoring** → Take a regular measurements in a changing environment
5. Active SLAM → Determine paths to simultaneously build a map and localised within it.
## Problem Setup
- Robot equipped with sensor
- Occupancy grid map representation
- Performance metrics: exploration time, map quality
- For the mapping, consider a bounded 3D space. Partition it into subvolumes of free and occupied space. The unioon of the free an occupied space is the total volume minus the residual volume. Residual volume is where the robot cannot see.![[Pasted image 20231120162037.png]]
## Occupancy Grid Map
- It is a way of representing structures in an environment in a probabilistic way.
- Divide environment into cells
- Each is cell is a random binary variable representing occupancy. 
- ![[Pasted image 20231120162310.png]]
- ![[Pasted image 20231120162403.png]] The product of probabilities
## Frontier-based Exploration
**Idea:** Move robots towards unknown regions
Frontier: Border between free and unknown space in an environment.
There is a definition for free space, obstacles and frontiers.![[Pasted image 20231120162953.png]]
-  A greedy technique is to go to the nearest frontier.