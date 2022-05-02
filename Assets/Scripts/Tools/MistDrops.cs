using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistDrops : MonoBehaviour
{
    public float lifetime = 2.0f;
    public float velocity = 10.0f;
    public float waterAmount = 0.05f;

    void Start()
    {
        Invoke(nameof(DestroySelf), lifetime);
    }

    void Update()
    {
        transform.position += Vector3.left * velocity * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Plants"))
        {
            PlantStateManager plant = col.gameObject.GetComponent<PlantStateManager>();
            plant.waterPlant(waterAmount);
            DestroySelf();
        }
    }

    void DestroySelf() 
    {
        Destroy(gameObject);
    }
}
