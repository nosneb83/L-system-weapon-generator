using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;

public class UIManager : MonoBehaviour
{
    // Singleton
    private static UIManager _instance = null;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType(typeof(UIManager)) as UIManager;
            return _instance;
        }
    }

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
        // Singleton
        if (Instance != this) Destroy(this);
        mm = ManagerSingleton.Instance;

        // Weapon type buttons
        currentType = WeaponTypes.斧;

        chooseWeaponBtnPrefab = Resources.Load<Button>("Prefabs/ChooseWeaponButton");
        chooseWeaponBtns = new List<Button>();
        foreach (WeaponTypes item in Enum.GetValues(typeof(WeaponTypes)))
        {
            Button newBtn = Instantiate(chooseWeaponBtnPrefab, chooseWeaponPanel.transform);
            newBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50 - (int)item * 70);
            newBtn.GetComponentInChildren<Text>().text = item.ToString();
            newBtn.onClick.AddListener(() => ChangeCurrentWeaponType(item));
            chooseWeaponBtns.Add(newBtn);
        }

        // Parameters initialization
        parametersSetup = new Dictionary<string, List<object>>()
        {
            {"showLineIsOn",            new List<object>(){ false, false, true} },
            {"circleSubdivision",       new List<object>(){ 50, 10, 100 } },
            {"pommelSubdivision",       new List<object>(){ 50, 10, 100 } },
            {"pommelOuterDiameter",     new List<object>(){ 1.5f, 0.0f, 3.0f } },
            {"pommelInnerDiameter",     new List<object>(){ 0.5f, 0.0f, 3.0f } },
            {"gripSubdivision",         new List<object>(){ 50, 10, 100 } },
            {"gripLength",              new List<object>(){ 1.2f, 0.0f, 3.0f } },
            {"gripWidth",               new List<object>(){ 0.1f, 0.0f, 1.0f } },
            {"guardSubdivision",        new List<object>(){ 50, 10, 100 } },
            {"guardLength",             new List<object>(){ 0.2f, 0.0f, 3.0f } },
            {"guardWidth",              new List<object>(){ 1.5f, 0.0f, 3.0f } },
            {"bladeSubdivision",        new List<object>(){ 50, 10, 100 } },
            {"bladeLengthGrow",         new List<object>(){ 0.2f, 0.0f, 3.0f } },
            {"bladeLengthGrowFactor",   new List<object>(){ 0.0f, 0.0f, 2.0f } },
            {"bladeWidth",              new List<object>(){ 1.2f, 0.0f, 3.0f } },
            {"bladeWidthFactorA",       new List<object>(){ 0.0f, 0.0f, 0.2f } },
            {"bladeWidthFactorB",       new List<object>(){ 0.0f, 0.0f, 1.0f } },
            {"bladeThick",              new List<object>(){ 0.0f, 0.0f, 2.0f } },
            {"bladeWaveFreq",           new List<object>(){ 0.0f, 0.0f, 5.0f } },
            {"bladeWaveAmp",            new List<object>(){ 0.0f, 0.0f, 5.0f } },
            {"bladeCurv",               new List<object>(){ 2.5f, -5.0f, 5.0f } },
            {"edgeRatio",               new List<object>(){ 0.15f, 0.0f, 1.0f } },
            {"maxIter",                 new List<object>(){ 47, 10, 80 } },
            {"spearGripWidth",          new List<object>(){ 0.6f, 0.0f, 3.0f } },
            {"spearGripLength",         new List<object>(){ 5.0f, 0.0f, 10.0f } },
            {"crescentL",               new List<object>(){ 0.75f, 0.0f, 1.5f } },
            {"crescentW",               new List<object>(){ 0.3f, 0.0f, 1.0f } },
            {"crescentD",               new List<object>(){ 0.6f, 0.0f, 0.999f } },
            {"crescentT",               new List<object>(){ 0.3f, 0.0f, 1.0f } }
        };

        // slider setup
        parameterNamePrefab = Resources.Load<Text>("Prefabs/Text");
        parameterSliderPrefab = Resources.Load<Slider>("Prefabs/ParameterSlider");
        parameters = new Dictionary<string, Selectable>();
        for (int i = 0; i < parametersSetup.Count; i++)
        {
            KeyValuePair<string, List<object>> pair = parametersSetup.ElementAt(i);
            Type pType = pair.Value[0].GetType();
            if (pType == typeof(float))
            {
                Text newText = Instantiate(parameterNamePrefab, parameterPanel.transform);
                newText.name = pair.Key;
                newText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-170, -i * 20);
                newText.text = pair.Key;

                Slider newSlider = Instantiate(parameterSliderPrefab, parameterPanel.transform);
                newSlider.name = pair.Key + "Value";
                newSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -i * 20);
                newSlider.maxValue = (float)(pair.Value[2]);
                newSlider.minValue = (float)pair.Value[1];
                newSlider.value = (float)pair.Value[0];
                if (pType == typeof(int)) newSlider.wholeNumbers = true;
                newSlider.onValueChanged.AddListener(delegate { mm.MakeWeapon(); });
                parameters.Add(pair.Key, newSlider);
            }
            else if (pType == typeof(int))
            {
                Text newText = Instantiate(parameterNamePrefab, parameterPanel.transform);
                newText.name = pair.Key;
                newText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-170, -i * 20);
                newText.text = pair.Key;

                Slider newSlider = Instantiate(parameterSliderPrefab, parameterPanel.transform);
                newSlider.name = pair.Key + "Value";
                newSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -i * 20);
                newSlider.maxValue = (int)(pair.Value[2]);
                newSlider.minValue = (int)pair.Value[1];
                newSlider.value = (int)pair.Value[0];
                if (pType == typeof(int)) newSlider.wholeNumbers = true;
                newSlider.onValueChanged.AddListener(delegate { mm.MakeWeapon(); });
                parameters.Add(pair.Key, newSlider);
            }
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

    void ChangeCurrentWeaponType(WeaponTypes type)
    {
        mm.meshReady = false;
        currentType = type;

        // set init params
        switch (type)
        {
            case WeaponTypes.刀:
                (parameters["gripLength"] as Slider).value = 0.35f;
                (parameters["gripWidth"] as Slider).value = 0.05f;
                (parameters["bladeLengthGrow"] as Slider).value = 0.4f;
                (parameters["bladeLengthGrowFactor"] as Slider).value = 1.0f;
                (parameters["bladeWidth"] as Slider).value = 0.2f;
                (parameters["bladeWidthFactorA"] as Slider).value = 0.016f;
                (parameters["bladeWidthFactorB"] as Slider).value = 1.0f;
                (parameters["bladeThick"] as Slider).value = 0.06f;
                (parameters["bladeCurv"] as Slider).value = 0.3f;
                (parameters["edgeRatio"] as Slider).value = 0.3f;
                break;
            case WeaponTypes.槍:
                (parameters["gripLength"] as Slider).value = 1.5f;
                (parameters["gripWidth"] as Slider).value = 0.025f;
                (parameters["bladeLengthGrow"] as Slider).value = 0.007f;
                (parameters["bladeLengthGrowFactor"] as Slider).value = 0.0f;
                (parameters["bladeWidth"] as Slider).value = 0.06f;
                (parameters["bladeWidthFactorA"] as Slider).value = 0.003f;
                (parameters["bladeWidthFactorB"] as Slider).value = 1.0f;
                (parameters["bladeThick"] as Slider).value = 0.045f;
                (parameters["bladeCurv"] as Slider).value = 0.0f;
                break;
            case WeaponTypes.劍:
                (parameters["gripLength"] as Slider).value = 0.35f;
                (parameters["gripWidth"] as Slider).value = 0.05f;
                (parameters["bladeLengthGrow"] as Slider).value = 0.4f;
                (parameters["bladeLengthGrowFactor"] as Slider).value = 1.0f;
                (parameters["bladeWidth"] as Slider).value = 0.075f;
                (parameters["bladeWidthFactorA"] as Slider).value = 0.004f;
                (parameters["bladeWidthFactorB"] as Slider).value = 1.0f;
                (parameters["bladeThick"] as Slider).value = 0.03f;
                (parameters["bladeCurv"] as Slider).value = 0.0f;
                break;
            case WeaponTypes.戟:
                (parameters["gripLength"] as Slider).value = 1.7f;
                (parameters["gripWidth"] as Slider).value = 0.05f;
                (parameters["bladeLengthGrow"] as Slider).value = 0.01f;
                (parameters["bladeLengthGrowFactor"] as Slider).value = 0.0f;
                (parameters["bladeWidth"] as Slider).value = 0.1f;
                (parameters["bladeWidthFactorA"] as Slider).value = 0.003f;
                (parameters["bladeWidthFactorB"] as Slider).value = 1.0f;
                (parameters["bladeThick"] as Slider).value = 0.045f;
                (parameters["bladeCurv"] as Slider).value = 0.0f;
                (parameters["edgeRatio"] as Slider).value = 0.4f;
                (parameters["crescentL"] as Slider).value = 0.4f;
                (parameters["crescentW"] as Slider).value = 0.33f;
                (parameters["crescentD"] as Slider).value = 0.45f;
                (parameters["crescentT"] as Slider).value = 0.1f;
                break;
            case WeaponTypes.斧:
                (parameters["gripLength"] as Slider).value = 1.5f;
                (parameters["gripWidth"] as Slider).value = 0.04f;
                (parameters["bladeWidth"] as Slider).value = 1.2f;
                (parameters["bladeCurv"] as Slider).value = 3.0f;
                (parameters["crescentL"] as Slider).value = 0.4f;
                (parameters["crescentW"] as Slider).value = 0.15f;
                (parameters["crescentD"] as Slider).value = 0.6f;
                (parameters["crescentT"] as Slider).value = 0.3f;
                break;
            case WeaponTypes.三尖刀:
                (parameters["gripLength"] as Slider).value = 1.25f;
                (parameters["gripWidth"] as Slider).value = 0.04f;
                (parameters["guardLength"] as Slider).value = 0.05f;
                (parameters["guardWidth"] as Slider).value = 0.11f;
                (parameters["bladeLengthGrow"] as Slider).value = 0.025f;
                (parameters["bladeLengthGrowFactor"] as Slider).value = 0.2f;
                (parameters["bladeWidth"] as Slider).value = 0.1f;
                (parameters["bladeWidthFactorA"] as Slider).value = 0.0015f;
                (parameters["bladeWidthFactorB"] as Slider).value = 1.0f;
                (parameters["bladeThick"] as Slider).value = 0.075f;
                (parameters["bladeCurv"] as Slider).value = 1.25f;
                break;
            default:
                break;
        }

        StartCoroutine(mm.AfterStart());
    }

    void ShowLSystemLine(Toggle change)
    {
        //parameters["showLineIsOn"][0] = showLine.isOn;
        mm.MakeWeapon();
    }
}
