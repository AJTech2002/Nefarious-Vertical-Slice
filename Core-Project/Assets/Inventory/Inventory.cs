using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int totalItem = 42;
    public Transform panel;
    public Transform itemUI;
    public Transform gridLayoutGroup;
    public Transform itemPanel;
    public List<RectTransform> instantiatedPanels = new List<RectTransform>();
    public List<RectTransform> instantiatedItemPanels = new List<RectTransform>();
    public List<Item> uniqueItems = new List<Item>();
    public int currentListSize;

    public Item testItem;

    public List<Item> inventorySlots = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        currentListSize = uniqueItems.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(uniqueItems.Count < totalItem)
            {
                uniqueItems.Add(testItem);
                updateUI(testItem);
            } else // Inventory full
            {

            }
            
        }
    }

    void updateUI(Item grabbedItem)
    {
        Transform t = Instantiate(itemUI, instantiatedPanels[inventorySlots.Count-1].position, Quaternion.identity, itemPanel);
        instantiatedItemPanels.Add(t.GetComponent<RectTransform>());
        t.gameObject.GetComponent<DragOut>().instantiatedPanels = instantiatedPanels;
        t.gameObject.GetComponent<DragOut>().instantiatedItemPanels = instantiatedItemPanels;
        inventorySlots.Add(grabbedItem);
    }

    int indexToArray(int row, int col)
    {
        return (row * 6) + col ;
    }
}
