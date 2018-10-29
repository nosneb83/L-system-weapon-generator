using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class ChooseWeapon : MonoBehaviour
{
    public Button btn_knife, btn_spear;

    void Start()
    {
        btn_knife.onClick.AddListener(OnChooseKnife);
        btn_spear.onClick.AddListener(OnChooseSpear);
    }

    void OnChooseKnife()
    {
        Debug.Log("Choose Knife");
        Destroy(ManagerSingleton.Instance.testObj.GetComponent<BasicMesh>());
    }
    
    void OnChooseSpear()
    {
        Debug.Log("Choose Spear");
    }
}
