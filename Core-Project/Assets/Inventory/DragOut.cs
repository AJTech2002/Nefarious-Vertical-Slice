using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragOut : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 origin;
    RectTransform inventoryPanel;
    public List<RectTransform> instantiatedPanels = new List<RectTransform>();
    public List<RectTransform> instantiatedItemPanels = new List<RectTransform>();

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;
    }

    //required for OnDrag() to work
    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("dragging");
        origin = transform.position;
    }

    //required for OnDrag() to work
    public void OnEndDrag(PointerEventData eventData) 
    {  
        if(!RectTransformUtility.RectangleContainsScreenPoint(transform.parent.GetComponent<RectTransform>(), Input.mousePosition))
        {
            transform.position = eventData.position;
        } else // dropped within the items panel
        {
            //transform.position = origin;
            // check if the item is dropped on a grid with an item already there
            // need access to array of items
            for (int i = 0; i < instantiatedPanels.Count; i++)
            {
                if(RectTransformUtility.RectangleContainsScreenPoint(instantiatedPanels[i].GetComponent<RectTransform>(), Input.mousePosition)) // check if there the drag ended on a panel square
                {
                    for (int j = 0; j < instantiatedItemPanels.Count; j++)
                    {
                        if(RectTransformUtility.RectangleContainsScreenPoint(instantiatedItemPanels[j].GetComponent<RectTransform>(), Input.mousePosition))
                        {
                            transform.position = origin;
                            Debug.Log("item there");
                            break;
                        } else
                        {
                            transform.position = instantiatedPanels[i].position;
                            Debug.Log("no item there");
                            Debug.Log(transform.position);
                            Debug.Log(instantiatedPanels[i].position);
                            break;
                        }
                    }
                } else
                {
                    //transform.position = origin;
                }
            }
        }
    }

}
