using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerSingleton : MonoBehaviour
{
    enum Target { Sword1, Spear1, Jian};
    Target target;

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

    List<Symbol> theString;

    // Use this for initialization
    void Start()
    {
        if (Instance != this) Destroy(this);

        target = Target.Sword1;

        weapon = new GameObject("Weapon Model");
        weapon.transform.parent = gameObject.transform;

        switch (target)
        {
            case Target.Sword1:
                sword1 = weapon.AddComponent<Sword1>();
                break;
            case Target.Spear1:
                spear1 = weapon.AddComponent<Spear1>();
                break;
            case Target.Jian:
                jian = weapon.AddComponent<Jian>();
                break;
            default:
                break;
        }

        // Axiom
        theString = new List<Symbol>();
        theString.Add(new Symbol("P", new object[] { 0 }));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onIteration()
    {
        switch (target)
        {
            case Target.Sword1:
                for (int i = 0; i < 50; i++)
                {
                    theString = sword1.RewriteString(theString);
                    sword1.TurtleInterpretation(theString);
                }
                break;
            case Target.Spear1:
                for (int i = 0; i < 50; i++)
                {
                    theString = spear1.RewriteString(theString);
                    spear1.TurtleInterpretation(theString);
                }
                break;
            case Target.Jian:
                for (int i = 0; i < 50; i++)
                {
                    theString = jian.RewriteString(theString);
                    jian.TurtleInterpretation(theString);
                }
                break;
            default:
                break;
        }
    }
}
