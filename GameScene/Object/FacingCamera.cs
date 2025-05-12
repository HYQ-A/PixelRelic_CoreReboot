using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour
{
    //存储环境中每个对象的位置
    private Transform[] childs;

    // Start is called before the first frame update
    void Start()
    {
        //开辟存储空间
        childs = new Transform[this.transform.childCount];
        //获取每个环境子对象进行存储
        for (int i = 0; i < this.transform.childCount; i++)
        {
            childs[i] = this.transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //将场景中的环境中每个Sprite的朝向始终朝向摄像机 形成2.5D效果
        for (int i = 0; i < childs.Length; i++)
        {
            if (childs[i] != null)
                childs[i].rotation = Camera.main.transform.rotation;
        }
    }
}
