using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int totalItem = 42;
    public Transform panel;
    public Transform itemPanel;
    public Transform gridLayoutGroup;
    public Transform itemsGridLayoutGroup;
    public List<RectTransform> instantiatedPanels = new List<RectTransform>();
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
        Transform t = Instantiate(itemPanel, instantiatedPanels[inventorySlots.Count-1].position, Quaternion.identity);
        t.SetParent(itemsGridLayoutGroup, false);
        inventorySlots.Add(grabbedItem);
    }

    int indexToArray(int row, int col)
    {
        return (row * 6) + col ;
    }
}
