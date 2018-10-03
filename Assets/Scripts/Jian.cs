using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Jian : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter filter;
    public new MeshRenderer renderer;

    private List<Vector3> vertices;
    private List<Vector3> normals;
    private List<Vector2> uvs;
    private List<int> triangles;

    // Use this for initialization
    void Start()
    {
        mesh = new Mesh();
        filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;
        renderer = gameObject.AddComponent<MeshRenderer>();

        Material bladeMaterial = new Material(Shader.Find("Standard"));
        renderer.material = Resources.Load<Material>("Materials/Jian");

        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        uvs = new List<Vector2>();
        triangles = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Symbol> RewriteString(List<Symbol> inputString)
    {
        List<Symbol> newString = new List<Symbol>();
        foreach (var item in inputString)
        {
            switch (item.s)
            {
                case "P": // point
                    int age = (int)item.p[0];

                    if (age < 12)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.4f, 0 })); // age, angle, distance ratio
                        newString.Add(new Symbol("V", new object[] { 1, 45.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 135.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 180.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 225.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 270.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 315.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("]", new object[] { }));
                        newString.Add(new Symbol("U", new object[] { 1.0f })); // distance
                    }
                    else if (age < 14)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.8f, 1 })); // age, angle, distance ratio
                        newString.Add(new Symbol("V", new object[] { 1, 45.0f, 0.8f, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, 0.8f, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 135.0f, 0.8f, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 180.0f, 0.8f, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 225.0f, 0.8f, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 270.0f, 0.8f, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 315.0f, 0.8f, 1 }));
                        newString.Add(new Symbol("]", new object[] { }));
                        newString.Add(new Symbol("U", new object[] { 0.3f })); // distance
                    }
                    else
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.5f, 2 })); // age, angle, distance ratio
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.5f, 2 })); // age, angle, distance ratio
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, 0.5f, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, 0.5f, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, 180.0f, 0.5f, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, 180.0f, 0.5f, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, 270.0f, 0.5f, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, 270.0f, 0.5f, 2 }));
                        newString.Add(new Symbol("]", new object[] { }));
                        newString.Add(new Symbol("U", new object[] { 1.0f })); // distance
                    }
                    item.p[0] = (int)item.p[0] + 1; // P(0) -> P(1)
                    newString.Add(item);
                    break;
                case "V": // vertex
                    item.p[0] = (int)item.p[0] + 1; // V(0,...) -> V(1,...)
                    newString.Add(item);
                    break;
                default:
                    newString.Add(item);
                    break;
            }
        }

        return newString;
    }

    public void TurtleInterpretation(List<Symbol> inputString)
    {
        Vector3 turtlePosition = Vector3.zero;
        List<List<Vector3>> rings = new List<List<Vector3>>();
        List<Vector3> ring = new List<Vector3>();

        foreach (var item in inputString)
        {
            switch (item.s)
            {
                case "[": // start a ring
                    ring = new List<Vector3>();
                    break;
                case "]": // end a ring
                    rings.Add(ring);
                    break;
                case "V":
                    Vector3 newVertex = Vector3.right;
                    newVertex = Quaternion.AngleAxis((float)item.p[1], -Vector3.up) * newVertex; // angle
                    //newVertex *= GeometricSeries((float)item.p[2], 0.7f, (int)item.p[0]); // angle
                    newVertex *= (float)item.p[2] * 5.0f;
                    newVertex += turtlePosition;
                    ring.Add(newVertex);
                    //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.Translate(newVertex);
                    break;
                case "U": // go upward
                    turtlePosition += Vector3.up * (float)item.p[0] * 2.0f;
                    break;
                case "Gr":
                    break;
                default:
                    break;
            }
        }

        SetMesh(rings, turtlePosition);

        //// print it out as a string
        //string outputString = "";
        //foreach (var item in inputString)
        //{
        //    switch (item.symbol)
        //    {
        //        //case 'V':
        //        //    outputString = outputString + "V(" + item.parameter[0].ToString() + ", " + item.parameter[1].ToString() + ") ";
        //        //    break;
        //        default:
        //            outputString = outputString + item.symbol + " ";
        //            break;
        //    }
        //}
        //Debug.Log(outputString.Trim());
    }

    void SetMesh(List<List<Vector3>> rings, Vector3 point)
    {
        mesh.Clear();
        vertices.Clear();
        normals.Clear();
        uvs.Clear();
        triangles.Clear();

        Vector3 p0, p1, p2, p3;

        int ringVerNum = rings[0].Count; // total number of vertices in a ring
        for (int i = 0; i < rings.Count; i++)
        {
            if (i < rings.Count - 1) // between rings
            {
                for (int j = 0; j < ringVerNum; j++)
                {
                    p0 = rings[i][j];
                    p1 = rings[i][(j + 1) % ringVerNum];
                    p2 = rings[i + 1][j];
                    p3 = rings[i + 1][(j + 1) % ringVerNum];
                    DrawQuadrangle(p0, p1, p2, p3, i);
                }
            }
            else // point
            {
                for (int j = 0; j < ringVerNum; j++)
                {
                    p0 = rings[i][j];
                    p1 = rings[i][(j + 1) % ringVerNum];
                    p2 = point;
                    DrawTriangle(p0, p1, p2);
                }
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void DrawTriangle(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        Vector3 normal = Vector3.Cross(p2 - p0, p1 - p0).normalized;
        int triOffset = vertices.Count;

        vertices.Add(p0);
        normals.Add(normal);
        vertices.Add(p1);
        normals.Add(normal);
        vertices.Add(p2);
        normals.Add(normal);
        triangles.Add(triOffset);
        triangles.Add(triOffset + 2);
        triangles.Add(triOffset + 1);

        uvs.Add(new Vector2(0.5f, 0.75f));
        uvs.Add(new Vector2(0.5f, 0.75f));
        uvs.Add(new Vector2(0.5f, 0.75f));
    }

    void DrawQuadrangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int i)
    {
        Vector3 normal = Vector3.Cross(p2 - p0, p1 - p0).normalized;
        int triOffset = vertices.Count;

        vertices.Add(p0);
        normals.Add(normal);
        vertices.Add(p1);
        normals.Add(normal);
        vertices.Add(p2);
        normals.Add(normal);
        vertices.Add(p3);
        normals.Add(normal);
        triangles.Add(triOffset);
        triangles.Add(triOffset + 2);
        triangles.Add(triOffset + 1);
        triangles.Add(triOffset + 1);
        triangles.Add(triOffset + 2);
        triangles.Add(triOffset + 3);

        if (i < 11)
        {
            uvs.Add(new Vector2(0.5f, 0.125f));
            uvs.Add(new Vector2(0.5f, 0.125f));
            uvs.Add(new Vector2(0.5f, 0.125f));
            uvs.Add(new Vector2(0.5f, 0.125f));
        }
        else if (i < 14)
        {
            uvs.Add(new Vector2(0.5f, 0.375f));
            uvs.Add(new Vector2(0.5f, 0.375f));
            uvs.Add(new Vector2(0.5f, 0.375f));
            uvs.Add(new Vector2(0.5f, 0.375f));
        }
        else
        {
            uvs.Add(new Vector2(0.5f, 0.75f));
            uvs.Add(new Vector2(0.5f, 0.75f));
            uvs.Add(new Vector2(0.5f, 0.75f));
            uvs.Add(new Vector2(0.5f, 0.75f));
        }
    }

    float GeometricSeries(float init, float ratio, float n)
    {
        return init * (1 - Mathf.Pow(ratio, n)) / (1 - ratio);
    }
}
