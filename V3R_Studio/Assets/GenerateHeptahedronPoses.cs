using System.Collections.Generic;
using UnityEngine;

public class HeptahedronCameraPoses : MonoBehaviour
{
    public Vector3 boundingBoxDimensions = new Vector3(2, 3, 1); // Bounding box dimensions
    public Vector3 centroid = Vector3.zero; // Centroid of the heptahedron
    public float scale = 1.0f; // Scaling factor
    public float arrowLength = 0.5f; // Length of the arrow for visualization
    private List<Vector3> vertices; // List of vertices (camera positions)
    private List<Quaternion> rotations; // List of rotations for each pose

    private void GenerateHeptahedronCameraPoses()
    {
        vertices = new List<Vector3>();
        rotations = new List<Quaternion>();

        // Calculate parameters
        float sideLength = Mathf.Max(boundingBoxDimensions.x, boundingBoxDimensions.y) * scale;
        float halfSide = sideLength / 2;
        float height = boundingBoxDimensions.z * scale; // Separation between faces

        // Positions of the quadrilateral (square base)
        float quadZ = centroid.z - height / 2; // Quadrilateral z-position
        vertices.Add(centroid + new Vector3(-halfSide, -halfSide, quadZ)); // Bottom-left
        vertices.Add(centroid + new Vector3(halfSide, -halfSide, quadZ));  // Bottom-right
        vertices.Add(centroid + new Vector3(halfSide, halfSide, quadZ));   // Top-right
        vertices.Add(centroid + new Vector3(-halfSide, halfSide, quadZ));  // Top-left

        // Position of the triangular face (top face)
        float triZ = centroid.z + height / 2; // Triangle z-position
        float triangleHeight = Mathf.Sqrt(3) * halfSide; // Height of the equilateral triangle
        Vector3 triangleCenter = centroid + new Vector3(0, 0, triZ);

        // Triangle vertices
        vertices.Add(triangleCenter + new Vector3(0, 2 * triangleHeight / 3, 0));           // Top vertex
        vertices.Add(triangleCenter + new Vector3(-halfSide, -triangleHeight / 3, 0));      // Bottom-left vertex
        vertices.Add(triangleCenter + new Vector3(halfSide, -triangleHeight / 3, 0));       // Bottom-right vertex

        // Calculate rotations for each vertex based on direction to the centroid
        foreach (var vertex in vertices)
        {
            Vector3 directionToCentroid = (centroid - vertex).normalized;
            Quaternion rotation = Quaternion.LookRotation(directionToCentroid, Vector3.up);
            rotations.Add(rotation);
        }
    }

}
