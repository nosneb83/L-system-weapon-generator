using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Knife : BasicMesh
{
    public override void CreateMesh(Turtle turtle)
    {
        float bladeLengthGrow = (myUI.parameters["bladeLengthGrow"] as Slider).value;
        float bladeLengthGrowFactor = (myUI.parameters["bladeLengthGrowFactor"] as Slider).value;
        float bladeWidth = (myUI.parameters["bladeWidth"] as Slider).value;
        float bladeThick = (myUI.parameters["bladeThick"] as Slider).value;
        float bladeCurv = (myUI.parameters["bladeCurv"] as Slider).value;
        float bladeWidthFactorA = (myUI.parameters["bladeWidthFactorA"] as Slider).value;
        float bladeWidthFactorB = (myUI.parameters["bladeWidthFactorB"] as Slider).value;
        float edgeRatio = (myUI.parameters["edgeRatio"] as Slider).value;
        float maxIter = (myUI.parameters["maxIter"] as Slider).value;

        renderer.materials = new Material[] {
            Resources.Load<Material>("Materials/Knife/Blade")
        };
        List<List<List<Vertex>>> submeshes = new List<List<List<Vertex>>>();
        List<List<Vertex>> rings = new List<List<Vertex>>();
        List<Vertex> ring = new List<Vertex>();
        List<Vector3> startPoints = new List<Vector3>();
        List<Vector3> endPoints = new List<Vector3>();

        startPoints.Add(turtle.p);
        for (int i = 0; i <= maxIter; i++)
        {
            float sectionWidth = (bladeWidth + i * bladeWidthFactorA) * (maxIter - i * bladeWidthFactorB) / maxIter;
            float sectionThick = bladeThick * (maxIter - i * bladeWidthFactorB) / maxIter;

            rings.Add(KnifeRing(new Turtle(turtle), sectionWidth, sectionThick, edgeRatio));
            if (i < maxIter)
            {
                turtle.RotateAroundUp(bladeCurv);
                turtle = MoveTurtle(turtle, turtle.f, bladeLengthGrow / Mathf.Pow(i + 1, bladeLengthGrowFactor));
            }
        }
        endPoints.Add(turtle.p);

        submeshes.Add(rings);
        SetMesh(submeshes, startPoints, endPoints);
    }

    List<Vertex> KnifeRing(Turtle turtle, float sectionWidth, float sectionThick, float edgeRatio)
    {
        List<Vertex> ring = new List<Vertex>();
        Stack<Vertex> temp = new Stack<Vertex>();

        Turtle branchTurtle = new Turtle(turtle);
        branchTurtle = MoveTurtle(branchTurtle, branchTurtle.u, sectionThick);
        temp.Push(CreateVertex(branchTurtle.p));
        branchTurtle = new Turtle(turtle);
        branchTurtle = MoveTurtle(branchTurtle, -branchTurtle.u, sectionThick);
        ring.Add(CreateVertex(branchTurtle.p));

        turtle = MoveTurtle(turtle, -turtle.r, sectionWidth * (1 - edgeRatio));
        branchTurtle = new Turtle(turtle);
        branchTurtle = MoveTurtle(branchTurtle, branchTurtle.u, sectionThick);
        temp.Push(CreateVertex(branchTurtle.p));
        branchTurtle = new Turtle(turtle);
        branchTurtle = MoveTurtle(branchTurtle, -branchTurtle.u, sectionThick);
        ring.Add(CreateVertex(branchTurtle.p));

        turtle = MoveTurtle(turtle, -turtle.r, sectionWidth * edgeRatio);
        ring.Add(CreateVertex(turtle.p));

        foreach (var item in temp)
        {
            ring.Add(item);
        }

        return ring;
    }
}
