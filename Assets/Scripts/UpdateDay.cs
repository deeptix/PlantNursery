using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpdateDay : MonoBehaviour, IPointerClickHandler
{
    GameObject GameManager;
    //GameObject skipButton;
    System.DateTime day;

    void Start()
    { 
        GameManager = GameObject.Find("Game Manager");

        // Have sped up animations (multiple) for passing days
        /*skipButton = GameObject.Find("Skip Day Button");

        skipButton.GetComponent<SkipDay>().celestialAnimator.speed = 2;
        skipButton.GetComponent<SkipDay>().dawnSkyAnimator.speed = 2;
        skipButton.GetComponent<SkipDay>().blueSkyAnimator.speed = 2;*/
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) 
    {
        if (this.gameObject.GetComponent<Button>().IsInteractable())
        {
            if (System.DateTime.TryParse(this.gameObject.name, out day))
            {
                GameManager.GetComponent<Calendar>().VoidInteractables();
                /*(System.TimeSpan interval = day - GameManager.GetComponent<Calendar>().GetCurrentDate();
                skipButton.GetComponent<SkipDay>().Initialize();
                for (int i = 0; i < (int)interval.TotalDays; i++)
                {
                    skipButton.GetComponent<SkipDay>().IterateDate();
                }*/
                GameManager.GetComponent<Calendar>().SetCurrentDate(day);
                GameManager.GetComponent<Calendar>().SetInteractables();
                EventSystem.current.SetSelectedGameObject(GameObject.Find(day.ToString()));
            }
        }
    }
}
