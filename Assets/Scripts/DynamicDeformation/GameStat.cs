using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStat : MonoBehaviour
{
    public Toggle statToggle;
    public Text statName;
    public Text currentValueText;
    public InputField desiredValueInputField;
    private double desiredValue;

    // Use this for initialization
    void Start()
    {
        currentValueText.text = "0.0";
        desiredValueInputField.text = "5.0";
        desiredValueInputField.onEndEdit.AddListener(delegate { ValidateInputString(desiredValueInputField); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ValidateInputString(InputField input)
    {
        if (!double.TryParse(input.text, out desiredValue))
        {
            Debug.Log("Invalid Input Format");
        }
    }

    public double getDesiredValue()
    {
        return desiredValue / 10.0;
    }

    public void setCurrentValue(double currentValue)
    {
        currentValueText.text = (currentValue * 10).ToString("F1");
    }
}
