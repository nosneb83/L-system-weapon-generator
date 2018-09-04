using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Knife3 : MonoBehaviour
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

        Material bladeMaterial = new Material(Shader.Find("Standard (Roughness setup)"));
        bladeMaterial.SetFloat("_Metallic", 1.0f);
        bladeMaterial.SetFloat("_Glossiness", 0.0f);
        renderer.material = bladeMaterial;

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
                    int t = (int)item.p[0];

                    float bladeWidth = 1.2f * (50 - t) / 35;
                    float bladeThick = 0.2f * (50 - t) / 35;
                    float edgeRatio = 0.15f;
                    float guardDiameter = 0.8f;

                    if (t < 11)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.4f, 0 })); // age, angle, distance ratio, texture
                        newString.Add(new Symbol("V", new object[] { 1, 45.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 135.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 180.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 225.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 270.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 315.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("]", new object[] { }));
                        newString.Add(new Symbol("F", new object[] { 1.0f }));
                    }
                    else if (t == 11)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.4f, 0 })); // age, angle, distance ratio, texture
                        newString.Add(new Symbol("V", new object[] { 1, 45.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 135.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 180.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 225.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 270.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 315.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("]", new object[] { }));
                    }
                    else if (t == 12)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 45.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 135.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 180.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 225.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 270.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 315.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("]", new object[] { }));
                        newString.Add(new Symbol("F", new object[] { 0.3f }));
                    }
                    else if (t == 13)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 45.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 135.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 180.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 225.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 270.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 315.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("]", new object[] { }));
                    }
                    else if (t == 14)
                    {
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(90.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { 0.55f }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                    }
                    else if (t == 15)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.0f, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 1 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * 0.7f }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 1 }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * 0.3f }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.0f, 1 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(180.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * 0.3f }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 1 }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * 0.7f }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 1 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                        newString.Add(new Symbol("]", new object[] { }));
                    }
                    else if (t == 16)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.0f, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * (1 - edgeRatio) }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 3 }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * edgeRatio }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.0f, 4 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(180.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * edgeRatio }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 3 }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * (1 - edgeRatio) }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                        newString.Add(new Symbol("]", new object[] { }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(0.3f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { 5.0f / Mathf.Pow(t - 15, 1) }));
                    }
                    else
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.0f, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * (1 - edgeRatio) }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 3 }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * edgeRatio }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.0f, 4 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(180.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * edgeRatio }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 3 }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * (1 - edgeRatio) }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                        newString.Add(new Symbol("]", new object[] { }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(0.3f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { 5.0f / Mathf.Pow(t - 15, 1) }));
                    }
                    item.p[0] = t + 1; // P(t) -> P(t+1)
                    newString.Add(item);
                    break;
                case "V": // vertex
                    item.p[0] = (int)item.p[0] + 1; // V(t,...) -> V(tt+1,...)
                    newString.Add(item);
                    break;
                default:
                    newString.Add(item);
                    break;
            }
        }

        return newString;
    }

    struct Vertex
    {
        public Vector3 pos;
        public int tex;
    }

    public void TurtleInterpretation(List<Symbol> inputString)
    {
        Vector3 turtlePosition = Vector3.zero;
        Vector3 turtleForword = Vector3.forward;
        Vector3 turtleUp = Vector3.up;
        Vector3 turtleRight = Vector3.right;
        List<List<Vertex>> rings = new List<List<Vertex>>();
        List<Vertex> ring = new List<Vertex>();

        foreach (var item in inputString)
        {
            switch (item.s)
            {
                case "[": // start a ring
                    ring = new List<Vertex>();
                    break;
                case "]": // end a ring
                    rings.Add(ring);
                    break;
                case "V":
                    Vertex newVertex;
                    newVertex.pos = (Quaternion.AngleAxis((float)item.p[1], -turtleForword) * turtleRight) * (float)item.p[2] + turtlePosition;
                    newVertex.tex = (int)item.p[3];
                    ring.Add(newVertex);
                    //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.Translate(newVertex);
                    break;
                case "U": // go upward
                    turtlePosition += turtleUp * (float)item.p[0];
                    break;
                case "F": // go forward
                    turtlePosition += turtleForword * (float)item.p[0];
                    break;
                case "+": // rotation
                    turtleForword = (Quaternion)item.p[0] * turtleForword;
                    turtleUp = (Quaternion)item.p[0] * turtleUp;
                    turtleRight = (Quaternion)item.p[0] * turtleRight;
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

    void SetMesh(List<List<Vertex>> rings, Vector3 point)
    {
        mesh.Clear();
        vertices.Clear();
        normals.Clear();
        uvs.Clear();
        triangles.Clear();

        Vertex v0, v1, v2, v3;

        int ringVerNum = rings[0].Count; // total number of vertices in a ring
        for (int i = 0; i < rings.Count; i++)
        {
            if (i < rings.Count - 1) // between rings
            {
                for (int j = 0; j < ringVerNum; j++)
                {
                    v0 = rings[i][j];
                    v1 = rings[i][(j + 1) % ringVerNum];
                    v2 = rings[i + 1][j];
                    v3 = rings[i + 1][(j + 1) % ringVerNum];
                    DrawQuadrangle(v0.pos, v1.pos, v2.pos, v3.pos, v0.tex, v1.tex, v2.tex, v3.tex);
                }
            }
            else // point
            {
                for (int j = 0; j < ringVerNum; j++)
                {
                    v0 = rings[i][j];
                    v1 = rings[i][(j + 1) % ringVerNum];
                    v2.pos = point;
                    DrawTriangle(v0.pos, v1.pos, v2.pos);
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

        uvs.Add(GetUV(2));
        uvs.Add(GetUV(2));
        uvs.Add(GetUV(2));
    }

    void DrawQuadrangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int t0, int t1, int t2, int t3)
    {
        Vector3 normal = Vector3.Cross(p2 - p0, p1 - p0).normalized;
        int triOffset = vertices.Count;

        vertices.Add(p0);
        uvs.Add(GetUV(t0));
        normals.Add(normal);

        vertices.Add(p1);
        uvs.Add(GetUV(t1));
        normals.Add(normal);

        vertices.Add(p2);
        uvs.Add(GetUV(t2));
        normals.Add(normal);

        vertices.Add(p3);
        uvs.Add(GetUV(t3));
        normals.Add(normal);

        triangles.Add(triOffset);
        triangles.Add(triOffset + 2);
        triangles.Add(triOffset + 1);
        triangles.Add(triOffset + 1);
        triangles.Add(triOffset + 2);
        triangles.Add(triOffset + 3);
    }

    Vector2 GetUV(int tex)
    {
        Vector2 uv;
        switch (tex)
        {
            case 0:
                uv = new Vector2(0.5f, 0.0f);
                break;
            case 1:
                uv = new Vector2(0.5f, 0.25f);
                break;
            case 2:
                uv = new Vector2(0.5f, 0.51f);
                break;
            case 3:
                uv = new Vector2(0.5f, 0.75f);
                break;
            case 4:
                uv = new Vector2(0.5f, 1.0f);
                break;
            default:
                uv = Vector2.zero;
                break;
        }
        return uv;
    }

    float GeometricSeries(float init, float ratio, float n)
    {
        return init * (1 - Mathf.Pow(ratio, n)) / (1 - ratio);
    }
}
