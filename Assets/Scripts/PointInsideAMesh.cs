using System.Collections.Generic;
using System.Linq;
using CustomMath;
using UnityEngine;
using Plane = CustomMath.Plane;

[RequireComponent(typeof(MeshFilter))]
public class PointInsideAMesh : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private bool showPlaneNormals;
    [SerializeField, Range(0, 1)] private float pointGizmosSize = .25f;
    [SerializeField] Color normalColor = Color.cyan;
    [SerializeField] Color colisionColor = Color.red;

    [SerializeField] private bool isColliding;
    
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
        List<Plane> planes = new List<Plane>();
        Vec3 pos = new Vec3(transform.position);
        
        // TODO 1.Crear un Bounding Box, 2.Reordenar caras de Mayor a menor, 3.Tener en cuenta la escala del objeto.
        
        // Crear planos y meterlos en una lista
        for (int i = 0; i < meshIndices.Length; i += 3)
        {
            Vec3 v1 = new Vec3(mesh.vertices[meshIndices[i]]);
            Vec3 v2 = new Vec3(mesh.vertices[meshIndices[i + 1]]);
            Vec3 v3 = new Vec3(mesh.vertices[meshIndices[i + 2]]);
            
            // Paso las coordenadas locales a globales...
            v1 = FromLocalToWolrd(v1, transform);
            v2 = FromLocalToWolrd(v2, transform);
            v3 = FromLocalToWolrd(v3, transform);
            
            Plane plane = new Plane(v1, v2, v3);

            planes.Add(plane);
        }
        
        // Ordeno de mayor a menor
        planes = planes.OrderByDescending(plane1 => plane1.distance).ToList();

        // Checkear los planos
        foreach (Plane plane in planes)
        {
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

    private Vector3[] CalculateBoundingBox(Vector3[] vertices)
    {
        Vector3[] result = new Vector3[6];

        Vector3 left = Vector3.zero;
        Vector3 right = Vector3.zero;
        Vector3 forward = Vector3.zero;
        Vector3 backward = Vector3.zero;
        Vector3 up = Vector3.zero;
        Vector3 down = Vector3.zero;

        foreach (var vert in vertices)
        {
            if (vert.x < left.x) left = -vert.x * Vector3.left;
            if (vert.x > right.x) right = vert.x * Vector3.right;
            if (vert.z > forward.z) forward = vert.z * Vector3.forward;
            if (vert.z < backward.z) backward = -vert.z * Vector3.back;
            if (vert.y > up.y) up = vert.y * Vector3.up;
            if (vert.y < down.y) down = -vert.y * Vector3.down;
        }
        
        result[0] = left;
        result[1] = right;
        result[2] = forward;
        result[3] = backward;
        result[4] = up;
        result[5] = down;

        return result;
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

            Vector3[] bound = CalculateBoundingBox(meshFilter.mesh.vertices);

            for (int i = 0; i < bound.Length; i++)
            {
                Gizmos.DrawSphere(bound[i], pointGizmosSize);
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