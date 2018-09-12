using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KnifeParametric
{
    public static List<Symbol> RewriteString(List<Symbol> inputString)
    {
        int circleSubdivision = 50;

        List<Symbol> newString = new List<Symbol>();
        foreach (var item in inputString)
        {
            switch (item.s)
            {
                case "Knife":
                    newString.Add(new Symbol("Pommel", new object[] { 1.5f, 1.0f }));
                    newString.Add(new Symbol("Grip", new object[] {  }));
                    newString.Add(new Symbol("Guard", new object[] {  }));
                    newString.Add(new Symbol("Blade", new object[] {  }));
                    break;
                case "Pommel": // outerDiameter, innerDiameter
                    float PommelRadius = ((float)item.p[0] + (float)item.p[1]) / 4.0f;
                    float PommelRingRadius = ((float)item.p[0] - (float)item.p[1]) / 4.0f;

                    newString.Add(new Symbol("F", new object[] { PommelRadius + PommelRingRadius })); // move to the center
                    newString.Add(new Symbol("{", new object[] { })); // start submesh

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
                    newString.Add(new Symbol("F", new object[] { PommelRingRadius })); // move to the end point
                    break;
                case "Grip":
                    break;
                case "Guard":
                    break;
                case "Blade":
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

                    float bladeWidth = 1.2f * (50 - t) / 35;
                    float bladeThick = 0.2f * (50 - t) / 35;
                    float edgeRatio = 0.15f;
                    float guardDiameter = 0.8f;

                    if (t < 11)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.4f, 0 })); // age, angle, distance ratio, texture
                        newString.Add(new Symbol("V", new object[] { 1, 45.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 135.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 180.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 225.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 270.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 315.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("]", new object[] { }));
                        newString.Add(new Symbol("F", new object[] { 1.0f }));
                    }
                    else if (t == 11)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.4f, 0 })); // age, angle, distance ratio, texture
                        newString.Add(new Symbol("V", new object[] { 1, 45.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 135.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 180.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 225.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 270.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("V", new object[] { 1, 315.0f, 0.4f, 0 }));
                        newString.Add(new Symbol("]", new object[] { }));
                    }
                    else if (t == 12)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 45.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 135.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 180.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 225.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 270.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 315.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("]", new object[] { }));
                        newString.Add(new Symbol("F", new object[] { 0.3f }));
                    }
                    else if (t == 13)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 45.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 135.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 180.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 225.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 270.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 315.0f, guardDiameter, 1 }));
                        newString.Add(new Symbol("]", new object[] { }));
                    }
                    else if (t == 14)
                    {
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(90.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { 0.55f }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                    }
                    else if (t == 15)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.0f, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 1 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * 0.7f }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 1 }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * 0.3f }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.0f, 1 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(180.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * 0.3f }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 1 }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * 0.7f }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 1 }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 1 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                        newString.Add(new Symbol("]", new object[] { }));
                    }
                    else if (t == 16)
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.0f, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * (1 - edgeRatio) }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 3 }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * edgeRatio }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.0f, 4 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(180.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * edgeRatio }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 3 }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * (1 - edgeRatio) }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                        newString.Add(new Symbol("]", new object[] { }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(0.3f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { 5.0f / Mathf.Pow(t - 15, 1) }));
                    }
                    else
                    {
                        newString.Add(new Symbol("[", new object[] { }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.0f, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * (1 - edgeRatio) }));
                        newString.Add(new Symbol("V", new object[] { 1, 90.0f, bladeThick, 3 }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * edgeRatio }));
                        newString.Add(new Symbol("V", new object[] { 1, 0.0f, 0.0f, 4 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(180.0f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * edgeRatio }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 3 }));
                        newString.Add(new Symbol("F", new object[] { bladeWidth * (1 - edgeRatio) }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("V", new object[] { 1, -90.0f, bladeThick, 2 }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(-90.0f, Vector3.up) }));
                        newString.Add(new Symbol("]", new object[] { }));
                        newString.Add(new Symbol("+", new object[] { Quaternion.AngleAxis(0.3f, Vector3.up) }));
                        newString.Add(new Symbol("F", new object[] { 5.0f / Mathf.Pow(t - 15, 1) }));
                    }
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
