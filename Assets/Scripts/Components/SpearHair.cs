using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpearHair : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter filter;
    private MeshRenderer meshRenderer;

    private List<Vector3> vertices;
    private List<Vector3> normals;
    private List<Vector2> uvs;
    private List<int> triangles;

    // Use this for initialization
    void Awake()
    {
        //mesh = new Mesh();
        //filter = gameObject.AddComponent<MeshFilter>();
        //filter.mesh = mesh;
        //meshRenderer = gameObject.AddComponent<MeshRenderer>();

        //Material bladeMaterial = new Material(Shader.Find("Standard"));
        //meshRenderer.material = Resources.Load<Material>("Materials/Red");

        //vertices = new List<Vector3>();
        //normals = new List<Vector3>();
        //uvs = new List<Vector2>();
        //triangles = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMesh()
    {
        mesh.Clear();
        vertices.Clear();
        normals.Clear();
        uvs.Clear();
        triangles.Clear();

        Vector3 p0, p1, p2, p3;

        for (int i = 0; i < 20; i++)
        {
            p0 = new Vector3(0.0f, 0.03f * i, 0);
            p1 = new Vector3(0.005f, 0.03f * i, 0);
            p2 = new Vector3(0.0f, 0.03f * (i + 1), 0);
            p3 = new Vector3(0.005f, 0.03f * (i + 1), 0);
            DrawQuadrangle(p0, p1, p2, p3);
        }

        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void DrawQuadrangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
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

        uvs.Add(new Vector2(0.0f, 0.0f));
        uvs.Add(new Vector2(0.0f, 0.0f));
        uvs.Add(new Vector2(0.0f, 0.0f));
        uvs.Add(new Vector2(0.0f, 0.0f));
    }
}
