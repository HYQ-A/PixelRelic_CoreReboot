using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : BasePanel
{
    public Button closeBtn;
    public Toggle musicTog;
    public Toggle soundTog;
    public Slider musicSlider;
    public Slider soundSlider;

    public override void Init()
    {
        //һ��ʼ��Ҫ��ʼ�����������������
        musicTog.isOn = GameDataMgr.Instance.musicData.musicIsOpen;
        soundTog.isOn = GameDataMgr.Instance.musicData.soundIsOpen;
        musicSlider.value = GameDataMgr.Instance.musicData.musicValue;
        soundSlider.value = GameDataMgr.Instance.musicData.soundValue;

        closeBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanle<OptionPanel>();
        });

        musicTog.onValueChanged.AddListener((isOpen) =>
        {
            BKMusic.Instance.SetIsOpen(isOpen);
            //��¼��������
            GameDataMgr.Instance.musicData.musicIsOpen = isOpen;
        });

        soundTog.onValueChanged.AddListener((isOpen) =>
        {
            //��¼��������
            GameDataMgr.Instance.musicData.soundIsOpen = isOpen;
        });

        musicSlider.onValueChanged.AddListener((value) =>
        {
            BKMusic.Instance.ChangeMusicValue(value);
            //��¼��������
            GameDataMgr.Instance.musicData.musicValue = value;
        });

        soundSlider.onValueChanged.AddListener((value) =>
        {
            //��¼��������
            GameDataMgr.Instance.musicData.soundValue = value;
        });
    }

}
