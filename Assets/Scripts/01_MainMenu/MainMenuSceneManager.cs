using UnityEngine;
using UnityEngine.UI;

public class MainMenuSceneManager : DefaultSceneManager
{
    public GameObject menuUI;
    WaitForSeconds wait = new WaitForSeconds(0.01f);
    public FadeInOutScript fadeInOutScript;
    public RawImage fadeInOutImage;


    void Start()
    {
        WhenStart();

        Cursor.visible = true;
        ArtGalleryDirector.isFromDream = false;
    }

    void Update()
    {
        WhenUpdate();
    }




    // ESC 누르면 창 꺼짐
    public override void PressESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingUI.activeInHierarchy) // 설정창 열려있으면 닫기
            {
                PressCancelButon();
            }
        }
    }




    // 언어 변경된 텍스트 새로고침
    public override void ReloadText()
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
        uiTexts[12].text = GameManager.instance.textFileManager.ui[19];
        uiTexts[13].text = GameManager.instance.textFileManager.ui[20];
        uiTexts[14].text = GameManager.instance.textFileManager.ui[23];
    }




    // 게임 종료
    public void Quit()
    {
        Application.Quit();
    }
}