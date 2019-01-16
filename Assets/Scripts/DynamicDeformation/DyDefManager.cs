using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using NeuralNetwork;
using System;

public class DyDefManager : MonoBehaviour
{
    public enum GameStatTypes
    {
        年齡,
        金幣,
        力量,
        敏捷,
        智力
    }

    public Button randomizeButton;
    public Toggle generateMode;
    public List<Slider> inputSliders;
    public List<GameStat> gameStatList;
    private List<double> inputValues, currentOutputValues, desiredOutputValues;

    public GameObject gameStatsPanel;
    public GameObject gameStatPrefab;

    /*** Neural Network Variables ***/
    private const double MinimumError = 0.01;
    private static NeuralNet net;
    private static List<DataSet> dataSets;
    /********************************/

    // Use this for initialization
    void Start()
    {
        inputValues = new List<double>();
        desiredOutputValues = new List<double>();

        randomizeButton.onClick.AddListener(delegate { OnRandomizeButtonClicked(); });

        gameStatList = new List<GameStat>();
        foreach (GameStatTypes item in Enum.GetValues(typeof(GameStatTypes)))
        {
            GameObject newGameStatObj = Instantiate(gameStatPrefab, gameStatsPanel.transform);
            newGameStatObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -120 - (int)item * 100);
            GameStat newGameStat = newGameStatObj.GetComponent<GameStat>();
            newGameStat.statName.text = item.ToString();
            gameStatList.Add(newGameStat);
        }

        foreach (var item in inputSliders)
        {
            item.onValueChanged.AddListener(delegate { UpdateCurrentValue(); });
        }

        /*** Neural Network ***/
        net = new NeuralNet(inputSliders.Count, 10, gameStatList.Count);
        dataSets = new List<DataSet>();
        /**********************/
    }

    // Update is called once per frame
    void Update()
    {
        if (generateMode.isOn)
        {
            ModifyInput();
            UpdateCurrentValue();
        }
    }

    public void ModifyInput()
    {
        // input values
        inputValues.Clear();
        for (int i = 0; i < inputSliders.Count; i++)
        {
            inputValues.Add(inputSliders[i].value);
        }

        // modify input
        desiredOutputValues.Clear();
        for (int i = 0; i < gameStatList.Count; i++)
        {
            desiredOutputValues.Add(gameStatList[i].getDesiredValue());
        }
        double[] modifiedInput = net.GenerateInput(inputValues.ToArray(), desiredOutputValues.ToArray());
        for (int i = 0; i < modifiedInput.Length; i++)
        {
            //inputSliders[i].value = ((float)modifiedInput[i] * parameterRange[i]) + inputSliders[i].minValue;
            inputSliders[i].value = (float)modifiedInput[i] + inputSliders[i].minValue;
        }
    }

    private void UpdateCurrentValue()
    {
        // input values
        inputValues.Clear();
        for (int i = 0; i < inputSliders.Count; i++)
        {
            inputValues.Add(inputSliders[i].value);
        }

        // update current values text
        double[] output = net.Compute(inputValues.ToArray());
        for (int i = 0; i < output.Length; i++)
        {
            gameStatList[i].setCurrentValue(output[i]);
        }
    }

    private void OnRandomizeButtonClicked()
    {
        foreach (var item in inputSliders)
        {
            item.value = UnityEngine.Random.Range(0.0f, 1.0f);
        }
    }
}
