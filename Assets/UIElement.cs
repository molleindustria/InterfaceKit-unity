using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

//template for a custom UI class that detect all the main pointer events
public class UIElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

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

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //call the events set up in the inspector (if any)
        DownEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpEvent.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragEvent.Invoke();
    }


    public void OnDrag(PointerEventData eventData)
    {
        DragEvent.Invoke();
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragEvent.Invoke();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        EnterEvent.Invoke();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        ExitEvent.Invoke();
    }
}
