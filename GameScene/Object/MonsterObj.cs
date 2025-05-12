using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class MonsterObj : MonoBehaviour
{
    //巡逻目标位置
    public Vector2 targetPos;
    public float moveSpeed = 0.5f;
    public float rotationSpeed = 300;
    //等待时间
    public float waitTime = 2;
    //是否处于等待状态
    private bool isWaiting = false;
    private Animator animator;
    //怪物初始点位
    private Vector2 startPos;
    //巡逻范围
    private float patrolRange = 1;
    //是否已经移动到目标点
    private bool movingOver = false;
    //防止协程重复开启
    private bool isCoroutineRunning = false;
    private PlayerObj player;
    //怪物是否死亡
    public bool isDeath = false;
    //怪兽信息
    private MonsterInfo monsterInfo;
    //攻击检测点位
    private Transform attackBoxPos;

    private void Awake()
    {
        //初始化怪物信息
        monsterInfo = new MonsterInfo
        {
            id = GameDataMgr.Instance.monsterData[0].id,
            atk = GameDataMgr.Instance.monsterData[0].atk,
            def = GameDataMgr.Instance.monsterData[0].def,
            hp = GameDataMgr.Instance.monsterData[0].hp,
        };

        attackBoxPos = this.transform.GetChild(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        //一开始就获取怪兽的起始位置
        startPos = this.transform.position;
        //随机获取目标位置
        targetPos = GetNewWayPoint();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerObj>();
    }

    // Update is called once per frame
    void Update()
    {
        //范围内攻击
        if (Vector2.Distance(player.transform.position, this.transform.position) <= 0.2f && !PlayerObj.Instance.isDeath)
        {
            isWaiting = true;
            movingOver = false;
            animator.SetBool("Walk", false);

            //TODO:理解怪兽转向玩家逻辑，position计算结果理解，Mathf.Atan()

            // 计算朝向玩家的2D旋转角度
            Vector3 direction = player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // 修正Sprite朝向
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            // 平滑转向（固定角速度）
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // 根据水平方向翻转Sprite
            GetComponent<SpriteRenderer>().flipX = direction.x >= 0;

            animator.SetTrigger("Attack");
        }
        else
        {
            isWaiting = false;
        }

        //移动到目标点逻辑
        if (!isWaiting && !movingOver)
        {
            //当前点位朝目标点位移动
            animator.SetBool("Walk", true);
            this.transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            //将movingOver设为true 开始执行返回起始点程序
            if (Vector2.Distance(this.transform.position, targetPos) < 0.1f)
                StartCoroutine(WaitAtMoment());
        }

        //从目标点返回初始点逻辑
        if (movingOver)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, startPos, moveSpeed * Time.deltaTime);
            
            //将movingOver设为false isWaiting设为true 开始执行协程程序
            if (Vector2.Distance(this.transform.position, startPos) < 0.1f)
            {
                movingOver = false;
                isWaiting = true;
                animator.SetBool("Walk", false);
            }
        }

        //更新目标点逻辑
        if (isWaiting&&!isCoroutineRunning)
        {
            isCoroutineRunning = true;
            StartCoroutine(WaitAtPoint());
        }
    }

    /// <summary>
    /// 当前点位进入下一个点位的协程函数
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAtPoint()
    {
        yield return new WaitForSeconds(waitTime);
        targetPos = GetNewWayPoint();
        isWaiting = false;
        isCoroutineRunning = false;
    }

    /// <summary>
    /// 移动到起始点后等待一段时间的协程函数
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAtMoment()
    {
        animator.SetBool("Walk", false);
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("Walk", true);
        movingOver = true;
    }

    /// <summary>
    /// 获得一个新的巡逻点位的方法
    /// </summary>
    /// <returns></returns>
    public Vector2 GetNewWayPoint()
    {
        float randomX = UnityEngine.Random.Range(-patrolRange, patrolRange);
        float randomY = UnityEngine.Random.Range(-patrolRange, patrolRange);
        return startPos + new Vector2(randomX, randomY);
    }

    /// <summary>
    /// 怪物受伤的方法
    /// </summary>
    /// <param name="dmg"></param>
    public void Wound(int dmg)
    {
        //怪物已经死亡则不需要再判断受伤扣血
        if (isDeath)
            return;

        dmg -= monsterInfo.def;
        monsterInfo.hp -= dmg;
        //怪兽没血了则调用死亡方法
        if(monsterInfo.hp <= 0)
        {
            Dead();
        }
    }

    /// <summary>
    /// 怪兽死亡的方法
    /// </summary>
    public void Dead()
    {
        //判定死亡
        isDeath = true;
        //销毁游戏对象
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 怪兽攻击事件
    /// </summary>
    public void AttackEvent()
    {
        // 检测区域尺寸（Physics.OverlapBox 的第二个参数是半尺寸）
        UnityEngine.Vector2 boxSize = new UnityEngine.Vector2(0.5f, 0.5f);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackBoxPos.position, boxSize, 0f, 1 << LayerMask.NameToLayer("Player"));
        Debug.Log("被怪兽检测到的对象：" + colliders[0].name);

        if(!player.isDeath)
        {
            player.Wound(monsterInfo.atk);
        }

        //遍历检测到的碰撞器
        //for (int i = 0; i < colliders.Length; i++)
        //{
        //    PlayerObj monsterObj = colliders[i].GetComponent<PlayerObj>();
        //    if (monsterObj != null && !monsterObj.isDeath)
        //    {
        //        monsterObj.Wound(PlayerObj.Instance.playerInfo.atk);
        //        break;
        //    }
        //}
    }
}
