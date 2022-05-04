using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Zoom : MonoBehaviour, IPointerClickHandler
{
    //float timerSinceLastClick = 0f;
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

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        // can zoom only if notes not open
        if (!gameUIManager.GetNotesStatus() && pointerEventData.clickCount == 2) 
        {
            if (!IsZoomed)
            {
                EventBus.Broadcast(EventTypes.ZoomedIn);

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
        }
    }
}
