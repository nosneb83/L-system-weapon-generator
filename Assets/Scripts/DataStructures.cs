using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public struct Symbol
{
    public string s;
    public object[] p;

    public Symbol(string s, object[] p)
    {
        this.s = s;
        this.p = p;
    }
}

public struct Vertex
{
    public Vector3 pos;
    public Vector2 uv;

    public Vertex(Vector3 pos, Vector2 uv)
    {
        this.pos = pos;
        this.uv = uv;
    }
}

public struct Turtle
{
    public Vector3 p; // position
    public Vector3 f; // forward
    public Vector3 u; // up
    public Vector3 r; // right

    public Turtle(Vector3 p, Vector3 f, Vector3 u, Vector3 r)
    {
        this.p = p;
        this.f = f;
        this.u = u;
        this.r = r;
    }

    public Turtle(Turtle oldTurtle)
    {
        p = oldTurtle.p;
        f = oldTurtle.f;
        u = oldTurtle.u;
        r = oldTurtle.r;
    }

    public void Go(Vector3 direction, float distance)
    {
        p += direction.normalized * distance;
    }

    public void RotateAroundForward(float angle)
    {
        u = (Quaternion.AngleAxis(angle, f) * u).normalized;
        r = (Quaternion.AngleAxis(angle, f) * r).normalized;
    }

    public void RotateAroundUp(float angle)
    {
        f = (Quaternion.AngleAxis(angle, u) * f).normalized;
        r = (Quaternion.AngleAxis(angle, u) * r).normalized;
    }

    public void RotateAroundRight(float angle)
    {
        f = (Quaternion.AngleAxis(angle, r) * f).normalized;
        u = (Quaternion.AngleAxis(angle, r) * u).normalized;
    }
}

//public interface ILSystemRule
//{
//    List<Symbol> RewriteString(List<Symbol> inputString);
//}

//public interface LSystemInterpretation
//{
//    void Interpret(List<Symbol> inputString);
//}