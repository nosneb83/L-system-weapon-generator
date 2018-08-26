using UnityEngine;
using System.Collections;

public class Symbol : MonoBehaviour
{
    public char symbol;
    public object[] parameter;

    public Symbol(char s, object[] p)
    {
        symbol = s;
        parameter = p;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
