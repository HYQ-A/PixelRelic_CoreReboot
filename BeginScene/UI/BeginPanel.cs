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
            //���ؿ�ʼ����
            UIManager.Instance.HidePanle<BeginPanel>();
            //�ر�����
            BKMusic.Instance.audioSource.mute = true;
            //�򿪼��ؽ���
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
