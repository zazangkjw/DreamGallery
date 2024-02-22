using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSceneManager : MonoBehaviour
{
    public GameObject menuUI;
    public Scrollbar mouseSens;
    public Dropdown language;
    //public int width;
    //public int height;
    //public bool fullScreen;
    WaitForSeconds wait = new WaitForSeconds(0.01f);
    public FadeInOutScript fadeInOutScript;
    public RawImage fadeInOutImage;

    void Start()
    {
        // 메뉴UI 활성화
        menuUI.SetActive(true);

        // 마우스 감도 받아오기
        mouseSens.value = GameManager.instance.saveManager.settingData.mouseSens;

        // 언어 받아오고 텍스트 새로고침
        language.value = GameManager.instance.saveManager.settingData.language;
        ReloadText();

        // 해상도 받아오고 해상도 새로고침
        //width = GameManager.instance.saveManager.settingData.width; 
        //height = GameManager.instance.saveManager.settingData.height;
        //fullScreen = GameManager.instance.saveManager.settingData.fullScreen;
        //Screen.SetResolution(width, height, fullScreen);
    }

    // 설정 적용 버튼
    public void PressApplyButton()
    {
        // 마우스 감도 보내기
        GameManager.instance.saveManager.settingData.mouseSens = mouseSens.value;

        // 언어 보내고 텍스트 새로고침
        GameManager.instance.saveManager.settingData.language = language.value;
        GameManager.instance.textFileManager.Reload(language.value);
        ReloadText();

        // 해상도 보내고 해상도 새로고침
        //GameManager.instance.saveManager.settingData.width = width;
        //GameManager.instance.saveManager.settingData.height = height;
        //GameManager.instance.saveManager.settingData.fullScreen = fullScreen;
        //Screen.SetResolution(width, height, fullScreen);

        // 보낸 데이터로 설정 파일 저장
        GameManager.instance.saveManager.SaveSettingData();
    }

    // 설정 취소 버튼
    public void PressCancelButon()
    {
        // 마우스 감도 받아오기(적용 안 눌렀으면 바꾸기 전으로)
        mouseSens.value = GameManager.instance.saveManager.settingData.mouseSens;

        // 언어 받아오기(적용 안 눌렀으면 바꾸기 전으로)
        language.value = GameManager.instance.saveManager.settingData.language;
    }

    // 언어 변경된 텍스트 새로고침
    public Text[] uiTexts;
    public void ReloadText()
    {
        uiTexts[0].text = GameManager.instance.textFileManager.ui[0];
        uiTexts[1].text = GameManager.instance.textFileManager.ui[1];
        uiTexts[2].text = GameManager.instance.textFileManager.ui[2];
        uiTexts[3].text = GameManager.instance.textFileManager.ui[3];
        uiTexts[4].text = GameManager.instance.textFileManager.ui[7];
        uiTexts[5].text = GameManager.instance.textFileManager.ui[8];
        uiTexts[6].text = GameManager.instance.textFileManager.ui[9];
        uiTexts[7].text = GameManager.instance.textFileManager.ui[10];
        uiTexts[8].text = GameManager.instance.textFileManager.ui[11];
        uiTexts[9].text = GameManager.instance.textFileManager.ui[12];
        uiTexts[10].text = GameManager.instance.textFileManager.ui[13];
    }

    public void GoToNextScene()
    {
        StartCoroutine(NextSceneCoroutine());
    }

    // 다음 씬 이동 코루틴
    IEnumerator NextSceneCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        fadeInOutScript.FadeIn(fadeInOutImage);
        yield return new WaitForSeconds(2f);
        LoadSceneScript.LoadScene("02_ArtGallery");
    }

    // 게임 종료
    public void Quit()
    {
        Application.Quit();
    }
}