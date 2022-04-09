using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trim : MonoBehaviour
{
    public GameObject sprout;

    BoxCollider2D shearsCollider;
    BoxCollider2D sproutCollider;

    // Start is called before the first frame update
    void Start()
    {
        shearsCollider = this.GetComponent<BoxCollider2D>();
        sproutCollider = sprout.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shearsCollider.IsTouching(sproutCollider)) { 
            // cut object (should splice and piece should fall)
            // play cut sound
        }
    }
}
