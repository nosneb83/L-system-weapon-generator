using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ManagerSingleton : MonoBehaviour
{
    /*** 1. ***/
    public enum Target
    {
        Sword1,
        Spear1,
        Jian,
        Knife1,
        Knife2,
        Knife3,
        KnifeParametric,
        Spear,
        Crescent,
        Axe,
        Fork
    };
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

    UIManager myUI;

    //public GameObject weapon;
    //Sword1 sword1;
    //Spear1 spear1;
    //Jian jian;
    //TurtleInterpretation turtle;
    //KnifeParametric knifeParametric;

    //List<Symbol> theString;
    //public int iter;
    //int t;
    //bool run;

    public GameObject testObj, pommel, grip, guard, blade;
    public BasicMesh testAxe;
    public Cylinder testCylinder;
    public Parameters p;

    public GameObject panel_p;
    private List<Slider> sli_p;

    // Use this for initialization
    void Start()
    {
        if (Instance != this) Destroy(this);
        myUI = UIManager.Instance;

        /*** 2. ***/
        //target = Target.Fork;

        //weapon = new GameObject("Sword1");
        //weapon.transform.parent = gameObject.transform;
        //sword1 = weapon.AddComponent<Sword1>();

        //weapon = new GameObject("Spear1");
        //weapon.transform.parent = gameObject.transform;
        //spear1 = weapon.AddComponent<Spear1>();

        //weapon = new GameObject("Jian");
        //weapon.transform.parent = gameObject.transform;
        //jian = weapon.AddComponent<Jian>();

        //weapon = new GameObject("Weapon");
        //weapon.transform.parent = gameObject.transform;
        //knifeParametric = weapon.AddComponent<KnifeParametric>();
        //turtle = weapon.AddComponent<TurtleInterpretation>();

        testObj = new GameObject("Test");
        testObj.layer = 8;
        testObj.transform.localEulerAngles = new Vector3(-5, 0, 5);
        p = testObj.AddComponent<Parameters>();

        grip = new GameObject("Grip");
        grip.transform.parent = testObj.transform;
        grip.layer = 8;
        testCylinder = grip.AddComponent<Cylinder>();

        blade = new GameObject("Blade");
        blade.transform.parent = testObj.transform;
        blade.layer = 8;
        //testAxe = blade.AddComponent<Axe>();

        //theString = new List<Symbol>();

        //iter = 50;
        //t = 0;
        //run = false;

        sli_p = new List<Slider>(panel_p.GetComponentsInChildren<Slider>());
        foreach (var item in sli_p)
        {
            item.onValueChanged.AddListener(delegate { MakeWeapon(); });
        }

        StartCoroutine(AfterStart());
    }

    public IEnumerator AfterStart()
    {
        yield return new WaitForEndOfFrame();

        // remove old one
        BasicMesh oldMesh = blade.GetComponent<BasicMesh>();
        if (oldMesh != null) Destroy(oldMesh);

        switch (myUI.currentType)
        {
            case UIManager.WeaponTypes.刀:
                testAxe = blade.AddComponent<Knife>();
                break;
            case UIManager.WeaponTypes.槍:
                break;
            case UIManager.WeaponTypes.劍:
                break;
            case UIManager.WeaponTypes.戟:
                testAxe = blade.AddComponent<Crescent>();
                break;
            case UIManager.WeaponTypes.斧:
                testAxe = blade.AddComponent<Axe>();
                break;
            default:
                break;
        }

        yield return new WaitForEndOfFrame();
        MakeWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        //onIteration();
        testObj.transform.position = new Vector3(testObj.transform.position.x, 0.75f + 0.05f * Mathf.Sin(Time.time * 1.5f), testObj.transform.position.z);
    }

    public void MakeWeapon()
    {
        testAxe.linePoints.Clear();

        // parameters
        //myUI.parameters["gripLength"][0] = sli_p[0].value;
        //myUI.parameters["gripWidth"][0] = sli_p[1].value;
        //myUI.parameters["crescentL"][0] = sli_p[2].value;
        //myUI.parameters["crescentW"][0] = sli_p[3].value;
        //myUI.parameters["crescentT"][0] = sli_p[4].value;
        //myUI.parameters["bladeCurv"][0] = sli_p[5].value;

        /*** create mesh ***/

        // start turtle
        Turtle turtle = new Turtle(Vector3.zero, Vector3.up, -Vector3.forward, Vector3.right);

        // grip
        CreateComponentMesh(grip, new Turtle(turtle));

        // blade
        switch (myUI.currentType)
        {
            case UIManager.WeaponTypes.刀:
                turtle.Go(turtle.f, p.gripLength);
                break;
            case UIManager.WeaponTypes.槍:
                break;
            case UIManager.WeaponTypes.劍:
                break;
            case UIManager.WeaponTypes.戟:
                turtle.Go(turtle.f, p.gripLength);
                //turtle.RotateAroundUp(90);
                turtle.Go(-turtle.f, p.gripWidth);
                break;
            case UIManager.WeaponTypes.斧:
                turtle.Go(turtle.f, p.gripLength);
                turtle.RotateAroundUp(90);
                turtle.Go(-turtle.f, p.gripWidth);
                break;
            default:
                break;
        }
        CreateComponentMesh(blade, new Turtle(turtle));
    }

    private void CreateComponentMesh(GameObject component, Turtle turtle)
    {
        BasicMesh bm = component.GetComponent<BasicMesh>();
        bm.CreateMesh(new Turtle(turtle), p);
    }

    //public void onIteration()
    //{
    //    theString.Clear();
    //    run = true;

    //    switch (target)
    //    {
    //        /*** 3. ***/
    //        case Target.Sword1:
    //            theString.Add(new Symbol("P", new object[] { 0 })); // Axiom
    //            for (int i = 0; i < t; i++)
    //            {
    //                theString = sword1.RewriteString(theString);
    //                sword1.TurtleInterpretation(theString);
    //            }
    //            break;
    //        case Target.Spear1:
    //            theString.Add(new Symbol("P", new object[] { 0 })); // Axiom
    //            for (int i = 0; i < iter; i++)
    //            {
    //                theString = spear1.RewriteString(theString);
    //                spear1.TurtleInterpretation(theString);
    //            }
    //            break;
    //        case Target.Jian:
    //            theString.Add(new Symbol("P", new object[] { 0 })); // Axiom
    //            for (int i = 0; i < t; i++)
    //            {
    //                theString = jian.RewriteString(theString);
    //                jian.TurtleInterpretation(theString);
    //            }
    //            break;
    //        case Target.Knife1:
    //            theString.Add(new Symbol("Knife", new object[] { })); // Axiom
    //            for (int i = 0; i < 81; i++)
    //            {
    //                theString = Knife1.RewriteString(theString);
    //                turtle.Interpret(theString);
    //            }
    //            break;
    //        case Target.Knife2:
    //            theString.Add(new Symbol("Knife", new object[] { })); // Axiom
    //            for (int i = 0; i < 50; i++)
    //            {
    //                theString = Knife2.RewriteString(theString);
    //                turtle.Interpret(theString);
    //            }
    //            break;
    //        case Target.Knife3:
    //            theString.Add(new Symbol("Knife", new object[] { })); // Axiom
    //            for (int i = 0; i < 50; i++)
    //            {
    //                theString = Knife3.RewriteString(theString);
    //                turtle.Interpret(theString);
    //            }
    //            break;
    //        case Target.KnifeParametric:
    //            theString.Add(new Symbol("Knife", new object[] { })); // Axiom
    //            for (int i = 0; i < knifeParametric.maxIter + 3; i++)
    //            {
    //                theString = knifeParametric.RewriteString(theString);
    //            }
    //            turtle.Interpret(theString);
    //            break;
    //        case Target.Spear:
    //            theString.Add(new Symbol("Spear", new object[] { })); // Axiom
    //            for (int i = 0; i < knifeParametric.maxIter + 3; i++)
    //            {
    //                theString = knifeParametric.RewriteStringSpear(theString);
    //            }
    //            turtle.Interpret(theString);
    //            break;
    //        case Target.Crescent:
    //            theString.Add(new Symbol("Crescent", new object[] { })); // Axiom
    //            for (int i = 0; i < knifeParametric.maxIter + 3; i++)
    //            {
    //                theString = knifeParametric.RewriteStringCrescent(theString);
    //            }
    //            turtle.Interpret(theString);
    //            break;
    //        case Target.Axe:
    //            theString.Add(new Symbol("Axe", new object[] { })); // Axiom
    //            for (int i = 0; i < knifeParametric.maxIter + 3; i++)
    //            {
    //                theString = knifeParametric.RewriteStringAxe(theString);
    //            }
    //            turtle.Interpret(theString);
    //            break;
    //        case Target.Fork:
    //            theString.Add(new Symbol("Fork", new object[] { })); // Axiom
    //            for (int i = 0; i < knifeParametric.maxIter + 3; i++)
    //            {
    //                theString = knifeParametric.RewriteStringFork(theString);
    //            }
    //            turtle.Interpret(theString);
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
