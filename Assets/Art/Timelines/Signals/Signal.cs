using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Signal : MonoBehaviour
{
    public PlayableDirector director;
    public float skipPoint;
    public PutDialogScript putDialogScript;
    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트
    public GameObject skipText;

    public void Skip()
    {
        StartCoroutine(SkipCoroutine());
    }

    IEnumerator SkipCoroutine()
    {
        skipText.SetActive(true);
        float timer = 0;

        while (true)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                timer += Time.deltaTime;

                if (timer >= 2)
                {
                    director.Pause();
                    skipText.SetActive(false);
                    director.time = skipPoint;
                    director.Play();
                    break;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                timer = 0;
            }

            if(director.time > skipPoint)
            {
                skipText.SetActive(false);
                break;
            }

            yield return null;
        }
    }

    public void SSAO_ON()
    {
        GameManager.instance.urpRenderer.rendererFeatures[0].SetActive(true);
    }

    public void SSAO_OFF()
    {
        GameManager.instance.urpRenderer.rendererFeatures[0].SetActive(false);
    }

    public void Black()
    {
        fadeInOutImage.color = new Color(0f, 0f, 0f, 1f);
    }

    public void FadeIn()
    {
        fadeInOutScript.FadeIn(fadeInOutImage);
    }

    public void FadeOut()
    {
        fadeInOutScript.FadeOut(fadeInOutImage);
    }

    public void EmptyDialog()
    {
        putDialogScript.putDialogPrint("", 0f);
    }

    public void Dialog(int num)
    {
        putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[num]["Content"], 3f);
    }

    public void DialogClick(string str)
    {
        string[] nums = str.Split(' ');
        for(int i = 0; i < nums.Length; i++)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[int.Parse(nums[i])]["Content"]});
        }
        
    }
}
