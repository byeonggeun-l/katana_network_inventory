using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    public GameObject cachePrefab;
    public int cacheCount;

    public static Queue<GameObject> shadows = new Queue<GameObject>();


    public static ShadowManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<ShadowManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static ShadowManager m_instance; // 싱글톤이 할당될 static 변수

    public Queue<GameObject> Shadows
    {
        get
        {
            return shadows;
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

    public GameObject GetShadow(GameObject player, int shCount)
    {
        GameObject Shadow = Shadows.Dequeue();
        Shadow.transform.position = player.transform.position;

        Shadow.SetActive(true);
        Shadow.GetComponent<Shadow>().player = player;
        Shadow.GetComponent<Shadow>().TwSpeed = 15 - shCount;

        return Shadow;
    }

    public void Prepare()
    {
        for (int i = 0; i < cacheCount; ++i)
        {
            cachePrefab.SetActive(false);
            GameObject go = Instantiate(cachePrefab, transform.position, Quaternion.identity);
            Shadows.Enqueue(go);
        }
    }
}
