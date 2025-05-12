using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    public Button sureBtn;
    public Button closeBtn;

    public override void Init()
    {
        sureBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanle<GamePanel>();
            UIManager.Instance.HidePanle<TipPanel>();
            SceneManager.LoadScene("BeginScene");
        });

        closeBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanle<TipPanel>();
        });
    }
}
