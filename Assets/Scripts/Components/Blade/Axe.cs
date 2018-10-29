using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Axe : BasicMesh
{
    private Turtle turtle;

    // parameters
    public float L;
    public float W;
    public float D;
    public float bladeCurv;
    public float circleSubdivision;

    // l : length, w : width, t : thick, c : curve angle, s : subdivision
    public void CreateAxe(Turtle turtle, float l, float w, float t, float c, int s)
    {
        renderer.materials = new Material[] {
            Resources.Load<Material>("Materials/Knife/Blade")
        };
        List<List<List<Vertex>>> submeshes = new List<List<List<Vertex>>>();
        List<List<Vertex>> rings = new List<List<Vertex>>();
        List<Vertex> ring = new List<Vertex>();
        Vertex newVertex;
        List<Vector3> startPoints = new List<Vector3>();
        List<Vector3> endPoints = new List<Vector3>();

        startPoints.Add(turtle.p);
        endPoints.Add(turtle.p + turtle.f * l);

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

        submeshes.Add(rings);
        SetMesh(submeshes, startPoints, endPoints);
    }
}
