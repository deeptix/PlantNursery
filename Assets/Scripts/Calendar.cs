using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Calendar : MonoBehaviour
{
    public TMP_Text[] daysOfWeek;
    public Button nextButton;
    public Button prevButton;
    public Button dayButton;

    public TMP_Text monthText;
    public int numberOfSkipDays = 2;

    [Header("Animation Settings")]
    public Animator celestialAnimator;
    public Animator dawnSkyAnimator;
    public Animator blueSkyAnimator;
    public int animationSpeed = 2;

    System.DateTime currentDate;
    System.DateTime firstDay;
    float fixedSpacing;
    bool animationStarted;
    int daysPassed;

    // Start is called before the first frame update
    private void Awake()
    {
        nextButton.onClick.AddListener(NextMonth);
        prevButton.onClick.AddListener(PrevMonth);
        currentDate = System.DateTime.Now;

        firstDay = currentDate.AddDays(-(currentDate.Day - 1));

        MakeMonthlyCalendar();
        InitializeAnim();
    }

    public void InitializeAnim()
    {
        Time.timeScale = 0;
        celestialAnimator.speed = animationSpeed;
        dawnSkyAnimator.speed = animationSpeed;
        blueSkyAnimator.speed = animationSpeed;
        animationStarted = false;
        Time.timeScale = 1;
    }

    void Update()
    {
        if (dawnSkyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dawn")) animationStarted = true;
        IsAnimationFinished();
    }

    void NextMonth()
    {
        firstDay = firstDay.AddMonths(1);
        DestroyCalendar();
        MakeMonthlyCalendar();
    }

    void PrevMonth()
    {
        firstDay = firstDay.AddMonths(-1);
        DestroyCalendar();
        MakeMonthlyCalendar();
    }

    void DestroyCalendar()
    {
        GameObject[] days = GameObject.FindGameObjectsWithTag("Day");
        foreach (GameObject day in days) {
            Destroy(day);
        }
    }

    void MakeMonthlyCalendar()
    {
        monthText.text = firstDay.ToString("MMMM yyyy");

        var i = firstDay;

        for (int j = 0; j < (int)i.DayOfWeek; j++) {
            Transform parent = daysOfWeek[j].transform;
            Button newDayButton = Instantiate(dayButton, parent) as Button;
            newDayButton.GetComponent<Image>().enabled = false;
            newDayButton.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
        }

        // interate days until the month changes
        while (i.Month == firstDay.Month)
        {
            //make button
            Transform parent = daysOfWeek[((int)i.DayOfWeek)].transform;
            Button newDayButton = Instantiate(dayButton, parent) as Button;

            // adjust row placement
            GameObject canvas = GameObject.Find("Canvas");
            GameObject calendarPanel = GameObject.Find("Calendar");

            // set button text to day number
            newDayButton.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = i.Day.ToString();
            newDayButton.gameObject.name = i.ToString();

            // change day
            i = i.AddDays(1);

            SetInteractables();
        }
    }

    public System.DateTime GetCurrentDate()
    {
        return currentDate;
    }

    public void SetCurrentDate(System.DateTime date) {
        if (date == currentDate) return;
        IterateDate(date.Subtract(currentDate).Days);
        currentDate = date;
    }

    public void SetInteractables() 
    {
        //Set interactable buttons
        GameObject today = GameObject.Find(currentDate.ToString());
        if (today != null) {
            today.GetComponent<Button>().interactable = true;
            ColorBlock colorVar = today.GetComponent<Button>().colors;
            colorVar.normalColor = new Color(126f/255f, 181f/255f, 236f/255f);
            colorVar.highlightedColor = new Color(126f/255f, 181f/255f, 236f/255f);
            colorVar.pressedColor = new Color(126f/255f, 181f/255f, 236f/255f);
            today.GetComponent<Button>().colors = colorVar;
        }

        for (int i = 1; i <= numberOfSkipDays; i++)
        {
            GameObject nextDay = GameObject.Find(currentDate.AddDays(i).ToString());
            if (nextDay != null) nextDay.GetComponent<Button>().interactable = true;
        }
    }


    public void VoidInteractables() 
    {
        //set interactable buttons
        GameObject today = GameObject.Find(currentDate.ToString());
        if (today != null) {
            today.GetComponent<Button>().interactable = false;
            ColorBlock colorVar = today.GetComponent<Button>().colors;
            colorVar.normalColor = Color.white;
            today.GetComponent<Button>().colors = colorVar;
        }

        for (int i = 1; i <= numberOfSkipDays; i++)
        {
            GameObject nextDay = GameObject.Find(currentDate.AddDays(i).ToString());
            if (nextDay != null) nextDay.GetComponent<Button>().interactable = false;
        }
    }

    public void IterateDate(int daysPassed)
    {
        this.daysPassed = daysPassed;

        celestialAnimator.SetTrigger("Start");
        dawnSkyAnimator.SetTrigger("Start");
        blueSkyAnimator.SetTrigger("Start");

        // pause user actions and wait for day/night animation to finish
        VoidInteractables();
        Time.timeScale = 0;
    }

    void IsAnimationFinished() 
    {
        if (animationStarted && dawnSkyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Do Nothing"))
        {
            SetInteractables();
            EventBus.Broadcast<int>(EventTypes.DayPassed, this.daysPassed);
            Time.timeScale = 1;
            InitializeAnim();
        }
    }
}
