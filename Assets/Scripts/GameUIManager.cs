using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public GameObject toolbarUI;
    bool isNoteOpen = false;

    public void SetNotesStatus(bool notesOpen)
    {
        isNoteOpen = notesOpen;
    }

    public bool GetNotesStatus()
    {
        return isNoteOpen;
    }

    public void ShowToolbar() {
        toolbarUI.GetComponent<Animator>().SetTrigger("Show");
    }

    public void HideToolbar() {
        toolbarUI.GetComponent<Animator>().SetTrigger("Hide");
    }
}
