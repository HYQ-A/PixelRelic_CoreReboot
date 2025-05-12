using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背景音乐单例
/// </summary>
public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instance => instance;

    public AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        //获得音乐组件
        audioSource = this.GetComponent<AudioSource>();
        //获取数据管理器中的音乐数据
        MusicData data = GameDataMgr.Instance.musicData;
        SetIsOpen(data.musicIsOpen);
        ChangeMusicValue(data.musicValue);
    }

    /// <summary>
    /// 是否开启音乐的方法
    /// </summary>
    /// <param name="isOpen"></param>
    public void SetIsOpen(bool isOpen)
    {
        audioSource.mute = !isOpen;
    }

    /// <summary>
    /// 改变音乐大小的方法
    /// </summary>
    /// <param name="value"></param>
    public void ChangeMusicValue(float value)
    {
        audioSource.volume = value;
    }
}
