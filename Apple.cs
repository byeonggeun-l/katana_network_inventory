using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Apple : Items
{

    public override void Use(GameObject target)
    {
        // 전달 받은 게임 오브젝트로부터 PlayerShooter 컴포넌트를 가져오기 시도
        Player player = target.GetComponent<Player>();
        player.setHp(player.startingHealth);

        Destroy(gameObject);
    }
}
