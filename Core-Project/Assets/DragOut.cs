using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragOut : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnDrag(PointerEventData eventData) {
        Debug.Log(eventData.position);
    }

    //required for OnDrag() to work
    public void OnBeginDrag(PointerEventData eventData) { Debug.Log("dragging"); }

    //required for OnDrag() to work
    public void OnEndDrag(PointerEventData eventData) { }

}
