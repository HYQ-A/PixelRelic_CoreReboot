using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    //Ѫ��UI
    public List<Image> hpImages;
    //��ѪUI
    public List<Image> dsHpImages;
    public Button backBtn;
    public Button bagBtn;
    public Image ESkillImage;
    public Image QSkillImage;
    public Text EtimeText;
    public Text QtimeText;

    public override void Init()
    {
        backBtn.onClick.AddListener(() =>
        {
            //����ʾ���
            UIManager.Instance.ShowPanle<TipPanel>();
        });

        bagBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanle<BagPanel>();
        });
    }

    protected override void Update()
    {
        base.Update();
        
    }
}
