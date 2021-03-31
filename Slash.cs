using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : AttackType
{




    private void OnEnable()
    {
        Transform tr = player.GetComponent<Transform>();
        Vector3 Pos = new Vector3(MousePos.x, MousePos.y, 0);
        dir = Pos - tr.position;

        // 바라보는 각도.
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

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
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        transform.position = player.transform.position;
    }

    public void Des()
    {
        gameObject.SetActive(false);
        SlashManager.instance.Slashes.Enqueue(gameObject);
    }
}
