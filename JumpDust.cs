using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDust : MonoBehaviour
{
    public float lifeTime = 0.5f;

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }


}
