using UnityEngine;

public class Rot3D : MonoBehaviour
{
    [SerializeField] Vector3 angle;

    private void Update()
    {
        float real;
        float imaginaryX;
        float imaginaryY;
        float imaginaryZ;
        
        imaginaryZ = Mathf.Sin(Mathf.Deg2Rad * angle.z / 2);
        real = Mathf.Cos(Mathf.Deg2Rad * angle.z / 2);

        Quaternion rotZ = Quaternion.identity;
        rotZ.w = real;
        rotZ.z = imaginaryZ;

        imaginaryX = Mathf.Sin(Mathf.Deg2Rad * angle.x / 2);
        real = Mathf.Cos(Mathf.Deg2Rad * angle.x / 2);

        Quaternion rotX = Quaternion.identity;
        rotX.w = real;
        rotX.x = imaginaryX;

        imaginaryY = Mathf.Sin(Mathf.Deg2Rad * angle.y / 2);
        real = Mathf.Cos(Mathf.Deg2Rad * angle.y / 2);

        Quaternion rotY = Quaternion.identity;
        rotY.w = real;
        rotY.y = imaginaryY;

        transform.rotation = (rotX * rotY * rotZ);
    }
}
