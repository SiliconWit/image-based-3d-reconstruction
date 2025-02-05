using UnityEditor;
using UnityEngine;

namespace ThaIntersect.V3RLite{
    public class DepthAnalysisDialog : EditorWindow
    {
        private int indexValue;
        private string positionString = ""; // User inputs "x,y,z"
        private DepthAnalysisSnaps targetScript;

        public static void ShowDialog(DepthAnalysisSnaps target)
        {
            DepthAnalysisDialog window = GetWindow<DepthAnalysisDialog>(true, "Depth Analysis Input");
            window.targetScript = target;
            window.minSize = new Vector2(300, 150);
        }

        private void OnGUI()
        {
            GUILayout.Label("Enter Index and Position", EditorStyles.boldLabel);

            indexValue = EditorGUILayout.IntField("Index", indexValue);
            positionString = EditorGUILayout.TextField("Position (x,y,z)", positionString);

            if (GUILayout.Button("Submit"))
            {
                if (TryParseVector3(positionString, out Vector3 parsedVector))
                {
                    targetScript.ObtainGoI(indexValue, parsedVector);
                    Close();
                }
                else
                {
                    EditorUtility.DisplayDialog("Input Error", "Invalid Vector3 format. Use 'x,y,z'", "OK");
                }
            }
        }

        private bool TryParseVector3(string input, out Vector3 result)
        {
            result = Vector3.zero;
            string[] values = input.Split(',');

            if (values.Length == 3 && 
                float.TryParse(values[0], out float x) &&
                float.TryParse(values[1], out float y) &&
                float.TryParse(values[2], out float z))
            {
                result = new Vector3(x, y, z);
                return true;
            }
            return false;
        }
    }

}