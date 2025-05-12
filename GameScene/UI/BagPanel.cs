using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : BasePanel
{
    public Button closeBtn;
    public Image roleImg;
    private Animator animator;

    public override void Init()
    {
        closeBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanle<BagPanel>();
        });
    }

    protected override void Start()
    {
        base.Start();

        animator = roleImg.GetComponent<Animator>();
    }
}
