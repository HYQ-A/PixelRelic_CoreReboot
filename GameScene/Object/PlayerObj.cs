using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj : MonoBehaviour
{
    private static PlayerObj instance;
    public static PlayerObj Instance => instance;

    private Rigidbody2D rigbd;
    private Animator animator;
    public float moveSpeed;
    private float inputX;
    private float inputY;
    private float stopX;
    private float stopY;
    public Transform talkUIPos;
    //玩家信息
    public PlayerInfo playerInfo;
    //玩家是否死亡
    public bool isDeath;

    private void Awake()
    {
        instance = this;

        //加载玩家信息
        playerInfo = GameDataMgr.Instance.playerData[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        if(talkUIPos==null)
        {
            talkUIPos = this.transform.GetChild(0);
        }
        rigbd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        //将速度单位话 避免叠加
        Vector2 input = (this.transform.right * inputX + this.transform.up * inputY).normalized;
        rigbd.velocity = input * moveSpeed;

        //运动状态时
        if (input != Vector2.zero)
        {
            animator.SetBool("IsMoving", true);
            stopX = inputX;
            stopY = inputY;
        }
        //非运动状态时
        else
        {
            animator.SetBool("IsMoving", false);
        }

        animator.SetFloat("Xvlue", stopX);
        animator.SetFloat("Yvlue", stopY);
    }

    /// <summary>
    /// 玩家受伤的方法
    /// </summary>
    /// <param name="dmg"></param>
    public void Wound(int dmg)
    {
        if (isDeath)
            return;

        dmg -= playerInfo.def;
        playerInfo.hp -= dmg;
        if(playerInfo.hp <= 0)
        {
            Death();
        }
    }

    /// <summary>
    /// 玩家死亡的方法
    /// </summary>
    public void Death()
    {
        isDeath = true;
        Destroy(this.gameObject);
    }
}
