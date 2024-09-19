using UnityEngine;

public class SpaceshipSceneManager : DefaultSceneManager
{
    public SpaceshipDirector spaceshipDirector; // �� ���� �ƾ��� ����ִ� ��ũ��Ʈ

    public bool isIntro = true;


    void Start()
    {
        WhenStart();

        if (isIntro) 
        { 
            spaceshipDirector.OpeningDirector(); 
            isIntro = false; 
        }
    }

    void Update()
    {
        WhenUpdate();
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
        uiTexts[13].text = GameManager.instance.textFileManager.ui[23];
    }
}
