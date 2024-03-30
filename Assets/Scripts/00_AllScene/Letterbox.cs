using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letterbox : MonoBehaviour
{
    public RawImage letterbox_Up;
    public RawImage letterbox_Down;

    WaitForSeconds wait = new WaitForSeconds(0.016f);




    // 레터박스 즉시 활성화
    public void LetterboxOnImmediately()
    {
        letterbox_Up.rectTransform.anchoredPosition = new Vector2(0, 0);
        letterbox_Down.rectTransform.anchoredPosition = new Vector2(0, 0);
    }

    // 레터박스 천천히(2초) 활성화
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




    // 레터박스 즉시 활성화
    public void LetterboxOffImmediately()
    {
        letterbox_Up.rectTransform.anchoredPosition = new Vector2(0, letterbox_Up.rectTransform.sizeDelta.y);
        letterbox_Down.rectTransform.anchoredPosition = new Vector2(0, -letterbox_Down.rectTransform.sizeDelta.y);
    }

    // 레터박스 천천히(2초) 비활성화
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
