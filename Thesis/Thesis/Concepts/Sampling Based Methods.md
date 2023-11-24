#tutorial #lecture #robotics #path-planning 
source:  [Sampling-based Methods; Lecture, Marija Popović](https://www.youtube.com/@MarijaPopovic)
- The basic idea behind Sampling-based methods is to sample the configuration space of the robot in an environment in order to obtain a roadmap capturing the connectivity free space. The edges of the road-map is collision free.
- The classic Path planning problem is on how to find the shortest geometric path for a robot between points A and B.
- Some of the techniques for path planning are listed below.
![[Pasted image 20231119072546.png]]
- A sampling approach assumes, there is a **known environment**(with knowledge of obstacles), a robot model and a **start and goa**l. 
- Search-based strategies have poor scalability.
- **How to Sample**? given that the robot config is known, (**random**)uniform random sampling is commonly used. Some other sampling methods are: Grid, Bridge test and medial axis![[Pasted image 20231119105750.png]]
- What about **Collision Checking?**
- Comparison of different Sampling Based algorithms involve the following criteria:
	- Completeness - ability to always find a solution if it exists. Rate of convergence.
	- Optimality - what is the cost? the shorter the paths the better.
	- Complexity - Time and Space. Complexity as a function of the input. Complexity per sample.
## Multi-Query vs Single-Query
**Multi-query** - creates multiple roadmaps. The environment and robot model is assumed to be static. The roadmap is then repeatedly queried for different start and goal configurations, since the roadmaps will already be present. Sampling is completely done first, then queries done afterwards. A search-based algorithm can then be used on the roadmap networks to connect the edges of the roadmap.
![[Pasted image 20231119113002.png]]
**Single-Query** ⇾ Tree-based. The tree is extended by sampling towards the goal. It is incremental, with the roadmap/tree getting built from the start configuration.
![[Pasted image 20231119113245.png]]
## Probablistic Road Map(PRM) - Multi-Query
- Is a multi-query type
- Consists of 2 steps
	  1. The learning phases-create samples, create netweorks
	  2. The start and goal are specified, additional edges are added using a search-based algorithm like A-star.
- Multiple queries can be made assuming there are no changes to the environment.![[Pasted image 20231119115004.png]]
- Sample randomly within the configuration space.
- Perform collision checks of the samples to eliminate interfering samples.
- Create paths by connecting samples. Apply subsample and check for collisions to conclude the learning phase.![[Pasted image 20231119115251.png]]
- The query phase will begin where the start and goal are specified and edges are added to connect the goals and start to the roadmap then a search-based algorithm is implemented.
- In the learning phase, when adding new edges, for a given point, there is a radius over which neighbour nodes are considered for edge detection.![[Pasted image 20231119131329.png]]
- Disadvantages of PRM are: Narrow Passages, and a lack of connectivity. It is also not suitable for dynamic paths.![[Pasted image 20231119131701.png]]
## Tree-Based planners-Single Query
- Grow  a tree rooted at the starting node.
- Randomly sample new nodes.
- Connect new nodes to trees if connecting path is collision-free.
- Terminate until tree is sufficiently close to goal.
- Greed in RRT is where there is a bias towards the goal when connecting new edges.
- There is a delta variable for the radius between 2 nodes that if the Euclidean distance of the goal to the closes not is greater than a node is inserted along the vector between the 2 otherwise, the last node directly connects to the goal.
- ![[Pasted image 20231119133457.png]] This is when you want to make sure your robot can actually work with a robotic system. This considers the kinematic constraints of the robot.
RRT and PRM do not necessarily give the shortest path and when it is done, it cannot improve the path.

## Asymptotic Optimality
- PRM, RRT are probabilistically complete but not asymptotically optimal.
- RRG(Rapidly-exploring Random Graphs) is similar to RRT but it may contain cycles.
- The radius is a function of other variables which include number of nodes in the graph. The radius will therefore be adaptable.
- 