using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public float rotationAngle = 40;
    public GameObject sprout;
    public GameObject soil;
    public ParticleSystem fallingWater;

    BoxCollider2D wateringCanCollider;
    BoxCollider2D seedlingCollider;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        wateringCanCollider = this.gameObject.GetComponent<BoxCollider2D>();
        seedlingCollider = sprout.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;
        Quaternion originalRotation = fallingWater.transform.rotation;

        if (wateringCanCollider.IsTouching(seedlingCollider))
        {
            // rotate can clockwise
            rb.SetRotation(rotationAngle);

            fallingWater.gameObject.SetActive(true);
            Color currentColor = soil.GetComponent<SpriteRenderer>().color;
            soil.GetComponent<SpriteRenderer>().color = Color.Lerp(currentColor, Color.black, Mathf.PingPong(Time.time, 1));

        }
        else {
            // rotate can to default angle
            rb.SetRotation(0);

            //fallingWater.transform.rotation = originalRotation;
            fallingWater.gameObject.SetActive(false);
        }
    }
}
