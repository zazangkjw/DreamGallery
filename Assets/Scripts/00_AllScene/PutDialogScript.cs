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

    bool isPrintMode;
    public bool isClickMode;
    int textNum;
    WaitForSeconds wait = new WaitForSeconds(0.005f);
    WaitForSeconds wait2 = new WaitForSeconds(0.025f);
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
        textStacks.Add(text);
        if (!isClickMode)
        {
            dialogText.text = textStacks[textNum];
            dialogText.enabled = true;
            timer = time;
        }
        textStacks.Clear();
    }

    // 대사 넣기(타이머형, 텍스트 하나씩 출력)
    public void putDialogPrint(string text, float time)
    {
        textStacks.Add(text);
        if (!isClickMode)
        {
            timer = 100f;
            TextPrint(time);
            dialogText.enabled = true;
        }
        textStacks.Clear();
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
        for (int i = 0; i < text.Length; i++)
        {
            textStacks.Add(text[i]);
        }

        // 클릭형 대사가 출력 중이지 않을 때
        if (!isClickMode)
        {
            isPrintMode = false;
            isClickMode = true;
            dialogText.text = textStacks[textNum];
            dialogText.enabled = true;
            dialogArrow.enabled = true;
            if (!playerController.myRigid.isKinematic) { playerController.myRigid.velocity = Vector3.zero; }
            playerController.enabled = false;
            playerController.myRigid.isKinematic = true;
        }
    }

    // 대사 넣기(클릭형, 텍스트 하나씩 출력)
    public void putDialogPrintWithClick(string[] text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            textStacks.Add(text[i]);
        }

        // 클릭형 대사가 출력 중이지 않을 때
        if (!isClickMode)
        {
            isPrintMode = true;
            isClickMode = true;
            TextPrint();
            dialogText.enabled = true;
            dialogArrow.enabled = true;
            if (!playerController.myRigid.isKinematic) { playerController.myRigid.velocity = Vector3.zero; }
            playerController.enabled = false;
            playerController.myRigid.isKinematic = true;
        }
    }

    // 클릭형 대사
    void TextClick()
    {
        if (isClickMode && Input.GetMouseButtonDown(0))
        {
            textNum++;
            if (textStacks.Count > textNum)
            {
                if (isPrintMode)
                {
                    TextPrint();
                }
                else
                {
                    dialogText.text = textStacks[textNum];
                }
            }
            else
            {
                textStacks.Clear();
                textNum = 0;
                isClickMode = false;
                dialogText.enabled = false;
                dialogArrow.enabled = false;
                playerController.enabled = true;
                playerController.myRigid.isKinematic = false;
            }
        }
    }




    // 대사 화살표 반짝거림
    IEnumerator DialogArrow()
    {
        while (true)
        {
            if (isClickMode)
            {
                if (dialogArrow.color.a <= 0)
                {
                    isDialogArrowOn = false;
                }
                else if (dialogArrow.color.a >= 1)
                {
                    isDialogArrowOn = true;
                }

                if (isDialogArrowOn)
                {
                    dialogArrow.color = new Color(1f, 1f, 1f, dialogArrow.color.a - ((2.55f / 255f) * 100f * Time.deltaTime));
                }
                else
                {
                    dialogArrow.color = new Color(1f, 1f, 1f, dialogArrow.color.a + ((2.55f / 255f) * 100f * Time.deltaTime));
                }  
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

    void TextPrint(float time)
    {
        if (textPrintCoroutine != null)
        {
            StopCoroutine(textPrintCoroutine);
        }
        textPrintCoroutine = StartCoroutine(TextPrintCoroutine(textStacks[textNum], time));
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

    IEnumerator TextPrintCoroutine(string text, float time)
    {
        dialogText.text = "";

        for (int i = 0; i < text.Length; i++)
        {
            dialogText.text += text[i];
            yield return wait2;
        }

        timer = time;
    }
}
