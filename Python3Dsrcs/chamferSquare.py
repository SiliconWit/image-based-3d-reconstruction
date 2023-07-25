from mathutils import Vector

'''
1. Define the Bounding Rectangle and chamfer ratio 
2. Define the chamfer ratio for each edge
3. Create Edges i.e. Vector Pairs
4. Create Points
''' 

class ChamferSquare:
    
    def __init__(self, dimensions, c_ratio, pivot = (0,0,0), normal = 2):
        self.dimensions = dimensions
        self.c_ratio = c_ratio
        self.pivot = Vector(pivot)
        self.normal = normal
        self.n_edges = list()
        self.generate_rectangle_data()

    def generate_rectangle_data(self):
        # center is a Vector representing the center of the rectangle
        # width and height are the dimensions of the rectangle

        width, height = self.dimensions

        # Create vectors for half the width and height, oriented correctly based on the plane
        if self.normal == 1:
            half_width = Vector((width / 2, 0, 0))
            half_height = Vector((0, height / 2, 0))
        elif self.normal == "xz":
            half_width = Vector((width / 2, 0, 0))
            half_height = Vector((0, 0, height / 2))
        elif self.normal == "yz":
            half_width = Vector((0, width / 2, 0))
            half_height = Vector((0, 0, height / 2))

        # Create vectors for half the width and height
        half_width = Vector((self.dimensions[0] / 2, 0, 0))
        half_height = Vector((0, self.dimensions[1] / 2, 0))

        # Calculate the vertices
        v1 = self.pivot - half_width - half_height
        v2 = self.pivot + half_width - half_height
        v3 = self.pivot + half_width + half_height
        v4 = self.pivot - half_width + half_height
        
        # Calculate the vertices
        self.vertices = [v1, v2, v3, v4]
        
        # Calculate the edges
        self.edges = [(v1, v2), (v2, v3), (v3, v4), (v4, v1)]
        
        # Printing the results
        print("Vertices:")
        for vertex in self.vertices:
            print(vertex)

        print("\nEdges:")
        for edge in self.edges:
            print(edge[0], "->", edge[1])


        # return self.vertices, self.edges
    def get_new_edges(self):
        n_edges = []
        
        for edge in self.edges:
            n_edges.append( self.scale_edge(edge, self.c_ratio) )
        
        for n_edge_index in range(len(n_edges)):
            self.n_edges.append(n_edges[n_edge_index])
            # if n_edge_index < 3:
            self.n_edges.append( (n_edges[n_edge_index][1], n_edges[(n_edge_index+1)%len(n_edges)][0] ) )
        print( len(self.n_edges) )
        print(self.n_edges)
        
    
    @classmethod
    def scale_edge(cls, edge, scale):
        # edge is a tuple of two Vectors representing the start and end points of the edge
        # scale is a float representing the desired scale factor

        # Calculate the midpoint of the edge
        midpoint = (edge[0] + edge[1]) / 2

        # Calculate vectors from the midpoint to the original points
        vector1 = edge[0] - midpoint
        vector2 = edge[1] - midpoint

        # Scale the vectors
        vector1 *= scale
        vector2 *= scale

        # Calculate the new points
        new_point1 = midpoint + vector1
        new_point2 = midpoint + vector2

        # Return the new edge
        return (new_point1, new_point2)


c = ChamferSquare((4,2), .8)
c.get_new_edges()





