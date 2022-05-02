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

    [SerializeField] private bool isBoundingColliding;
    [SerializeField] private bool isMeshColliding;

    private BoundingBox boundingBox;
    private MeshFilter meshFilter;
    private MeshRenderer meshRend;
    private int[] meshIndices;

    private void Awake()
    {
        meshRend = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshIndices = meshFilter.mesh.GetIndices(0);
    }

    private void Update()
    {
        if (meshFilter && point)
        {
            isMeshColliding = DetectCollision(meshFilter.mesh, new Vec3(point.position));
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
        
        boundingBox = CalculateBoundingBox(RotateAndScale(mesh.vertices, transform));
        isBoundingColliding = boundingBox.Contains(point);
        
        if (isBoundingColliding)
        {
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

        return false;
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
        
        result = new Vec3(transformRef.localRotation * result);

        return result + new Vec3(transformRef.position);
    }

    private Vec3[] RotateAndScale(Vector3[] points, Transform transformRef)
    {
        Vec3[] result = new Vec3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            result[i] = new Vec3(points[i].x * transformRef.localScale.x, points[i].y * transformRef.localScale.y, points[i].z * transformRef.localScale.z);
            result[i] = new Vec3(transformRef.localRotation * result[i]);
        }

        return result;
    }

    private BoundingBox CalculateBoundingBox(Vec3[] vertices)
    {
        Vec3 left = Vec3.Zero;
        Vec3 right = Vec3.Zero;
        Vec3 forward = Vec3.Zero;
        Vec3 backward = Vec3.Zero;
        Vec3 up = Vec3.Zero;
        Vec3 down = Vec3.Zero;

        foreach (var vert in vertices)
        {
            if (vert.x < left.x) left = -vert.x * Vec3.Left;
            if (vert.x > right.x) right = vert.x * Vec3.Right;
            if (vert.z > forward.z) forward = vert.z * Vec3.Forward;
            if (vert.z < backward.z) backward = -vert.z * Vec3.Back;
            if (vert.y > up.y) up = vert.y * Vec3.Up;
            if (vert.y < down.y) down = -vert.y * Vec3.Down;
        }

        Vec3 center = new Vec3(transform.position);
        Vec3 size = Vec3.Zero;
        size.x = Vec3.Distance(left, right);
        size.y = Vec3.Distance(up, down);
        size.z = Vec3.Distance(forward, backward);
        

        return new BoundingBox(center, size);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color result = isMeshColliding ? colisionColor : normalColor;

        if (meshFilter)
        {
            Gizmos.color = result;
            
            BoundingBox bound = CalculateBoundingBox(RotateAndScale(meshFilter.mesh.vertices, transform));

            Gizmos.DrawWireCube(bound.Center, bound.Size);

            if (isBoundingColliding)
            {
                meshRend.material.color = result;

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
            else
            {
                meshRend.material.color = Color.white;
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