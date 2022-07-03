using CustomMath;
using UnityEditor;
using UnityEngine;

public class LerpRotationTest : MonoBehaviour
{
    enum QuaternionType { Unity, Custom }

    enum LerpType { LERP, SLERP }

    [SerializeField] QuaternionType quaternionType = QuaternionType.Unity;
    [SerializeField] LerpType lerpType = LerpType.LERP;
    [SerializeField] bool clamped = false;
    [SerializeField, Range(0, 2)] float lerpValue = 0;
    [SerializeField] Transform rotationA;
    [SerializeField] Transform rotationB;
    [SerializeField] Transform pivot;

    private void OnValidate()
    {
        switch (quaternionType)
        {
            case QuaternionType.Unity:
                switch (lerpType)
                {
                    case LerpType.LERP:
                        pivot.rotation = clamped ? Quaternion.Lerp(rotationA.rotation, rotationB.rotation, lerpValue) : Quaternion.LerpUnclamped(rotationA.rotation, rotationB.rotation, lerpValue);
                        break;
                    case LerpType.SLERP:
                        pivot.rotation = clamped ? Quaternion.Slerp(rotationA.rotation, rotationB.rotation, lerpValue) : Quaternion.SlerpUnclamped(rotationA.rotation, rotationB.rotation, lerpValue);
                        break;
                }
                break;
            case QuaternionType.Custom:
                switch (lerpType)
                {
                    case LerpType.LERP:
                        pivot.rotation = clamped ? Quat.Lerp(rotationA.rotation, rotationB.rotation, lerpValue) : Quat.LerpUnclamped(rotationA.rotation, rotationB.rotation, lerpValue);
                        break;
                    case LerpType.SLERP:
                        pivot.rotation = clamped ? Quat.Slerp(rotationA.rotation, rotationB.rotation, lerpValue) : Quat.SlerpUnclamped(rotationA.rotation, rotationB.rotation, lerpValue);
                        break;
                }
                break;
        }
        
    }

    private void OnDrawGizmos()
    {
        if(rotationA && rotationB && pivot)
        {
            Handles.color = Color.green;
            Handles.matrix = rotationA.localToWorldMatrix;
            Handles.ArrowHandleCap(0, Vector3.zero, Quaternion.identity, 1,EventType.Repaint);

            Handles.color = Color.red;
            Handles.matrix = rotationB.localToWorldMatrix;
            Handles.ArrowHandleCap(0, Vector3.zero, Quaternion.identity, 1, EventType.Repaint);

            Handles.color = Color.yellow;
            Handles.matrix = pivot.localToWorldMatrix;
            Handles.ArrowHandleCap(0, Vector3.zero, Quaternion.identity, 1, EventType.Repaint);
        }
    }
}

