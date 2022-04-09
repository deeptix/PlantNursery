using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HideShow : MonoBehaviour, IPointerClickHandler
{
    public GameObject toggleObject;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) 
    {
        if (GameObject.Find(toggleObject.name) == null)
            GameObject.Instantiate(toggleObject, Input.mousePosition, Quaternion.identity);
        else
            toggleObject.SetActive(!toggleObject.activeInHierarchy);

    }
}
