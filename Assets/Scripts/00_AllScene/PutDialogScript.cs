using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PutDialogScript : MonoBehaviour
{
    // ��� ������� Ÿ�̸�, ��� �ֱ� 
    public TextMeshProUGUI dialogText;
    public List<string> textStacks; // ������� �ؽ�Ʈ��
    public Image dialogArrow;
    float timer; // ��� ������� Ÿ�̸�
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




    // ��� �ֱ�(Ÿ�̸���)
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

    // Ÿ�̸��� ���
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




    // ��� �ֱ�(Ŭ����)
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

    // Ŭ���� ���
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




    // ��� ȭ��ǥ ��¦�Ÿ�
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




    // �ؽ�Ʈ �ϳ��� ���
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
