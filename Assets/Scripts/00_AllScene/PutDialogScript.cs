using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PutDialogScript : MonoBehaviour
{
    // 대사 사라지는 타이머, 대사 넣기 
    public TextMeshProUGUI dialogText;
    public List<string> textStacks; // 대기중인 텍스트들
    public Image dialogArrow;
    float timer; // 대사 사라지는 타이머
    public PlayerController playerController;

    public bool isClickMode;
    int textNum;
    WaitForSeconds wait = new WaitForSeconds(0.005f);
    WaitForSeconds wait2 = new WaitForSeconds(0.05f);
    bool isDialogArrowOn;

    Coroutine textPrintCoroutine;

    void Start()
    {
        StartCoroutine(DialogArrow());
    }

    void Update()
    {
        TextTimer();
        TextClick();
    }




    // 대사 넣기(타이머형)
    public void putDialog(string text, float time)
    {
        textStacks[0] = text;
        if (!isClickMode)
        {
            TextPrint();
            dialogText.enabled = true;
            timer = time;
        }
    }

    // 타이머형 대사
    void TextTimer()
    {
        if (!isClickMode)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }

            if (timer <= 0)
            {
                dialogText.enabled = false;
            }
        }
    }




    // 대사 넣기(클릭형)
    public void putDialogWithClick(string[] text)
    {
        textStacks.Clear();
        for (int i = 0; i < text.Length; i++)
        {
            textStacks.Add(text[i]);
        }
        isClickMode = true;
        TextPrint();
        dialogText.enabled = true;
        dialogArrow.enabled = true;
        playerController.enabled = false;
    }

    // 클릭형 대사
    void TextClick()
    {
        if (isClickMode && Input.GetMouseButtonDown(0))
        {
            textNum++;
            if (textStacks.Count > textNum)
            {
                TextPrint();
            }
            else
            {
                textNum = 0;
                isClickMode = false;
                dialogText.enabled = false;
                dialogArrow.enabled = false;
                playerController.enabled = true;
            }
        }
    }




    // 대사 화살표 반짝거림
    IEnumerator DialogArrow()
    {
        while (true && dialogArrow) 
        { 
            if(dialogArrow.color.a <= 0)
            {
                isDialogArrowOn = false;
            }
            else if(dialogArrow.color.a >= 1)
            {
                isDialogArrowOn = true;
            }

            if (isDialogArrowOn)
            {
                dialogArrow.color = new Color(0f, 0f, 0f, dialogArrow.color.a - ((2.55f / 255f) * 50f * Time.deltaTime));
            }
            else
            {
                dialogArrow.color = new Color(0f, 0f, 0f, dialogArrow.color.a + ((2.55f / 255f) * 50f * Time.deltaTime));
            }
            
            yield return wait;
        }
    }




    // 텍스트 하나씩 출력
    void TextPrint()
    {
        if (textPrintCoroutine != null)
        {
            StopCoroutine(textPrintCoroutine);
        }
        textPrintCoroutine = StartCoroutine(TextPrintCoroutine(textStacks[textNum]));
    }

    IEnumerator TextPrintCoroutine(string text)
    {
        dialogText.text = "";

        for (int i = 0; i < text.Length; i++)
        {
            dialogText.text += text[i];
            yield return wait2;
        }
    }
}
