using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class MonsterObj : MonoBehaviour
{
    //Ѳ��Ŀ��λ��
    public Vector2 targetPos;
    public float moveSpeed = 0.5f;
    public float rotationSpeed = 300;
    //�ȴ�ʱ��
    public float waitTime = 2;
    //�Ƿ��ڵȴ�״̬
    private bool isWaiting = false;
    private Animator animator;
    //�����ʼ��λ
    private Vector2 startPos;
    //Ѳ�߷�Χ
    private float patrolRange = 1;
    //�Ƿ��Ѿ��ƶ���Ŀ���
    private bool movingOver = false;
    //��ֹЭ���ظ�����
    private bool isCoroutineRunning = false;
    private PlayerObj player;
    //�����Ƿ�����
    public bool isDeath = false;
    //������Ϣ
    private MonsterInfo monsterInfo;
    //��������λ
    private Transform attackBoxPos;

    private void Awake()
    {
        //��ʼ��������Ϣ
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
        //һ��ʼ�ͻ�ȡ���޵���ʼλ��
        startPos = this.transform.position;
        //�����ȡĿ��λ��
        targetPos = GetNewWayPoint();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerObj>();
    }

    // Update is called once per frame
    void Update()
    {
        //��Χ�ڹ���
        if (Vector2.Distance(player.transform.position, this.transform.position) <= 0.2f && !PlayerObj.Instance.isDeath)
        {
            isWaiting = true;
            movingOver = false;
            animator.SetBool("Walk", false);

            //TODO:������ת������߼���position��������⣬Mathf.Atan()

            // ���㳯����ҵ�2D��ת�Ƕ�
            Vector3 direction = player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // ����Sprite����
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            // ƽ��ת�򣨹̶����ٶȣ�
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // ����ˮƽ����תSprite
            GetComponent<SpriteRenderer>().flipX = direction.x >= 0;

            animator.SetTrigger("Attack");
        }
        else
        {
            isWaiting = false;
        }

        //�ƶ���Ŀ����߼�
        if (!isWaiting && !movingOver)
        {
            //��ǰ��λ��Ŀ���λ�ƶ�
            animator.SetBool("Walk", true);
            this.transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            //��movingOver��Ϊtrue ��ʼִ�з�����ʼ�����
            if (Vector2.Distance(this.transform.position, targetPos) < 0.1f)
                StartCoroutine(WaitAtMoment());
        }

        //��Ŀ��㷵�س�ʼ���߼�
        if (movingOver)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, startPos, moveSpeed * Time.deltaTime);
            
            //��movingOver��Ϊfalse isWaiting��Ϊtrue ��ʼִ��Э�̳���
            if (Vector2.Distance(this.transform.position, startPos) < 0.1f)
            {
                movingOver = false;
                isWaiting = true;
                animator.SetBool("Walk", false);
            }
        }

        //����Ŀ����߼�
        if (isWaiting&&!isCoroutineRunning)
        {
            isCoroutineRunning = true;
            StartCoroutine(WaitAtPoint());
        }
    }

    /// <summary>
    /// ��ǰ��λ������һ����λ��Э�̺���
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
    /// �ƶ�����ʼ���ȴ�һ��ʱ���Э�̺���
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
    /// ���һ���µ�Ѳ�ߵ�λ�ķ���
    /// </summary>
    /// <returns></returns>
    public Vector2 GetNewWayPoint()
    {
        float randomX = UnityEngine.Random.Range(-patrolRange, patrolRange);
        float randomY = UnityEngine.Random.Range(-patrolRange, patrolRange);
        return startPos + new Vector2(randomX, randomY);
    }

    /// <summary>
    /// �������˵ķ���
    /// </summary>
    /// <param name="dmg"></param>
    public void Wound(int dmg)
    {
        //�����Ѿ���������Ҫ���ж����˿�Ѫ
        if (isDeath)
            return;

        dmg -= monsterInfo.def;
        monsterInfo.hp -= dmg;
        //����ûѪ���������������
        if(monsterInfo.hp <= 0)
        {
            Dead();
        }
    }

    /// <summary>
    /// ���������ķ���
    /// </summary>
    public void Dead()
    {
        //�ж�����
        isDeath = true;
        //������Ϸ����
        Destroy(this.gameObject);
    }

    /// <summary>
    /// ���޹����¼�
    /// </summary>
    public void AttackEvent()
    {
        // �������ߴ磨Physics.OverlapBox �ĵڶ��������ǰ�ߴ磩
        UnityEngine.Vector2 boxSize = new UnityEngine.Vector2(0.5f, 0.5f);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackBoxPos.position, boxSize, 0f, 1 << LayerMask.NameToLayer("Player"));
        Debug.Log("�����޼�⵽�Ķ���" + colliders[0].name);

        if(!player.isDeath)
        {
            player.Wound(monsterInfo.atk);
        }

        //������⵽����ײ��
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
