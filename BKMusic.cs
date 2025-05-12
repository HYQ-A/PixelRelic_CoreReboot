using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������ֵ���
/// </summary>
public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instance => instance;

    public AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        //����������
        audioSource = this.GetComponent<AudioSource>();
        //��ȡ���ݹ������е���������
        MusicData data = GameDataMgr.Instance.musicData;
        SetIsOpen(data.musicIsOpen);
        ChangeMusicValue(data.musicValue);
    }

    /// <summary>
    /// �Ƿ������ֵķ���
    /// </summary>
    /// <param name="isOpen"></param>
    public void SetIsOpen(bool isOpen)
    {
        audioSource.mute = !isOpen;
    }

    /// <summary>
    /// �ı����ִ�С�ķ���
    /// </summary>
    /// <param name="value"></param>
    public void ChangeMusicValue(float value)
    {
        audioSource.volume = value;
    }
}
