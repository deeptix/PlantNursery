using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Date : MonoBehaviour
{
    public Button nextButton;
    public Button prevButton;
    public Text monthText;
    int currMonth = 0;
    string[] months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    // create variable for date

    // Start is called before the first frame update
    void Start()
    {
        // set initial button press
        // assign date var to pressed button (e.g. first day of month)
    }

    // Update is called once per frame
    void Update()
    {
        nextButton.onClick.AddListener(GetNextMonth);

        // change variable to other date (e.g. button.pressed)
    }

    void GetNextMonth() {
        monthText.text = months[currMonth];
    }

    void GetPrevMonth() {
        if (currMonth == 0) {
            currMonth = currMonth
        }
    }
}
