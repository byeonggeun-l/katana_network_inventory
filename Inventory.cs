using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion Singleton

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public delegate void OnUseItem(int number);
    public OnUseItem onUseItem;

    public bool bInventory = false;


    public List<Item> items = new List<Item>();

    private int slotCnt;
    public int SlotCnt { 
        get => slotCnt;
        set {
            slotCnt = value;
            if(onSlotCountChange != null)
                onSlotCountChange(SlotCnt);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        SlotCnt = 18;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Items _item)
    {
        Item item = new Item();
        item = _item.itemInvenInfo;
        items.Add(item);
        if (onChangeItem != null)
            onChangeItem.Invoke();
    }

    public void ClickSort(int number)
    {
        if (onUseItem != null)
            onUseItem.Invoke(number);
        items.RemoveAt(number);
        if (onChangeItem != null)
            onChangeItem.Invoke();
    }

    public void UseItem(int number, GameObject _gameobject)
    {
        items[number].itemPrefab.GetComponent<Items>().Use(_gameobject);
        // 아이템을 사용했으면 삭제하자.
        items.RemoveAt(number);
        // 아이템을 삭제했으면 재갱신하자.
        onChangeItem.Invoke();
    }
}
