using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPhone : MonoBehaviour
{
    public GameObject SearchText;
    public GameObject PlantNameText;
    public GameObject SearchButton;


    private bool isDragging;
    private Vector3 mouseOffsetPos;

    public void OnMouseDown()
    {
        isDragging = true;
        Vector3 currentPos = new Vector3(transform.position.x, transform.position.y, 0);
        mouseOffsetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - currentPos;
    }

    public void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - (transform.position + mouseOffsetPos);
            transform.Translate(mousePosition);
        }
    }
}
