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
    private TextMeshProUGUI endText;

    [SerializeField]
    private AudioSource rewindSound;

    [SerializeField]
    private AudioSource endSound;

    public static string nextScene;
    public static string loadingImageName;
    private static bool isLoadingImageOn;
    private static bool isFailLoadScene;
    private static bool isSuccessLoadScene;

    // �ε� ���� ������ �ٷ� �ε� �� �ڷ�ƾ ����
    void Start()
    {
        ReloadText();
        Cursor.visible = false;
        loadingImage.enabled = false;
        progressBar.enabled = false;
        progressBarFrame.enabled = false;
        rewindText.enabled = false;
        endText.enabled = false;
        
        if (isLoadingImageOn)
        {
            loadingImage.texture = Resources.Load<Texture2D>(loadingImageName);
            loadingImage.enabled = true;
        }

        if (isFailLoadScene)
        {
            StartCoroutine(FailLoadSceneCoroutine());
        }
        else if (isSuccessLoadScene)
        {
            StartCoroutine(SuccessLoadSceneCoroutine());
        }
        else
        {
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
        isSuccessLoadScene = false;
        SceneManager.LoadScene("00_LoadingScene");
    }
    public static void LoadScene(string sceneName) // �ε� �̹����� ���� ���� ȭ�� �ε�
    {
        nextScene = sceneName;
        isLoadingImageOn = false;
        isFailLoadScene = false;
        isSuccessLoadScene = false;
        SceneManager.LoadScene("00_LoadingScene");
    }

    // �ε� �� �ڷ�ƾ
    IEnumerator LoadSceneCoroutine()
    {
        progressBar.enabled = true;
        progressBarFrame.enabled = true;

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




    // ����, ���� ���� �ε� ��
    public static void SuccessLoadScene(string sceneName)
    {
        nextScene = sceneName;
        isLoadingImageOn = false;
        isFailLoadScene = false;
        isSuccessLoadScene = true;
        SceneManager.LoadScene("00_LoadingScene");
    }

    // ����, ���� ���� �ε� �� �ڷ�ƾ
    IEnumerator SuccessLoadSceneCoroutine()
    {
        // �񵿱� �� �ε� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(1f);

        endText.enabled = true;
        endSound.Play();

        yield return new WaitForSeconds(2f);

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress < 0.9f)
            {
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                endText.enabled = false;
                System.GC.Collect();
                yield return new WaitForSeconds(1f);
                asyncLoad.allowSceneActivation = true;
                break;
            }
        }
    }




    // �׾ �ǵ����� �ε� ��
    public static void FailLoadScene(string sceneName)
    {
        nextScene = sceneName;
        isLoadingImageOn = false;
        isFailLoadScene = true;
        isSuccessLoadScene = false;
        SceneManager.LoadScene("00_LoadingScene");
    }

    // �׾ �ǵ����� �ε� �� �ڷ�ƾ
    IEnumerator FailLoadSceneCoroutine()
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
        endText.text = GameManager.instance.textFileManager.ui[17];
    }
}
