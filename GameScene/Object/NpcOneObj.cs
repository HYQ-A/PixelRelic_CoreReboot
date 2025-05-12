using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NpcOneObj : MonoBehaviour
{
    //�����ļ�
    private TextAsset talkDataFile;
    //��ǰ����ֵ
    public int dialogIndex;
    //�Ի��ı����л���
    public string[] dialogRows;
    private Animator animator;
    //�������
    private Camera mainCamera;
    //NPC��������ת�������Ļ����
    private Vector3 bkScreenPos1;
    //�����������ת�������Ļ����
    private Vector3 bkScreenPos2;
    //��ʼ�Ի�
    private bool canTalk;
    //�Ի���λ��
    public Transform talkUIPos;
    //���
    private PlayerObj playerObj;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //���ضԻ��ı�����csv�ļ�
        talkDataFile = Resources.Load<TextAsset>("TalkAssets/TalkSystem");
        playerObj = GameObject.FindWithTag("Player").GetComponent<PlayerObj>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //����csv�Ի��ı�����
        ReadText(talkDataFile);
        //��ȡ�������
        if (mainCamera == null)
            mainCamera = Camera.main;
        //��ȡ�Ի���λ��
        if (talkUIPos == null)
            talkUIPos = this.transform.GetChild(0).gameObject.transform;
    }

    private void Update()
    {
        if (canTalk && Input.GetMouseButtonDown(0))
        {
            ShowDialogRow();
        }
    }

    /// <summary>
    /// �����봥�����������ʾ�Ի��ı�
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bkScreenPos1 = mainCamera.WorldToScreenPoint(talkUIPos.position);
        bkScreenPos2 = mainCamera.WorldToScreenPoint(playerObj.talkUIPos.position);

        animator.SetTrigger("Greeting");
        
        ShowDialogRow();
        //���Կ�ʼ�Ի�
        canTalk = true;
    }

    /// <summary>
    /// ���˳���������������ضԻ��ı�
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        UIManager.Instance.HidePanle<TalkPanel>();
        //���¿�ʼ���� ���¿�ʼ�Ի�
        dialogIndex = 0;
        canTalk = false;
    }

    /// <summary>
    /// ���ֶԻ��ı�(csv)�ļ�
    /// </summary>
    /// <param name="textAsset"></param>
    public void ReadText(TextAsset textAsset)
    {
        //�Ի��зָ�
        dialogRows = textAsset.text.Split('\n');
    }

    /// <summary>
    /// ���߼���ʾ�Ի��ı�
    /// </summary>
    public void ShowDialogRow()
    {
        animator.SetBool("Communication", true);

        for (int i = 0; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');

            if (cells[0]=="#"&&int.Parse(cells[1])==dialogIndex)
            {
                UIManager.Instance.ShowPanle<TalkPanel>().talkText.rectTransform.position = cells[2] == "�������" ? bkScreenPos1 : bkScreenPos2;
                UIManager.Instance.ShowPanle<TalkPanel>().BK.rectTransform.position = cells[2] == "�������" ? bkScreenPos1 : bkScreenPos2;

                //���ø����ı��ķ���
                UpdateText(cells[3]);

                //���µ�ǰ����ֵ
                dialogIndex = int.Parse(cells[4]);
                break;
            }
            else if (cells[0] == "END" && int.Parse(cells[1])==dialogIndex)
            {
                UIManager.Instance.HidePanle<TalkPanel>();
                animator.SetBool("Joy", true);
            }
        }
    }

    /// <summary>
    /// ���¶Ի��ı���Ϣ
    /// </summary>
    /// <param name="text"></param>
    public void UpdateText(string text)
    {
        UIManager.Instance.GetPanel<TalkPanel>().talkText.text = text;
    }
}
