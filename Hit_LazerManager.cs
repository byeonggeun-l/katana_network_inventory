using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_LazerManager : MonoBehaviour
{
    public GameObject cachePrefab;
    public int cacheCount;

    public static Queue<GameObject> hit_Lazers = new Queue<GameObject>();


    public static Hit_LazerManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<Hit_LazerManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static Hit_LazerManager m_instance; // 싱글톤이 할당될 static 변수

    public Queue<GameObject> Hit_Lazeres
    {
        get
        {
            return hit_Lazers;
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

    public GameObject GetHit_Lazer(GameObject player, Vector2 vecMousePoint)
    {
        GameObject hit_lazer = Hit_Lazeres.Dequeue();
        hit_lazer.transform.position = player.transform.position;
        hit_lazer.GetComponent<Hit_Lazer>().player = player;
        hit_lazer.GetComponent<Hit_Lazer>().MousePos = vecMousePoint;
        hit_lazer.SetActive(true);

        return hit_lazer;
    }

    public void Prepare()
    {
        int ahahahah = 0;
        for (int i = 0; i < cacheCount; ++i)
        {
            cachePrefab.SetActive(false);
            GameObject go = Instantiate(cachePrefab, transform.position, Quaternion.identity);
            Hit_Lazeres.Enqueue(go);
        }
    }
}
