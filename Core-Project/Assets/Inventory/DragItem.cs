using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : EventTrigger
{
    public override void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("OnBeginDrag called.");
    }
}
