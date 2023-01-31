using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 dragOffset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = transform.position - Input.mousePosition;
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition + dragOffset;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
    

}
