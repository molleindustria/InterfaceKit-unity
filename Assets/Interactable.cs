using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

public class Interactable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Main main;
    public Image image;

    //can it be dragged?
    public bool draggable = false;

    //do dragged interactables snap to it?
    public bool snappable = false;

    //do mouse interaction trigger changes in color?
    public bool changesColor = true;

    public Color NORMAL_COLOR;
    public Color ROLLOVER_COLOR;
    public Color CLICKED_COLOR;

    //this enables the event trigger in the inspector
    //which allows you to call function without hard coding them
    [Serializable]
    public class MyEvent : UnityEvent { }

    [Space(20)]
    public MyEvent DownEvent;
    public MyEvent UpEvent;
    public MyEvent BeginDragEvent;
    public MyEvent EndDragEvent;
    public MyEvent DragEvent;
    public MyEvent EnterEvent;
    public MyEvent ExitEvent;



    // Start is called before the first frame update
    void Start()
    {
        //if main is not set find it
        if (main == null)
            main = GameObject.FindObjectOfType<Main>();

        if (image == null)
            image = GetComponent<Image>();

        //whatever initial color will be the normal
        NORMAL_COLOR = image.color;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (changesColor)
            image.color = CLICKED_COLOR;

        //tell the "brain about the event
        main.InteractableDown(this, eventData);

        //call the events set up in the inspector (if any)
        DownEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (changesColor)
            image.color = ROLLOVER_COLOR;

        main.InteractableUp(this, eventData);

        UpEvent.Invoke();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        main.InteractableBeginDrag(this, eventData);

        BeginDragEvent.Invoke();

    }


    public void OnDrag(PointerEventData eventData)
    {
        main.InteractableDrag(this, eventData);

        DragEvent.Invoke();
    }


    public void OnEndDrag(PointerEventData eventData)
    {

        main.InteractableEndDrag(this, eventData);

        EndDragEvent.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (changesColor)
            image.color = ROLLOVER_COLOR;

        main.InteractableEnter(this, eventData);

        EnterEvent.Invoke();
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData eventData)
    {
        if (changesColor)
            image.color = NORMAL_COLOR;

        main.InteractableExit(this, eventData);

        ExitEvent.Invoke();

    }
}
