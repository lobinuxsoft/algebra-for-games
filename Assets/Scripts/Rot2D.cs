#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class Rot2D : MonoBehaviour
{
    [SerializeField] private float angle = 2f;
    private Vector3 rot = Vector3.zero;

    private void Update()
    {
        rot = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = new Vector3(
                transform.position.x * rot.x - transform.position.y * rot.y,
                transform.position.y * rot.x + transform.position.x * rot.y,
                0);
        }
    }

#if UNITY_EDITOR
    [SerializeField] Color gizmosColor = Color.red;

    private void OnDrawGizmos()
    {
        Handles.color = gizmosColor;
        Handles.DrawDottedLine(Vector3.zero, transform.position, 1f);
    }
#endif
}
