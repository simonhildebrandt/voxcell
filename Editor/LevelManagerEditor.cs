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

        // Handles.BeginGUI();
        // if (adding) {
        //     if (GUILayout.Button("Remove")) {
        //         adding = false;
        //     }
        // } else {
        //     if (GUILayout.Button("Add")) {
        //         adding = true;
        //     }
        // }
        // Handles.EndGUI();

        var evt = Event.current;
        // if (evt.type == EventType.Repaint) {
        // }
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

    // public override void OnDrawGizmos() {
    //     Gizmos.DrawCube(Vector3.zero, Vector3.one);
    // }
}

// public class BuildLevel: EditorWindow {
//     public static bool editing = false;

//     [MenuItem("Component/Level Tools")]
//     static void ShowWindow() {
//         BuildLevel window = ScriptableObject.CreateInstance(typeof(BuildLevel)) as BuildLevel;
//         window.ShowUtility();
//     }

//     void OnSceneGUI() {
//     }

//     void OnGUI() {
//         GUILayout.Label("Level Editor");
//         int selected = 0;
//         string[] options = new string[]
//         {
//             "Option1", "Option2", "Option3",
//         };
//         selected = EditorGUILayout.Popup("Label", selected, options);
//     }

//     private static void OnScene(SceneView sceneview) {
//         Event e = Event.current;

//         if (e.type == EventType.MouseDown && e.button == 0) {
//             // Debug.Log("here");
//             Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
//             RaycastHit hitInfo;
//             if (Physics.Raycast(worldRay, out hitInfo)) {
//                 Vector3 point = hitInfo.transform.InverseTransformPoint(hitInfo.point) * 1.05f;
//                 Vector3Int direction = Vector3Int.RoundToInt(point);
//                 Debug.Log(direction);

//                 LevelManager[] lms = FindObjectsOfType<LevelManager>();
//                 Debug.Log(lms);
//             }

//             // Steal control to prevent other selection behaviour
//             // int controlId = GUIUtility.GetControlID(FocusType.Passive);
//             // GUIUtility.hotControl = controlId;
//         }
//     }

//     void OnEnable() {
//         editing = true;
//         SceneView.duringSceneGui += OnScene;
//     }

//     void OnDestroy() {
//         editing = false;
//         SceneView.duringSceneGui -= OnScene;

//         // Free selection control
//         // GUIUtility.hotControl = 0;
//     }
// }



[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
//     private static bool m_editMode = false;


    public override void OnInspectorGUI()
    {
        LevelManager lm = target as LevelManager;

        int blockCount = lm.tiles.Count;
        GUILayout.Label($"{blockCount} blocks");

        if (ToolManager.activeToolType == typeof(LevelTool)) {
            if (blockCount == 0) {
                if (GUILayout.Button("Add")) {
                    lm.AddBlock(Vector3Int.zero);
                }
            }
            if (GUILayout.Button("Done")) {
                ToolManager.RestorePreviousTool();
            }
        } else {
            if (GUILayout.Button("EDIT")) {
                ToolManager.SetActiveTool<LevelTool>();
            }
        }
    }

//         LevelManager level = (LevelManager)target;

//         GameObject source = level.block;
//         source = (GameObject)EditorGUILayout.ObjectField(source, typeof(GameObject), true);
//         if (source) {
//             level.SetBlockPrefab(source);
//         }

//         if (m_editMode) {
//             // if (Event.current.type == EventType.MouseDown) {
//             //     Event.current.Use();
//             // }

//             // if (Event.current.type == EventType.MouseUp)
//             // {
//             //     Debug.Log("here");
//             //     // Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
//             //     // RaycastHit hitInfo;


//             //     // if (Physics.Raycast(worldRay, out hitInfo))
//             //     // {
//             //     //     GameObject waypoint = Resources.LoadAssetAtPath("Assets/Prefabs/Waypoint.prefab", typeof(GameObject)) as GameObject;
//             //     //     GameObject waypointInstance = Instantiate(waypoint) as GameObject;
//             //     //     waypointInstance.transform.position = hitInfo.point;
//             //     //     waypointInstance.name = "Waypoint" + m_count.ToString("00");
//             //     //     waypointInstance.transform.parent = m_container.transform;
//             //     //     Waypoint waypointScript = waypointInstance.GetComponent("Waypoint") as Waypoint;
//             //     //     waypointScript.id = m_count;

//             //     //     EditorUtility.SetDirty(waypointInstance);

//             //     //     m_count++;
//             //     // }
//             //     Event.current.Use();
//             // }


//             if (GUILayout.Button("Finish Editing")) {
//                 m_editMode = false;
//             }
//         } else {
//             if (GUILayout.Button("Enable Editing")) {
//                 m_editMode = true;

//                 level.EnsureBlock();
//             }
//         }
//     }
}
