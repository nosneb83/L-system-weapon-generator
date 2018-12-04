using UnityEngine;
using UnityEngine.UI;

public class ChooseWeapon : MonoBehaviour
{
    public Button btn_knife, btn_spear, btn_sword, btn_halberd, btn_axe;

    void Start()
    {
        //btn_knife.onClick.AddListener(() => OnChooseKnife());
        //btn_spear.onClick.AddListener(() => OnChooseSpear());
        //btn_sword.onClick.AddListener(() => OnChooseSword());
        //btn_halberd.onClick.AddListener(() => OnChooseHalberd());
        //btn_axe.onClick.AddListener(() => OnChooseAxe());
    }

    void OnChooseKnife()
    {
        ManagerSingleton.Instance.target = ManagerSingleton.Target.KnifeParametric;
    }

    void OnChooseSpear()
    {
        ManagerSingleton.Instance.target = ManagerSingleton.Target.Spear;
    }

    void OnChooseSword()
    {
        ManagerSingleton.Instance.target = ManagerSingleton.Target.Sword1;
    }

    void OnChooseHalberd()
    {
        ManagerSingleton.Instance.target = ManagerSingleton.Target.Crescent;
    }

    void OnChooseAxe()
    {
        ManagerSingleton.Instance.target = ManagerSingleton.Target.Axe;
    }
}
