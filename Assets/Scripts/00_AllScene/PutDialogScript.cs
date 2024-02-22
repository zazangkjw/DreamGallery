using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PutDialogScript : MonoBehaviour
{
    // ��� ������� Ÿ�̸�, ��� �ֱ� 
    public Text dialogText;
    public List<string> textStacks; // ������� �ؽ�Ʈ��
    public Image dialogArrow;
    float timer; // ��� ������� Ÿ�̸�
    public PlayerController playerController;
    public bool isClickMode;
    int textNum;
    WaitForSeconds wait = new WaitForSeconds(0.005f);
    bool isDialogArrowOn;

    void Start()
    {
        StartCoroutine(DialogArrow());
    }

    void Update()
    {
        TextTimer();
        TextClick();
    }

    // ��� �ֱ�
    public void putDialog(string text, float time)
    {
        if (!isClickMode)
        {
            dialogText.text = text;
            dialogText.enabled = true;
            timer = time;
        }
    }

    public void putDialogWithClick(string[] text)
    {
        textStacks.Clear();
        for (int i = 0; i < text.Length; i++)
        {
            textStacks.Add(text[i]);
        }
        isClickMode = true;
        dialogText.text = textStacks[0];
        dialogText.enabled = true;
        dialogArrow.enabled = true;
        playerController.enabled = false;
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

    // Ŭ���� ���
    void TextClick()
    {
        if (isClickMode && Input.GetMouseButtonDown(0))
        {
            textNum++;
            if (textStacks.Count > textNum)
            {
                dialogText.text = textStacks[textNum];
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
}
