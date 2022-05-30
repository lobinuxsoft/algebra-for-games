using UnityEngine;

public class QuatRot3D : MonoBehaviour
{
    [SerializeField] Quaternion quaternion1;
    [SerializeField] Quaternion quaternion2;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = MultiplicacionQuaternion(quaternion1, quaternion2);
    }

    Quaternion MultiplicacionQuaternion(Quaternion q1, Quaternion q2)
    {
        float w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z; // Real
        float x = q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y; // I
        float y = q1.w * q2.y - q1.x * q2.z + q1.y * q2.w + q1.z * q2.x; // J
        float z = q1.w * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.w; // K

        return new Quaternion(x, y, z, w);
    }
}
