using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Items : MonoBehaviourPun, IItem
{
    public Item itemInvenInfo;

    public virtual void Use(GameObject target)
    {
    }

    public void SetInvenInfo(Item _item, GameObject itemPrefab)
    {
        itemInvenInfo.itemName = _item.itemName;
        itemInvenInfo.itemImage = _item.itemImage;
        itemInvenInfo.itemType = _item.itemType;
        itemInvenInfo.itemPrefab = itemPrefab;
    }
}
