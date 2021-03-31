using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    HpUP,
    PowerUp,
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public GameObject itemPrefab;
    public bool mine;


}
