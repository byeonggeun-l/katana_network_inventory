using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandDust : MonoBehaviour
{
    public float lifeTime = 0.5f;

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }


}
