using CustomMath;
using UnityEditor;
using UnityEngine;

public class ReflectTest : MonoBehaviour
{
    [SerializeField] Transform normal;
    [SerializeField] Transform inDir;

    private void OnDrawGizmos()
    {
        Vec3 rayInDir = new Vec3(normal.position - inDir.position);
        Vec3 result = Vec3.Reflect(rayInDir, new Vec3(normal.forward));


        Gizmos.matrix = normal.localToWorldMatrix;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(5, 5, 0));
        Gizmos.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, .25f);
        Gizmos.DrawCube(Vector3.zero, new Vector3(5, 5, 0));

        Handles.color = Color.green;
        Handles.SphereHandleCap(0, result, Quaternion.identity, .25f, EventType.Repaint);
        Handles.DrawDottedLine(normal.position, result, 1f);
        Handles.ArrowHandleCap(0, normal.position, Quaternion.LookRotation(result), 1f, EventType.Repaint);


        Handles.matrix = normal.localToWorldMatrix;
        Handles.color = Color.blue;

        Handles.SphereHandleCap(0, Vector3.zero, Quaternion.identity, .25f, EventType.Repaint);
        Handles.ArrowHandleCap(0, Vector3.zero, Quaternion.LookRotation(Vector3.forward), 1f, EventType.Repaint);

        Handles.matrix = inDir.localToWorldMatrix;
        Handles.color = Color.red;
        Handles.SphereHandleCap(0, Vector3.zero, Quaternion.identity, .25f, EventType.Repaint);

        Handles.matrix = inDir.localToWorldMatrix;
        Handles.DrawDottedLine(Vector3.zero, rayInDir, 1f);
        Handles.ArrowHandleCap(0, Vector3.zero, Quaternion.LookRotation(rayInDir), 1f, EventType.Repaint);

        Handles.Label(Vector3.zero, $"Distance= {-Vector3.Dot(normal.forward, inDir.position)}");
    }
}