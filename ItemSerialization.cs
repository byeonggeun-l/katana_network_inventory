using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class ItemSerialization
{
    public static byte[] SerializeItem(object _gameobject)
    {
        byte[] bytes = Protocol.Serialize(_gameobject);

        return bytes;
    }

    public static GameObject DeserializeItem(byte[] bytes)
    {
        GameObject item = (GameObject)Protocol.Deserialize(bytes);
        int ahahahaha = 0;

        return item;
    }
}
