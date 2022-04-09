using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Thermostat : MonoBehaviour
{
    public Button increaseTemp;
    public Button decreaseTemp;
    public Text tempText;

    // Start is called before the first frame update
    void Start()
    {
        increaseTemp.onClick.AddListener(UpTemp);
        decreaseTemp.onClick.AddListener(LowerTemp);
    }

    void UpTemp() 
    {
        tempText.text = (Int32.Parse(tempText.text) + 1).ToString();
    }

    void LowerTemp()
    {
        tempText.text = (Int32.Parse(tempText.text) - 1).ToString();
    }
}
