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
    private int[] meshIndices;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshIndices = meshFilter.mesh.GetIndices(0);
    }

    private void Update()
    {
        if (meshFilter && point)
        {
            isColliding = DetectCollision(meshFilter.mesh, new Vec3(point.position));
        }
    }


    /// <summary>
    /// Detecta si un punto esta dentro de una mesh utilizando un <see cref="Plane"/>.>
    /// </summary>
    /// <param name="mesh">Se usa para poder calcular los planos.</param>
    /// <param name="point">La posicion de un punto en el espacio.</param>
    /// <returns></returns>
    bool DetectCollision(Mesh mesh, Vec3 point)
    {
        for (int i = 0; i < meshIndices.Length; i += 3)
        {
            Vec3 v1 = new Vec3(mesh.vertices[meshIndices[i]]);
            Vec3 v2 = new Vec3(mesh.vertices[meshIndices[i + 1]]);
            Vec3 v3 = new Vec3(mesh.vertices[meshIndices[i + 2]]);
            
            // Paso las coordenadas locales a globales...
            v1 = FromLocalToWolrd(v1, transform);
            v2 = FromLocalToWolrd(v2, transform);
            v3 = FromLocalToWolrd(v3, transform);

            Vec3 pos = new Vec3(transform.position);
            
            Plane plane = new Plane(v1, v2, v3);

            if (plane.SameSide(pos + plane.normal, point)) return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// Transforma una posicion local en global
    /// </summary>
    /// <param name="point">Una posicion en el espacio</param>
    /// <param name="transformRef">Un trasform que se usa de referencia para poder transformar las posiciones globales en locales.</param>
    /// <returns></returns>
    private Vec3 FromLocalToWolrd(Vec3 point, Transform transformRef)
    {
        Vec3 result = Vec3.Zero;

        result = new Vec3(point.x * transformRef.localScale.x, point.y * transformRef.localScale.y, point.z * transformRef.localScale.z);
        
        result *= transformRef.localRotation;

        return result + new Vec3(transformRef.position);
    }

#if UNITY_EDITOR
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
                    v1 = FromLocalToWolrd(v1, transform);
                    v2 = FromLocalToWolrd(v2, transform);
                    v3 = FromLocalToWolrd(v3, transform);

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
#endif
}