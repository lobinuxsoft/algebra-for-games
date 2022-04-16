using CustomMath;
using UnityEngine;
using Plane = CustomMath.Plane;

[RequireComponent(typeof(MeshFilter))]
public class PointInsideAMesh : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private bool showPlaneNormals = false;
    [SerializeField, Range(0, 1)] private float pointGizmosSize = .25f;
    [SerializeField] Color normalColor = Color.cyan;
    [SerializeField] Color colisionColor = Color.red;

    [SerializeField] private bool isColliding = false;
    
    private MeshFilter meshFilter;
    
    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        if (meshFilter && point)
        {
            isColliding = DetectCollision(meshFilter.mesh, new Vec3(point.position));
        }
    }


    bool DetectCollision(Mesh mesh, Vec3 point)
    {
        for (int i = 0; i < mesh.GetIndices(0).Length; i += 3)
        {
            Vec3 v1 = new Vec3(mesh.vertices[mesh.GetIndices(0)[i]]);
            Vec3 v2 = new Vec3(mesh.vertices[mesh.GetIndices(0)[i + 1]]);
            Vec3 v3 = new Vec3(mesh.vertices[mesh.GetIndices(0)[i + 2]]);
            
            // Paso las coordenadas locales a globales...
            v1 = new Vec3(FromLocalToWolrd(new Vector3(v1.x,v1.y,v1.z), transform));
            v2 = new Vec3(FromLocalToWolrd(new Vector3(v2.x,v2.y,v2.z), transform));
            v3 = new Vec3(FromLocalToWolrd(new Vector3(v3.x,v3.y,v3.z), transform));

            Vec3 pos = new Vec3(transform.position);
            
            Plane plane = new Plane(v1, v2, v3);

            if (plane.SameSide(pos + plane.normal, point)) return false;
        }
        
        return true;
    }
    
    private Vector3 FromLocalToWolrd(Vector3 point, Transform transformRef)
    {
        Vector3 result = Vector3.zero;

        result = new Vector3(point.x * transformRef.localScale.x, point.y * transformRef.localScale.y, point.z * transformRef.localScale.z);

        result = transformRef.localRotation * result;

        return result + transformRef.position;
    }
    
    private void OnDrawGizmos()
    {
        Color result = isColliding ? colisionColor : normalColor;
        
        Gizmos.color = result;
        if(meshFilter) Gizmos.DrawWireMesh(meshFilter.mesh, transform.position, transform.rotation);

        result.a = .5f;
        Gizmos.color = result;
        
        if (meshFilter)
        {
            Gizmos.DrawMesh(meshFilter.mesh, transform.position, transform.rotation);

            if (showPlaneNormals)
            {
                for (int i = 0; i < meshFilter.mesh.GetIndices(0).Length; i += 3)
                {
                    Vec3 v1 = new Vec3(meshFilter.mesh.vertices[meshFilter.mesh.GetIndices(0)[i]]);
                    Vec3 v2 = new Vec3(meshFilter.mesh.vertices[meshFilter.mesh.GetIndices(0)[i + 1]]);
                    Vec3 v3 = new Vec3(meshFilter.mesh.vertices[meshFilter.mesh.GetIndices(0)[i + 2]]);
                
                    // Paso las coordenadas locales a globales...
                    v1 = new Vec3(FromLocalToWolrd(new Vector3(v1.x,v1.y,v1.z), transform));
                    v2 = new Vec3(FromLocalToWolrd(new Vector3(v2.x,v2.y,v2.z), transform));
                    v3 = new Vec3(FromLocalToWolrd(new Vector3(v3.x,v3.y,v3.z), transform));

                    Plane plane = new Plane(v1, v2, v3);

                    Vector3 normal = new Vector3(plane.normal.x, plane.normal.y, plane.normal.z);
                    
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.position, normal + transform.position);
                }
            }
        }

        if (point)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(point.position, pointGizmosSize);
        }
    }
}
