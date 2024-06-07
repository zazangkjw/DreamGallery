using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using System;

public class MainMenuSceneManager : MonoBehaviour
{
    public GameObject menuUI;
    public TMP_Dropdown resolution;
    public Toggle isFullScreen;
    public Slider mouseSens;
    public TMP_Dropdown language;
    public TextMeshProUGUI[] uiTexts;
    public Slider volume;
    public AudioMixer audioMixer;
    public TMP_InputField fpsLimit;
    public Toggle isDisplayFps;

    WaitForSeconds wait = new WaitForSeconds(0.01f);
    public FadeInOutScript fadeInOutScript;
    public RawImage fadeInOutImage;

    public GameObject settingUI; // 설정 UI


    void Start()
    {
        GameManager.instance.urpRenderer.rendererFeatures[0].SetActive(true);

        Cursor.visible = true;

        ArtGalleryDirector.isFromDream = false;

        // 메뉴UI 활성화
        menuUI.SetActive(true);

        // 프레임 제한 받아오기
        fpsLimit.text = GameManager.instance.saveManager.settingData.fpsLimit.ToString();
        isDisplayFps.isOn = GameManager.instance.saveManager.settingData.isDisplayFps;

        // 마우스 감도 받아오기
        mouseSens.value = GameManager.instance.saveManager.settingData.mouseSens;

        // 언어 받아오고 텍스트 새로고침
        language.value = GameManager.instance.saveManager.settingData.language;
        ReloadText();

        // 해상도 받아오기
        resolution.value = GameManager.instance.saveManager.settingData.resolution;
        isFullScreen.isOn = GameManager.instance.saveManager.settingData.isFullScreen;

        // 볼륨 받아오고 볼륨 새로고침
        volume.value = GameManager.instance.saveManager.settingData.volume;
        if(volume.value == -40f)
        {
            audioMixer.SetFloat("Master", -80f);
        }
        else
        {
            audioMixer.SetFloat("Master", volume.value);
        }
    }

    void Update()
    {
        PressESC();
    }

    // ESC 누르면 창 꺼짐
    public void PressESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingUI.activeInHierarchy) // 설정창 열려있으면 닫기
            {
                PressCancelButon();
            }
        }
    }

    // 설정 적용 버튼
    public void PressApplyButton()
    {
        // 프레임 제한 보내고 새로고침
        try { GameManager.instance.saveManager.settingData.fpsLimit = int.Parse(fpsLimit.text); } catch (FormatException) { }
        GameManager.instance.saveManager.settingData.isDisplayFps = isDisplayFps.isOn;
        GameManager.instance.fps_Limit.setLimit();
        GameManager.instance.fps_Limit.setActive();

        // 마우스 감도 보내기
        GameManager.instance.saveManager.settingData.mouseSens = mouseSens.value;

        // 언어 보내고 텍스트 새로고침
        GameManager.instance.saveManager.settingData.language = language.value;
        GameManager.instance.textFileManager.Reload(language.value);
        ReloadText();

        // 해상도 보내고 해상도 새로고침
        GameManager.instance.saveManager.SetResolution(resolution.value);
        GameManager.instance.saveManager.settingData.isFullScreen = isFullScreen.isOn;
        Screen.SetResolution(GameManager.instance.saveManager.settingData.width, GameManager.instance.saveManager.settingData.height, isFullScreen.isOn);

        // 볼륨 보내고 볼륨 새로고침
        GameManager.instance.saveManager.settingData.volume = volume.value;
        if (volume.value == -40f)
        {
            audioMixer.SetFloat("Master", -80f);
        }
        else
        {
            audioMixer.SetFloat("Master", volume.value);
        }

        // 보낸 데이터로 설정 파일 저장
        GameManager.instance.saveManager.SaveSettingData();
    }

    // 설정 취소 버튼
    public void PressCancelButon()
    {
        settingUI.SetActive(false);

        // 프레임 제한 받아오기(적용 안 눌렀으면 바꾸기 전으로)
        fpsLimit.text = GameManager.instance.saveManager.settingData.fpsLimit.ToString();
        isDisplayFps.isOn = GameManager.instance.saveManager.settingData.isDisplayFps;

        // 마우스 감도 받아오기(적용 안 눌렀으면 바꾸기 전으로)
        mouseSens.value = GameManager.instance.saveManager.settingData.mouseSens;

        // 언어 받아오기(적용 안 눌렀으면 바꾸기 전으로)
        language.value = GameManager.instance.saveManager.settingData.language;

        // 해상도 받아오기(적용 안 눌렀으면 바꾸기 전으로)
        resolution.value = GameManager.instance.saveManager.settingData.resolution;
        isFullScreen.isOn = GameManager.instance.saveManager.settingData.isFullScreen;

        // 볼륨 받아오기(적용 안 눌렀으면 바꾸기 전으로)
        volume.value = GameManager.instance.saveManager.settingData.volume;
    }

    // 언어 변경된 텍스트 새로고침
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
        uiTexts[11].text = GameManager.instance.textFileManager.ui[18];
        uiTexts[12].text = GameManager.instance.textFileManager.ui[19];
        uiTexts[13].text = GameManager.instance.textFileManager.ui[20];
    }

    // 게임 종료
    public void Quit()
    {
        Application.Quit();
    }
}