using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    public Text loadText;
    public Slider loadSlider;

    public override void Init()
    {

    }

    public void LoadNextScene()
    {
        //��������Э�̺���
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        //������Ϸ����
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("GameScene");
        //������������Ϊfalse �ȼ��ص�90%ʱ�ټ���
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            loadSlider.value = asyncOperation.progress;
            loadText.text = asyncOperation.progress * 100 + "%";
            if (asyncOperation.progress >= 0.9f)
            {
                loadSlider.value = 1;
                loadText.text = "Please Input Any KeyDown To Ent Game";
                if (Input.anyKeyDown)
                {
                    asyncOperation.allowSceneActivation = true;
                    //���ؼ��ؽ���
                    UIManager.Instance.HidePanle<LoadingPanel>();
                    //��ʾ��Ϸ����
                    UIManager.Instance.ShowPanle<GamePanel>();
                }
            }
            yield return null;
        }
    }

}
