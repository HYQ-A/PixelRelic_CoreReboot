using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 面板基类
/// </summary>
public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    //淡入淡出的速度
    private float canvasSpeed = 10;
    //删除panle的函数
    private UnityAction hideCallBack;
    //panel是否显示
    public bool isShow = false;

    protected virtual void Awake()
    {
        //一开始就获取负责淡入淡出的脚本
        canvasGroup = this.GetComponent<CanvasGroup>();
        //如果没有就添加获取
        if (canvasGroup == null)
            canvasGroup = this.AddComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// 子类用来初始化的方法
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 面板显示的方法
    /// </summary>
    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShow = true;
    }

    /// <summary>
    /// 面板隐藏的方法
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
        //淡入
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += canvasSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
            }
        }
        //淡出
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= canvasSpeed * Time.deltaTime;
            if(canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //执行删除panel函数
                hideCallBack?.Invoke();
            }
        }
    }
}
