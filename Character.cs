using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// 생명체로서 동작할 게임 오브젝트들을 위한 뼈대를 제공
// 체력, 데미지 받아들이기, 사망 기능, 사망 이벤트를 제공
public class Character : MonoBehaviourPun
{
    public enum CharacterType : int
    {
        Player = 0,
        Monster,
    }

    public Animator pAnimator;

    public float speed = 5;
    public float power = 5;
    public float jumpUp = 10;
    public float damage = 30;

    public float startingHealth = 100f; // 시작 체력
    public float health { get; protected set; } // 현재 체력
    public bool dead { get; protected set; } // 사망 상태
    public event Action onDeath; // 사망시 발동할 이벤트

    public Slider HPBar;

    public CharacterType characterType;

    public Rigidbody2D pRig2D;

    protected virtual void OnEnable()
    {
        // 사망하지 않은 상태로 시작
        dead = false;
        // 체력을 시작 체력으로 초기화
        health = startingHealth;

        HPBar = gameObject.GetComponentInChildren<Slider>();

        pRig2D = GetComponent<Rigidbody2D>();
    }

    public virtual bool OnDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
            return true;
        }

        HPBar.value = health / 100f;
        if (health <= 0)
        {
            HPBar.fillRect.gameObject.SetActive(false);
        }

        return false;
    }


    protected void AddForce(Collider2D collision)
    {
        Vector3 v3RelativePoint = gameObject.transform.InverseTransformPoint(collision.gameObject.transform.position);
        if (v3RelativePoint.x > 0f)
        {
            pRig2D.AddForce(Vector2.left * power, ForceMode2D.Impulse);
        }
        else if (v3RelativePoint.x < 0f)
        {
            pRig2D.AddForce(Vector2.right * power, ForceMode2D.Impulse);
        }
    }

    public void SetScale(float scale)
    {
        gameObject.transform.localScale *= scale;
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
    public void setHp(float hp)
    {
        health += hp;
        if (health > startingHealth)
            health = startingHealth;

        HPBar.value = health / 100f;
    }

    public virtual void Die()
    {
        ////// 사망 상태를 참으로 변경
        dead = true;
        gameObject.SetActive(false);
    }
}