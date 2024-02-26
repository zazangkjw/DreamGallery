using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneScript : MonoBehaviour
{
    [SerializeField]
    private RawImage loadingImage;

    [SerializeField]
    private Image progressBar;

    [SerializeField]
    private Image progressBarFrame;

    [SerializeField]
    private TextMeshProUGUI rewindText;

    [SerializeField]
    private AudioSource rewindSound;

    public static string nextScene;
    public static string loadingImageName;
    private static bool isLoadingImageOn;
    private static bool isFailLoadScene;

    // �ε� ���� ������ �ٷ� �ε� �� �ڷ�ƾ ����
    void Start()
    {
        ReloadText();

        if (isLoadingImageOn)
        {
            loadingImage.texture = Resources.Load<Texture2D>(loadingImageName);
            loadingImage.enabled = true;
        }
        else
        {
            loadingImage.enabled = false;
        }

        if (isFailLoadScene)
        {
            progressBar.enabled = false;
            progressBarFrame.enabled = false;
            rewindText.enabled = false;
            StartCoroutine(failLoadSceneCoroutine());
        }
        else
        {
            progressBar.enabled = true;
            progressBarFrame.enabled = true;
            rewindText.enabled = false;
            StartCoroutine(LoadSceneCoroutine());
        }
    }




    // ���� �� ���� ��, �ε� ������ �̵�
    public static void LoadScene(string sceneName, string imageName)
    {
        nextScene = sceneName;
        loadingImageName = imageName;
        isLoadingImageOn = true;
        isFailLoadScene = false;
        SceneManager.LoadScene("LoadingScene");
    }
    public static void LoadScene(string sceneName) // �ε� �̹����� ���� ���� ȭ�� �ε�
    {
        nextScene = sceneName;
        isLoadingImageOn = false;
        isFailLoadScene = false;
        SceneManager.LoadScene("LoadingScene");
    }

    // �ε� �� �ڷ�ƾ
    IEnumerator LoadSceneCoroutine()
    {
        yield return null;

        // �񵿱� �� �ε� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
        asyncLoad.allowSceneActivation = false;

        // �ε� ���� ���� Ȯ��
        while(!asyncLoad.isDone) 
        {
            if(asyncLoad.progress < 0.9f)
            {
                progressBar.fillAmount = asyncLoad.progress;
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                progressBar.fillAmount = 1f;
                System.GC.Collect();
                asyncLoad.allowSceneActivation = true;
                break;
            }
        }
    }




    // �׾ �ǵ����� �ε� ��
    public static void failLoadScene(string sceneName)
    {
        nextScene = sceneName;
        isLoadingImageOn = false;
        isFailLoadScene = true;
        SceneManager.LoadScene("LoadingScene");
    }

    // �׾ �ǵ����� �ε� �� �ڷ�ƾ
    IEnumerator failLoadSceneCoroutine()
    {
        // �񵿱� �� �ε� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
        asyncLoad.allowSceneActivation = false;
        
        yield return new WaitForSeconds(1f);
        rewindText.enabled = true;
        rewindSound.Play();
        yield return new WaitForSeconds(2f);

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress < 0.9f)
            {
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                rewindText.enabled = false;
                rewindSound.Stop();
                System.GC.Collect();
                yield return new WaitForSeconds(1f);
                asyncLoad.allowSceneActivation = true;
                break;
            }
        }
    }




    // ��� ����� �ؽ�Ʈ ���ΰ�ħ
    public void ReloadText()
    {
        rewindText.text = GameManager.instance.textFileManager.ui[16];
    }
}
