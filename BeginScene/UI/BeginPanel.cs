using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button startBtn;
    public Button optionBtn;
    public Button quitBtn;

    public override void Init()
    {
        startBtn.onClick.AddListener(() =>
        {
            //隐藏开始界面
            UIManager.Instance.HidePanle<BeginPanel>();
            //关闭音乐
            BKMusic.Instance.audioSource.mute = true;
            //打开加载界面
            UIManager.Instance.ShowPanle<LoadingPanel>().LoadNextScene();
        });

        optionBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanle<OptionPanel>();
        });

        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
