using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;

public class UIManager : MonoBehaviour
{
    //// Singleton
    //private static UIManager _instance = null;
    //public static UIManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null) _instance = FindObjectOfType(typeof(UIManager)) as UIManager;
    //        return _instance;
    //    }
    //}

    ManagerSingleton mm;

    // Weapon type
    public enum WeaponTypes
    {
        刀,
        槍,
        劍,
        戟,
        斧,
        三尖刀
    }
    public WeaponTypes currentType;
    public GameObject chooseWeaponPanel;
    public Button chooseWeaponBtnPrefab;
    public List<Button> chooseWeaponBtns;

    // Parameters
    public Dictionary<string, List<object>> parametersSetup; // { key, [value, min, max] }
    public Dictionary<string, Selectable> parameters;
    public GameObject parameterPanel;
    public Text parameterNamePrefab;
    public Slider parameterSliderPrefab;
    public List<Slider> parameterSliders;

    // Show line toggle
    public Toggle showLine;

    // 控制slider顯示的位置
    private int showSliderIndex;

    public int circleSubdivision = 50;

    #region Pommel
    public int pommelSubdivision = 50;
    public float pommelOuterDiameter = 1.5f;
    public float pommelInnerDiameter = 0.5f;
    #endregion

    #region Grip
    public int gripSubdivision = 50;
    public float gripLength = 1.2f;
    public float gripWidth = 0.1f;
    #endregion

    #region Guard
    public int guardSubdivision = 50;
    public float guardLength = 0.2f;
    public float guardWidth = 1.5f;
    #endregion

    #region Blade
    public int bladeSubdivision = 50;

    // knife
    public float bladeLengthGrow = 0.2f;
    [Range(0.0f, 2.0f)]
    public float bladeLengthGrowFactor = 0.0f;
    public float bladeWidth = 1.2f;
    [Range(0.0f, 0.2f)]
    public float bladeWidthFactorA = 0.0f;
    [Range(0.0f, 1.0f)]
    public float bladeWidthFactorB = 0.0f;
    public float bladeThick = 0.2f;
    public float bladeWaveFreq = 0.0f;
    public float bladeWaveAmp = 0.0f;
    [Range(-5.0f, 5.0f)]
    public float bladeCurv = 2.5f;
    [Range(0.0f, 1.0f)]
    public float edgeRatio = 0.15f;
    [Range(10, 80)]
    public int maxIter = 47;

    // spear
    public float spearGripWidth = 0.6f;
    public float spearGripLength = 5.0f;
    public AnimationCurve edgeCurve;

    // crescent
    [Range(0.0f, 5.0f)]
    public float crescentL = 0.75f;
    [Range(0.0f, 5.0f)]
    public float crescentW = 0.3f;
    [Range(0.0f, 0.999f)]
    public float crescentD = 0.6f;
    [Range(0.0f, 1.0f)]
    public float crescentT = 0.3f;
    #endregion

    // Use this for initialization
    void Start()
    {
        //// Singleton
        //if (Instance != this) Destroy(this);
        //mm = ManagerSingleton.Instance;
        mm = FindObjectOfType<ManagerSingleton>();

        // Weapon type buttons
        currentType = WeaponTypes.斧;

        chooseWeaponBtnPrefab = Resources.Load<Button>("Prefabs/ChooseWeaponButton");
        chooseWeaponBtns = new List<Button>();
        foreach (WeaponTypes item in Enum.GetValues(typeof(WeaponTypes)))
        {
            Button newBtn = Instantiate(chooseWeaponBtnPrefab, chooseWeaponPanel.transform);
            newBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50 - (int)item * 70);
            newBtn.GetComponentInChildren<Text>().text = item.ToString();
            newBtn.onClick.AddListener(delegate { ChangeCurrentWeaponType(item); });
            chooseWeaponBtns.Add(newBtn);
        }

        // Parameters initialization
        parametersSetup = new Dictionary<string, List<object>>()
        {
            {"showLineIsOn",            new List<object>(){ false, false, true, "顯示L-system"} },
            {"circleSubdivision",       new List<object>(){ 50, 10, 100, "細緻度" } },
            {"pommelSubdivision",       new List<object>(){ 50, 10, 100, "pommelSubdivision" } },
            {"pommelOuterDiameter",     new List<object>(){ 1.5f, 0.0f, 3.0f, "pommelOuterDiameter" } },
            {"pommelInnerDiameter",     new List<object>(){ 0.5f, 0.0f, 3.0f, "pommelInnerDiameter" } },
            {"pommelLength",            new List<object>(){ 0.5f, 0.0f, 1.0f, "pommel長" } },
            {"pommelWidth",             new List<object>(){ 0.3f, 0.0f, 0.5f, "pommel寬" } },
            {"gripSubdivision",         new List<object>(){ 50, 10, 100, "gripSubdivision" } },
            {"gripLength",              new List<object>(){ 1.2f, 0.0f, 2.0f, "柄長" } },
            {"gripWidth",               new List<object>(){ 0.1f, 0.0f, 0.1f, "柄徑" } },
            {"guardSubdivision",        new List<object>(){ 50, 10, 100, "guardSubdivision" } },
            {"guardLength",             new List<object>(){ 0.1f, 0.0f, 0.3f, "護手長" } },
            {"guardWidth",              new List<object>(){ 0.3f, 0.0f, 0.5f, "護手寬" } },
            {"bladeSubdivision",        new List<object>(){ 50, 10, 100, "bladeSubdivision" } },
            {"bladeLengthGrow",         new List<object>(){ 0.2f, 0.0f, 0.5f, "刃長" } },
            {"bladeLengthGrowFactor",   new List<object>(){ 0.0f, 0.0f, 1.0f, "刃長係數A" } },
            {"bladeWidth",              new List<object>(){ 0.2f, 0.0f, 0.3f, "刃寬" } },
            {"bladeWidthFactorA",       new List<object>(){ 0.0f, 0.0f, 0.02f, "刃寬係數A" } },
            {"bladeWidthFactorB",       new List<object>(){ 1.0f, 0.0f, 1.0f, "刃寬係數B" } },
            {"bladeThick",              new List<object>(){ 0.0f, 0.0f, 0.1f, "刃厚" } },
            {"bladeWaveFreq",           new List<object>(){ 0.0f, 0.0f, 5.0f, "刀刃蛇形頻率" } },
            {"bladeWaveAmp",            new List<object>(){ 0.0f, 0.0f, 5.0f, "刀刃蛇形振幅" } },
            {"bladeCurv",               new List<object>(){ 2.5f, -3.0f, 3.0f, "刃彎" } },
            {"edgeRatio",               new List<object>(){ 0.15f, 0.0f, 1.0f, "刃鋒比例" } },
            {"maxIter",                 new List<object>(){ 47, 10, 80, "迭代次數" } },
            {"spearGripWidth",          new List<object>(){ 0.6f, 0.0f, 3.0f, "spearGripWidth" } },
            {"spearGripLength",         new List<object>(){ 5.0f, 0.0f, 10.0f, "spearGripLength" } },
            {"crescentL",               new List<object>(){ 0.75f, 0.0f, 1.5f, "特殊刃長" } },
            {"crescentW",               new List<object>(){ 0.3f, 0.0f, 1.0f, "特殊刃寬" } },
            {"crescentD",               new List<object>(){ 0.6f, 0.0f, 0.999f, "特殊刃凹" } },
            {"crescentT",               new List<object>(){ 0.3f, 0.0f, 1.0f, "特殊刃厚" } }
        };

        // slider setup
        parameterNamePrefab = Resources.Load<Text>("Prefabs/Text");
        parameterSliderPrefab = Resources.Load<Slider>("Prefabs/ParameterSlider");
        parameters = new Dictionary<string, Selectable>();
        for (int i = 0; i < parametersSetup.Count; i++)
        {
            KeyValuePair<string, List<object>> pair = parametersSetup.ElementAt(i);
            Type pType = pair.Value[0].GetType();

            Slider newSlider = Instantiate(parameterSliderPrefab, parameterPanel.transform);
            newSlider.name = pair.Key;
            if (pType == typeof(float))
            {
                newSlider.maxValue = (float)(pair.Value[2]);
                newSlider.minValue = (float)pair.Value[1];
                newSlider.value = (float)pair.Value[0];
            }
            else if (pType == typeof(int))
            {
                newSlider.maxValue = (int)(pair.Value[2]);
                newSlider.minValue = (int)pair.Value[1];
                newSlider.value = (int)pair.Value[0];
                newSlider.wholeNumbers = true;
            }
            newSlider.onValueChanged.AddListener(delegate { mm.MakeWeapon(); });
            newSlider.gameObject.SetActive(false);

            Text newText = Instantiate(parameterNamePrefab, newSlider.transform);
            newText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-170, 0);
            newText.text = (string)pair.Value[3];
            newText.alignment = TextAnchor.MiddleLeft;

            parameters.Add(pair.Key, newSlider);
        }

        // Show line
        showLine.onValueChanged.AddListener(delegate
        {
            ShowLSystemLine(showLine);
        });

        edgeCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeCurrentWeaponType(WeaponTypes type)
    {
        mm.meshReady = false;
        currentType = type;

        foreach (KeyValuePair<string, Selectable> item in parameters)
        {
            item.Value.gameObject.SetActive(false);
        }

        // set init params
        showSliderIndex = 0;
        switch (type)
        {
            case WeaponTypes.刀:
                ShowSlider("gripLength", 0.35f);
                ShowSlider("gripWidth", 0.05f);
                ShowSlider("guardLength", 0.05f);
                ShowSlider("bladeLengthGrow", 0.4f);
                ShowSlider("bladeLengthGrowFactor", 1.0f);
                ShowSlider("bladeWidth", 0.2f);
                ShowSlider("bladeWidthFactorA", 0.016f);
                ShowSlider("bladeWidthFactorB", 1.0f);
                ShowSlider("bladeThick", 0.06f);
                ShowSlider("bladeCurv", 0.3f);
                ShowSlider("edgeRatio", 0.3f);
                break;
            case WeaponTypes.槍:
                ShowSlider("gripLength", 1.7f);
                ShowSlider("gripWidth", 0.03f);
                ShowSlider("bladeLengthGrow", 0.01f);
                ShowSlider("bladeLengthGrowFactor", 0.0f);
                ShowSlider("bladeWidth", 0.03f);
                ShowSlider("bladeWidthFactorA", 0.003f);
                ShowSlider("bladeThick", 0.03f);
                ShowSlider("bladeCurv", 0.0f);
                break;
            case WeaponTypes.劍:
                ShowSlider("gripLength", 0.35f);
                ShowSlider("gripWidth", 0.05f);
                ShowSlider("guardLength", 0.05f);
                ShowSlider("bladeLengthGrow", 0.4f);
                ShowSlider("bladeLengthGrowFactor", 1.0f);
                ShowSlider("bladeWidth", 0.075f);
                ShowSlider("bladeWidthFactorA", 0.004f);
                ShowSlider("bladeThick", 0.03f);
                ShowSlider("bladeCurv", 0.0f);
                break;
            case WeaponTypes.戟:
                ShowSlider("gripLength", 1.7f);
                ShowSlider("gripWidth", 0.03f);
                ShowSlider("bladeLengthGrow", 0.01f);
                ShowSlider("bladeLengthGrowFactor", 0.0f);
                ShowSlider("bladeWidth", 0.03f);
                ShowSlider("bladeWidthFactorA", 0.003f);
                ShowSlider("bladeThick", 0.03f);
                ShowSlider("bladeCurv", 0.0f);
                ShowSlider("edgeRatio", 0.5f);
                ShowSlider("crescentL", 0.25f);
                ShowSlider("crescentW", 0.25f);
                ShowSlider("crescentD", 0.4f);
                ShowSlider("crescentT", 0.1f);
                break;
            case WeaponTypes.斧:
                ShowSlider("gripLength", 1.5f);
                ShowSlider("gripWidth", 0.04f);
                ShowSlider("bladeCurv", 3.0f);
                ShowSlider("crescentL", 0.4f);
                ShowSlider("crescentW", 0.15f);
                ShowSlider("crescentD", 0.6f);
                ShowSlider("crescentT", 0.3f);
                break;
            case WeaponTypes.三尖刀:
                ShowSlider("gripLength", 1.25f);
                ShowSlider("gripWidth", 0.04f);
                ShowSlider("guardLength", 0.05f);
                ShowSlider("guardWidth", 0.11f);
                ShowSlider("bladeLengthGrow", 0.025f);
                ShowSlider("bladeLengthGrowFactor", 0.2f);
                ShowSlider("bladeWidth", 0.1f);
                ShowSlider("bladeWidthFactorA", 0.0015f);
                ShowSlider("bladeThick", 0.075f);
                ShowSlider("bladeCurv", 1.25f);
                break;
            default:
                break;
        }

        mm.AfterStart();
    }

    void ShowSlider(string sliderName, float value)
    {
        showSliderIndex++;
        Slider slider = parameters[sliderName] as Slider;
        slider.value = value;
        slider.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -showSliderIndex * 25);
        slider.gameObject.SetActive(true);
    }

    void ShowLSystemLine(Toggle change)
    {
        //parameters["showLineIsOn"][0] = showLine.isOn;
        //mm.MakeWeapon();
    }
}
