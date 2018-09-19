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

//public interface ILSystemRule
//{
//    List<Symbol> RewriteString(List<Symbol> inputString);
//}

//public interface LSystemInterpretation
//{
//    void Interpret(List<Symbol> inputString);
//}