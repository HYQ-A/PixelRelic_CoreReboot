using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// UI������ д�ɵ���ģʽ
/// </summary>
public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;

    //�洢��� �����滻ԭ�� ����װ����
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    private Transform canvas;

    private UIManager()
    {
        //һ��ʼ�ʹ���������Ψһ��canvas ����ȡ��canvas
        canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas")).transform;
        //ȷ��canvasΨһ�� ��������ɾ��
        GameObject.DontDestroyOnLoad(canvas);
    }

    //��ʾ���
    //public BasePanel ShowPanle()
    public T ShowPanle<T>()where T : BasePanel
    {
        //��ȡT���͵����� �Զ������ ��T���͵���������Ԥ����������Ϊһ��
        string panelName = typeof(T).Name;

        //���ж��ֵ����Ƿ��Ѿ��д����
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }

        //��ʾ��� ���ø�����
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        panelObj.transform.SetParent(canvas.transform, false);
        //��ʾ���
        T panel = panelObj.GetComponent<T>();
        panel.ShowMe();
        //�洢���ֵ���
        panelDic.Add(panelName, panel);

        return panel;
    }


    /// <summary>
    /// �������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="isFade">�Ƿ񵭳�ɾ�� Ĭ����</param>
    public void HidePanle<T>(bool isFade = true) where T : BasePanel
    {
        string panleName = typeof(T).Name;

        //�����ֵ��н����ж�
        if (panelDic.ContainsKey(panleName))
        {
            if (isFade)
            {
                //�����������ķ��� �������������� �Դﵽ������ɺ�ɾ����岢���ֵ����Ƴ�
                panelDic[panleName].HideMe(() =>
                {
                    GameObject.Destroy(panelDic[panleName].gameObject);
                    //���ֵ����Ƴ�
                    panelDic.Remove(panleName);
                });
            }
            else
            {
                GameObject.Destroy(panelDic[panleName].gameObject);
                //���ֵ����Ƴ�
                panelDic.Remove(panleName);
            }
        }
    }

    //��ȡ���
    public T GetPanel<T>()where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;

        //û�ҵ��򷵻ؿ�
        return null;
    }


}
