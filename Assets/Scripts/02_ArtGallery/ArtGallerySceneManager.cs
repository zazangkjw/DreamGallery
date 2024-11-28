using UnityEngine;
using UnityEngine.UI;

public class ArtGallerySceneManager : DefaultSceneManager
{
    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트




    void Start()
    {
        WhenStart();
    }

    void Update()
    {
        WhenUpdate();
    }


    // ESC 누르면 일시정지 및 창 꺼짐
    public override void PressESC()
    {
        if (!isPausing && Input.GetKeyDown(KeyCode.Escape) && !ArtGalleryRaycast.is_pop_up) // 일시정지
        {
            // 인벤토리가 열려있으면 닫기
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



    // 나가기 버튼
    public override void PressExitButton()
    {
        isPausing = false;
        audioMixer.SetFloat("Pitch", 1f);
        Time.timeScale = 1f;
        LoadSceneScript.LoadScene("01_MainMenu");
    }




    // 언어 변경된 텍스트 새로고침
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
        uiTexts[14].text = GameManager.instance.textFileManager.ui[25];
        uiTexts[15].text = GameManager.instance.textFileManager.ui[29];
        uiTexts[16].text = GameManager.instance.textFileManager.ui[30];
        uiTexts[17].text = GameManager.instance.textFileManager.ui[31];
        uiTexts[18].text = GameManager.instance.textFileManager.ui[32];
        uiTexts[19].text = GameManager.instance.textFileManager.ui[33];
        uiTexts[20].text = GameManager.instance.textFileManager.ui[34];
        uiTexts[21].text = GameManager.instance.textFileManager.ui[35];
        uiTexts[22].text = GameManager.instance.textFileManager.ui[36];
    }
}