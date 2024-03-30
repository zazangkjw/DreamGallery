using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letterbox : MonoBehaviour
{
    public RawImage letterbox_Up;
    public RawImage letterbox_Down;

    WaitForSeconds wait = new WaitForSeconds(0.016f);




    // ���͹ڽ� ��� Ȱ��ȭ
    public void LetterboxOnImmediately()
    {
        letterbox_Up.rectTransform.anchoredPosition = new Vector2(0, 0);
        letterbox_Down.rectTransform.anchoredPosition = new Vector2(0, 0);
    }

    // ���͹ڽ� õõ��(2��) Ȱ��ȭ
    public void LetterboxOn()
    {
        StartCoroutine(LetterboxOnCoroutine());
    }

    IEnumerator LetterboxOnCoroutine()
    {
        for (int i = 0; i < 60; i++)
        {

            if (letterbox_Up.rectTransform.anchoredPosition.y > 0)
            {
                letterbox_Up.rectTransform.anchoredPosition = new Vector2(0, letterbox_Up.rectTransform.anchoredPosition.y - 2);
                letterbox_Down.rectTransform.anchoredPosition = new Vector2(0, letterbox_Down.rectTransform.anchoredPosition.y + 2);
                yield return wait;
            }
        }

        letterbox_Up.rectTransform.anchoredPosition = new Vector2(0, 0);
        letterbox_Down.rectTransform.anchoredPosition = new Vector2(0, 0);
    }




    // ���͹ڽ� ��� Ȱ��ȭ
    public void LetterboxOffImmediately()
    {
        letterbox_Up.rectTransform.anchoredPosition = new Vector2(0, letterbox_Up.rectTransform.sizeDelta.y);
        letterbox_Down.rectTransform.anchoredPosition = new Vector2(0, -letterbox_Down.rectTransform.sizeDelta.y);
    }

    // ���͹ڽ� õõ��(2��) ��Ȱ��ȭ
    public void LetterboxOff()
    {
        StartCoroutine(LetterboxOffCoroutine());
    }

    IEnumerator LetterboxOffCoroutine()
    {
        for (int i = 0; i < 60; i++)
        {
            if (letterbox_Up.rectTransform.anchoredPosition.y < letterbox_Up.rectTransform.sizeDelta.y)
            {
                letterbox_Up.rectTransform.anchoredPosition = new Vector2(0, letterbox_Up.rectTransform.anchoredPosition.y + 2);
                letterbox_Down.rectTransform.anchoredPosition = new Vector2(0, letterbox_Down.rectTransform.anchoredPosition.y - 2);
                yield return wait;
            }
        }

        letterbox_Up.rectTransform.anchoredPosition = new Vector2(0, letterbox_Up.rectTransform.sizeDelta.y);
        letterbox_Down.rectTransform.anchoredPosition = new Vector2(0, -letterbox_Down.rectTransform.sizeDelta.y);
    }
}
