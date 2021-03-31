using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashManager : MonoBehaviour
{
    public GameObject cachePrefab;
    public int cacheCount;

    public static Queue<GameObject> slashes = new Queue<GameObject>();


    public static SlashManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<SlashManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static SlashManager m_instance; // 싱글톤이 할당될 static 변수

    public Queue<GameObject> Slashes
    {
        get
        {
            return slashes;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Prepare();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetSlash(GameObject player, Vector2 mousePoint)
    {
        GameObject slash = slashes.Dequeue();
        slash.gameObject.transform.position = player.transform.position;
        slash.GetComponent<Slash>().player = player;
        slash.GetComponent<Slash>().MousePos = mousePoint;
        slash.SetActive(true);
        Debug.Log("mousePoint : " + mousePoint);

        return slash;
    }

    public void Prepare()
    {
        int ahahahah = 0;
        for(int i=0;i<cacheCount; ++i)
        {
            cachePrefab.SetActive(false);
            GameObject go = Instantiate(cachePrefab, transform.position, Quaternion.identity);
            slashes.Enqueue(go);
        }
    }
}
