using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using Unity.Mathematics;
using System;


namespace ThaIntersect.V3RLite
{
    public class DepthAnalysisSnaps : MonoBehaviour
    {
        [SerializeField] CameraUnit cameraUnit;
        [SerializeField] MeshRenderer mesh;
        [ReadOnly] [SerializeField] float norm_rad;
        Vector3 centroid, bounds;
        Vector3 top_pitch_target_pt, btm_pitch_target_pt;
        float pitch_disp;
        [SerializeField] string basename;
        [SerializeField] float pitching_scale = 0.5f;
        enum LongitudinalLevel{ bareMiss, PerfFit, }
        [SerializeField] LongitudinalLevel longitudinalLevel;
        [ReadOnly][SerializeField] float long_rad;
        [SerializeField] float pitch_allowance = 1.1f;    
        private RenderTexture depthRT;
        private Material depthMaterial;    

        [SerializeField] private List<Vector3> vertices; // List of vertices (camera positions)
        private List<Quaternion> rotations; // List of rotations for each pose
        bool show_new_point;
        Vector3 ngoi = new Vector3();
        public List<Vector3[]> snapPoints = new List<Vector3[]>();

        [SerializeField] List<Pose> capturePoses = new List<Pose>();


        void Start() {
            // Create a RenderTexture for the depth
            depthRT = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.RFloat);
            depthRT.Create();

            // Load the depth shader
            Shader depthShader = Shader.Find("Hidden/Shader/EuclideanDepthRenderer");
            if (depthShader == null)
            {
                Debug.LogError("Depth shader not found!");
                return;
            }

            // Create a material with the depth shader
            depthMaterial = new Material(depthShader); 
        }

        public void ProcessSnapPoints(int index)
        {
            if (index < 0 || index >= vertices.Count || index >= rotations.Count)
            {
                Debug.LogError("Invalid index for vertices/rotations lists");
                return;
            }

            Vector3 pos = vertices[index];
            Quaternion rot = rotations[index];
            
            foreach(Vector3[] points in snapPoints)
            {
                if (points.Length != 3) continue;
                
                // Transform all points by the rotation and position
                Vector3 target = rot * points[0] + pos;
                Vector3 pos1 = rot * points[1] + pos;
                Vector3 pos2 = rot * points[2] + pos;
                
                // Create rotations by looking at target
                Quaternion rot1 = Quaternion.LookRotation(target - pos1);
                Quaternion rot2 = Quaternion.LookRotation(target - pos2);
                capturePoses.Add(new Pose(pos1, rot1));
                capturePoses.Add(new Pose(pos2, rot2));
                
                // StartCoroutine( CapturePhotos(pos1, rot1, cnt.ToString()+"_pri") );
                // StartCoroutine( CapturePhotos(pos2, rot2, cnt.ToString()+"_sec") );
                // Visualize the transformed points
                // Debug.DrawLine(pos1, target, Color.red, 5f);
                // Debug.DrawLine(pos2, target, Color.blue, 5f);
            }
        }

        public IEnumerator CapturePhotos(){
                var cnt = 1;
                foreach(Pose _pose in capturePoses){

                    transform.DOMove(_pose.position,.7f);
                    transform.DORotate(_pose.rotation.eulerAngles,.7f);

                    yield return new WaitForSeconds(1.3f);
                    var _filename = cameraUnit.SingleSnap( cnt.ToString(), basename);
                    cnt += 1;

               }

        }

        [Button]
        public void VisualizePoses(){
            GetBoundingBox();
                     
        }

        void GetBoundingBox(){
            if (mesh == null)
            {
                Debug.LogError("Mesh is null");
                return;
            }
            
            bounds = mesh.bounds.size;
            // Debug.Log($"Bounds: {bounds}");
            
            centroid = mesh.bounds.center;
            // Debug.Log($"Centroid: {centroid}");
            
            
            var rad =  Mathf.Pow( ( Mathf.Pow(bounds.x, 2) + Mathf.Pow(bounds.z, 2) ), 0.5f );
            print($"Radius: {rad}");

            // calc NormRadius
            norm_rad = rad * 1/Mathf.Tan( Mathf.Deg2Rad * cameraUnit.Get_hFov()/2 ) * 0.5f;
            print($"Normal Radius: {norm_rad}");

            switch (longitudinalLevel)
            {
                case LongitudinalLevel.bareMiss:
                    long_rad = rad/2f;
                    break;

                case LongitudinalLevel.PerfFit:
                    long_rad = norm_rad;
                    break;

                default:
                    long_rad = rad;
                    break;
            }

            /// <summary>
            /// Find pitching scale
            /// It is a function of the h and H
            /// Where:
            ///     h is the vertical distance length covered by long_rad
            ///     H is the vertical length of the target Object
            /// </summary>
            var h = long_rad * Mathf.Tan( .5f * cameraUnit.Get_vFov() * Mathf.Deg2Rad );
            pitching_scale = 1 - (2 * h)/bounds.y * pitch_allowance;
            print(pitching_scale);


            pitch_disp = pitching_scale * bounds.y/2;

            
            btm_pitch_target_pt = new Vector3(
                centroid.x,
                centroid.y - pitch_disp,
                centroid.z                
            );

            top_pitch_target_pt = new Vector3(
                centroid.x,
                centroid.y + pitch_disp,
                centroid.z                
            );

            GenerateHeptahedron(new Vector3(bounds.x, bounds.y/1.5f, bounds.z), centroid, long_rad);
        }

        /// <summary>
        /// Generates a heptahedron with vertices and rotations based on the provided parameters.
        /// </summary>
        /// <param name="boundingBoxDimensions">The dimensions of the bounding box (Vector3).</param>
        /// <param name="_centroid">The centroid of the heptahedron (Vector3).</param>
        /// <param name="scale">The scale factor for the size of the heptahedron (float).</param>
        public void GenerateHeptahedron(Vector3 boundingBoxDimensions, Vector3 _centroid, float scale = 1.0f)
        {
            vertices = new List<Vector3>();
            rotations = new List<Quaternion>();

            float _offset = boundingBoxDimensions.y/1.5f;

            // Calculate parameters
            // float sideLength = Mathf.Max(boundingBoxDimensions.x, boundingBoxDimensions.z) * scale;
            float sideLength = Mathf.Max(scale*2, scale*2) * 1;
            float halfSide = sideLength / 2;
            float height = boundingBoxDimensions.y; // Total height of the heptahedron

            // Quadrilateral (square base)
            float quadY = _centroid.y - (height / 2) ; // Y-position for the base quadrilateral
            vertices.Add(new Vector3(_centroid.x - halfSide, quadY, _centroid.z - halfSide)); // Bottom-left
            vertices.Add(new Vector3(_centroid.x + halfSide, quadY, _centroid.z - halfSide)); // Bottom-right
            vertices.Add(new Vector3(_centroid.x + halfSide, quadY, _centroid.z + halfSide)); // Top-right
            vertices.Add(new Vector3(_centroid.x - halfSide, quadY, _centroid.z + halfSide)); // Top-left

            // Triangle (top face, parallel to quadrilateral)
            float triY = _centroid.y + (height / 1) + _offset; // Y-position for the top triangle
            Vector3 triCenter = new Vector3(_centroid.x, triY, _centroid.z); // Triangle center aligned with _centroid

            // Calculate triangle vertices around the center (equilateral triangle)
            float triangleRadius = Mathf.Sqrt(3) * halfSide / 2; // Radius of the circumscribed circle for an equilateral triangle
            float angleOffset = Mathf.Deg2Rad * 30; // 30-degree rotation
            for (int i = 0; i < 3; i++)
            {
                float angle = angleOffset + i * Mathf.PI * 2 / 3; // 120-degree increments
                vertices.Add(triCenter + new Vector3(
                    triangleRadius * Mathf.Cos(angle),
                    0, // Keep the triangle vertices on the same Y-plane
                    triangleRadius * Mathf.Sin(angle)
                ));
            }
        
            for (int j = 0; j < vertices.Count; j++)
            {
                float lerp = (float)( j)/(vertices.Count-1);
                var y = Mathf.Lerp(
                    quadY,
                    triY,
                    lerp
                );
                vertices[j] = new Vector3(vertices[j].x , y, vertices[j].z ); 
                
            }

            // Calculate rotations for each vertex based on direction to the _centroid
            foreach (var vertex in vertices)
            {
                Vector3 directionToCentroid = (_centroid - vertex).normalized;
                Quaternion rotation = Quaternion.LookRotation(directionToCentroid, Vector3.up);
                rotations.Add(rotation);
            }
        }

        [Button]
        IEnumerator TakePoses(){
            GetBoundingBox();
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector3 vertex = vertices[i];
                // Vector3 vertex = poses[i].position;
                Quaternion rotation = rotations[i];

                transform.DOMove(vertex,1.7f);
                transform.DORotate(rotation.eulerAngles,.7f);

                yield return new WaitForSeconds(1f);
                var _filename = cameraUnit.SingleSnap( i.ToString(), basename);

                // CaptureDepth( _filename );
            }
        }

        void CaptureDepth(string _filename)
        {
            // Render the depth to the RenderTexture
            Graphics.Blit(null, depthRT, depthMaterial);

            // Read the RenderTexture into a Texture2D
            Texture2D tex = new Texture2D(depthRT.width, depthRT.height, TextureFormat.RFloat, false);
            RenderTexture.active = depthRT;
            tex.ReadPixels(new Rect(0, 0, depthRT.width, depthRT.height), 0, 0);
            tex.Apply();

            // Encode to EXR and save
            byte[] bytes = tex.EncodeToEXR(Texture2D.EXRFlags.CompressZIP);
            cameraUnit.SaveImage(bytes, _filename+".exr");

            // Clean up
            RenderTexture.active = null;
            Destroy(tex);
        }

        public void ObtainGoI( int pose_index, Vector3 goi_pos ){
            var pos = vertices[pose_index];
            var rot = rotations[pose_index];
            goi_pos = new Vector3( goi_pos.x, -goi_pos.y, goi_pos.z );
            ngoi = (rot * goi_pos) + pos;
            show_new_point = true;
            Debug.Log( ngoi );
        }
        
        private void OnDrawGizmos()
        {
            if (vertices == null || rotations == null || vertices.Count == 0)
                return;

            if(show_new_point){
                 // Draw vertex as sphere
                Gizmos.DrawSphere(ngoi, 0.05f);
                // show_new_point = false;
            }

            // Draw vertices and rotation arrows
            Gizmos.color = Color.red;
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector3 vertex = vertices[i];
                Quaternion rotation = rotations[i];

                // Draw vertex as sphere
                Gizmos.DrawSphere(vertex, 0.05f);

                // Draw direction arrow
                Vector3 arrowEnd = vertex + (rotation * Vector3.forward) * 0.5f; // Arrow length of 0.5
                Gizmos.color = Color.green;
                Gizmos.DrawLine(vertex, arrowEnd);

                // Draw labels for debugging
    #if UNITY_EDITOR
                UnityEditor.Handles.Label(vertex, $"Vertex {i}");
    #endif
            }
        }

        internal void TriggerCapture()
        {
            StartCoroutine("CapturePhotos");
        }
    }

    
    
}
