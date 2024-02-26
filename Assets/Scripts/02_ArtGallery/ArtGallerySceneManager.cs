using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArtGallerySceneManager : MonoBehaviour
{
    public Scrollbar mouseSens; // ���콺 ���� ��ũ�ѹ�
    public TMP_Dropdown language; // ����â ��� ��Ӵٿ�
    public TextMeshProUGUI[] uiTexts; // UI �ؽ�Ʈ ���

    public PlayerController playerController; // �÷��̾� ��Ʈ�ѷ� ��ũ��Ʈ
    public ArtGalleryDirector ArtGalleryDirector; // �� ���� �ƾ��� ����ִ� ��ũ��Ʈ
    public PutDialogScript putDialogScript; // ��� �ִ� ��ũ��Ʈ

    public RawImage fadeInOutImage; // ���̵�-��, �ƿ� �̹���
    public FadeInOutScript fadeInOutScript; // ���̵�-��, �ƿ� ��ũ��Ʈ

    public GameObject settingUI; // ���� UI
    public GameObject pauseUI; // �Ͻ����� UI
    public bool isPausing; // �Ͻ����� ��������

    //public int width;
    //public int height;
    //public bool fullScreen;




    void Start()
    {
        // ���콺 ���� �޾ƿ��� �÷��̾�� ����
        mouseSens.value = GameManager.instance.saveManager.settingData.mouseSens;
        playerController.lookSensitivity = mouseSens.value;

        // ��� �޾ƿ��� �ؽ�Ʈ ���ΰ�ħ
        language.value = GameManager.instance.saveManager.settingData.language;
        ReloadText();

        // �ػ� �޾ƿ��� �ػ� ���ΰ�ħ
        //width = GameManager.instance.saveManager.settingData.width;
        //height = GameManager.instance.saveManager.settingData.height;
        //fullScreen = GameManager.instance.saveManager.settingData.fullScreen;
        //Screen.SetResolution(width, height, fullScreen);
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
            isPausing = true;
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
        isPausing = false;
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
        // ���콺 ���� ������ �÷��̾�� ����
        GameManager.instance.saveManager.settingData.mouseSens = mouseSens.value;
        playerController.lookSensitivity = mouseSens.value;

        // ��� ������ �ؽ�Ʈ ���ΰ�ħ
        GameManager.instance.saveManager.settingData.language = language.value;
        GameManager.instance.textFileManager.Reload(language.value);
        ReloadText();

        // �ػ� ������ �ػ� ���ΰ�ħ
        //GameManager.instance.saveManager.settingData.width = width;
        //GameManager.instance.saveManager.settingData.height = height;
        //GameManager.instance.saveManager.settingData.fullScreen = fullScreen;
        //Screen.SetResolution(width, height, fullScreen);

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
    }

    // ������ ��ư
    public void PressExitButton()
    {
        Time.timeScale = 1f;
        LoadSceneScript.LoadScene("01_MainMenu");
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
    }
}