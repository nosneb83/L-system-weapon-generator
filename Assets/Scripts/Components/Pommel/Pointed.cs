using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Pointed : BasicMesh
{
    public AnimationCurve edgeCurve;

    public override void CreateMesh(Turtle turtle)
    {
        //float l = (myUI.parameters["pommelLength"] as Slider).value;
        //float w = (myUI.parameters["pommelWidth"] as Slider).value;
        //int s = (int)(myUI.parameters["circleSubdivision"] as Slider).value;

        //meshRenderer.materials = new Material[] {
        //    Resources.Load<Material>("Materials/Knife/Pommel")
        //};
        //List<List<List<Vertex>>> submeshes = new List<List<List<Vertex>>>();
        //List<List<Vertex>> rings = new List<List<Vertex>>();
        //List<Vertex> ring = new List<Vertex>();
        //List<Vector3> startPoints = new List<Vector3>();
        //List<Vector3> endPoints = new List<Vector3>();

        //startPoints.Add(turtle.p);
        //for (int i = 0; i <= s; i++)
        //{
        //    float sectionWidth = (bladeWidth + i * bladeWidthFactorA) * (maxIter - i * bladeWidthFactorB) / maxIter;
        //    float sectionThick = bladeThick * (maxIter - i * bladeWidthFactorB) / maxIter;

        //    rings.Add(KnifeRing(new Turtle(turtle), sectionWidth, sectionThick, edgeRatio));
        //    if (i < maxIter)
        //    {
        //        turtle.RotateAroundUp(bladeCurv);
        //        turtle = MoveTurtle(turtle, turtle.f, bladeLengthGrow / Mathf.Pow(i + 1, bladeLengthGrowFactor));
        //    }
        //}
        //endPoints.Add(turtle.p);

        //submeshes.Add(rings);
        //SetMesh(submeshes, startPoints, endPoints);
    }

    List<Vertex> Ring(Turtle turtle, float w, int s)
    {
        List<Vertex> ring = new List<Vertex>();

        for (int i = 0; i < s; i++)
        {
            ring.Add(CreateVertex((Quaternion.AngleAxis(360.0f * i / s, turtle.f) * turtle.r) * w + turtle.p));
        }

        return ring;
    }
}
