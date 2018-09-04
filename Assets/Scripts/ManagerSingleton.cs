using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerSingleton : MonoBehaviour
{
    /*** 1. ***/
    public enum Target { Sword1, Spear1, Jian, Knife1, Knife2, Knife3 };
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

    /*** 2. ***/
    GameObject weapon;
    Sword1 sword1;
    Spear1 spear1;
    Jian jian;
    Knife1 knife1;
    Knife2 knife2;
    Knife3 knife3;

    List<Symbol> theString;
    public int iter;

    // Use this for initialization
    void Start()
    {
        if (Instance != this) Destroy(this);

        /*** 3. ***/
        target = Target.Knife3;

        /*** 4. ***/
        weapon = new GameObject("Sword1");
        weapon.transform.parent = gameObject.transform;
        sword1 = weapon.AddComponent<Sword1>();

        weapon = new GameObject("Spear1");
        weapon.transform.parent = gameObject.transform;
        spear1 = weapon.AddComponent<Spear1>();

        weapon = new GameObject("Jian");
        weapon.transform.parent = gameObject.transform;
        jian = weapon.AddComponent<Jian>();

        weapon = new GameObject("Knife1");
        weapon.transform.parent = gameObject.transform;
        knife1 = weapon.AddComponent<Knife1>();

        weapon = new GameObject("Knife2");
        weapon.transform.parent = gameObject.transform;
        knife2 = weapon.AddComponent<Knife2>();

        weapon = new GameObject("Knife3");
        weapon.transform.parent = gameObject.transform;
        knife3 = weapon.AddComponent<Knife3>();

        // Axiom
        theString = new List<Symbol>();
        theString.Add(new Symbol("P", new object[] { 0 }));

        iter = 50;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onIteration()
    {
        theString.Clear();
        theString.Add(new Symbol("P", new object[] { 0 }));

        switch (target)
        {
            /*** 5. ***/
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
                    theString = knife1.RewriteString(theString);
                    knife1.TurtleInterpretation(theString);
                }
                break;
            case Target.Knife2:
                for (int i = 0; i < 50; i++)
                {
                    theString = knife2.RewriteString(theString);
                    knife2.TurtleInterpretation(theString);
                }
                break;
            case Target.Knife3:
                for (int i = 0; i < 50; i++)
                {
                    theString = knife3.RewriteString(theString);
                    knife3.TurtleInterpretation(theString);
                }
                break;
            default:
                break;
        }
    }
}
