using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Crescent : BasicMesh
{
    public override void CreateMesh(Turtle turtle)
    {
        float l = (myUI.parameters["crescentL"] as Slider).value;
        float w = (myUI.parameters["crescentW"] as Slider).value;
        float d = (myUI.parameters["crescentD"] as Slider).value;
        float t = (myUI.parameters["crescentT"] as Slider).value;
        float c = (myUI.parameters["bladeCurv"] as Slider).value;
        float edgeRatio = (myUI.parameters["edgeRatio"] as Slider).value;
        int s = (int)(myUI.parameters["circleSubdivision"] as Slider).value;

        if (meshRenderer == null) Debug.Log("null");
        meshRenderer.materials = new Material[] {
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

        List<Vector3> outerArc = GetArcPoints(new Turtle(turtle), l, w, s);
        turtle.p += turtle.f * l * d;
        List<Vector3> innerArc = GetArcPoints(new Turtle(turtle), l * (1.0f - d), w, s);

        for (int i = 0; i <= s; i++)
        {
            ring = new List<Vertex>();

            Vector3 thick = turtle.u * t / 2.0f * Mathf.Sin(i / (float)s * Mathf.PI);

            newVertex.pos = outerArc[i] + thick;
            newVertex.uv = new Vector2(newVertex.pos.y * uvScale, newVertex.pos.x  * uvScale);
            ring.Add(newVertex);

            newVertex.pos = outerArc[i] * edgeRatio + innerArc[i] * (1.0f - edgeRatio) + thick;
            newVertex.uv = new Vector2(newVertex.pos.y * uvScale, newVertex.pos.x * uvScale);
            ring.Add(newVertex);

            newVertex.pos = innerArc[i];
            newVertex.uv = new Vector2(newVertex.pos.y * uvScale, newVertex.pos.x * uvScale);
            ring.Add(newVertex);

            newVertex.pos = outerArc[i] * edgeRatio + innerArc[i] * (1.0f - edgeRatio) - thick;
            newVertex.uv = new Vector2(newVertex.pos.y * uvScale, newVertex.pos.x * uvScale);
            ring.Add(newVertex);

            newVertex.pos = outerArc[i] - thick;
            newVertex.uv = new Vector2(newVertex.pos.y * uvScale, newVertex.pos.x * uvScale);
            ring.Add(newVertex);

            rings.Add(ring);
        }

        submeshes.Add(rings);
        SetMesh(submeshes, startPoints, endPoints);
    }
}
