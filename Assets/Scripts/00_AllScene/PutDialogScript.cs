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




    // ��� �ֱ�(Ÿ�̸���)
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

    // ��� �ֱ�(Ÿ�̸���, �ؽ�Ʈ �ϳ��� ���)
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
        for (int i = 0; i < text.Length; i++)
        {
            textStacks.Add(text[i]);
        }

        // Ŭ���� ��簡 ��� ������ ���� ��
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

    // ��� �ֱ�(Ŭ����, �ؽ�Ʈ �ϳ��� ���)
    public void putDialogPrintWithClick(string[] text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            textStacks.Add(text[i]);
        }

        // Ŭ���� ��簡 ��� ������ ���� ��
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

    // Ŭ���� ���
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




    // ��� ȭ��ǥ ��¦�Ÿ�
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




    // �ؽ�Ʈ �ϳ��� ���
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
