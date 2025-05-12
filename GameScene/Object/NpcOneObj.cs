using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NpcOneObj : MonoBehaviour
{
    //交流文件
    private TextAsset talkDataFile;
    //当前索引值
    public int dialogIndex;
    //对话文本按行划分
    public string[] dialogRows;
    private Animator animator;
    //主摄像机
    private Camera mainCamera;
    //NPC世界坐标转换后的屏幕坐标
    private Vector3 bkScreenPos1;
    //玩家世界坐标转换后的屏幕坐标
    private Vector3 bkScreenPos2;
    //开始对话
    private bool canTalk;
    //对话框位置
    public Transform talkUIPos;
    //玩家
    private PlayerObj playerObj;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //加载对话文本数据csv文件
        talkDataFile = Resources.Load<TextAsset>("TalkAssets/TalkSystem");
        playerObj = GameObject.FindWithTag("Player").GetComponent<PlayerObj>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //划分csv对话文本数据
        ReadText(talkDataFile);
        //获取主摄像机
        if (mainCamera == null)
            mainCamera = Camera.main;
        //获取对话框位置
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
    /// 当进入触发器区域就显示对话文本
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bkScreenPos1 = mainCamera.WorldToScreenPoint(talkUIPos.position);
        bkScreenPos2 = mainCamera.WorldToScreenPoint(playerObj.talkUIPos.position);

        animator.SetTrigger("Greeting");
        
        ShowDialogRow();
        //可以开始对话
        canTalk = true;
    }

    /// <summary>
    /// 当退出触发器区域就隐藏对话文本
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        UIManager.Instance.HidePanle<TalkPanel>();
        //重新开始索引 重新开始对话
        dialogIndex = 0;
        canTalk = false;
    }

    /// <summary>
    /// 划分对话文本(csv)文件
    /// </summary>
    /// <param name="textAsset"></param>
    public void ReadText(TextAsset textAsset)
    {
        //以换行分割
        dialogRows = textAsset.text.Split('\n');
    }

    /// <summary>
    /// 按逻辑显示对话文本
    /// </summary>
    public void ShowDialogRow()
    {
        animator.SetBool("Communication", true);

        for (int i = 0; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');

            if (cells[0]=="#"&&int.Parse(cells[1])==dialogIndex)
            {
                UIManager.Instance.ShowPanle<TalkPanel>().talkText.rectTransform.position = cells[2] == "奇怪老人" ? bkScreenPos1 : bkScreenPos2;
                UIManager.Instance.ShowPanle<TalkPanel>().BK.rectTransform.position = cells[2] == "奇怪老人" ? bkScreenPos1 : bkScreenPos2;

                //调用更新文本的方法
                UpdateText(cells[3]);

                //更新当前索引值
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
    /// 更新对话文本信息
    /// </summary>
    /// <param name="text"></param>
    public void UpdateText(string text)
    {
        UIManager.Instance.GetPanel<TalkPanel>().talkText.text = text;
    }
}
