using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Lazer : AttackType
{
    Transform tr;

    float Speed = 50.0f;


    Vector3 dirNo;

    private void OnEnable()
    {

        tr = player.transform;
        Vector3 Pos = new Vector3(MousePos.x, MousePos.y, 0);
        dir = Pos - tr.position; // 마우스 - 플레이어 포지션을 빼면 마우스를 바라보는 벡터가 나온다.

        // 바라보는 각도 구하기.
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // normalized 단위벡터
        dirNo = new Vector3(dir.x, dir.y, 0).normalized;


        Vector3 v3RelativePoint = tr.transform.InverseTransformPoint(MousePos);
        if (v3RelativePoint.x > 0f)
        {
            // B가 A의 오른쪽
            player.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (v3RelativePoint.x < 0f)
        {
            // B가 A의 왼쪽
            player.GetComponent<SpriteRenderer>().flipX = true;
        }

        Invoke("Des", 4.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 회전 적용
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 이동
        transform.position += dirNo * Speed * Time.deltaTime;
    }

    void Des()
    {
        gameObject.SetActive(false);
        Hit_LazerManager.instance.Hit_Lazeres.Enqueue(gameObject);
    }
}
