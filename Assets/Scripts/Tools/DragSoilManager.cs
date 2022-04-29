using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSoilManager : MonoBehaviour
{
    private const int MAX_DIST = 10;
    private int ADJ_POSITION = 1;
    private bool isDragging;
    private bool isSoilAdded;
    private SoilChangeManager soilChangeManager;
    public GameObject soil;
    public ParticleSystem fallingSoil;
    private Vector2 oldPosition;

    private SoundManager soundManager;

    void OnEnable()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        transform.position = new Vector3(cameraPos.x + 1.5f, cameraPos.y, 0);
    }

    public void OnMouseDown()
    {
        isDragging = true;
        isSoilAdded = false;
        soilChangeManager = GameObject.Find("SoilManager").GetComponent<SoilChangeManager>();
        oldPosition = transform.position;

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    public void OnMouseUp()
    {
        LayerMask mask = LayerMask.GetMask("Plants");
        RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.down, MAX_DIST, mask);

        // check if scoop hit a plant (drop zone)
        if (hit.collider != null)
        {
            // hit an empty drop zone! change positioning to match drop zone
            Transform dropZoneRect = hit.transform.gameObject.transform;
            Vector2 newPosition = new Vector2(dropZoneRect.transform.position.x, dropZoneRect.transform.position.y + ADJ_POSITION);
            transform.position = newPosition;
            soil.SetActive(false); // TODO: change to moving position down as soil falls from scoop
            fallingSoil.Play();
            hit.transform.gameObject.GetComponent<PlantStateManager>().changeSoil(soilChangeManager.GetSoilType());
            isSoilAdded = true;

            soundManager.PlaySoilSound();
        }

        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
        else if (isSoilAdded && fallingSoil.isStopped)
        {
            soil.SetActive(true);
            transform.position = oldPosition;
            isSoilAdded = false;
            gameObject.SetActive(false);
        }
    }
}

