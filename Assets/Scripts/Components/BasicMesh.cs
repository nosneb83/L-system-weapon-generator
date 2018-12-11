using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class BasicMesh : MonoBehaviour
{
    protected Mesh mesh;
    protected MeshFilter filter;
    protected MeshRenderer meshRenderer;
    protected List<Vector3> vertices;
    protected List<Vector3> normals;
    protected List<Vector2> uvs;
    protected List<List<int>> triangles;
    protected List<int> triangleSubmesh;

    protected UIManager myUI;

    // for line
    protected Material lineMat;
    public List<List<Vector3>> linePoints;

    public float uvScale = 1.5f;

    // Use this for initialization
    void Awake()
    {
        mesh = new Mesh();
        filter = gameObject.GetComponent<MeshFilter>();
        if (filter == null) filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;

        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null) meshRenderer = gameObject.AddComponent<MeshRenderer>();
        //renderer.materials = new Material[] {
        //    Resources.Load<Material>("Materials/Knife/Grip"),
        //    Resources.Load<Material>("Materials/Knife/Blade")
        //};

        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        uvs = new List<Vector2>();
        triangles = new List<List<int>>();

        myUI = FindObjectOfType<UIManager>();
        //Debug.Log("myUI");

        lineMat = new Material(Shader.Find("Sprites/Default"));
        linePoints = new List<List<Vector3>>();
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (var item in linePoints)
        //{
        //    Debug.DrawLine(item[0] + transform.position, item[1] + transform.position);
        //}
    }

    public virtual void CreateMesh(Turtle turtle)
    {

    }

    protected void SetMesh(List<List<List<Vertex>>> submeshes, List<Vector3> startPoints, List<Vector3> endPoints)
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
                        //if (!(bool)myUI.parameters["showLineIsOn"][0])
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
                        //if (!(bool)myUI.parameters["showLineIsOn"][0])
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
                        //if (!(bool)myUI.parameters["showLineIsOn"][0])
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
            //if (!(bool)myUI.parameters["showLineIsOn"][0])
                mesh.SetTriangles(triangles[i], i);
        }

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
        triangleSubmesh.Add(triOffset);
        triangleSubmesh.Add(triOffset + 2);
        triangleSubmesh.Add(triOffset + 1);

        uvs.Add(new Vector2(p0.x / 10.0f, p0.z / 10.0f));
        uvs.Add(new Vector2(p1.x / 10.0f, p1.z / 10.0f));
        uvs.Add(new Vector2(p2.x / 10.0f, p2.z / 10.0f));
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
            //if ((bool)myUI.parameters["showLineIsOn"][0])
                linePoints.Add(new List<Vector3>() { turtle.p, turtle.p + turtle.u * t / 2 * (s - i) / s });
            //turtle.Go(turtle.f, l / s);
            turtle = MoveTurtle(turtle, turtle.f, l / s);
            turtle.RotateAroundUp(c);
        }

        return curvePoints;
    }

    protected Turtle MoveTurtle(Turtle turtle, Vector3 dir, float dis)
    {
        Vector3 lineStart, lineEnd;
        lineStart = turtle.p;
        turtle.Go(dir, dis);
        lineEnd = turtle.p;

        // draw (GL)
        //if ((bool)myUI.parameters["showLineIsOn"][0])
        //{
        //    GL.PushMatrix();

        //    lineMat.SetPass(0);
        //    GL.LoadIdentity();
        //    //GL.MultMatrix(Camera.main.worldToCameraMatrix);

        //    GL.Begin(GL.LINES);
        //    GL.Color(Color.red);
        //    GL.Vertex(Vector3.zero);
        //    GL.Vertex(Vector3.one);
        //    GL.End();

        //    GL.PopMatrix();

        //    //Debug.Log("start = " + lineStart.ToString("G4"));
        //    //Debug.Log("end   = " + lineEnd.ToString("G4"));
        //}

        // draw (line renderer)
        //LineRenderer newLine = new GameObject().AddComponent<LineRenderer>();
        //newLine.material = lineMat;

        // draw line (Debug)
        //if ((bool)myUI.parameters["showLineIsOn"][0])
            linePoints.Add(new List<Vector3>() { lineStart, lineEnd });

        return turtle;
    }

    void OnPostRender()
    {
        GL.PushMatrix();

        lineMat.SetPass(0);
        GL.LoadIdentity();
        //GL.MultMatrix(Camera.main.worldToCameraMatrix);

        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex(Vector3.zero);
        GL.Vertex(Vector3.one);
        GL.End();

        GL.PopMatrix();
    }

    protected Vertex CreateVertex(Vector3 pos)
    {
        Vertex newVertex = new Vertex();
        newVertex.pos = pos;
        newVertex.uv = new Vector2(newVertex.pos.x * uvScale + 0.5f, newVertex.pos.y * uvScale);
        return newVertex;
    }

    protected Vertex CreateVertex(Vector3 pos, Vector2 uv)
    {
        Vertex newVertex = new Vertex();
        newVertex.pos = pos;
        newVertex.uv = uv;
        return newVertex;
    }
}
