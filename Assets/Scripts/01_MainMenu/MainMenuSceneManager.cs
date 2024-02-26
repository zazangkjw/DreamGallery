using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuSceneManager : MonoBehaviour
{
    public GameObject menuUI;
    public Scrollbar mouseSens;
    public TMP_Dropdown language;
    public TextMeshProUGUI[] uiTexts;

    //public int width;
    //public int height;
    //public bool fullScreen;

    WaitForSeconds wait = new WaitForSeconds(0.01f);
    public FadeInOutScript fadeInOutScript;
    public RawImage fadeInOutImage;

    public GameObject settingUI; // ���� UI

    void Start()
    {
        // �޴�UI Ȱ��ȭ
        menuUI.SetActive(true);

        // ���콺 ���� �޾ƿ���
        mouseSens.value = GameManager.instance.saveManager.settingData.mouseSens;

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
    }

    // ���� ����
    public void Quit()
    {
        Application.Quit();
    }
}