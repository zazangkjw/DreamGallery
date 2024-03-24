using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBar : MonoBehaviour
{
    public RawImage blackBar_Up;
    public RawImage blackBar_Down;




    // 블랙바 즉시 활성화
    public void BlackBarOnImmediately()
    {
        blackBar_Up.rectTransform.anchoredPosition = new Vector2(0, 0);
        blackBar_Down.rectTransform.anchoredPosition = new Vector2(0, 0);
    }

    // 블랙바 천천히(2초) 활성화
    public void BlackBarOn()
    {
        StartCoroutine(BlackBarOnCoroutine());
    }

    IEnumerator BlackBarOnCoroutine()
    {
        for (int i = 0; i < 60; i++)
        {

            if (blackBar_Up.rectTransform.anchoredPosition.y > 0)
            {
                blackBar_Up.rectTransform.anchoredPosition = new Vector2(0, blackBar_Up.rectTransform.anchoredPosition.y - 2);
                blackBar_Down.rectTransform.anchoredPosition = new Vector2(0, blackBar_Down.rectTransform.anchoredPosition.y + 2);
                yield return new WaitForSeconds(0.016f);
            }
        }

        blackBar_Up.rectTransform.anchoredPosition = new Vector2(0, 0);
        blackBar_Down.rectTransform.anchoredPosition = new Vector2(0, 0);
    }




    // 블랙바 즉시 활성화
    public void BlackBarOffImmediately()
    {
        blackBar_Up.rectTransform.anchoredPosition = new Vector2(0, blackBar_Up.rectTransform.sizeDelta.y);
        blackBar_Down.rectTransform.anchoredPosition = new Vector2(0, -blackBar_Down.rectTransform.sizeDelta.y);
    }

    // 블랙바 천천히(2초) 비활성화
    public void BlackBarOff()
    {
        StartCoroutine(BlackBarOffCoroutine());
    }

    IEnumerator BlackBarOffCoroutine()
    {
        for (int i = 0; i < 60; i++)
        {
            if (blackBar_Up.rectTransform.anchoredPosition.y < blackBar_Up.rectTransform.sizeDelta.y)
            {
                blackBar_Up.rectTransform.anchoredPosition = new Vector2(0, blackBar_Up.rectTransform.anchoredPosition.y + 2);
                blackBar_Down.rectTransform.anchoredPosition = new Vector2(0, blackBar_Down.rectTransform.anchoredPosition.y - 2);
                yield return new WaitForSeconds(0.016f);
            }
        }

        blackBar_Up.rectTransform.anchoredPosition = new Vector2(0, blackBar_Up.rectTransform.sizeDelta.y);
        blackBar_Down.rectTransform.anchoredPosition = new Vector2(0, -blackBar_Down.rectTransform.sizeDelta.y);
    }
}
