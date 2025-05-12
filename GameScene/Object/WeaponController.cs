using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;
    //�������λ�����
    private Transform attackBoxPos;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        sr = this.GetComponent<SpriteRenderer>();
        attackBoxPos = this.transform.GetChild(0);
        Debug.Log("attackBoxPos:" + attackBoxPos.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            //���ҹ���
            if (Input.GetAxisRaw("Horizontal") <= 0 || Input.GetAxisRaw("Vertical") <= 0)
            {
                sr.flipX = false;
                animator.SetTrigger("Attack1");
            }

            if (Input.GetAxisRaw("Horizontal") > 0|| Input.GetAxisRaw("Vertical") > 0)
            {
                sr.flipX = true;
                animator.SetTrigger("Attack1");
            }
        }

    }

    /// <summary>
    /// ���������¼�
    /// </summary>
    public void AttackEvent()
    {
        // �������ߴ磨Physics.OverlapBox �ĵڶ��������ǰ�ߴ磩
        UnityEngine.Vector2 boxSize = new UnityEngine.Vector2(1f, 1f);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackBoxPos.position, boxSize, 0f, 1 << LayerMask.NameToLayer("Monster"));

        //������⵽����ײ��
        for (int i = 0; i < colliders.Length; i++)
        {
            MonsterObj monsterObj = colliders[i].GetComponent<MonsterObj>();
            if (monsterObj != null && !monsterObj.isDeath)
            {
                monsterObj.Wound(PlayerObj.Instance.playerInfo.atk);
                break;
            }
        }

        Debug.Log(colliders.Length);
    }
}
