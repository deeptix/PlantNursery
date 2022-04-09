using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOneDayManager : MonoBehaviour
{
    public PlantStateManager cactusManager;

    void Awake() {
        cactusManager = GameObject.Find("cactus").GetComponent<PlantStateManager>();
    }

    public void OnButtonPress(){
      cactusManager.passTime(1);
    }
}
