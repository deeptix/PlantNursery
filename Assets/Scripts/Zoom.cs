using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    float timerSinceLastClick = 0f;
    private GameUIManager gameUIManager;
    private PhoneManager phoneManager;

    public bool IsZoomed;

    private void Start()
    {
        gameUIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
        phoneManager = GameObject.Find("PhoneManager").GetComponent<PhoneManager>();

        IsZoomed = false;
        gameUIManager.HideToolbar();
        phoneManager.ActivateCameraButton();
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
                if (!IsZoomed)
                {
                    Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
                    Camera.main.orthographicSize = 2.5f;
                    gameUIManager.ShowToolbar();
                    phoneManager.DeactivateCameraButton();
                    phoneManager.HidePhoneButton(); // TODO: fix UI so this isn't disabled
                    IsZoomed = true;
                }
                else
                {
                    Camera.main.transform.position = new Vector3(0, 0, -10);
                    Camera.main.orthographicSize = 5;
                    gameUIManager.HideToolbar();
                    phoneManager.ActivateCameraButton();
                    phoneManager.ShowPhoneButton(); // TODO: fix UI so this isn't disabled
                    IsZoomed = false;
                }
                timerSinceLastClick = 0f; // reset timer
            }
        }
    }
}
