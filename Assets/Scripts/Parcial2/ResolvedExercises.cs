using UnityEngine;
using MathDebbuger;
using CustomMath;

public class ResolvedExercises : MonoBehaviour
{
    [SerializeField, Range (1,3)] int exercises = 1;
    [SerializeField] float angle;

    Vec3 vectorA = new Vec3(10, 0, 0);
    Vec3 vectorB = new Vec3(10, 10, 0);
    Vec3 vectorC = new Vec3(20, 10, 0);
    Vec3 vectorD = new Vec3(20, 10, 0);

    // Start is called before the first frame update
    void Start()
    {
        Vector3Debugger.AddVector(Vector3.zero , vectorA, Color.red, nameof(vectorA));
        Vector3Debugger.AddVector(vectorA, vectorB, Color.green, nameof(vectorB));
        Vector3Debugger.AddVector(vectorB , vectorC, Color.blue, nameof(vectorC));
        Vector3Debugger.AddVector(vectorC, vectorD, Color.cyan, nameof(vectorD));
        Vector3Debugger.EnableEditorView();
    }

    // Update is called once per frame
    void Update()
    {
        switch (exercises)
        {
            case 1:
                vectorA = Quat.Euler(new Vec3(0, angle, 0)) * vectorA;
                Vector3Debugger.UpdatePosition(nameof(vectorA), vectorA);
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
