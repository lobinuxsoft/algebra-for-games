using UnityEditor;
using UnityEngine;

public class MatrixDebugger : EditorWindow
{
    public GameObject target;
    GUIStyle style = new GUIStyle();
    private Vector2 scroll;

    [MenuItem("Window/MatrixDebugger")]
    static void Init()
    {
        MatrixDebugger window = (MatrixDebugger)EditorWindow.GetWindow(typeof(MatrixDebugger));
        window.Show();
    }

    private void OnGUI()
    {
        target = EditorGUILayout.ObjectField("Target",target, typeof(GameObject), true) as GameObject;
        if (target != null)
        {
            style.fontSize = 30;
            style.normal.textColor = Color.red;
            scroll = EditorGUILayout.BeginScrollView(scroll);
            target.transform.position = EditorGUILayout.Vector3Field("Position: ", target.transform.position);
            target.transform.rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation: ", target.transform.rotation.eulerAngles));
            target.transform.localScale = EditorGUILayout.Vector3Field("Scale: ", target.transform.localScale);
            EditorGUILayout.LabelField("Translation matrix", style);
            EditorGUILayout.Space(10);
            DrawMatrix(Matrix4x4.Translate(target.transform.position));

            EditorGUILayout.Space(40);

            EditorGUILayout.LabelField("Rotation matrix", style);
            EditorGUILayout.Space(10);
            DrawMatrix(Matrix4x4.Rotate(target.transform.rotation));

            EditorGUILayout.Space(40);

            EditorGUILayout.LabelField("Scale matrix", style);
            EditorGUILayout.Space(10);
            DrawMatrix(Matrix4x4.Scale(target.transform.localScale));

            EditorGUILayout.Space(40);

            EditorGUILayout.LabelField("TRS matrix", style);
            EditorGUILayout.Space(10);
            DrawMatrix(Matrix4x4.TRS(target.transform.position, target.transform.rotation, target.transform.localScale));
            EditorGUILayout.EndScrollView();
        }
    }

    private void DrawMatrix(Matrix4x4 matrix)
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.red;
        EditorGUILayout.LabelField("| " + matrix.m00.ToString("00.00") + " " + matrix.m01.ToString("00.00") + " " + matrix.m02.ToString("00.00") + " " + matrix.m03.ToString("00.00") + " |", style);
        EditorGUILayout.LabelField("| " + matrix.m10.ToString("00.00") + " " + matrix.m11.ToString("00.00") + " " + matrix.m12.ToString("00.00") + " " + matrix.m13.ToString("00.00") + " |", style);
        EditorGUILayout.LabelField("| " + matrix.m20.ToString("00.00") + " " + matrix.m21.ToString("00.00") + " " + matrix.m22.ToString("00.00") + " " + matrix.m23.ToString("00.00") + " |", style);
        EditorGUILayout.LabelField("| " + matrix.m30.ToString("00.00") + " " + matrix.m31.ToString("00.00") + " " + matrix.m32.ToString("00.00") + " " + matrix.m33.ToString("00.00") + " |", style);
    }

}