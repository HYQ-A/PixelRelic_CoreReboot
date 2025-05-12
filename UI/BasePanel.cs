using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ������
/// </summary>
public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    //���뵭�����ٶ�
    private float canvasSpeed = 10;
    //ɾ��panle�ĺ���
    private UnityAction hideCallBack;
    //panel�Ƿ���ʾ
    public bool isShow = false;

    protected virtual void Awake()
    {
        //һ��ʼ�ͻ�ȡ�����뵭���Ľű�
        canvasGroup = this.GetComponent<CanvasGroup>();
        //���û�о���ӻ�ȡ
        if (canvasGroup == null)
            canvasGroup = this.AddComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// ����������ʼ���ķ���
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// �����ʾ�ķ���
    /// </summary>
    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShow = true;
    }

    /// <summary>
    /// ������صķ���
    /// </summary>
    public virtual void HideMe(UnityAction callBack) 
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 1;

        isShow = false;
        hideCallBack = callBack;
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        //����
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += canvasSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
            }
        }
        //����
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= canvasSpeed * Time.deltaTime;
            if(canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //ִ��ɾ��panel����
                hideCallBack?.Invoke();
            }
        }
    }
}
