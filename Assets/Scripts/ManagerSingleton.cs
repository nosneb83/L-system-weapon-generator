using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerSingleton : MonoBehaviour
{
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

    List<Symbol> theString;

    // Use this for initialization
    void Start()
    {
        if (Instance != this) Destroy(this);

        weapon = new GameObject("Weapon Model");
        weapon.transform.parent = gameObject.transform;
        sword1 = weapon.AddComponent<Sword1>();

        theString = new List<Symbol>();
        theString.Add(new Symbol('P', new object[] { }));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onIteration()
    {
        theString = sword1.RewriteString(theString);
        sword1.TurtleInterpretation(theString);
    }
}
