using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource SFXPlayer;

    [Header("SFX Clips")]
    public AudioClip soil;
    public AudioClip soilSelect;
    public AudioClip fertilizer;
    public AudioClip fertilizerSelect;
    public AudioClip water;
    public AudioClip waterSelect;
    public AudioClip mist;
    public AudioClip mistSelect;

    public void PlaySoilSound() {
        SFXPlayer.clip = soil;
        SFXPlayer.loop = false;
        SFXPlayer.Play();
    }

    public void PlaySoilSelectSound() {
        SFXPlayer.clip = soilSelect;
        SFXPlayer.loop = false;
        SFXPlayer.Play();
    }

    public void PlayFertilizerSound() {
        SFXPlayer.clip = fertilizer;
        SFXPlayer.loop = false;
        SFXPlayer.Play();
    }

    public void StopFertilizerSound() {
        SFXPlayer.loop = false;
        SFXPlayer.Stop();
    }

    public void PlayFertilizerSelectSound() {
        SFXPlayer.clip = fertilizerSelect;
        SFXPlayer.loop = false;
        SFXPlayer.Play();
    }

    public void PlayWaterSound() {
        SFXPlayer.clip = water;
        SFXPlayer.loop = true;
        SFXPlayer.Play();
    }

    public void StopWaterSound() {
        SFXPlayer.loop = false;
        SFXPlayer.Stop();
    }

    public void PlayWaterSelectSound() {
        SFXPlayer.clip = waterSelect;
        SFXPlayer.loop = false;
        SFXPlayer.Play();
    }

    public void PlayMistSound() {
        SFXPlayer.clip = mist;
        SFXPlayer.loop = false;
        SFXPlayer.Play();
    }

    public void PlayMistSelectSound() {
        SFXPlayer.clip = mistSelect;
        SFXPlayer.loop = false;
        SFXPlayer.Play();
    }
}
