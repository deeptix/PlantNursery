using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : MonoBehaviour
{
    public ParticleSystem fallingWater;

    private SoundManager soundManager;

    void Start() {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;

    }

    void OnMouseDown()
    {
        //Debug.Log("spray water");
        fallingWater.gameObject.SetActive(true);
        soundManager.PlayMistSound();
    }

    void OnMouseUp() 
    {
        fallingWater.gameObject.SetActive(false);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButton(0)) {
            LayerMask mask = LayerMask.GetMask("Plants");
            RaycastHit2D hit = Physics2D.Raycast(fallingWater.transform.position, Vector2.zero, Mathf.Infinity, mask);
 
            if (hit.collider != null)
            {
                PlantStateManager plant = hit.transform.gameObject.GetComponent<PlantStateManager>();
                plant.waterPlant(Time.deltaTime / 4);
            }
        }
    }
}
