using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : MonoBehaviour
{
    public ParticleSystem fallingWater;

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;
    }

    void OnMouseDown()
    {
        Debug.Log("spray water");
        fallingWater.gameObject.SetActive(true);
    }

    void OnMouseUp() 
    {
        fallingWater.gameObject.SetActive(false);
    }
}