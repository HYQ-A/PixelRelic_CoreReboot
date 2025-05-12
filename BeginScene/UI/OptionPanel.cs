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
        //一开始就要初始化设置面板的相关数据
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
            //记录音乐数据
            GameDataMgr.Instance.musicData.musicIsOpen = isOpen;
        });

        soundTog.onValueChanged.AddListener((isOpen) =>
        {
            //记录音乐数据
            GameDataMgr.Instance.musicData.soundIsOpen = isOpen;
        });

        musicSlider.onValueChanged.AddListener((value) =>
        {
            BKMusic.Instance.ChangeMusicValue(value);
            //记录音乐数据
            GameDataMgr.Instance.musicData.musicValue = value;
        });

        soundSlider.onValueChanged.AddListener((value) =>
        {
            //记录音乐数据
            GameDataMgr.Instance.musicData.soundValue = value;
        });
    }

}
