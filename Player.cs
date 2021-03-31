using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using Photon.Pun;

public class Player : Character
{


    public Vector3 direction;
    public GameObject slash;

    // Shadow
    public GameObject shadow1;
    List<GameObject> Sh = new List<GameObject>();

    // HitEffect
    public GameObject hit_lazer;

    SpriteRenderer sp;

    // 점프 더스트.
    public GameObject jDust;

    // 벽점프
    public Transform wallChk;
    public float wallChkDistance;
    public LayerMask wLayer;
    bool isWall;
    public float slidingSpeed;
    public float wallJumpPower;
    public bool isWallJump;
    float isRight = 1;

    Inventory inven;

    int Score = 0;

    public GameObject walldust;

    private Text ScoreText;

    public GameObject monsterPrefab; // 몬스터 프리팹. 테스트를 위해.

    private void Awake()
    {
        PhotonPeer.RegisterType(typeof(GameObject), 128, ItemSerialization.SerializeItem,
            ItemSerialization.DeserializeItem);
    }

    // Start is called before the first frame update
    void Start()
    {
        inven = Inventory.instance;
        pAnimator = GetComponent<Animator>();
        direction = Vector3.zero;
        sp = GetComponent<SpriteRenderer>();

        ScoreText = gameObject.GetComponentInChildren<Text>();
        characterType = CharacterType.Player;

        if(photonView.IsMine)
            inven.onUseItem += GetItemForUsing;

        damage = 80.0f;

        // 마지막 플레이어가 들어왔으면 몬스터를 생성하자!
        gameObject.GetComponentInChildren<HPBar>().move = true;
        if(photonView.OwnerActorNr == 2)
        {
            photonView.RPC("MakeMonsterAsAHost_Phpton", RpcTarget.Others);
        }
    }


    public override bool OnDamage(float damage)
    {
        bool Die = base.OnDamage(damage);

        return Die;
    }

    public void GetItemForUsing(int number)
    {
        Debug.Log("_item : " + inven.items[number]);
        photonView.RPC("UseItem_Photon", RpcTarget.All, inven.items[number].itemName);
    }

    [PunRPC]
    public void UseItem_Photon(string itemName)
    {
        if(itemName == "Pineapple")
        {
            SetScale(1.1f);
            SetDamage(damage + 20);
        }
        else if (itemName == "Apple")
        {
            setHp(20);
        }
            
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Lazer" ||
            collision.gameObject.tag == "Slash")
        {
            if (collision.GetComponent<AttackType>().player == gameObject)
                return;

            Collisioned(collision);
            // 레이저는 충돌 즉시 사라진다.
            if (collision.gameObject.tag == "Lazer")
                collision.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Items")
        {
            Items Items = collision.gameObject.GetComponent<Items>();
            if(photonView.IsMine)
                Inventory.instance.AddItem(Items);
            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }


    [PunRPC]
    public void MakeMonsterAsAHost_Phpton()
    {
        if (!photonView.IsMine)
            return;

        Debug.Log("Hmm");

        PhotonNetwork.Instantiate(monsterPrefab.name, new Vector3(-8.89f, -0.94f, 0f), Quaternion.identity);
        PhotonNetwork.Instantiate(monsterPrefab.name, new Vector3(-2.73f, -1.13f, 0f), Quaternion.identity);
        PhotonNetwork.Instantiate(monsterPrefab.name, new Vector3(-1.63f, 3.50f, 0f), Quaternion.identity);
        PhotonNetwork.Instantiate(monsterPrefab.name, new Vector3(2.63f, -1.01f, 0f), Quaternion.identity);
        PhotonNetwork.Instantiate(monsterPrefab.name, new Vector3(17.8f, 1.88f, 0f), Quaternion.identity);
        PhotonNetwork.Instantiate(monsterPrefab.name, new Vector3(21.5f, -1.0f, 0f), Quaternion.identity);
        PhotonNetwork.Instantiate(monsterPrefab.name, new Vector3(17.9f, -0.97f, 0f), Quaternion.identity);
        PhotonNetwork.Instantiate(monsterPrefab.name, new Vector3(11.68f, 1.87f, 0f), Quaternion.identity);
    }


    private void Collisioned(Collider2D collision)
    {
        GameObject player = collision.GetComponent<AttackType>().player;
        if (player.GetComponent<Character>().dead)
            return;

        AddForce(collision);

        bool Die = OnDamage(player.GetComponent<Character>().damage);
        // 날 죽인 사람이 플레이어인 경우에만 점수를 준다.
        if (Die && player.GetComponent<Character>().characterType == CharacterType.Player)
        {
            Debug.Log("Collisioned SetScore");
            player.GetComponent<Player>().SetScore(10);
        }
    }

    void KeyInput()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SetDirection", RpcTarget.All, Input.GetAxisRaw("Horizontal"));
        }

        if (Input.GetMouseButtonDown(0) && photonView.IsMine)
        {
            if (Inventory.instance.bInventory)
                return;


            Vector2 vecMousePoint = GetMouseWorldPoint();
            photonView.RPC("MakeHitLazer_Photon", RpcTarget.All, vecMousePoint);
        }

        if (Input.GetMouseButtonDown(1) && photonView.IsMine)
        {
            Vector2 vecMousePoint = GetMouseWorldPoint();
            photonView.RPC("AttSlash_Photon", RpcTarget.All, vecMousePoint);
        }

        if (Input.GetKeyDown(KeyCode.Space) && photonView.IsMine)
        {
            if (pAnimator.GetBool("Jump") == false)
            {
                photonView.RPC("Jump_Photon", RpcTarget.All);
            }
        }
    }

    private Vector2 GetMouseWorldPoint()
    {
        Vector2 vecMousePoint = Input.mousePosition;
        vecMousePoint = Camera.main.ScreenToWorldPoint(vecMousePoint);

        return vecMousePoint;
    }

    void Jump()
    {
        pRig2D.velocity = Vector2.zero;

        // 위쪽으로 힘을 준다.
        pRig2D.AddForce(new Vector2(0, jumpUp), ForceMode2D.Impulse);
    }


    [PunRPC]
    public void SetKeyInput_Photon(float x)
    {
        direction.x = x;
    }


    public void SetScore(int Score)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SetScore_Phpton", RpcTarget.All, Score);
        }
    }

    [PunRPC]
    public void SetScore_Phpton(int _Score)
    {
        Score += _Score;
        ScoreText.text = Score.ToString();
    }

    [PunRPC]
    public void SetDirection(float directon_x)
    {
        if (pAnimator == null)
            return;

        direction.x = directon_x;

        if (direction.x < 0)
        {
            photonView.RPC("SetFlipX_Photon", RpcTarget.All, true);

            pAnimator.SetBool("Run", true);

            // 점프 벽 잡기 방향
            isRight = -1;

            //ShadowFlip();
            photonView.RPC("ShadowFlip_Photon", RpcTarget.All);
        }
        else if (direction.x > 0)
        {
            photonView.RPC("SetFlipX_Photon", RpcTarget.All, false);

            pAnimator.SetBool("Run", true);

            // 점프 벽 잡기 방향
            isRight = 1;

            //ShadowFlip();
            photonView.RPC("ShadowFlip_Photon", RpcTarget.All);
        }
        else if (direction.x == 0)
        {
            pAnimator.SetBool("Run", false);
            if (Sh.Count > 0)
            {
                photonView.RPC("DeleteShadow_Photon", RpcTarget.All);
            }
        }

    }

    [PunRPC]
    public void MakeHitLazer_Photon(Vector2 vecMousePoint)
    {
        pAnimator.SetTrigger("Attack");

        Hit_LazerManager.instance.GetHit_Lazer(gameObject, vecMousePoint);
    }

    [PunRPC]
    public void AttSlash_Photon(Vector2 vecMousePoint)
    {
        pAnimator.SetTrigger("Attack");

        RenewFlipX(vecMousePoint);
        GameObject slash = SlashManager.instance.GetSlash(gameObject, vecMousePoint);
    }

    [PunRPC]
    public void MakeWallDustEffect(bool b)
    {
        GameObject go = Instantiate(walldust, transform.position + new Vector3(0.8f * isRight, 0, 0), Quaternion.identity);
        go.GetComponent<SpriteRenderer>().flipX = b;
    }

    [PunRPC]
    public void SetFlipX_Photon(bool b)
    {
        sp.flipX = b;
    }

    [PunRPC]
    public void DeleteShadow_Photon()
    {
        for (int i = 0; i < Sh.Count; ++i)
        {
            Sh[i].gameObject.SetActive(false);
            ShadowManager.instance.Shadows.Enqueue(Sh[i]);
            Sh.RemoveAt(i);
        }
    }

    [PunRPC]
    public void Jump_Photon()
    {
        Jump();
        photonView.RPC("SetBool_Photon", RpcTarget.All, "Jump", true);
        JumpDust();
    }

    [PunRPC]
    public void ShadowFlip_Photon()
    {
        // Shadow Flip
        for (int i = 0; i < Sh.Count; ++i)
        {
            Sh[i].GetComponent<SpriteRenderer>().flipX = sp.flipX;
        }
    }

    [PunRPC]
    public void SetBool_Photon(string boolName, bool b)
    {
        pAnimator.SetBool(boolName, b);
    }



    // Update is called once per frame
    void Update()
    {
        if (!isWallJump)
        {
            KeyInput();
            Move();
        }

        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallChkDistance, wLayer);
        pAnimator.SetBool("Grab", isWall);

        if (isWall)
        {
            isWallJump = false;
            pRig2D.velocity = new Vector2(pRig2D.velocity.x, pRig2D.velocity.y * slidingSpeed);
            // 벽을 잡고 있는 상태에서 점프를 하면.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isWallJump = true;
                // 벽 점프 먼지.


                GameObject go = Instantiate(walldust, transform.position + new Vector3(0.8f * isRight, 0, 0), Quaternion.identity);
                go.GetComponent<SpriteRenderer>().flipX = sp.flipX;
                // others
                photonView.RPC("MakeWallDustEffect", RpcTarget.All, sp.flipX);

                // test
                Invoke("FreezeX", 0.3f);
                pRig2D.velocity = new Vector2(-isRight * wallJumpPower, 0.9f * wallJumpPower);

                //photonView.RPC("SetFlipX_Photon", RpcTarget.All, sp.flipX);
                photonView.RPC("SetWallJumpState_Photon", RpcTarget.All, sp.flipX == false ? true : false, -isRight);
                //sp.flipX = sp.flipX == false ? true : false;
                //isRight = -isRight;
            }
        }
    }

    [PunRPC]
    private void SetWallJumpState_Photon(bool flipState, float _isRight)
    {
        sp.flipX = flipState;
        isRight = _isRight;
    }

    void FreezeX()
    {
        isWallJump = false;
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(pRig2D.position, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(pRig2D.position, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if (pRig2D.velocity.y < 0)
        {
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.7f)
                {
                    //pAnimator.SetBool("Jump", false);
                    photonView.RPC("SetBool_Photon", RpcTarget.All, "Jump", false);
                }
            }
            else
            {
                if (!isWall)
                {
                    // 떨어지는 중.
                    pAnimator.SetBool("Jump", true);
                }
                else
                {
                    // 벽타는 중.
                    pAnimator.SetBool("Grab", true);
                }

            }
        }


        RaycastHit2D rayWallGroundHit = Physics2D.Raycast(pRig2D.position, Vector3.down, 1, LayerMask.GetMask("WallGround"));
        // 특수상황
        if (rayWallGroundHit.collider != null & pAnimator.GetBool("Jump"))
        {
            pAnimator.SetBool("Jump", false);
        }
    }

    void Move()
    {
        transform.position += direction * speed * Time.deltaTime;
        direction = Vector3.zero;
    }

    private void RenewFlipX(Vector2 MouseWorldPoint)
    {
        Vector3 v3RelativePoint = gameObject.transform.InverseTransformPoint(MouseWorldPoint);
        if (v3RelativePoint.x > 0f)
        {
            photonView.RPC("SetFlipX_Photon", RpcTarget.All, false);
        }
        else if (v3RelativePoint.x < 0f)
        {
            photonView.RPC("SetFlipX_Photon", RpcTarget.All, true);
        }
    }

    // Shadow
    public void RunShadow()
    {
        if (Sh.Count < 8)
        {
            GameObject shadow = ShadowManager.instance.GetShadow(gameObject, Sh.Count);
            Sh.Add(shadow);

            // test
            GameObject shadow2 = ShadowManager.instance.GetShadow(gameObject, Sh.Count);
            Sh.Add(shadow2);
        }
    }

    // 흙먼지 추가.
    public void RandDust(GameObject dust)
    {
        Instantiate(dust, transform.position + new Vector3(0, -0.43f, 0), Quaternion.identity);
    }

    public void JumpDust()
    {
        if (!isWall)
        {
            // 기본
            Instantiate(jDust, transform.position, Quaternion.identity);
        }
        else
        {
            // 벽점프
            Instantiate(walldust, transform.position, Quaternion.identity);
        }
    }

    // 벽점프 확인
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallChkDistance);
    }
}