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
            
            // progress�� 10, 20, 30, 40, 50, 60, 70, 80�� ������ loadingText ������Ʈ
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
    // progressPercent�� ���� loadingText ������Ʈ
    private void UpdateLoadingText(int progressPercent)
    {
        // loadingText�� ���� �ݺ��Ǵ� ������ �迭�� ����
        string[] loadingTexts = { loadingtext1, loadingtext2, loadingtext3, loadingtext4 };

        // progressPercent�� 10���� ���� �� ���� �ش��ϴ� loadingTexts�� �ε���
        int index = (progressPercent / 10) % 4;

        // �ε����� ����Ͽ� loadingText�� ������Ʈ
        loadingText.text = loadingTexts[index];
    }
}
