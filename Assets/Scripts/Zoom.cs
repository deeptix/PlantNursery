using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    float timerSinceLastClick = 0f;
    private GameUIManager gameUIManager;
    private PhoneManager phoneManager;

    private void Start()
    {
        gameUIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
        phoneManager = GameObject.Find("PhoneManager").GetComponent<PhoneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerSinceLastClick != 0)
            timerSinceLastClick = Mathf.Clamp(timerSinceLastClick - Time.deltaTime, 0, 1f);        
    }

    // Clicked on plant
    void OnMouseDown()
    {
        // can zoom only if notes not open
        if (!gameUIManager.GetNotesStatus())
        {
            if (timerSinceLastClick == 0)
                timerSinceLastClick = 0.2f; // start timer
            else // double clicked 
            {
                if (Camera.main.orthographicSize > 1)
                {
                    Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
                    Camera.main.orthographicSize = 1;
                    gameUIManager.HideToolbar();
                    phoneManager.DeactivateCameraButton();
                }
                else
                {
                    Camera.main.transform.position = new Vector3(0, 0, -10);
                    Camera.main.orthographicSize = 5;
                    gameUIManager.ShowToolbar();
                    phoneManager.ActivateCameraButton();
                }
                timerSinceLastClick = 0f; // reset timer
            }
        }
    }
}
