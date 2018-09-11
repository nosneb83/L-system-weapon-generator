using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turtle : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter filter;
    public new MeshRenderer renderer;

    private List<Vector3> vertices;
    private List<Vector3> normals;
    private List<Vector2> uvs;
    private List<int> triangles;

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

    public void Interpret(List<Symbol> inputString)
    {
        Vector3 turtlePosition = Vector3.zero;
        Vector3 turtleForword = Vector3.forward;
        Vector3 turtleUp = Vector3.up;
        Vector3 turtleRight = Vector3.right;
        List<List<Vertex>> rings = new List<List<Vertex>>();
        List<Vertex> ring = new List<Vertex>();

        Vector3 startPoint = turtlePosition;
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

        SetMesh(rings, startPoint, turtlePosition);
    }

    void SetMesh(List<List<Vertex>> rings, Vector3 startPoint, Vector3 endPoint)
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
            if (i == 0 && startPoint != null) // start point
            {
                for (int j = 0; j < ringVerNum; j++)
                {
                    v0 = rings[i][(j + 1) % ringVerNum];
                    v1 = rings[i][j];
                    v2.pos = startPoint;
                    DrawTriangle(v0.pos, v1.pos, v2.pos);
                }
            }
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
            else if (endPoint != null) // end point
            {
                for (int j = 0; j < ringVerNum; j++)
                {
                    v0 = rings[i][j];
                    v1 = rings[i][(j + 1) % ringVerNum];
                    v2.pos = endPoint;
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

    void PrintInputString(List<Symbol> inputString)
    {
        // print it out as a string
        string outputString = "";
        foreach (var item in inputString)
        {
            switch (item.s)
            {
                //case 'V':
                //    outputString = outputString + "V(" + item.parameter[0].ToString() + ", " + item.parameter[1].ToString() + ") ";
                //    break;
                default:
                    outputString = outputString + item.s + " ";
                    break;
            }
        }
        Debug.Log(outputString.Trim());
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