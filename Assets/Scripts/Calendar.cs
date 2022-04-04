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
    public float spacing;
    public Text monthText;

    System.DateTime currentDate;
    System.DateTime firstDay;

    // Start is called before the first frame update
    private void Start()
    {
        nextButton.onClick.AddListener(NextMonth);
        prevButton.onClick.AddListener(PrevMonth);
        currentDate = System.DateTime.Now;

        firstDay = currentDate.AddDays(-(currentDate.Day - 1));

        MakeMonthlyCalendar();

        EventSystem.current.firstSelectedGameObject = GameObject.Find(currentDate.ToString()); ;
    }

    private void Update()
    {
        // fix!!
        // have current date updated on date select and change interactables respectively
        /*if ()
        {
            VoidInteractables();
            System.DateTime.TryParse(EventSystem.current.currentSelectedGameObject.name, out currentDate);
            SetInteractables();
        }*/
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
            newDayButton.gameObject.transform.Translate(0, - (spacing * 
                ( ( ((int)firstDay.DayOfWeek + i.Day - 1) / 7 ) + 1 ) ), 0);

            // set button text to day number
            newDayButton.gameObject.transform.GetChild(0).GetComponent<Text>().text = i.Day.ToString();
            newDayButton.gameObject.name = i.ToString();

            // change day
            i = i.AddDays(1);
            //Debug.Log(i);

            SetInteractables();
        }
        
    }


    void SetInteractables() 
    {
        //set interactable buttons
        GameObject today = GameObject.Find(currentDate.ToString());
        if (today != null) today.GetComponent<Button>().interactable = true;

        GameObject nextDay = GameObject.Find(currentDate.AddDays(1).ToString());
        if (nextDay != null) nextDay.GetComponent<Button>().interactable = true;
        GameObject nextToNextDay = GameObject.Find(currentDate.AddDays(2).ToString());
        if (nextToNextDay != null) nextToNextDay.GetComponent<Button>().interactable = true;
    }


    void VoidInteractables() 
    {
        //set interactable buttons
        GameObject today = GameObject.Find(currentDate.ToString());
        if (today != null) today.GetComponent<Button>().interactable = false;

        GameObject nextDay = GameObject.Find(currentDate.AddDays(1).ToString());
        if (nextDay != null) nextDay.GetComponent<Button>().interactable = false;
        GameObject nextToNextDay = GameObject.Find(currentDate.AddDays(2).ToString());
        if (nextToNextDay != null) nextToNextDay.GetComponent<Button>().interactable = false;
    }
}
