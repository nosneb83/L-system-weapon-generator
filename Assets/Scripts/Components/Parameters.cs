using UnityEngine;
using System.Collections;

public class Parameters : MonoBehaviour
{
    public int circleSubdivision = 50;
    public float pommelOuterDiameter = 1.5f;
    public float pommelInnerDiameter = 0.5f;
    public float gripLength = 2.0f;
    public float gripWidth = 0.5f;
    public float guardLength = 0.2f;
    public float guardWidth = 1.5f;
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
    public float bladeCurv = 0.0f;
    [Range(0.0f, 1.0f)]
    public float edgeRatio = 0.15f;
    [Range(10, 80)]
    public int maxIter = 47;

    [Range(0.0f, 5.0f)]
    public float crescentL = 1.0f;
    [Range(0.0f, 5.0f)]
    public float crescentW = 1.0f;
    [Range(0.0f, 0.999f)]
    public float crescentD = 0.6f;
    [Range(0.0f, 1.0f)]
    public float crescentT = 0.3f;

    // spear edge curve
    public AnimationCurve edgeCurve;

    /*** Spear parameters ***/

    public float spearGripWidth = 0.6f;
    public float spearGripLength = 5.0f;

    // Use this for initialization
    void Start()
    {
        edgeCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.0f);
    }
}
