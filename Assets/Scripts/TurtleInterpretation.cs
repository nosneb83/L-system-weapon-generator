using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurtleInterpretation : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter filter;
    public new MeshRenderer renderer;

    private Stack<Turtle> turtles;
    private Turtle turtle; // current turtle
    private List<Vector3> vertices;
    private List<Vector3> normals;
    private List<Vector2> uvs;
    private List<List<int>> triangles;
    private List<int> triangleSubmesh;

    void Start()
    {
        mesh = new Mesh();
        filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;
        renderer = gameObject.AddComponent<MeshRenderer>();

        Material bladeMaterial = new Material(Shader.Find("Standard (Roughness setup)"));
        bladeMaterial.SetFloat("_Metallic", 1.0f);
        bladeMaterial.SetFloat("_Glossiness", 0.0f);
        renderer.materials = new Material[] {
            Resources.Load<Material>("Materials/Knife/Pommel")
            ,Resources.Load<Material>("Materials/Knife/Grip")
            ,Resources.Load<Material>("Materials/Knife/Guard")
            ,Resources.Load<Material>("Materials/Knife/Blade")
            ,Resources.Load<Material>("Materials/Knife/Blade")
            ,Resources.Load<Material>("Materials/Knife/Blade")
            ,Resources.Load<Material>("Materials/Knife/Blade")
            ,Resources.Load<Material>("Materials/Knife/Blade")
            ,Resources.Load<Material>("Materials/Knife/Blade")
            ,Resources.Load<Material>("Materials/Knife/Blade")
        };

        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        uvs = new List<Vector2>();
        triangles = new List<List<int>>();
        turtles = new Stack<Turtle>();
    }

    public void Interpret(List<Symbol> inputString)
    {
        turtles.Clear();
        turtle = new Turtle(Vector3.zero, Vector3.forward, Vector3.up, Vector3.right);
        List<List<List<Vertex>>> submeshes = new List<List<List<Vertex>>>();
        List<List<Vertex>> rings = new List<List<Vertex>>();
        List<Vertex> ring = new List<Vertex>();
        List<Vector3> startPoints = new List<Vector3>();
        List<Vector3> endPoints = new List<Vector3>();
        Vertex newVertex;

        foreach (var sy in inputString)
        {
            switch (sy.s)
            {
                case "{": // start a submesh
                    rings = new List<List<Vertex>>();
                    startPoints.Add(turtle.p);
                    break;
                case "}": // end a submesh
                    submeshes.Add(rings);
                    endPoints.Add(turtle.p);
                    break;
                case "[": // start a ring
                    ring = new List<Vertex>();
                    break;
                case "]": // end a ring
                    rings.Add(ring);
                    break;
                case "push": // push current turtle from stack
                    turtles.Push(turtle);
                    turtle = new Turtle(turtle);
                    break;
                case "pop": // pop current turtle from stack
                    turtle = turtles.Pop();
                    break;
                case "V":
                    newVertex.pos = (Quaternion.AngleAxis((float)sy.p[1], -turtle.f) * turtle.r) * (float)sy.p[2] + turtle.p;
                    if (sy.p[3].GetType() == typeof(Vector2)) newVertex.uv = (Vector2)sy.p[3];
                    else newVertex.uv = new Vector2(newVertex.pos.x / 10.0f, newVertex.pos.z / 10.0f);
                    ring.Add(newVertex);
                    //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.Translate(newVertex);
                    break;
                case "U": // go upward
                    turtle.p += turtle.u * (float)sy.p[0];
                    break;
                case "F": // go forward
                    turtle.p += turtle.f * (float)sy.p[0];
                    break;
                case "rf": // rotation around forward vector
                    //turtle.u = Quaternion.AngleAxis((float)sy.p[0], turtle.f) * turtle.u;
                    //turtle.r = Quaternion.AngleAxis((float)sy.p[0], turtle.f) * turtle.r;
                    turtle.RotateAroundForward((float)sy.p[0]);
                    break;
                case "ru": // rotation around up vector
                    //turtle.f = Quaternion.AngleAxis((float)sy.p[0], turtle.u) * turtle.f;
                    //turtle.r = Quaternion.AngleAxis((float)sy.p[0], turtle.u) * turtle.r;
                    turtle.RotateAroundUp((float)sy.p[0]);
                    break;
                case "rr": // rotation around right vector
                    //turtle.f = Quaternion.AngleAxis((float)sy.p[0], turtle.r) * turtle.f;
                    //turtle.u = Quaternion.AngleAxis((float)sy.p[0], turtle.r) * turtle.u;
                    turtle.RotateAroundRight((float)sy.p[0]);
                    break;
                case "crescent": // l, w, d(0~1), t, edgeRatio, subdivision
                    // push turtle
                    turtles.Push(turtle);
                    turtle = new Turtle(turtle);

                    // end points
                    startPoints.Add(turtle.p + turtle.f * (float)sy.p[0] + turtle.r * (float)sy.p[1]);
                    endPoints.Add(turtle.p + turtle.f * (float)sy.p[0] - turtle.r * (float)sy.p[1]);

                    // crescent submesh
                    submeshes.Add(Crescent((float)sy.p[0], (float)sy.p[1], (float)sy.p[2], (float)sy.p[3], (float)sy.p[4], (int)sy.p[5]));

                    // pop turtle
                    turtle = turtles.Pop();

                    break;

                case "axe": // length, width, thick, curve, subdivision
                    startPoints.Add(turtle.p);
                    endPoints.Add(turtle.p + turtle.f * (float)sy.p[0]);

                    // push turtle
                    turtles.Push(turtle);
                    turtle = new Turtle(turtle);

                    // axe
                    submeshes.Add(Axe((float)sy.p[0], (float)sy.p[1], (float)sy.p[2], (float)sy.p[3], (int)sy.p[4]));

                    // pop turtle
                    turtle = turtles.Pop();

                    break;

                case "fork": // length, width, thick, curve, subdivision
                    startPoints.Add(turtle.p);
                    endPoints.Add(turtle.p + turtle.f * (float)sy.p[0]);

                    // push turtle
                    turtles.Push(turtle);
                    turtle = new Turtle(turtle);

                    // axe
                    submeshes.Add(Axe((float)sy.p[0], (float)sy.p[1], (float)sy.p[2], (float)sy.p[3], (int)sy.p[4]));

                    // pop turtle
                    turtle = turtles.Pop();

                    break;

                case "*": // test sphere
                    GameObject testSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    testSphere.transform.localScale = new Vector3((float)sy.p[0], (float)sy.p[0], (float)sy.p[0]);
                    testSphere.transform.Translate(turtle.p);
                    break;
                default:
                    break;
            }
        }

        SetMesh(submeshes, startPoints, endPoints);
        //PrintInputString(inputString);
    }

    void SetMesh(List<List<List<Vertex>>> submeshes, List<Vector3> startPoints, List<Vector3> endPoints)
    {
        mesh.Clear();
        vertices.Clear();
        normals.Clear();
        uvs.Clear();
        triangles.Clear();

        Vertex v0, v1, v2, v3;

        for (int i = 0; i < submeshes.Count; i++)
        {
            int ringVerNum = submeshes[i][0].Count; // total number of vertices in a ring
            triangleSubmesh = new List<int>();
            for (int j = 0; j < submeshes[i].Count; j++)
            {
                if (j == 0) // start point
                {
                    for (int k = 0; k < ringVerNum; k++)
                    {
                        v0 = submeshes[i][j][(k + 1) % ringVerNum];
                        v1 = submeshes[i][j][k];
                        v2.pos = startPoints[i];
                        DrawTriangle(v0.pos, v1.pos, v2.pos);
                    }
                }
                if (j < submeshes[i].Count - 1) // between rings
                {
                    for (int k = 0; k < ringVerNum; k++)
                    {
                        v0 = submeshes[i][j][k];
                        v1 = submeshes[i][j][(k + 1) % ringVerNum];
                        v2 = submeshes[i][j + 1][k];
                        v3 = submeshes[i][j + 1][(k + 1) % ringVerNum];
                        //DrawQuadrangleInterpolated(v0.pos, v1.pos, v2.pos, v3.pos, v0.tex, v1.tex, v2.tex, v3.tex);
                        DrawQuadrangleUV(v0, v1, v2, v3);
                    }
                }
                else // end point
                {
                    for (int k = 0; k < ringVerNum; k++)
                    {
                        v0 = submeshes[i][j][k];
                        v1 = submeshes[i][j][(k + 1) % ringVerNum];
                        v2.pos = endPoints[i];
                        DrawTriangle(v0.pos, v1.pos, v2.pos);
                    }
                }
            }
            triangles.Add(triangleSubmesh);
        }

        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.subMeshCount = triangles.Count;
        for (int i = 0; i < triangles.Count; i++)
        {
            mesh.SetTriangles(triangles[i], i);
        }

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
        triangleSubmesh.Add(triOffset);
        triangleSubmesh.Add(triOffset + 2);
        triangleSubmesh.Add(triOffset + 1);

        uvs.Add(GetUV(2, p0));
        uvs.Add(GetUV(2, p1));
        uvs.Add(GetUV(2, p2));
    }

    void DrawQuadrangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int t0, int t1, int t2, int t3)
    {
        Vector3 normal = Vector3.Cross(p2 - p0, p1 - p0).normalized;
        int triOffset = vertices.Count;

        vertices.Add(p0);
        vertices.Add(p1);
        vertices.Add(p2);
        vertices.Add(p3);

        normals.Add(normal);
        normals.Add(normal);
        normals.Add(normal);
        normals.Add(normal);

        //uvs.Add(GetUV(t0, p0));
        //uvs.Add(GetUV(t1, p1));
        //uvs.Add(GetUV(t2, p2));
        //uvs.Add(GetUV(t3, p3));
        uvs.Add(new Vector2(p0.x / 10.0f, p0.z / 10.0f));
        uvs.Add(new Vector2(p1.x / 10.0f, p1.z / 10.0f));
        uvs.Add(new Vector2(p2.x / 10.0f, p2.z / 10.0f));
        uvs.Add(new Vector2(p3.x / 10.0f, p3.z / 10.0f));

        triangleSubmesh.Add(triOffset);
        triangleSubmesh.Add(triOffset + 2);
        triangleSubmesh.Add(triOffset + 1);
        triangleSubmesh.Add(triOffset + 1);
        triangleSubmesh.Add(triOffset + 2);
        triangleSubmesh.Add(triOffset + 3);
    }

    void DrawQuadrangleInterpolated(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int t0, int t1, int t2, int t3)
    {
        Vector3 pc = (p0 + p1 + p2 + p3) / 4; // center

        Vector3 normal01 = Vector3.Cross(pc - p1, pc - p0).normalized;
        Vector3 normal13 = Vector3.Cross(pc - p3, pc - p1).normalized;
        Vector3 normal32 = Vector3.Cross(pc - p2, pc - p3).normalized;
        Vector3 normal20 = Vector3.Cross(pc - p0, pc - p2).normalized;
        int triOffset = vertices.Count;

        vertices.Add(p0);
        vertices.Add(p1);
        vertices.Add(p2);
        vertices.Add(p3);
        vertices.Add(pc);

        normals.Add((normal01 + normal20) / 2);
        normals.Add((normal01 + normal13) / 2);
        normals.Add((normal32 + normal20) / 2);
        normals.Add((normal13 + normal32) / 2);
        normals.Add((normal01 + normal13 + normal32 + normal20) / 4);

        //uvs.Add(GetUV(t0, p0));
        //uvs.Add(GetUV(t1, p1));
        //uvs.Add(GetUV(t2, p2));
        //uvs.Add(GetUV(t3, p3));
        uvs.Add(new Vector2(p0.x / 10.0f, p0.z / 10.0f));
        uvs.Add(new Vector2(p1.x / 10.0f, p1.z / 10.0f));
        uvs.Add(new Vector2(p2.x / 10.0f, p2.z / 10.0f));
        uvs.Add(new Vector2(p3.x / 10.0f, p3.z / 10.0f));
        uvs.Add(new Vector2(pc.x / 10.0f, pc.z / 10.0f));

        triangleSubmesh.Add(triOffset);
        triangleSubmesh.Add(triOffset + 4);
        triangleSubmesh.Add(triOffset + 1);
        triangleSubmesh.Add(triOffset + 1);
        triangleSubmesh.Add(triOffset + 4);
        triangleSubmesh.Add(triOffset + 3);
        triangleSubmesh.Add(triOffset + 3);
        triangleSubmesh.Add(triOffset + 4);
        triangleSubmesh.Add(triOffset + 2);
        triangleSubmesh.Add(triOffset + 2);
        triangleSubmesh.Add(triOffset + 4);
        triangleSubmesh.Add(triOffset + 0);
    }

    void DrawQuadrangleUV(Vertex v0, Vertex v1, Vertex v2, Vertex v3)
    {
        Vector3 p0 = v0.pos;
        Vector3 p1 = v1.pos;
        Vector3 p2 = v2.pos;
        Vector3 p3 = v3.pos;
        Vector3 pc = (p0 + p1 + p2 + p3) / 4; // center

        Vector3 normal01 = Vector3.Cross(pc - p1, pc - p0).normalized;
        Vector3 normal13 = Vector3.Cross(pc - p3, pc - p1).normalized;
        Vector3 normal32 = Vector3.Cross(pc - p2, pc - p3).normalized;
        Vector3 normal20 = Vector3.Cross(pc - p0, pc - p2).normalized;
        int triOffset = vertices.Count;

        vertices.Add(p0);
        vertices.Add(p1);
        vertices.Add(p2);
        vertices.Add(p3);
        vertices.Add(pc);

        normals.Add((normal01 + normal20) / 2);
        normals.Add((normal01 + normal13) / 2);
        normals.Add((normal32 + normal20) / 2);
        normals.Add((normal13 + normal32) / 2);
        normals.Add((normal01 + normal13 + normal32 + normal20) / 4);

        uvs.Add(v0.uv);
        uvs.Add(v1.uv);
        uvs.Add(v2.uv);
        uvs.Add(v3.uv);
        uvs.Add((v0.uv + v1.uv + v2.uv + v3.uv) / 4);

        triangleSubmesh.Add(triOffset);
        triangleSubmesh.Add(triOffset + 4);
        triangleSubmesh.Add(triOffset + 1);
        triangleSubmesh.Add(triOffset + 1);
        triangleSubmesh.Add(triOffset + 4);
        triangleSubmesh.Add(triOffset + 3);
        triangleSubmesh.Add(triOffset + 3);
        triangleSubmesh.Add(triOffset + 4);
        triangleSubmesh.Add(triOffset + 2);
        triangleSubmesh.Add(triOffset + 2);
        triangleSubmesh.Add(triOffset + 4);
        triangleSubmesh.Add(triOffset + 0);
    }

    public List<Vector3> GetArcPoints(Turtle turtle, float l, float w, int subdivision)
    {
        List<Vector3> curvePoints = new List<Vector3>();

        float R = (l * l + w * w) / (2.0f * l);
        float A = (l < w) ? 180.0f - Mathf.Asin(w / R) * 180 / Mathf.PI : Mathf.Asin(w / R) * 180 / Mathf.PI;
        float C = 2.0f * (180.0f - A);
        //Debug.Log("l = " + l.ToString());
        //Debug.Log("w = " + w.ToString());
        //Debug.Log("R = " + R.ToString());
        //Debug.Log("A = " + A.ToString());
        turtle.p += turtle.f * R;
        turtle.f = Quaternion.AngleAxis((360.0f - C) / 2.0f, turtle.u) * turtle.f;
        turtle.r = Quaternion.AngleAxis((360.0f - C) / 2.0f, turtle.u) * turtle.r;

        turtle.p += turtle.f * R;
        curvePoints.Add(turtle.p);
        turtle.p -= turtle.f * R;
        for (int i = 0; i < subdivision; i++)
        {
            turtle.f = Quaternion.AngleAxis(C / subdivision, turtle.u) * turtle.f;
            turtle.r = Quaternion.AngleAxis(C / subdivision, turtle.u) * turtle.r;
            turtle.p += turtle.f * R;
            curvePoints.Add(turtle.p);
            turtle.p -= turtle.f * R;
        }

        return curvePoints;
    }

    public List<Vector3> GetCurvePoints(Turtle turtle, float l, float t, float c, int s)
    {
        List<Vector3> curvePoints = new List<Vector3>();

        for (int i = 0; i <= s; i++)
        {
            curvePoints.Add(turtle.p + turtle.u * t / 2 * (s - i) / s);
            turtle.Go(turtle.f, l / s);
            turtle.RotateAroundUp(c);
        }

        return curvePoints;
    }

    private List<List<Vertex>> Crescent(float l, float w, float d, float thick, float edgeRatio, int subdivision)
    {
        List<List<Vertex>> rings = new List<List<Vertex>>();

        List<Vector3> outerArc = GetArcPoints(new Turtle(turtle), l, w, subdivision);
        turtle.p += turtle.f * l * d;
        List<Vector3> innerArc = GetArcPoints(new Turtle(turtle), l * (1.0f - d), w, subdivision);
        //foreach (var item in outerArc)
        //{
        //    GameObject o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    o.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //    o.transform.Translate(item);
        //}
        //foreach (var item in innerArc)
        //{
        //    GameObject o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    o.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //    o.transform.Translate(item);
        //}
        Vertex newVertex;
        for (int i = 0; i <= subdivision; i++)
        {
            List<Vertex> ring = new List<Vertex>();

            Vector3 t = turtle.u * thick / 2.0f * Mathf.Sin(i / (float)subdivision * Mathf.PI);

            newVertex.pos = outerArc[i] + t;
            newVertex.uv = new Vector2(newVertex.pos.x / 10.0f, newVertex.pos.z / 10.0f);
            ring.Add(newVertex);

            newVertex.pos = outerArc[i] * edgeRatio + innerArc[i] * (1.0f - edgeRatio) + t;
            newVertex.uv = new Vector2(newVertex.pos.x / 10.0f, newVertex.pos.z / 10.0f);
            ring.Add(newVertex);

            newVertex.pos = innerArc[i];
            newVertex.uv = new Vector2(newVertex.pos.x / 10.0f, newVertex.pos.z / 10.0f);
            ring.Add(newVertex);

            newVertex.pos = outerArc[i] * edgeRatio + innerArc[i] * (1.0f - edgeRatio) - t;
            newVertex.uv = new Vector2(newVertex.pos.x / 10.0f, newVertex.pos.z / 10.0f);
            ring.Add(newVertex);

            newVertex.pos = outerArc[i] - t;
            newVertex.uv = new Vector2(newVertex.pos.x / 10.0f, newVertex.pos.z / 10.0f);
            ring.Add(newVertex);

            rings.Add(ring);
        }

        return rings;
    }

    private List<List<Vertex>> Axe(float l, float w, float t, float c, int s)
    {
        List<List<Vertex>> rings = new List<List<Vertex>>();

        List<List<Vector3>> curves = new List<List<Vector3>>();
        List<Turtle> axeTurtles = new List<Turtle>();
        for (int i = 0; i <= s; i++)
        {
            Turtle newTurtle = new Turtle(turtle);
            newTurtle.Go(-newTurtle.r, w / 2.0f);
            newTurtle.Go(newTurtle.r, w * i / s);
            axeTurtles.Add(newTurtle);
        }

        for (int i = 0; i <= s; i++)
        {
            curves.Add(GetCurvePoints(new Turtle(axeTurtles[i]), l, t, -c + (2 * c) * i / s, s));
        }
        axeTurtles.Reverse();
        for (int i = 0; i <= s; i++)
        {
            curves.Add(GetCurvePoints(new Turtle(axeTurtles[i]), l, -t, c - (2 * c) * i / s, s));
        }

        List<Vertex> ring;
        Vertex newVertex;
        for (int i = 0; i <= s; i++)
        {
            ring = new List<Vertex>();
            for (int j = 0; j < curves.Count; j++)
            {
                newVertex = new Vertex();
                newVertex.pos = curves[j][i];
                newVertex.uv = new Vector2(newVertex.pos.x / 10.0f, newVertex.pos.z / 10.0f);
                ring.Add(newVertex);
            }
            rings.Add(ring);
        }

        return rings;
    }

    Vector2 GetUV(int tex, Vector3 p)
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