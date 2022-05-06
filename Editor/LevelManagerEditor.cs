using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.ShortcutManagement;
using UnityEngine.UIElements;
using System.Linq;


[EditorTool("Draw Level", typeof(LevelManager))]
class LevelTool : EditorTool {

    [Shortcut("Activate Level Tool", KeyCode.B)]
    static void LevelToolShortcut() {
        LevelManager[] selectedLevels = Selection.GetFiltered<LevelManager>(SelectionMode.TopLevel);
        if (selectedLevels.Length == 0) {
            Debug.Log("No levels selected!");
        } else if (selectedLevels.Length > 1) {
            Debug.Log("Can't select more than one level to edit!");
        } else {
            ToolManager.SetActiveTool<LevelTool>();
        }
    }

    public override void OnToolGUI(EditorWindow window)
    {
        if (!(window is SceneView sceneView))
            return;

        bool adding = !Event.current.shift;

        float size = .45f;
        float pickSize = size;

        Handles.color = adding ? Color.green : Color.red;

        LevelManager lm = (LevelManager)target;
        GameObject go = lm.gameObject;

        Handles.matrix = go.transform.localToWorldMatrix;

        LevelManager.Face[] faces = lm.getFaces().ToArray();
        foreach(LevelManager.Face face in faces) {
            Vector3 direction = go.transform.TransformVector(face.direction);
            if (Vector3.Dot(sceneView.camera.transform.forward, direction) < 0) {
                if (Handles.Button(face.centre(), face.rotation(), size, pickSize, Handles.RectangleHandleCap)) {
                    if (adding) {
                        lm.AddBlock(face.position + face.direction);
                    } else {
                        lm.RemoveBlock(face.position);
                    }
                }
            }
        }
    }
}


[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor {
    public override void OnInspectorGUI()
    {
        LevelManager lm = target as LevelManager;

        int blockCount = lm.tiles.Count;
        GUILayout.Label($"{blockCount} blocks");

        if (ToolManager.activeToolType == typeof(LevelTool)) {
            if (blockCount == 0) {
                if (GUILayout.Button("Seed")) {
                    lm.AddBlock(Vector3Int.zero);
                }
            }
        } else {
            if (GUILayout.Button("Start editing")) {
                ToolManager.SetActiveTool<LevelTool>();
            }
        }
    }
}
