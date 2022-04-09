using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipDay : MonoBehaviour
{
    public Animator celestialAnimator;
    public Animator dawnSkyAnimator;
    public Animator blueSkyAnimator;
    GameObject GameManager;

    bool animationStarted;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        this.GetComponent<Button>().onClick.AddListener(IterateDate);

        Initialize();
    }

    public void Initialize()
    {
        Time.timeScale = 0;
        animationStarted = false;
        Time.timeScale = 1;
    }

    void Update()
    {
        if (dawnSkyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dawn")) animationStarted = true;

        IsAnimationFinished();
    }

    public void IterateDate()
    {
        celestialAnimator.SetTrigger("Start");
        dawnSkyAnimator.SetTrigger("Start");
        blueSkyAnimator.SetTrigger("Start");

        // pause user actions and wait for day/night animation to finish
        GameManager.GetComponent<Calendar>().VoidInteractables();
        Time.timeScale = 0;
    }

    void IsAnimationFinished() 
    {
        if (animationStarted && dawnSkyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Do Nothing"))
        {
            System.DateTime day = GameManager.GetComponent<Calendar>().GetCurrentDate();
            GameManager.GetComponent<Calendar>().SetCurrentDate(day.AddDays(1));
            GameManager.GetComponent<Calendar>().SetInteractables();
            Time.timeScale = 1;
            Initialize();
        }
    }
}
