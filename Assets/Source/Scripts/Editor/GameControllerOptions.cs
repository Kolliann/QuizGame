#if UNITY_EDITOR
using System.IO;
using Controller;
using Source.Scripts;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(EditorGameController))]
public class GameControllerOptions : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Create Screenshot"))
        {
            var path = Path.Combine(Application.persistentDataPath, System.Guid.NewGuid() + ".png");
            ScreenCapture.CaptureScreenshot(path);
            Debug.Log(path);
        }
        
        if (GUILayout.Button("Clear Game State"))
        {
            GameState.Instance.Clear();
        }
    }
}
#endif