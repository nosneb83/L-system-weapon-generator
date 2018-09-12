using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerSingleton : MonoBehaviour
{
    /*** 1. ***/
    public enum Target { Sword1, Spear1, Jian, Knife1, Knife2, Knife3, KnifeParametric };
    public Target target;

    private static ManagerSingleton _instance = null;
    public static ManagerSingleton Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType(typeof(ManagerSingleton)) as ManagerSingleton;
            return _instance;
        }
    }
    
    GameObject weapon;
    Sword1 sword1;
    Spear1 spear1;
    Jian jian;
    Turtle turtle;

    List<Symbol> theString;
    public int iter;

    // Use this for initialization
    void Start()
    {
        if (Instance != this) Destroy(this);

        /*** 2. ***/
        target = Target.KnifeParametric;
        
        weapon = new GameObject("Sword1");
        weapon.transform.parent = gameObject.transform;
        sword1 = weapon.AddComponent<Sword1>();

        weapon = new GameObject("Spear1");
        weapon.transform.parent = gameObject.transform;
        spear1 = weapon.AddComponent<Spear1>();

        weapon = new GameObject("Jian");
        weapon.transform.parent = gameObject.transform;
        jian = weapon.AddComponent<Jian>();

        weapon = new GameObject("Weapon");
        weapon.transform.parent = gameObject.transform;
        turtle = weapon.AddComponent<Turtle>();

        theString = new List<Symbol>();

        iter = 50;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onIteration()
    {
        // Axiom
        theString.Clear();
        theString.Add(new Symbol("Pommel", new object[] { 5.0f, 3.5f }));

        switch (target)
        {
            /*** 3. ***/
            case Target.Sword1:
                for (int i = 0; i < iter; i++)
                {
                    theString = sword1.RewriteString(theString);
                    sword1.TurtleInterpretation(theString);
                }
                break;
            case Target.Spear1:
                for (int i = 0; i < iter; i++)
                {
                    theString = spear1.RewriteString(theString);
                    spear1.TurtleInterpretation(theString);
                }
                break;
            case Target.Jian:
                for (int i = 0; i < iter; i++)
                {
                    theString = jian.RewriteString(theString);
                    jian.TurtleInterpretation(theString);
                }
                break;
            case Target.Knife1:
                for (int i = 0; i < 81; i++)
                {
                    theString = Knife1.RewriteString(theString);
                    turtle.Interpret(theString);
                }
                break;
            case Target.Knife2:
                for (int i = 0; i < 50; i++)
                {
                    theString = Knife2.RewriteString(theString);
                    turtle.Interpret(theString);
                }
                break;
            case Target.Knife3:
                for (int i = 0; i < 50; i++)
                {
                    theString = Knife3.RewriteString(theString);
                    turtle.Interpret(theString);
                }
                break;
            case Target.KnifeParametric:
                for (int i = 0; i < 50; i++)
                {
                    theString = KnifeParametric.RewriteString(theString);
                }
                turtle.Interpret(theString);
                break;
            default:
                break;
        }
    }
}
