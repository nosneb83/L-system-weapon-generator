using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragonHead : BasicMesh
{
    public override void CreateMesh(Turtle turtle)
    {
        meshRenderer.materials = new Material[] {
            Resources.Load<Material>("Materials/Knife/Guard"),
            Resources.Load<Material>("Materials/Knife/Guard"),
            Resources.Load<Material>("Materials/Knife/Guard"),
            Resources.Load<Material>("Materials/Knife/Guard"),
            Resources.Load<Material>("Materials/Knife/Guard"),
            Resources.Load<Material>("Materials/Knife/Guard"),
            Resources.Load<Material>("Materials/Knife/Guard"),
            Resources.Load<Material>("Materials/Knife/Guard"),
            Resources.Load<Material>("Materials/Knife/Guard"),
            Resources.Load<Material>("Materials/Knife/Guard")
        };

        Mesh dragonFBX = Resources.Load<Mesh>("Models/dragon_fbx/HQSD493DNHDO7UIH8UQNVBJFM");
        mesh.Clear();
        vertices.Clear();
        normals.Clear();
        uvs.Clear();
        triangles.Clear();

        float vScale = 18.0f;
        foreach (var item in dragonFBX.vertices)
        {
            Vector3 newV = new Vector3(item.x * vScale, item.y * vScale, item.z * vScale);
            newV = Quaternion.FromToRotation(Vector3.right, turtle.f) * newV;
            newV = Quaternion.AngleAxis(180.0f, turtle.f) * newV;
            newV = Quaternion.AngleAxis(45.0f, turtle.u) * newV;
            newV += turtle.r * -0.03f;
            newV += turtle.f * 0.02f;
            vertices.Add(turtle.p + newV);
        }
        mesh.SetVertices(vertices);

        foreach (var item in dragonFBX.normals)
        {
            Vector3 newN = new Vector3(item.x, item.y, item.z);
            normals.Add(newN);
        }
        mesh.SetNormals(normals);

        foreach (var item in dragonFBX.uv)
        {
            Vector2 newUV = new Vector2(item.x, item.y);
            uvs.Add(newUV);
        }
        mesh.SetUVs(0, uvs);

        mesh.subMeshCount = dragonFBX.subMeshCount;
        for (int i = 0; i < dragonFBX.subMeshCount; i++)
        {
            List<int> triangleSubmesh = new List<int>();
            foreach (var item in dragonFBX.GetTriangles(i))
            {
                triangleSubmesh.Add(item);
            }
            triangles.Add(triangleSubmesh);
            mesh.SetTriangles(triangles[i], i);
        }
    }
}
