using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour
{
    //�洢������ÿ�������λ��
    private Transform[] childs;

    // Start is called before the first frame update
    void Start()
    {
        //���ٴ洢�ռ�
        childs = new Transform[this.transform.childCount];
        //��ȡÿ�������Ӷ�����д洢
        for (int i = 0; i < this.transform.childCount; i++)
        {
            childs[i] = this.transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�������еĻ�����ÿ��Sprite�ĳ���ʼ�ճ�������� �γ�2.5DЧ��
        for (int i = 0; i < childs.Length; i++)
        {
            if (childs[i] != null)
                childs[i].rotation = Camera.main.transform.rotation;
        }
    }
}
