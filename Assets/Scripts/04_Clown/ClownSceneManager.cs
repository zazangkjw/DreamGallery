using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClownSceneManager : MonoBehaviour

{
    public TMP_Dropdown resolution;
    public Toggle isFullScreen;
    public Slider mouseSens; // 마우스 감도 스크롤바
    public TMP_Dropdown language; // 설정창 언어 드롭다운
    public TextMeshProUGUI[] uiTexts; // UI 텍스트 목록
    public Slider volume;
    public AudioMixer audioMixer;

    public PlayerController playerController; // 플레이어 컨트롤러 스크립트
    //public LionDanceDirector lionDanceDirector; // 이 씬의 컷씬이 담겨있는 스크립트
    public PutDialogScript putDialogScript; // 대사 넣는 스크립트

    public GameObject settingUI; // 설정 UI
    public GameObject pauseUI; // 일시정지 UI
    public bool isPausing; // 일시정지 상태인지


    void Start()
    {
        Cursor.visible = false;

        // 마우스 감도 받아오고 플레이어에게 적용
        mouseSens.value = GameManager.instance.saveManager.settingData.mouseSens;
        playerController.lookSensitivity = mouseSens.value;

        // 언어 받아오고 텍스트 새로고침
        language.value = GameManager.instance.saveManager.settingData.language;
        ReloadText();

        // 해상도 받아오고 해상도 새로고침
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

    void Update()
    {
        PressESC();
    }

    // ESC 누르면 일시정지 및 창 꺼짐
    public void PressESC()
    {
        if (!isPausing && Input.GetKeyDown(KeyCode.Escape)) // 일시정지
        {
            Cursor.visible = true;
            isPausing = true;
            audioMixer.SetFloat("Pitch", 0f);
            Time.timeScale = 0f;
            playerController.enabled = false;
            putDialogScript.enabled = false;
            pauseUI.SetActive(true);
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
    public void PressReturnButton()
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
    public void PressApplyButton()
    {
        // 마우스 감도 보내고 플레이어에게 적용
        GameManager.instance.saveManager.settingData.mouseSens = mouseSens.value;
        playerController.lookSensitivity = mouseSens.value;

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
    public void PressCancelButon()
    {
        settingUI.SetActive(false);

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
    public void PressExitButton()
    {
        audioMixer.SetFloat("Pitch", 1f);
        Time.timeScale = 1f;
        LoadSceneScript.LoadScene("02_ArtGallery");
    }

    // 언어 변경된 텍스트 새로고침
    public void ReloadText()
    {
        uiTexts[0].text = GameManager.instance.textFileManager.ui[4];
        uiTexts[1].text = GameManager.instance.textFileManager.ui[5];
        uiTexts[2].text = GameManager.instance.textFileManager.ui[6];
        uiTexts[3].text = GameManager.instance.textFileManager.ui[7];
        uiTexts[4].text = GameManager.instance.textFileManager.ui[8];
        uiTexts[5].text = GameManager.instance.textFileManager.ui[9];
        uiTexts[6].text = GameManager.instance.textFileManager.ui[10];
        uiTexts[7].text = GameManager.instance.textFileManager.ui[11];
        uiTexts[8].text = GameManager.instance.textFileManager.ui[12];
        uiTexts[9].text = GameManager.instance.textFileManager.ui[13];
        uiTexts[10].text = GameManager.instance.textFileManager.ui[18];
    }
}
