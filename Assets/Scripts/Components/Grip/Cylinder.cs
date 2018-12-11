using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Cylinder : BasicMesh
{
    private Turtle turtle;

    // l : length, r : radius, s : subdivision
    public override void CreateMesh(Turtle turtle)
    {
        float l = (myUI.parameters["gripLength"] as Slider).value;
        float r = (myUI.parameters["gripWidth"] as Slider).value;
        int s = (int)(myUI.parameters["circleSubdivision"] as Slider).value;

        meshRenderer.materials = new Material[] {
            Resources.Load<Material>("Materials/Knife/Grip")
        };
        List<List<List<Vertex>>> submeshes = new List<List<List<Vertex>>>();
        List<List<Vertex>> rings = new List<List<Vertex>>();
        List<Vertex> ring = new List<Vertex>();
        Vertex newVertex;
        List<Vector3> startPoints = new List<Vector3>();
        List<Vector3> endPoints = new List<Vector3>();

        startPoints.Add(turtle.p);
        endPoints.Add(turtle.p + turtle.f * l);

        ring = new List<Vertex>();
        for (int i = 0; i < s; i++)
        {
            newVertex = new Vertex();
            newVertex.pos = (Quaternion.AngleAxis(360.0f * i / s, -turtle.f) * turtle.r) * r + turtle.p;
            newVertex.uv = new Vector2(newVertex.pos.x / 10.0f, newVertex.pos.z / 10.0f);
            ring.Add(newVertex);
        }
        rings.Add(ring);
        turtle.Go(turtle.f, l);
        ring = new List<Vertex>();
        for (int i = 0; i < s; i++)
        {
            newVertex = new Vertex();
            newVertex.pos = (Quaternion.AngleAxis(360.0f * i / s, -turtle.f) * turtle.r) * r + turtle.p;
            newVertex.uv = new Vector2((Mathf.PI * r) * i / s * uvScale, newVertex.pos.z * uvScale);
            ring.Add(newVertex);
        }
        rings.Add(ring);

        submeshes.Add(rings);
        SetMesh(submeshes, startPoints, endPoints);
    }
}
