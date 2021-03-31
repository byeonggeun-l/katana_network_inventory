using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PrefabCacheData
{
    public string filePath;
    public int cacheCount;
}



public class PrefabCacheSystem
{
    Dictionary<string, Queue<GameObject>> Caches = new Dictionary<string, Queue<GameObject>>();

    public void GenerateCache(string filePath, GameObject gameObject, int cacheCount, Transform parentTransform = null)
    {
        if (Caches.ContainsKey(filePath))
        {
            Debug.LogWarning("Already cache generated! filePath = " + filePath);
            return;
        }
        else
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < cacheCount; ++i)
            {
                GameObject go = Object.Instantiate<GameObject>(gameObject, parentTransform);

                go.SetActive(false);
                queue.Enqueue(go);
            }

            Caches.Add(filePath, queue);
        }
    }

    public GameObject Archive(string filePath)
    {
        if (!Caches.ContainsKey(filePath))
        {
            Debug.LogError("Archive Error! no cache generated! filePath = " + filePath);
            return null;
        }

        if (Caches[filePath].Count == 0)
        {
            Debug.LogError("Archive Error! not enough Count filePath = " + filePath);
            return null;
        }

        GameObject go = Caches[filePath].Dequeue();
        go.SetActive(true);

        return go;
    }

    public bool Restore(string filepath, GameObject gameObject)
    {
        if (!Caches.ContainsKey(filepath))
        {
            Debug.LogError("Restore Error! : " + filepath);
            return false;
        }

        gameObject.SetActive(false);

        Caches[filepath].Enqueue(gameObject);

        return true;
    }
}
