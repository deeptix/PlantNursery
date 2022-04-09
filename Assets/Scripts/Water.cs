using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public float rotationAngle = 40;
    public ParticleSystem fallingWater;

    BoxCollider2D wateringCanCollider;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        wateringCanCollider = this.gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;
        Quaternion originalRotation = fallingWater.transform.rotation;

        // TODO: check if falling water is colliding with plant
        // nikita: i used fallingWater.transform.position but I don't think this is accurate
        LayerMask mask = LayerMask.GetMask("Plants");
        RaycastHit2D hit = Physics2D.Raycast(fallingWater.transform.position, Vector2.zero, Mathf.Infinity, mask);

        if (hit.collider != null)
        {
            PlantStateManager plant = hit.transform.gameObject.GetComponent<PlantStateManager>();
            plant.waterPlant(Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer("Plants")) return;

        // rotate can clockwise
        rb.SetRotation(rotationAngle);

        fallingWater.gameObject.SetActive(true);

        // TODO: send event that watering is happening
        // Color currentColor = soil.GetComponent<SpriteRenderer>().color;
        // soil.GetComponent<SpriteRenderer>().color = Color.Lerp(currentColor, Color.black, Mathf.PingPong(Time.time, 1));
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer("Plants")) return;

        // rotate can to default angle
        rb.SetRotation(0);

        //fallingWater.transform.rotation = originalRotation;
        fallingWater.gameObject.SetActive(false);
    }
}
