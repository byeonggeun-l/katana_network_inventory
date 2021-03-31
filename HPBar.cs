using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HPBar : MonoBehaviourPun
{
    private GameObject hpBar;
    private float fSpeed = 1.0f;
    public bool move = false;
    void Start()
    {
        hpBar = gameObject.GetComponentInChildren<CanvasHPBar>().gameObject;
    }

    void Update()
    {

        // 서버 객체에만 HPBar가 부여되는 것이 아니다.
        // 몬스터도 HpBar 가 부여될 것을 대비해서 photonView도 
        // Null 여부를 체크한다.
        if (photonView != null && move)
        {
            if (photonView.IsMine)
            {
                float fHorizontal = Input.GetAxis("Horizontal");
                float fVertical = Input.GetAxis("Vertical");

                gameObject.transform.Translate(Vector3.right * Time.deltaTime * fSpeed * fHorizontal, Space.World);
                gameObject.transform.Translate(Vector3.up * Time.deltaTime * fSpeed * fVertical, Space.World);
            }
        }

        hpBar.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + new Vector3(0, 0.5f, 0));
    }
}
