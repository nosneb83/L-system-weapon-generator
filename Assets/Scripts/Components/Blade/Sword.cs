using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sword : MonoBehaviour
{
    private GameObject _obj;

    private Mesh mesh;
    private MeshFilter filter;
    public new MeshRenderer renderer;
    private List<Vector3> vertices;
    private List<Vector3> normals;
    private List<Vector2> uvs;
    private List<List<int>> triangles;

    public GameObject GetObj()
    {
        return _obj;
    }

    // Use this for initialization
    void Start()
    {
        mesh = new Mesh();
        filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;
        renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.materials = new Material[] {
            Resources.Load<Material>("Materials/Knife/Blade")
        };

        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        uvs = new List<Vector2>();
        triangles = new List<List<int>>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
