using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class DefaultSceneManager : MonoBehaviour
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

    public PlayerController playerController; // �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ
    public PutDialogScript putDialogScript; // ��� �ִ� ��ũ��Ʈ

    public GameObject settingUI; // ���� UI
    public GameObject pauseUI; // �Ͻ����� UI
    public bool isPausing; // �Ͻ����� ��������

    // Start�� �ֱ�
    public void WhenStart()
    {
        GameManager.instance.urpRenderer.rendererFeatures[0].SetActive(true); // SSAO �ѱ�
        Cursor.visible = false; // ���콺 Ŀ�� ����

        // ������ ���� �޾ƿ���
        fpsLimit.text = GameManager.instance.saveManager.settingData.fpsLimit.ToString();
        isDisplayFps.isOn = GameManager.instance.saveManager.settingData.isDisplayFps;
        isVSyncOn.isOn = GameManager.instance.saveManager.settingData.vSync == 0 ? false : true;

        // ���콺 ���� �޾ƿ��� �÷��̾�� ����
        mouseSens.value = GameManager.instance.saveManager.settingData.mouseSens;
        if (playerController != null) { playerController.lookSensitivity = mouseSens.value; playerController.lookSensitivity = mouseSens.value; }     

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

    // Update�� �ֱ�
    public void WhenUpdate()
    {
        PressESC();
    }

    // ESC ������ �Ͻ����� �� â ����
    public virtual void PressESC()
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

    // ���� ���� ��ư
    public virtual void PressApplyButton()
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
        if (playerController != null) { playerController.lookSensitivity = mouseSens.value; }

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
    public virtual void PressCancelButon()
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
    public virtual void PressExitButton()
    {
        audioMixer.SetFloat("Pitch", 1f);
        Time.timeScale = 1f;
        LoadSceneScript.LoadScene("02_ArtGallery");
    }

    // ��� ����� �ؽ�Ʈ ���ΰ�ħ
    public virtual void ReloadText()
    {

    }
}

