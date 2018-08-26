using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour
{
    public enum Symbol { A, B, C, D, E };
    public Symbol symbol;
    public float pDistance; // distance w.r.t. parent
    public float pRotation; // rotation w.r.t. parent

    public Vector3 orientation;
    public int age;

    // Use this for initialization
    void Start()
    {
        age = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Iterate()
    {
        if (age == 0) Grow();

        gameObject.GetComponent<MeshRenderer>().material.color = new Color(1 - 0.02f * age, 1, 1);

        age++;
    }

    public void Grow()
    {
        switch (symbol)
        {
            case Symbol.A:
                CreateChild(90.0f, Symbol.B, 1.0f, 0.0f);
                CreateChild(0.0f, Symbol.A, 1.0f, 0.0f);
                CreateChild(-90.0f, Symbol.B, 1.0f, 0.0f);
                break;

            case Symbol.B:
                CreateChild(0.0f, Symbol.B, 0.7f, 0.0f);
                break;

            case Symbol.C:
                CreateChild(90.0f, Symbol.D, 0.4f, -5.0f);
                CreateChild(0.0f, Symbol.A, 1.0f, 0.0f);
                CreateChild(-90.0f, Symbol.D, 0.4f, 5.0f);
                break;

            case Symbol.D:
                CreateChild(90.0f, Symbol.E, 0.1f, 0.0f);
                CreateChild(0.0f, Symbol.D, 1.0f, pRotation * 0.95f);
                CreateChild(-90.0f, Symbol.E, 0.1f, 0.0f);
                break;

            case Symbol.E:
                CreateChild(0.0f, Symbol.E, 0.9f, 0.0f);
                break;

            default:
                break;
        }
    }

    private void CreateChild(float angle, Symbol symbol, float distance, float rotateAngle)
    {
        transform.Rotate(transform.forward, angle);

        GameObject child = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        child.transform.parent = gameObject.transform.parent;
        child.transform.position = transform.position + pDistance * distance * transform.up;
        child.transform.rotation = Quaternion.AngleAxis(rotateAngle, Vector3.forward) * transform.rotation;

        Node node = child.AddComponent<Node>();
        node.symbol = symbol;
        node.pDistance = pDistance * distance;
        node.pRotation = rotateAngle;

        transform.Rotate(transform.forward, -angle);
    }
}
