using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KnifeParametric : MonoBehaviour
{
    public int circleSubdivision = 50;
    public float pommelOuterDiameter = 1.5f;
    public float pommelInnerDiameter = 0.5f;
    public float gripLength = 2.0f;
    public float gripWidth = 0.5f;
    public float guardLength = 0.2f;
    public float guardWidth = 1.5f;
    public float bladeLengthGrow = 5.0f;
    [Range(0.0f, 2.0f)]
    public float bladeLengthGrowFactor = 1.0f;
    public float bladeWidth = 1.2f;
    [Range(0.0f, 0.2f)]
    public float bladeWidthFactorA = 0.2f;
    [Range(0.0f, 1.0f)]
    public float bladeWidthFactorB = 0.0f;
    public float bladeThick = 0.2f;
    [Range(0.3f, 3.0f)]
    public float bladeCurv = 0.3f;
    [Range(0.1f, 0.3f)]
    public float edgeRatio = 0.15f;
    [Range(10, 80)]
    public int maxIter = 47;

    public List<Symbol> RewriteString(List<Symbol> inputString)
    {
        List<Symbol> newString = new List<Symbol>();
        foreach (var item in inputString)
        {
            switch (item.s)
            {
                case "Knife":
                    newString.Add(new Symbol("Pommel", new object[] { pommelOuterDiameter, pommelInnerDiameter }));
                    newString.Add(new Symbol("Grip", new object[] { gripWidth / 2.0f }));
                    newString.Add(new Symbol("Guard", new object[] { guardWidth / 2.0f }));
                    newString.Add(new Symbol("Blade", new object[] { }));
                    break;

                case "Pommel": // outerDiameter, innerDiameter
                    float PommelRadius = ((float)item.p[0] + (float)item.p[1]) / 4.0f;
                    float PommelRingRadius = ((float)item.p[0] - (float)item.p[1]) / 4.0f;

                    newString.Add(new Symbol("F", new object[] { 2 * PommelRadius + PommelRingRadius }));
                    newString.Add(new Symbol("{", new object[] { })); // start submesh
                    newString.Add(new Symbol("F", new object[] { -PommelRadius }));
                    for (int i = 0; i < circleSubdivision; i++) // go along the ring
                    {
                        newString.Add(new Symbol("F", new object[] { PommelRadius }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(90.0f, Vector3.up) }));
                        newString.Add(new Symbol("Circle", new object[] { PommelRingRadius }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { -PommelRadius }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(360.0f / circleSubdivision, Vector3.up) }));
                    }
                    newString.Add(new Symbol("F", new object[] { PommelRadius })); // last circle
                    newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(90.0f, Vector3.up) }));
                    newString.Add(new Symbol("Circle", new object[] { PommelRingRadius }));
                    newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                    newString.Add(new Symbol("}", new object[] { })); // end submesh
                    break;

                case "Grip":
                    newString.Add(new Symbol("{", new object[] { })); // start submesh
                    newString.Add(new Symbol("Circle", new object[] { item.p[0] }));
                    newString.Add(new Symbol("F", new object[] { gripLength }));
                    newString.Add(new Symbol("Circle", new object[] { item.p[0] }));
                    newString.Add(new Symbol("}", new object[] { })); // end submesh
                    break;

                case "Guard":
                    newString.Add(new Symbol("{", new object[] { })); // start submesh
                    newString.Add(new Symbol("Circle", new object[] { item.p[0] }));
                    newString.Add(new Symbol("F", new object[] { guardLength }));
                    newString.Add(new Symbol("Circle", new object[] { item.p[0] }));
                    newString.Add(new Symbol("}", new object[] { })); // end submesh
                    break;

                case "Blade":
                    newString.Add(new Symbol("{", new object[] { })); // start submesh
                    newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(90.0f, Vector3.up) }));
                    newString.Add(new Symbol("F", new object[] { bladeWidth / 2.0f }));
                    newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                    newString.Add(new Symbol("P", new object[] { 0 })); // start submesh
                    newString.Add(new Symbol("}", new object[] { })); // end submesh
                    break;

                case "Circle": // radius
                    newString.Add(new Symbol("[", new object[] { }));
                    for (int i = 0; i < circleSubdivision; i++)
                    {
                        newString.Add(new Symbol("V", new object[] { 1, 360.0f * i / circleSubdivision, item.p[0], 0 }));
                    }
                    newString.Add(new Symbol("]", new object[] { }));
                    break;

                case "P": // point
                    int t = (int)item.p[0];
                    float sectionWidth = (bladeWidth + t * bladeWidthFactorA) * (maxIter - t * bladeWidthFactorB) / maxIter;
                    float sectionThick = bladeThick * (maxIter - t * bladeWidthFactorB) / maxIter;

                    newString.Add(new Symbol("[", new object[] { }));
                    newString.Add(new Symbol("V", new object[] { 1, 90.0f, sectionThick, 2 }));
                    newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                    newString.Add(new Symbol("F", new object[] { sectionWidth * (1 - edgeRatio) }));
                    newString.Add(new Symbol("V", new object[] { 1, 90.0f, sectionThick, 3 }));
                    newString.Add(new Symbol("F", new object[] { sectionWidth * edgeRatio }));
                    newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.0f, 4 }));
                    newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(180.0f, Vector3.up) }));
                    newString.Add(new Symbol("F", new object[] { sectionWidth * edgeRatio }));
                    newString.Add(new Symbol("V", new object[] { 1, -90.0f, sectionThick, 3 }));
                    newString.Add(new Symbol("F", new object[] { sectionWidth * (1 - edgeRatio) }));
                    newString.Add(new Symbol("V", new object[] { 1, -90.0f, sectionThick, 2 }));
                    newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                    newString.Add(new Symbol("]", new object[] { }));
                    newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(bladeCurv, Vector3.up) }));
                    if (t < maxIter)
                        newString.Add(new Symbol("F", new object[] { bladeLengthGrow / Mathf.Pow(t + 1, bladeLengthGrowFactor) }));

                    item.p[0] = t + 1; // P(t) -> P(t+1)
                    newString.Add(item);
                    break;
                case "V": // vertex
                    item.p[0] = (int)item.p[0] + 1; // V(t,...) -> V(t+1,...)
                    newString.Add(item);
                    break;
                default:
                    newString.Add(item);
                    break;
            }
        }

        return newString;
    }
}
