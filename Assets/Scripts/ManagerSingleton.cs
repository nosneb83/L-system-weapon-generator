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

    //private static ManagerSingleton _instance = null;
    //public static ManagerSingleton Instance
    //{
    //    get
    //    {
    //        if (_instance == null) _instance = FindObjectOfType(typeof(ManagerSingleton)) as ManagerSingleton;
    //        return _instance;
    //    }
    //}

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

    public GameObject testObj, pommel, grip, guard, blade, blade1;
    public BasicMesh testAxe;
    public Parameters p;

    public GameObject panel_p;
    private List<Slider> sli_p;

    public bool meshReady = false;

    private void Awake()
    {
        
    }

    void Start()
    {
        //if (Instance != this) Destroy(this);
        //myUI = UIManager.Instance;
        myUI = FindObjectOfType<UIManager>();

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
        //p = testObj.AddComponent<Parameters>();

        grip = new GameObject("Grip");
        grip.transform.parent = testObj.transform;
        grip.layer = 8;
        grip.AddComponent<Cylinder>();

        guard = new GameObject("Guard");
        guard.transform.parent = testObj.transform;
        guard.layer = 8;
        guard.AddComponent<DragonHead>();

        blade = new GameObject("Blade");
        blade.transform.parent = testObj.transform;
        blade.layer = 8;
        blade.AddComponent<Knife>();

        blade1 = new GameObject("Blade1");
        blade1.transform.parent = blade.transform;
        blade1.layer = 8;
        blade1.AddComponent<Crescent>();

        //testAxe = blade.AddComponent<Axe>();

        //theString = new List<Symbol>();

        //iter = 50;
        //t = 0;
        //run = false;

        //sli_p = new List<Slider>(panel_p.GetComponentsInChildren<Slider>());
        //foreach (var item in sli_p)
        //{
        //    item.onValueChanged.AddListener(delegate { MakeWeapon(); });
        //}

        //StartCoroutine(AfterStart());
        //myUI.ChangeCurrentWeaponType(UIManager.WeaponTypes.三尖刀);
    }

    public void AfterStart()
    {
        //yield return new WaitForEndOfFrame();

        // remove old one
        BasicMesh oldMesh = blade.GetComponent<BasicMesh>();
        if (oldMesh != null) DestroyImmediate(oldMesh);

        //if (blade.GetComponent<BasicMesh>() == null) Debug.Log("null");
        blade1.SetActive(false);
        guard.SetActive(false);

        // make new one
        switch (myUI.currentType)
        {
            case UIManager.WeaponTypes.刀:
                blade.AddComponent<Knife>();
                guard.SetActive(true);
                break;
            case UIManager.WeaponTypes.槍:
                blade.AddComponent<Sword>();
                break;
            case UIManager.WeaponTypes.劍:
                blade.AddComponent<Sword>();
                guard.SetActive(true);
                break;
            case UIManager.WeaponTypes.戟:
                blade.AddComponent<Sword>();
                blade1.SetActive(true);
                break;
            case UIManager.WeaponTypes.斧:
                blade.AddComponent<Axe>();
                break;
            case UIManager.WeaponTypes.三尖刀:
                blade.AddComponent<TridentSword>();
                guard.SetActive(true);
                break;
            default:
                break;
        }
        meshReady = true;

        //yield return new WaitForEndOfFrame();

        if (meshReady) MakeWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        //onIteration();
        testObj.transform.position = new Vector3(0.3f, 0.65f + 0.05f * Mathf.Sin(Time.time * 1.5f), testObj.transform.position.z);
        testObj.transform.localEulerAngles = new Vector3(-7.5f, 7.5f, 20f);
    }

    public void MakeWeapon()
    {
        //testAxe.linePoints.Clear();

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
        switch (myUI.currentType)
        {
            case UIManager.WeaponTypes.刀:
            case UIManager.WeaponTypes.劍:
            case UIManager.WeaponTypes.槍:
            case UIManager.WeaponTypes.斧:
            case UIManager.WeaponTypes.三尖刀:
                CreateComponentMesh(grip, new Turtle(turtle));
                turtle.Go(turtle.f, (myUI.parameters["gripLength"] as Slider).value);
                break;
            case UIManager.WeaponTypes.戟:
                CreateComponentMesh(grip, new Turtle(turtle));
                turtle.Go(turtle.f, (myUI.parameters["gripLength"] as Slider).value * 0.7f);
                break;
            default:
                break;
        }

        // guard
        switch (myUI.currentType)
        {
            case UIManager.WeaponTypes.刀:
            case UIManager.WeaponTypes.劍:
            case UIManager.WeaponTypes.三尖刀:
                CreateComponentMesh(guard, new Turtle(turtle));
                turtle.Go(turtle.f, (myUI.parameters["guardLength"] as Slider).value);
                break;
            default:
                break;
        }

        // blade
        switch (myUI.currentType)
        {
            case UIManager.WeaponTypes.槍:
            case UIManager.WeaponTypes.劍:
            case UIManager.WeaponTypes.三尖刀:
                CreateComponentMesh(blade, new Turtle(turtle));
                break;
            case UIManager.WeaponTypes.刀:
                turtle.Go(-turtle.r, (myUI.parameters["bladeWidth"] as Slider).value * 0.5f);
                CreateComponentMesh(blade, new Turtle(turtle));
                break;
            case UIManager.WeaponTypes.戟:
                // left crescent
                Turtle leftCrescentTurtle = new Turtle(turtle);
                leftCrescentTurtle.RotateAroundUp(90);
                leftCrescentTurtle.Go(turtle.r, (myUI.parameters["gripWidth"] as Slider).value);
                CreateComponentMesh(blade1, leftCrescentTurtle);

                turtle.Go(turtle.f, (myUI.parameters["gripLength"] as Slider).value * 0.3f);
                CreateComponentMesh(blade, new Turtle(turtle));
                break;
            case UIManager.WeaponTypes.斧:
                turtle.RotateAroundUp(90);
                turtle.Go(-turtle.f, (myUI.parameters["gripWidth"] as Slider).value);
                CreateComponentMesh(blade, new Turtle(turtle));
                break;
            default:
                break;
        }
    }

    private void CreateComponentMesh(GameObject component, Turtle turtle)
    {
        BasicMesh bm = component.GetComponent<BasicMesh>();
        //if (bm == null) Debug.Log("bm null");
        //else Debug.Log("bm not null");
        bm.CreateMesh(new Turtle(turtle));
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
