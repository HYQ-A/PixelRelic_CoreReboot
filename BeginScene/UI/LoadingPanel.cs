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
        //开启加载协程函数
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        //加载游戏场景
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("GameScene");
        //允许场景激活设为false 等加载到90%时再激活
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
                    //隐藏加载界面
                    UIManager.Instance.HidePanle<LoadingPanel>();
                    //显示游戏界面
                    UIManager.Instance.ShowPanle<GamePanel>();
                }
            }
            yield return null;
        }
    }

}
