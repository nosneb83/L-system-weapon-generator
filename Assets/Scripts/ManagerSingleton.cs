using UnityEngine;
using System.Collections;

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

    public GameObject weapon;
    public Sword1 sword1;

    /***/
    private GameObject root;

    // Use this for initialization
    void Start()
    {
        if (Instance != this) Destroy(this);

        weapon = new GameObject("Weapon Model");
        weapon.transform.parent = gameObject.transform;
        sword1 = weapon.AddComponent<Sword1>();

        /***/
        root = new GameObject("Root");
        GameObject rootObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rootObj.transform.parent = root.transform;
        Node rootNode = rootObj.AddComponent<Node>();
        rootNode.symbol = Node.Symbol.C;
        rootNode.orientation = Vector3.up;
        rootNode.pDistance = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onIteration()
    {
        //sword1.SetMesh();

        /***/
        foreach (var item in root.GetComponentsInChildren<Node>())
        {
            item.Iterate();
        }
    }
}
