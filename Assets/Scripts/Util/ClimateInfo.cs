using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Climate 
{
    public string regionInfo;       // English description of region info
    public string temperatureInfo;  // English description of temperature info
    public string humidityInfo;     // English description of humidity info
}

[System.Serializable]
public class Climates 
{
    public Climate[] climates;
}
