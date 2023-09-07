using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace ThaIntersect.V3R
{
    public class ChamferedSquareWaypointGenerator : CameraPoseControl
    {
        [SerializeField] Vector2 Rectangle;
        [SerializeField] float chamferScale = 0.75f;
        [Tooltip("Min Pose Points for the shorter edges")][SerializeField] int chamfer_edges = 2;
        [Tooltip("Min Pose Points for the longer edges")][SerializeField] int longer_edges = 4;
        enum Planes{ XY=0, XZ, YZ  }
        [SerializeField] Planes plane;
        [Tooltip("Total number of poses")] public int elevations = 3;
        List<(Vector3, Vector3)> edges = new List<(Vector3, Vector3)>();
        List<FakeTransform> levelTransforms = new List<FakeTransform>();
        

        new void Start()
        {
            base.Start();
            edges = generate_rectangle();   
            CreatePosePoints();
            InitialisePhotoShoot();
        }

        private void CreatePosePoints()
        {
            
            for (int i = 0; i < edges.Count; i++)
            {
                var edge_vec = edges[i].Item2 - edges[i].Item1;
                var midpoint = (edges[i].Item1 + edges[i].Item2) / 2;
                var target_vec = (target.transform.position - midpoint);
                
                

                float cos_theta = Vector3.Dot( edge_vec, target_vec ) / ( edge_vec.magnitude * target_vec.magnitude );
                
                float angle_deg = Mathf.Acos(cos_theta) * Mathf.Rad2Deg;
                
                if( i%2 == 0 ){
          
                    distribute_points_around( edge_vec, midpoint, longer_edges, angle_deg );
                }else{
                    distribute_points_between( edge_vec, midpoint, chamfer_edges, angle_deg );
                }

            }
            float y = target.position.y;
            var y0 = y -  (boundingBox.y * mult_h)/2;
            print( levelTransforms.Count );
            for (int j = 0; j < elevations; j++)
            {
                foreach (var levelPose in levelTransforms)
                {   
                    // levelPose.position.y = y0 + ((boundingBox.y * mult_h/(elevations-1)) * j);
                    waypointTransforms.Add( 
                        new FakeTransform{
                            rotation = levelPose.rotation,
                            position = new Vector3(
                                levelPose.position.x,
                                y0 + ((boundingBox.y * mult_h/(elevations-1)) * j),
                                levelPose.position.z
                            )
                        }
                        );
                }
            }
            // foreach (var edge in edges)
            // {

            //     // print(edge_vec.magnitude);
            //     // distribute_points_around( edge_vec, midpoint, 3 );
            //     distribute_points_between( edge_vec, midpoint, 3 );
            //     float cos_theta = Vector3.Dot( edge_vec, target_vec ) / ( edge_vec.magnitude * target_vec.magnitude );
                
            //     float angle_deg = Mathf.Acos(cos_theta) * Mathf.Rad2Deg;
            // }
        }

        void distribute_points_around(Vector3 _edge_vec, Vector3 midpoint, float numberOfPoints, float angle)
        {
            // Calculate the spacing between points
            float spacing = (_edge_vec.magnitude / (numberOfPoints - 1))*((numberOfPoints - 1)/numberOfPoints);

            // Calculate the position of the first point (aligned to the start of the line)
            Vector3 startPoint = midpoint - (_edge_vec.normalized * _edge_vec.magnitude * 0.5f)+(_edge_vec.normalized*spacing/2);
            
            var rotationDummy = new GameObject();
            var qtn = LookAtTargetYAxisOnly( midpoint );
            Destroy(rotationDummy);
            
            // Create and position the points along the line
            for (int i = 0; i < numberOfPoints; i++)
            {
                Vector3 pointPosition = startPoint + (_edge_vec.normalized * i * spacing);
                
                var obj = new GameObject();
                // var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);//.transform.position = pointPosition;
                obj.transform.position = pointPosition;
                obj.transform.rotation = qtn;
                levelTransforms.Add(new FakeTransform{
                    position = obj.transform.position,
                    rotation = obj.transform.rotation
                });
                Destroy(obj);
                // t.transform.position = pointPosition;
                // t.transform.Rotate(transform.up, -angle);
            }
        }

        void distribute_points_between(Vector3 _edge_vec, Vector3 midpoint, float numberOfPoints, float angle)
        {
            // Calculate the spacing between points
            float spacing = _edge_vec.magnitude / (numberOfPoints - 1);

            // Calculate the position of the first point (aligned to the start of the line)
            Vector3 startPoint = midpoint - (_edge_vec.normalized * _edge_vec.magnitude * 0.5f);

            var rotationDummy = new GameObject();
            var qtn = LookAtTargetYAxisOnly( midpoint );
            Destroy(rotationDummy);

            // Create and position the points along the line
            for (int i = 0; i < numberOfPoints; i++)
            {
                Vector3 pointPosition = startPoint + (_edge_vec.normalized * i * spacing);
                
                var obj = new GameObject();
                // var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);//.transform.position = pointPosition;
                obj.transform.position = pointPosition;
                obj.transform.rotation = qtn;
                levelTransforms.Add(new FakeTransform{
                    position = obj.transform.position,
                    rotation = obj.transform.rotation
                });
                Destroy(obj);

                // GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = pointPosition;
                // var t = GameObject.CreatePrimitive(PrimitiveType.Sphere);//.transform.position = pointPosition;
                // t.transform.position = pointPosition;
                // t.transform.Rotate(transform.up, -angle);

            }
        }

        List<(Vector3, Vector3)> generate_rectangle(){
            var rect_vertices = new List<Vector3>();
            var rect_edges = new List<(Vector3, Vector3)>();
            var edges = new List<(Vector3, Vector3)>();

            float r = Mathf.Sqrt( 
                (boundingBox.x * boundingBox.x) + (boundingBox.z * boundingBox.z)
            );
            Rectangle.x = r/Math.Max( boundingBox.x, boundingBox.z ) *boundingBox.x * mult_r;
            Rectangle.y = r/Math.Max( boundingBox.x, boundingBox.z ) *boundingBox.z * mult_r;
            Math.Max( boundingBox.x, boundingBox.z );
            // Create vectors for half the width and height, oriented correctly based on the plane
            var half_width = new Vector3();
            var half_length = new Vector3();
            switch (plane)
            {
                case Planes.XY:
                    half_width = new Vector3(Rectangle.x/2, 0, 0);
                    half_length = new Vector3(0, Rectangle.y / 2, 0);
                    break;

                case Planes.XZ:
                    half_width = new Vector3(Rectangle.x / 2, 0, 0);
                    half_length = new Vector3(0, 0, Rectangle.y / 2);
                    break;
                    
                case Planes.YZ:
                    half_width = new Vector3(0, Rectangle.x / 2, 0);
                    half_length = new Vector3(0, 0, Rectangle.y / 2);
                    break;

                default:
                    break;
            }

            // Calculate the vertices
            var v1 = target.position - half_width - half_length;
            var v2 = target.position + half_width - half_length;
            var v3 = target.position + half_width + half_length;
            var v4 = target.position - half_width + half_length;

            rect_vertices = new List<Vector3>{ 
                target.position - half_width - half_length,
                target.position + half_width - half_length,
                target.position + half_width + half_length,
                target.position - half_width + half_length
            };
            
            rect_edges = new List<ValueTuple<Vector3, Vector3>>{ 
                (rect_vertices[0], rect_vertices[1]), 
                (rect_vertices[1], rect_vertices[2]), 
                (rect_vertices[2], rect_vertices[3]),
                (rect_vertices[3], rect_vertices[0])
            };
            
            var scaled_edges = new List<ValueTuple<Vector3, Vector3>>();
            
            foreach (var _edge in rect_edges)
                scaled_edges.Add( scale_edge( _edge ) );            
            
            for (int i = 0; i < scaled_edges.Count; i++){
                edges.Add(scaled_edges[i]);
                edges.Add((
                    scaled_edges[i].Item2, 
                    scaled_edges[(i+1)%scaled_edges.Count].Item1
                ));                
            }
            
            return edges;   
        }

        // void spawnSphere(Vector3 pos){
        //     foreach (var item in edges)
        //     {
        //         // Create a new sphere GameObject
        //         GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = item.Item1;
        //         GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = item.Item2;
        //     }
        // }

        ValueTuple<Vector3, Vector3> scale_edge(ValueTuple<Vector3, Vector3> _edge){
            // Calculate the midpoint of the edge
            var midpoint = (_edge.Item1 + _edge.Item2) / 2;

            // Calculate vectors from the midpoint to the original points
            var vector1 = _edge.Item1 - midpoint;
            var vector2 = _edge.Item2 - midpoint;

            // Calculate vectors from the midpoint to the original points
            vector1 *= chamferScale;
            vector2 *= chamferScale;

            return ( midpoint + vector1, midpoint + vector2 );
        }
    }
    
}
