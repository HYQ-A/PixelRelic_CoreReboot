using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// UI管理类 写成单例模式
/// </summary>
public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;

    //存储面板 里氏替换原则 父类装子类
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    private Transform canvas;

    private UIManager()
    {
        //一开始就创建场景上唯一的canvas 并获取其canvas
        canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas")).transform;
        //确保canvas唯一性 过场景不删除
        GameObject.DontDestroyOnLoad(canvas);
    }

    //显示面板
    //public BasePanel ShowPanle()
    public T ShowPanle<T>()where T : BasePanel
    {
        //获取T类型的名字 自定义规则 将T类型的名字与其预制体名字设为一样
        string panelName = typeof(T).Name;

        //先判断字典中是否已经有此面板
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }

        //显示面板 设置父对象
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        panelObj.transform.SetParent(canvas.transform, false);
        //显示面板
        T panel = panelObj.GetComponent<T>();
        panel.ShowMe();
        //存储到字典中
        panelDic.Add(panelName, panel);

        return panel;
    }


    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="isFade">是否淡出删除 默认是</param>
    public void HidePanle<T>(bool isFade = true) where T : BasePanel
    {
        string panleName = typeof(T).Name;

        //先在字典中进行判断
        if (panelDic.ContainsKey(panleName))
        {
            if (isFade)
            {
                //调用隐藏面板的方法 并传入匿名函数 以达到淡出完成后删除面板并从字典中移除
                panelDic[panleName].HideMe(() =>
                {
                    GameObject.Destroy(panelDic[panleName].gameObject);
                    //从字典中移除
                    panelDic.Remove(panleName);
                });
            }
            else
            {
                GameObject.Destroy(panelDic[panleName].gameObject);
                //从字典中移除
                panelDic.Remove(panleName);
            }
        }
    }

    //获取面板
    public T GetPanel<T>()where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;

        //没找到则返回空
        return null;
    }


}
