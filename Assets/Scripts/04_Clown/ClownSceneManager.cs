using UnityEngine;

public class ClownSceneManager : DefaultSceneManager

{
    public UnicycleController unicycleController;
    public ClownRaycast clownRaycast;




    void Start()
    {
        WhenStart();
    }

    void Update()
    {
        WhenUpdate();
    }




    // ESC ������ �Ͻ����� �� â ����
    public override void PressESC()
    {
        if (!isPausing && Input.GetKeyDown(KeyCode.Escape)) // �Ͻ�����
        {
            Cursor.visible = true;
            isPausing = true;
            audioMixer.SetFloat("Pitch", 0f);
            Time.timeScale = 0f;
            playerController.enabled = false;
            unicycleController.enabled = false;
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
    public override void PressReturnButton()
    {
        Cursor.visible = false;
        isPausing = false;
        audioMixer.SetFloat("Pitch", 1f);
        Time.timeScale = 1f;
        putDialogScript.enabled = true;
        if (!putDialogScript.isClickMode)
        {
            if (!clownRaycast.isRidingUnicycle)
            {
                playerController.enabled = true;
            }
            else if (clownRaycast.isRidingUnicycle)
            {
                unicycleController.enabled = true;
            }
        }
        
        pauseUI.SetActive(false);
    }




    // ��� ����� �ؽ�Ʈ ���ΰ�ħ
    public override void ReloadText()
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
        uiTexts[13].text = GameManager.instance.textFileManager.ui[21];
        uiTexts[14].text = GameManager.instance.textFileManager.ui[22];
        uiTexts[15].text = GameManager.instance.textFileManager.ui[23];
        uiTexts[16].text = GameManager.instance.textFileManager.ui[25];
    }
}
