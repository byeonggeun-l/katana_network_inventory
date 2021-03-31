using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{

    public enum ItemDatabaseType : int
    {
        Apple = 0,
        Pineapple,
    }


    public static ItemDatabase instance;
    private void Awake()
    {
        instance = this;
    }

    public List<Item> itemDB = new List<Item>();

    //public GameObject fieldItemPrefab;

    private void Start()
    {

    }
}
