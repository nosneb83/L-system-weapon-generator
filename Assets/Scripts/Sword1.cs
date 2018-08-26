using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sword1 : MonoBehaviour
{
    // L-system String Interpreter for Sword 1

    private Mesh mesh;
    private MeshFilter filter;
    public new MeshRenderer renderer;

    // Use this for initialization
    void Start()
    {
        mesh = new Mesh();
        filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;
        renderer = gameObject.AddComponent<MeshRenderer>();

        Material bladeMaterial = new Material(Shader.Find("Standard"));
        renderer.material = bladeMaterial;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMesh()
    {
        mesh.Clear();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector3> normals = new List<Vector3>();

        vertices.Add(new Vector3(0, 0, 0));
        vertices.Add(new Vector3(0, 1, 0));
        vertices.Add(new Vector3(1, 0, 0));
        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(1);

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetNormals(normals);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
