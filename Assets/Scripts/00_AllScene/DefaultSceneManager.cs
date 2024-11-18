using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class DefaultSceneManager : MonoBehaviour
{
    public PlayerController playerController; // �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ
    public PutDialogScript putDialogScript; // ��� �ִ� ��ũ��Ʈ
    public AudioMixer audioMixer;

    public GameObject settingUI; // ���� UI
    public GameObject pauseUI; // �Ͻ����� UI
    public static bool isPausing; // �Ͻ����� ��������

    public TMP_Dropdown resolution;
    public Toggle isFullScreen;
    public Slider mouseSens; // ���콺 ���� ��ũ�ѹ�
    public TMP_Dropdown language; // ����â ��� ��Ӵٿ�
    public Slider volume;
    public TMP_InputField fpsLimit;
    public Toggle isDisplayFps;
    public Toggle isVSyncOn;
    public TextMeshProUGUI[] uiTexts; // UI �ؽ�Ʈ ���

    public GameObject inventory;

    public void Reset()
    {
        settingUI = GameObject.Find("SettingUI");
        pauseUI = GameObject.Find("PauseUI");

        resolution = GameObject.Find("ResolutionDropdown").GetComponent<TMP_Dropdown>();
        isFullScreen = GameObject.Find("FullScreenToggle").GetComponent<Toggle>();
        mouseSens = GameObject.Find("MouseSensitiveSlider").GetComponent<Slider>(); // ���콺 ���� ��ũ�ѹ�
        language = GameObject.Find("LanguageDropdown").GetComponent<TMP_Dropdown>(); // ����â ��� ��Ӵٿ�
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
            // �κ��丮�� ���������� �ݱ�
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
        isPausing = false;
        audioMixer.SetFloat("Pitch", 1f);
        Time.timeScale = 1f;
        LoadSceneScript.LoadScene("02_ArtGallery");
    }

    // ��� ����� �ؽ�Ʈ ���ΰ�ħ
    public virtual void ReloadText()
    {

    }
}

