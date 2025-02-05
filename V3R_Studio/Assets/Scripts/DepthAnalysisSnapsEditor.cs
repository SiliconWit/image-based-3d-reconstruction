using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ThaIntersect.V3RLite{
    [CustomEditor(typeof(DepthAnalysisSnaps))]
    public class DepthAnalysisSnapsEditor : Editor
    {
        private string inputText = "";
        private int selectedIndex = 0;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DepthAnalysisSnaps depthanal = (DepthAnalysisSnaps)target;

            if (GUILayout.Button("Open Depth Analysis Dialog"))
            {
                DepthAnalysisDialog.ShowDialog((DepthAnalysisSnaps)target);
            }

            if (GUILayout.Button("Get Poses"))
            {
                    depthanal.VisualizePoses();
            }

            if( GUILayout.Button("Start Capture")){
                depthanal.TriggerCapture();
            }

            if(GUILayout.Button("Parse Snap Points"))
            {
                EditorInputDialog.Show(
                    "Input Snap Points", 
                    "Paste the Python array string:", 
                    "", 
                    selectedIndex,
                    (result, index) => {
                        if(string.IsNullOrEmpty(result)) return;
                        ParseInput(result, index, depthanal);
                    }
                );
            }
        }

        private void ParseInput(string input, int index, DepthAnalysisSnaps snaps)
        {
            snaps.snapPoints.Clear();
            
            // Remove whitespace and newlines
            input = Regex.Replace(input, @"\s+", "");
            
            // Extract vector values using regex
            var matches = Regex.Matches(input, @"array\(\[(.*?)\]\)");
            List<Vector3> currentGroup = new List<Vector3>();
            
            foreach(Match match in matches)
            {
                string[] values = match.Groups[1].Value.Split(',');
                if(values.Length >= 3)
                {
                    float x = float.Parse(values[0]);
                    float y = float.Parse(values[1]);
                    float z = float.Parse(values[2]);
                    
                    currentGroup.Add(new Vector3(x, y, z));
                    
                    if(currentGroup.Count == 3)
                    {
                        snaps.snapPoints.Add(currentGroup.ToArray());
                        currentGroup.Clear();
                    }
                }
            }
            
            snaps.ProcessSnapPoints(index);
        }
    }

   // Modified Input Dialog Window
    public class EditorInputDialog : EditorWindow
    {
        string text = "";
        string description = "";
        int index = 0;
        System.Action<string, int> onOK;
        
        public static void Show(string title, string description, string defaultText, int defaultIndex, System.Action<string, int> onOK)
        {
            var window = CreateInstance<EditorInputDialog>();
            window.titleContent = new GUIContent(title);
            window.description = description;
            window.text = defaultText;
            window.index = defaultIndex;
            window.onOK = onOK;
            window.position = new Rect(Screen.width/2, Screen.height/2, 400, 200);
            window.ShowModalUtility();
        }
        
        void OnGUI()
        {
            EditorGUILayout.LabelField(description);
            EditorGUILayout.Space(8);
            
            // Add index field
            EditorGUILayout.LabelField("Index:");
            index = EditorGUILayout.IntField(index);
            EditorGUILayout.Space(8);
            
            // Text area for array input
            text = EditorGUILayout.TextArea(text, GUILayout.Height(50));
            EditorGUILayout.Space(8);
            
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("OK"))
            {
                onOK?.Invoke(text, index);
                Close();
            }
            if(GUILayout.Button("Cancel"))
            {
                Close();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
