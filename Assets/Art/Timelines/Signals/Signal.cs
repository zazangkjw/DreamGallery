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
                    skipText.SetActive(false);
                    director.time = skipPoint;
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

    public void FadeInOutImageBlack()
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
}
