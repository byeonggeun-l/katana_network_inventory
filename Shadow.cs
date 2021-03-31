using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public float TwSpeed = 10;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 실시간 위치.
        transform.position = Vector3.Lerp(transform.position, player.transform.position, TwSpeed * Time.deltaTime);
    }

    public void des()
    {

    }
}
