using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LionDanceSceneManager : MonoBehaviour

{
    public TMP_Dropdown resolution;
    public Toggle isFullScreen;
    public Slider mouseSens; // ���콺 ���� ��ũ�ѹ�
    public TMP_Dropdown language; // ����â ��� ��Ӵٿ�
    public TextMeshProUGUI[] uiTexts; // UI �ؽ�Ʈ ���
    public Slider volume;
    public AudioMixer audioMixer;
    public TMP_InputField fpsLimit;
    public Toggle isDisplayFps;
    public Toggle isVSyncOn;

    public Material fogMat; // �Ȱ� ���׸���

    public PlayerController playerController; // �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ
    public LionDanceDirector lionDanceDirector; // �� ���� �ƾ��� ����ִ� ��ũ��Ʈ
    public PutDialogScript putDialogScript; // ��� �ִ� ��ũ��Ʈ

    public GameObject settingUI; // ���� UI
    public GameObject pauseUI; // �Ͻ����� UI
    public bool isPausing; // �Ͻ����� ��������


    void Start()
    {
        GameManager.instance.urpRenderer.rendererFeatures[0].SetActive(false); // SSAO ����. �ƽ� ī�޶� �������Ұ� ������� �̻��ϰ� ������ ������ �ƽ� ���� ��Ȱ��ȭ

        Cursor.visible = false;
        fogMat.color = new Color(1f, 1f, 1f, (255f / 255f)); // ���� �ʱ�ȭ

        // ������ ����
        lionDanceDirector.OpeningDirector();

        // ������ ���� �޾ƿ���
        fpsLimit.text = GameManager.instance.saveManager.settingData.fpsLimit.ToString();
        isDisplayFps.isOn = GameManager.instance.saveManager.settingData.isDisplayFps;
        isVSyncOn.isOn = GameManager.instance.saveManager.settingData.vSync == 0 ? false : true;

        // ���콺 ���� �޾ƿ��� �÷��̾�� ����
        mouseSens.value = GameManager.instance.saveManager.settingData.mouseSens;
        playerController.lookSensitivity = mouseSens.value;

        // ��� �޾ƿ��� �ؽ�Ʈ ���ΰ�ħ
        language.value = GameManager.instance.saveManager.settingData.language;
        ReloadText();

        // �ػ� �޾ƿ���
        resolution.value = GameManager.instance.saveManager.settingData.resolution;
        isFullScreen.isOn = GameManager.instance.saveManager.settingData.isFullScreen;

        // ���� �޾ƿ��� ���� ���ΰ�ħ
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

    // ESC ������ �Ͻ����� �� â ����
    public void PressESC()
    {
        if (!isPausing && Input.GetKeyDown(KeyCode.Escape)) // �Ͻ�����
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
            if (settingUI.activeInHierarchy) // ����â ���������� �ݱ�
            {
                PressCancelButon();
            }
            else // �Ͻ����� Ǯ��
            {
                PressReturnButton();
            }
        }
    }

    // ���ư��� ��ư
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

    // ���� ���� ��ư
    public void PressApplyButton()
    {
        // ������ ���� ������ ���ΰ�ħ
        try { GameManager.instance.saveManager.settingData.fpsLimit = int.Parse(fpsLimit.text); } catch (FormatException) { }
        GameManager.instance.saveManager.settingData.isDisplayFps = isDisplayFps.isOn;
        GameManager.instance.fps_Limit.setLimit();
        GameManager.instance.fps_Limit.setActive();
        GameManager.instance.saveManager.settingData.vSync = isVSyncOn.isOn == false ? 0 : 1;
        QualitySettings.vSyncCount = GameManager.instance.saveManager.settingData.vSync;

        // ���콺 ���� ������ �÷��̾�� ����
        GameManager.instance.saveManager.settingData.mouseSens = mouseSens.value;
        playerController.lookSensitivity = mouseSens.value;

        // ��� ������ �ؽ�Ʈ ���ΰ�ħ
        GameManager.instance.saveManager.settingData.language = language.value;
        GameManager.instance.textFileManager.Reload(language.value);
        ReloadText();

        // �ػ� ������ �ػ� ���ΰ�ħ
        GameManager.instance.saveManager.SetResolution(resolution.value);
        GameManager.instance.saveManager.settingData.isFullScreen = isFullScreen.isOn;
        Screen.SetResolution(GameManager.instance.saveManager.settingData.width, GameManager.instance.saveManager.settingData.height, isFullScreen.isOn);

        // ���� �����ͷ� ���� ���� ����
        GameManager.instance.saveManager.SaveSettingData();

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
    }

    // ���� ��� ��ư
    public void PressCancelButon()
    {
        settingUI.SetActive(false);

        // ������ ���� �޾ƿ���(���� �� �������� �ٲٱ� ������)
        fpsLimit.text = GameManager.instance.saveManager.settingData.fpsLimit.ToString();
        isDisplayFps.isOn = GameManager.instance.saveManager.settingData.isDisplayFps;
        isVSyncOn.isOn = GameManager.instance.saveManager.settingData.vSync == 0 ? false : true;

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

    // ������ ��ư
    public void PressExitButton()
    {
        audioMixer.SetFloat("Pitch", 1f);
        Time.timeScale = 1f;
        LoadSceneScript.LoadScene("02_ArtGallery");
    }

    // ��� ����� �ؽ�Ʈ ���ΰ�ħ
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
        uiTexts[11].text = GameManager.instance.textFileManager.ui[19];
        uiTexts[12].text = GameManager.instance.textFileManager.ui[20];
        uiTexts[13].text = GameManager.instance.textFileManager.ui[23];
    }

    // ���� ������� �ڷ�ƾ
    public void FogOut()
    {
        StartCoroutine(FogOutCoroutine());
    }

    IEnumerator FogOutCoroutine()
    {
        while (fogMat.color.a > 0f)
        {
            fogMat.color = new Color(1f, 1f, 1f, fogMat.color.a - ((1f / 10f) * Time.deltaTime));
            yield return null;
        }
    }
}
