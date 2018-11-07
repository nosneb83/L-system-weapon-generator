using UnityEngine;
using System.Collections;

public class Parameters : MonoBehaviour
{
    //private static Parameters _instance = null;
    //public static Parameters Instance
    //{
    //    get
    //    {
    //        if (_instance == null) _instance = FindObjectOfType(typeof(Parameters)) as Parameters;
    //        return _instance;
    //    }
    //}

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
        //if (Instance != this) Destroy(this);
        edgeCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.0f);
    }
}
