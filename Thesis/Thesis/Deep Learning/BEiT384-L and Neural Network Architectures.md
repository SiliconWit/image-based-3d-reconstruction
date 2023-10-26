#paper #depth-estimation #learning #deep-learning #neural-networks

> . All models use the BEiT384-L backbone from timm [44] that was pre-trained on ImageNet.

The quote above is lifted from the ZoeDepth paper. I wanted to understand what it means. 
In a nutshell, ZoeDepth is based of the **BEiT384-L** NN model. An NN model is usually based on an architecture. The architecture defines the overall structure, design, and organization of the neural network; it determines the capacity of the model to learn and represent patterns in the data.

BEiT is therefore a transformer type model designed for vision Transfer Learning of Vision tasks. BEiT384-L is **probably** a variant of the BEiT model.

### Nerual Network Architecture
A neural network architecture refers to the design or structure of a neural network, outlining how the individual components (neurons or nodes) are organized and interconnected.
Key components of an NN architecture:
	1. Layers - input, hidden and output
	2. Neurons/nodes - Basic unity of a network. Takes an input, computes and gives an output.
	3. Connections/Weights - connections between nodes have weights. During training, the weights are learnt and adjusted so as to give the desired output. The weight influences the impact of the preceding node to the next.
	4. Activation Functions - applied to the nodes. The activation function takes the weighted sum of its input, adds a bias, and then decides whether the neuron should be activated or not. Commonf activation functions include: sigmoid, tanh, ReLU, Leaky ReLU and Softmax
	5. Architecture Types 
	6. Loss Function - measures the difference between the predicted output and the actual target.
	7. Optimization Algorithm - An optimisation algorithm is used to update the weights based on the gradients of the loss function with respect to the weights.
### Architecture Types
1. **Convolutional Neural Networks (CNNs):** Well-suited for image-related tasks due to their ability to capture spatial hierarchies and patterns.
    
2. **Recurrent Neural Networks (RNNs):** Suitable for sequential data, making them useful for tasks like natural language processing and time series prediction.
    
3. **Transformers:** Originally designed for sequence-to-sequence tasks in natural language processing but later adapted to various domains due to their ability to capture long-range dependencies.
    
4. **Autoencoders:** Used for unsupervised learning and feature learning, where the model learns to encode and decode input data.
    
5. **Generative Adversarial Networks (GANs):** Designed for generating new data samples, often used in image generation tasks.
    
6. **Object Detection Architectures (e.g., YOLO, Faster R-CNN):** Specialized for detecting and localizing objects within images.