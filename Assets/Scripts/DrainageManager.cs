using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainageManager : MonoBehaviour
{
    public ParticleSystem drainingWater;

    public void turnDrainageOn() {
        drainingWater.Emit(1);
        drainingWater.Play();
    }
}
