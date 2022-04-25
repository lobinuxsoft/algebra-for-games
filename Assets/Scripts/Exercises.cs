using UnityEngine;
using CustomMath;

public class Exercises : MonoBehaviour
{
    [SerializeField, Range(1, 10)] int exerciseIndex = 1;
    [SerializeField] Vector3 vectorA;
    [SerializeField] Vector3 vectorB;

    Vec3 vecA = Vec3.Zero;
    Vec3 vecB = Vec3.Zero;
    Vec3 vecC = Vec3.Zero;

    float lerpTimeExercise5 = 0;
    float lerpTimeExercise10 = 0;

    // Start is called before the first frame update
    void Start()
    {
        MathDebbuger.Vector3Debugger.AddVector(transform.position, transform.position + vecA, Color.red, "VectorA");
        MathDebbuger.Vector3Debugger.AddVector(transform.position, transform.position + vecB, Color.green, "VectorB");
        MathDebbuger.Vector3Debugger.AddVector(transform.position, transform.position + vecC, Color.blue, "VectorC");
        MathDebbuger.Vector3Debugger.EnableEditorView();
    }

    private void OnValidate()
    {
        if(Application.isPlaying)
        {
            vecA = new Vec3(vectorA);
            vecB = new Vec3(vectorB);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (exerciseIndex)
        {
            case 1:
                vecC = vecA + vecB; // Suma de vectores
                break;
            case 2:
                vecC = vecB - vecA; // Diferencia de vectores
                break;
            case 3:
                vecC = new Vec3(vecA.x * vecB.x, vecA.y * vecB.y, vecA.z * vecB.z); // Multiplicacion de los componentes de los vectores
                break;
            case 4:
                vecC = -Vec3.Cross(vecA, vecB); // Producto cruz invertido 
                break;
            case 5:

                lerpTimeExercise5 = lerpTimeExercise5 > 1 ? 0 : lerpTimeExercise5 + Time.deltaTime;

                vecC = Vec3.Lerp(vecA, vecB, lerpTimeExercise5); // Interpolacion lineal
                break;
            case 6:
                vecC = Vec3.Max(vecA, vecB); // Se usa el maximo entre vectores
                break;
            case 7:
                vecC = Vec3.Project(vecA, vecB); // Proyeccion 
                break;
            case 8:
                vecC = new Vec3((vecA + vecB).normalized) * Vec3.Distance(vecA, vecB); // Suma de vectores normalizada, multiplicado por la distancia de los mismos.
                break;
            case 9:
                vecC = Vec3.Reflect(vecA, new Vec3(vecB.normalized)); // Reflexion
                break;
            case 10:

                lerpTimeExercise10 = lerpTimeExercise10 >= 10 ? 0 : lerpTimeExercise10 + Time.deltaTime;

                vecC = Vec3.LerpUnclamped(vecB, vecA, lerpTimeExercise10); // Interpolacion lineal sin limites
                break;
        }

        MathDebbuger.Vector3Debugger.UpdatePosition("VectorA", transform.position, transform.position + vecA);
        MathDebbuger.Vector3Debugger.UpdatePosition("VectorB", transform.position, transform.position + vecB);
        MathDebbuger.Vector3Debugger.UpdatePosition("VectorC", transform.position, transform.position + vecC);
    }
}
