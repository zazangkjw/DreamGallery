using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

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

    WaitForSeconds wait = new WaitForSeconds(0.01f);
    public FadeInOutScript fadeInOutScript;
    public RawImage fadeInOutImage;

    public GameObject settingUI; // ���� UI


    void Start()
    {
        GameManager.instance.urpRenderer.rendererFeatures[0].SetActive(true);

        Cursor.visible = true;

        ArtGalleryDirector.isFromDream = false;

        // �޴�UI Ȱ��ȭ
        menuUI.SetActive(true);

        // ���콺 ���� �޾ƿ���
        mouseSens.value = GameManager.instance.saveManager.settingData.mouseSens;

        // ��� �޾ƿ��� �ؽ�Ʈ ���ΰ�ħ
        language.value = GameManager.instance.saveManager.settingData.language;
        ReloadText();

        // �ػ� �޾ƿ���
        resolution.value = GameManager.instance.saveManager.settingData.resolution;
        isFullScreen.isOn = GameManager.instance.saveManager.settingData.isFullScreen;

        // ���� �޾ƿ��� ���� ���ΰ�ħ
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

    // ESC ������ â ����
    public void PressESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingUI.activeInHierarchy) // ����â ���������� �ݱ�
            {
                PressCancelButon();
            }
        }
    }

    // ���� ���� ��ư
    public void PressApplyButton()
    {
        // ���콺 ���� ������
        GameManager.instance.saveManager.settingData.mouseSens = mouseSens.value;

        // ��� ������ �ؽ�Ʈ ���ΰ�ħ
        GameManager.instance.saveManager.settingData.language = language.value;
        GameManager.instance.textFileManager.Reload(language.value);
        ReloadText();

        // �ػ� ������ �ػ� ���ΰ�ħ
        GameManager.instance.saveManager.SetResolution(resolution.value);
        GameManager.instance.saveManager.settingData.isFullScreen = isFullScreen.isOn;
        Screen.SetResolution(GameManager.instance.saveManager.settingData.width, GameManager.instance.saveManager.settingData.height, isFullScreen.isOn);

        // ���� ������ ���� ���ΰ�ħ
        GameManager.instance.saveManager.settingData.volume = volume.value;
        if (volume.value == -40f)
        {
            audioMixer.SetFloat("Master", -80f);
        }
        else
        {
            audioMixer.SetFloat("Master", volume.value);
        }

        // ���� �����ͷ� ���� ���� ����
        GameManager.instance.saveManager.SaveSettingData();
    }

    // ���� ��� ��ư
    public void PressCancelButon()
    {
        settingUI.SetActive(false);

        // ���콺 ���� �޾ƿ���(���� �� �������� �ٲٱ� ������)
        mouseSens.value = GameManager.instance.saveManager.settingData.mouseSens;

        // ��� �޾ƿ���(���� �� �������� �ٲٱ� ������)
        language.value = GameManager.instance.saveManager.settingData.language;

        // �ػ� �޾ƿ���(���� �� �������� �ٲٱ� ������)
        resolution.value = GameManager.instance.saveManager.settingData.resolution;
        isFullScreen.isOn = GameManager.instance.saveManager.settingData.isFullScreen;

        // ���� �޾ƿ���(���� �� �������� �ٲٱ� ������)
        volume.value = GameManager.instance.saveManager.settingData.volume;
    }

    // ��� ����� �ؽ�Ʈ ���ΰ�ħ
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
    }

    // ���� ����
    public void Quit()
    {
        Application.Quit();
    }
}