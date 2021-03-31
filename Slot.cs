using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public Image itemIcon;
    public bool used = false;

    public int slotNumber;

    public void UpdateSlotUI()
    {
        gameObject.GetComponent<Image>().sprite = item.itemImage;
        gameObject.SetActive(true);
    }
    public void RemoveSlot()
    {
        item = null;
        gameObject.SetActive(false);
        used = false;
    }

    public void MouseIn()
    {
        Debug.Log("MouseIn");
        Inventory.instance.bInventory = true;
    }
    public void MouseOut()
    {
        Debug.Log("MouseOut");
        Inventory.instance.bInventory = false;
    }

    public void ClickSlot()
    {
        if (!used)
            return;
        Inventory.instance.ClickSort(slotNumber);
        //Debug.Log(slotNumber + "번째를 클릭을 했어요");
    }



}
