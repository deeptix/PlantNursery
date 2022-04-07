using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour
{

    public Text climateText;
    // change plant per level with variable here

    // Start is called before the first frame update
    void Start()
    {
        climateText.text = "Climate Information\n\nRegion: Arizona\nTemperature: 90ºF\nHumidity: 85%";
    }

    void OnEnabled() {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
