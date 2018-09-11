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
    public int tex;

    public Vertex(Vector3 pos, int tex)
    {
        this.pos = pos;
        this.tex = tex;
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