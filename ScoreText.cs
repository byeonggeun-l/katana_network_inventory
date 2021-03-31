using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ScoreText : MonoBehaviourPun
{
    private GameObject hpBar;
    private float fSpeed = 1.0f;
    bool monsterHP = false;
    void Start()
    {
        hpBar = gameObject.GetComponentInChildren<CanvasScoreText>().gameObject;
        //hpBar = gameObject.GetComponent<ScoreText>();
        //gameObject.get
        //gameObject.find

    }

    void Update()
    {
        if (photonView.IsMine)
        {
            float fHorizontal = Input.GetAxis("Horizontal");
            float fVertical = Input.GetAxis("Vertical");

            gameObject.transform.Translate(Vector3.right * Time.deltaTime * fSpeed * fHorizontal, Space.World);
            gameObject.transform.Translate(Vector3.up * Time.deltaTime * fSpeed * fVertical, Space.World);
        }

        hpBar.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + new Vector3(0, 0.8f, 0));

    }
}
