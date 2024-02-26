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

    // 로딩 씬이 켜지면 바로 로드 씬 코루틴 실행
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




    // 다음 씬 지정 후, 로딩 씬으로 이동
    public static void LoadScene(string sceneName, string imageName)
    {
        nextScene = sceneName;
        loadingImageName = imageName;
        isLoadingImageOn = true;
        isFailLoadScene = false;
        SceneManager.LoadScene("LoadingScene");
    }
    public static void LoadScene(string sceneName) // 로딩 이미지가 없는 검은 화면 로딩
    {
        nextScene = sceneName;
        isLoadingImageOn = false;
        isFailLoadScene = false;
        SceneManager.LoadScene("LoadingScene");
    }

    // 로드 씬 코루틴
    IEnumerator LoadSceneCoroutine()
    {
        yield return null;

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




    // 죽어서 되돌리는 로딩 씬
    public static void failLoadScene(string sceneName)
    {
        nextScene = sceneName;
        isLoadingImageOn = false;
        isFailLoadScene = true;
        SceneManager.LoadScene("LoadingScene");
    }

    // 죽어서 되돌리는 로드 씬 코루틴
    IEnumerator failLoadSceneCoroutine()
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
    }
}
