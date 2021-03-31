using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;
    public GameObject inventoryPanel;
    bool activeInventory = false;

    public Slot[] slots;
    public Transform slotHolder;

    private void Start()
    {
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        inventoryPanel.SetActive(activeInventory);

        for (int i = 0; i < slots.Length; ++i)
        {
            slots[i].RemoveSlot();
        }

    }

    private void SlotChange(int cal)
    {
        for(int i=0; i< slots.Length; ++i)
        {
            if(i< inven.SlotCnt)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
            if (activeInventory == false)
                Inventory.instance.bInventory = false;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            AddSlot();
        }
    }

    public void AddSlot()
    {
        inven.SlotCnt++;
    }

    void RedrawSlotUI()
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            slots[i].RemoveSlot();
        }
        for (int i = 0; i < inven.items.Count; ++i)
        {
            slots[i].item = inven.items[i];
            slots[i].used = true;
            slots[i].slotNumber = i;
            slots[i].UpdateSlotUI();
        }
    }



}
