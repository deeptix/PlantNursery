using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Calendar : MonoBehaviour
{
    public Text[] daysOfWeek;
    public Button nextButton;
    public Button prevButton;
    public Button dayButton;
    public float buttonSpacing = 50;
    public Text monthText;
    public int numberOfSkipDays = 2;

    System.DateTime currentDate;
    System.DateTime firstDay;
    float fixedSpacing;

    // Start is called before the first frame update
    private void Start()
    {
        nextButton.onClick.AddListener(NextMonth);
        prevButton.onClick.AddListener(PrevMonth);
        currentDate = System.DateTime.Now;

        firstDay = currentDate.AddDays(-(currentDate.Day - 1));

        MakeMonthlyCalendar();
    }

    private void Update()
    {
        // Current day stays selected
        EventSystem.current.SetSelectedGameObject(GameObject.Find(currentDate.ToString()));
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

        // interate days until the month changes
        while (i.Month == firstDay.Month)
        {
            //make button
            Transform parent = daysOfWeek[((int)i.DayOfWeek)].transform;
            Button newDayButton = Instantiate(dayButton, parent) as Button;

            // adjust row placement
            GameObject canvas = GameObject.Find("Canvas");
            GameObject calendarPanel = GameObject.Find("Calendar");
            fixedSpacing = buttonSpacing * canvas.GetComponent<Canvas>().scaleFactor * calendarPanel.GetComponent<RectTransform>().localScale.y;
            newDayButton.gameObject.transform.Translate(0, - (fixedSpacing * 
                ( ( ((int)firstDay.DayOfWeek + i.Day - 1) / 7 ) + 1) ), 0);

            // set button text to day number
            newDayButton.gameObject.transform.GetChild(0).GetComponent<Text>().text = i.Day.ToString();
            newDayButton.gameObject.name = i.ToString();

            // change day
            i = i.AddDays(1);
            //Debug.Log(i);

            SetInteractables();
        }
        
    }

    public System.DateTime GetCurrentDate()
    {
        return currentDate;
    }

    public void SetCurrentDate(System.DateTime date) {
        currentDate = date;
    }


    public void SetInteractables() 
    {
        //set interactable buttons
        GameObject today = GameObject.Find(currentDate.ToString());
        if (today != null) today.GetComponent<Button>().interactable = true;

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
        if (today != null) today.GetComponent<Button>().interactable = false;

        for (int i = 1; i <= numberOfSkipDays; i++)
        {
            GameObject nextDay = GameObject.Find(currentDate.AddDays(i).ToString());
            if (nextDay != null) nextDay.GetComponent<Button>().interactable = false;
        }
    }
}
