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

    // 로딩 씬이 켜지면 바로 로드 씬 코루틴 실행
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




    // 다음 씬 지정 후, 로딩 씬으로 이동
    public static void LoadScene(string sceneName, string imageName)
    {
        nextScene = sceneName;
        loadingImageName = imageName;
        isLoadingImageOn = true;
        isFailLoadScene = false;
        isSuccessLoadScene = false;
        SceneManager.LoadScene("00_LoadingScene");
    }
    public static void LoadScene(string sceneName) // 로딩 이미지가 없는 검은 화면 로딩
    {
        nextScene = sceneName;
        isLoadingImageOn = false;
        isFailLoadScene = false;
        isSuccessLoadScene = false;
        SceneManager.LoadScene("00_LoadingScene");
    }

    // 로드 씬 코루틴
    IEnumerator LoadSceneCoroutine()
    {
        progressBar.enabled = true;
        progressBarFrame.enabled = true;

        // 비동기 씬 로딩 시작
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
        asyncLoad.allowSceneActivation = false;

        // 로딩 진행 상태 확인
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




    // 성공, 영상 종료 로딩 씬
    public static void SuccessLoadScene(string sceneName)
    {
        nextScene = sceneName;
        isLoadingImageOn = false;
        isFailLoadScene = false;
        isSuccessLoadScene = true;
        SceneManager.LoadScene("00_LoadingScene");
    }

    // 성공, 영상 종료 로드 씬 코루틴
    IEnumerator SuccessLoadSceneCoroutine()
    {
        // 비동기 씬 로딩 시작
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




    // 죽어서 되돌리는 로딩 씬
    public static void FailLoadScene(string sceneName)
    {
        nextScene = sceneName;
        isLoadingImageOn = false;
        isFailLoadScene = true;
        isSuccessLoadScene = false;
        SceneManager.LoadScene("00_LoadingScene");
    }

    // 죽어서 되돌리는 로드 씬 코루틴
    IEnumerator FailLoadSceneCoroutine()
    {
        // 비동기 씬 로딩 시작
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




    // 언어 변경된 텍스트 새로고침
    public void ReloadText()
    {
        rewindText.text = GameManager.instance.textFileManager.ui[16];
        endText.text = GameManager.instance.textFileManager.ui[17];
    }
}
