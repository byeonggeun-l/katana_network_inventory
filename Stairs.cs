using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    // 충돌처리
    // trigger          충돌이 일어났을 때 통과. = 미사일?
    // collision        충돌이 일어났을 때 막힘. = 계단?

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("중력 셋팅");
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = -0.5f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("중력 원본 셋팅");
            //player.GetComponent<Rigidbody2D>().gravityScale = 1;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            Animator pAnimator = collision.gameObject.GetComponent<Animator>();
            pAnimator.SetBool("Jump", false);
        }
    }
}
