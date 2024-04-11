using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    private readonly string loadingtext1= "Loading";
    private readonly string loadingtext2= "Loading.";
    private readonly string loadingtext3= "Loading..";
    private readonly string loadingtext4= "Loading...";
    
    public GameObject[] btnList;
    public GameObject loadingPanel;
    public TextMeshProUGUI loadingText;
    public Image progressImage;
    private float fakeDelay = 1.5f;
    public void MoveScene(string sceneName)
    {
        foreach (var button in btnList)
        {
            button.SetActive(false);
        }
        loadingPanel.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync((sceneName));
        asyncOp.allowSceneActivation = false;

        float progress = 0f;
        while (!asyncOp.isDone)
        {
            progress = Mathf.Lerp(progress, asyncOp.progress, 0.95f);
            progressImage.fillAmount = progress;
            
            // progress가 10, 20, 30, 40, 50, 60, 70, 80일 때마다 loadingText 업데이트
            int progressPercent = (int)(progress * 100);
            if (progressPercent % 10 == 0 && progressPercent > 0)
            {
                UpdateLoadingText(progressPercent);
            }

            if (asyncOp.progress >= 0.9f)
            {
                yield return new WaitForSeconds(fakeDelay);
                loadingPanel.SetActive(false);
                asyncOp.allowSceneActivation = true;
            }
        }

        yield return null;
    }
    // progressPercent에 따라 loadingText 업데이트
    private void UpdateLoadingText(int progressPercent)
    {
        // loadingText에 따라 반복되는 패턴의 배열을 생성
        string[] loadingTexts = { loadingtext1, loadingtext2, loadingtext3, loadingtext4 };

        // progressPercent를 10으로 나눈 몫에 따라 해당하는 loadingTexts의 인덱스
        int index = (progressPercent / 10) % 4;

        // 인덱스를 사용하여 loadingText를 업데이트
        loadingText.text = loadingTexts[index];
    }
}
