using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class DefaultSceneManager : MonoBehaviour
{
    public PlayerController playerController; // 플레이어 컨트롤러 스크립트
    public PutDialogScript putDialogScript; // 대사 넣는 스크립트
    public AudioMixer audioMixer;

    public GameObject settingUI; // 설정 UI
    public GameObject pauseUI; // 일시정지 UI
    public static bool isPausing; // 일시정지 상태인지

    public TMP_Dropdown resolution;
    public Toggle isFullScreen;
    public Slider mouseSens; // 마우스 감도 스크롤바
    public TMP_Dropdown language; // 설정창 언어 드롭다운
    public Slider volume;
    public TMP_InputField fpsLimit;
    public Toggle isDisplayFps;
    public Toggle isVSyncOn;
    public TextMeshProUGUI[] uiTexts; // UI 텍스트 목록

    public GameObject inventory;

    public void Reset()
    {
        settingUI = GameObject.Find("SettingUI");
        pauseUI = GameObject.Find("PauseUI");

        resolution = GameObject.Find("ResolutionDropdown").GetComponent<TMP_Dropdown>();
        isFullScreen = GameObject.Find("FullScreenToggle").GetComponent<Toggle>();
        mouseSens = GameObject.Find("MouseSensitiveSlider").GetComponent<Slider>(); // 마우스 감도 스크롤바
        language = GameObject.Find("LanguageDropdown").GetComponent<TMP_Dropdown>(); // 설정창 언어 드롭다운
        volume = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        fpsLimit = GameObject.Find("FPSLimitInputField").GetComponent<TMP_InputField>();
        isDisplayFps = GameObject.Find("FPSDisplayToggle").GetComponent<Toggle>();
        isVSyncOn = GameObject.Find("VSyncToggle").GetComponent<Toggle>();

        uiTexts = new TextMeshProUGUI[100];
        uiTexts[0] = GameObject.Find("ReturnText").GetComponent<TextMeshProUGUI>();
        uiTexts[1] = GameObject.Find("SettingText").GetComponent<TextMeshProUGUI>();
        uiTexts[2] = GameObject.Find("ExitText").GetComponent<TextMeshProUGUI>();
        uiTexts[3] = GameObject.Find("SettingTitleText").GetComponent<TextMeshProUGUI>();
        uiTexts[4] = GameObject.Find("ResolutionText").GetComponent<TextMeshProUGUI>();
        uiTexts[5] = GameObject.Find("MouseSensitiveText").GetComponent<TextMeshProUGUI>();
        uiTexts[6] = GameObject.Find("LanguageText").GetComponent<TextMeshProUGUI>();
        uiTexts[7] = GameObject.Find("VolumeText").GetComponent<TextMeshProUGUI>();
        uiTexts[8] = GameObject.Find("ApplyText").GetComponent<TextMeshProUGUI>();
        uiTexts[9] = GameObject.Find("CancelText").GetComponent<TextMeshProUGUI>();
        uiTexts[10] = GameObject.Find("FullScreenText").GetComponent<TextMeshProUGUI>();
        uiTexts[11] = GameObject.Find("FPSLimitText").GetComponent<TextMeshProUGUI>();
        uiTexts[12] = GameObject.Find("FPSDisplayText").GetComponent<TextMeshProUGUI>();
        uiTexts[13] = GameObject.Find("VSyncText").GetComponent<TextMeshProUGUI>();
        uiTexts[14] = GameObject.Find("SkipText").GetComponent<TextMeshProUGUI>();
    }

    // Start에 넣기
    public void WhenStart()
    {
        GameManager.instance.urpRenderer.rendererFeatures[0].SetActive(true); // SSAO 켜기
        Cursor.visible = false; // 마우스 커서 끄기

        // 프레임 제한 받아오기
        fpsLimit.text = GameManager.instance.saveManager.settingData.fpsLimit.ToString();
        isDisplayFps.isOn = GameManager.instance.saveManager.settingData.isDisplayFps;
        isVSyncOn.isOn = GameManager.instance.saveManager.settingData.vSync == 0 ? false : true;

        // 마우스 감도 받아오고 플레이어에게 적용
        mouseSens.value = GameManager.instance.saveManager.settingData.mouseSens;
        if (playerController != null) { playerController.lookSensitivity = mouseSens.value; playerController.lookSensitivity = mouseSens.value; }     

        // 언어 받아오고 텍스트 새로고침
        language.value = GameManager.instance.saveManager.settingData.language;
        ReloadText();

        // 해상도 받아오기
        resolution.value = GameManager.instance.saveManager.settingData.resolution;
        isFullScreen.isOn = GameManager.instance.saveManager.settingData.isFullScreen;

        // 볼륨 받아오고 볼륨 새로고침
        volume.value = GameManager.instance.saveManager.settingData.volume;
        if (volume.value == -40f)
        {
            audioMixer.SetFloat("Master", -80f);
        }
        else
        {
            audioMixer.SetFloat("Master", volume.value);
        }
    }

    // Update에 넣기
    public void WhenUpdate()
    {
        PressESC();
    }

    // ESC 누르면 일시정지 및 창 꺼짐
    public virtual void PressESC()
    {
        if (!isPausing && Input.GetKeyDown(KeyCode.Escape)) // 일시정지
        {
            // 인벤토리가 열려있으면 닫기
            if (inventory != null && inventory.activeSelf)
            {
                Cursor.visible = false;
                DefaultRaycast.inventoryOnOff = false;
                inventory.SetActive(false);
                playerController.isMouseLocked = false;

                Slot.selectedSlot = null;
                Slot.cursorImage.texture = null;
                Slot.cursorImage.gameObject.SetActive(false);
            }
            else
            {
                Cursor.visible = true;
                isPausing = true;
                audioMixer.SetFloat("Pitch", 0f);
                Time.timeScale = 0f;
                playerController.enabled = false;
                putDialogScript.enabled = false;
                pauseUI.SetActive(true);
            }
        }
        else if (isPausing && Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingUI.activeInHierarchy) // 설정창 열려있으면 닫기
            {
                PressCancelButon();
            }
            else // 일시정지 풀림
            {
                PressReturnButton();
            }
        }
    }

    // 돌아가기 버튼
    public virtual void PressReturnButton()
    {
        Cursor.visible = false;
        isPausing = false;
        audioMixer.SetFloat("Pitch", 1f);
        Time.timeScale = 1f;
        putDialogScript.enabled = true;
        if (!putDialogScript.isClickMode)
        {
            playerController.enabled = true;
        }

        pauseUI.SetActive(false);
    }

    // 설정 적용 버튼
    public virtual void PressApplyButton()
    {
        // 프레임 제한 보내고 새로고침
        try { GameManager.instance.saveManager.settingData.fpsLimit = int.Parse(fpsLimit.text); } catch (FormatException) { }
        GameManager.instance.saveManager.settingData.isDisplayFps = isDisplayFps.isOn;
        GameManager.instance.fps_Limit.setLimit();
        GameManager.instance.fps_Limit.setActive();
        GameManager.instance.saveManager.settingData.vSync = isVSyncOn.isOn == false ? 0 : 1;
        QualitySettings.vSyncCount = GameManager.instance.saveManager.settingData.vSync;

        // 마우스 감도 보내고 플레이어에게 적용
        GameManager.instance.saveManager.settingData.mouseSens = mouseSens.value;
        if (playerController != null) { playerController.lookSensitivity = mouseSens.value; }

        // 언어 보내고 텍스트 새로고침
        GameManager.instance.saveManager.settingData.language = language.value;
        GameManager.instance.textFileManager.Reload(language.value);
        ReloadText();

        // 해상도 보내고 해상도 새로고침
        GameManager.instance.saveManager.SetResolution(resolution.value);
        GameManager.instance.saveManager.settingData.isFullScreen = isFullScreen.isOn;
        Screen.SetResolution(GameManager.instance.saveManager.settingData.width, GameManager.instance.saveManager.settingData.height, isFullScreen.isOn);

        // 보낸 데이터로 설정 파일 저장
        GameManager.instance.saveManager.SaveSettingData();

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
    }

    // 설정 취소 버튼
    public virtual void PressCancelButon()
    {
        settingUI.SetActive(false);

        // 프레임 제한 받아오기(적용 안 눌렀으면 바꾸기 전으로)
        fpsLimit.text = GameManager.instance.saveManager.settingData.fpsLimit.ToString();
        isDisplayFps.isOn = GameManager.instance.saveManager.settingData.isDisplayFps;
        isVSyncOn.isOn = GameManager.instance.saveManager.settingData.vSync == 0 ? false : true;

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

    // 나가기 버튼
    public virtual void PressExitButton()
    {
        isPausing = false;
        audioMixer.SetFloat("Pitch", 1f);
        Time.timeScale = 1f;
        LoadSceneScript.LoadScene("02_ArtGallery");
    }

    // 언어 변경된 텍스트 새로고침
    public virtual void ReloadText()
    {

    }
}

