using UnityEngine;
using System.Collections.Generic;

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
}

//public interface ILSystemRule
//{
//    List<Symbol> RewriteString(List<Symbol> inputString);
//}

//public interface LSystemInterpretation
//{
//    void Interpret(List<Symbol> inputString);
//}