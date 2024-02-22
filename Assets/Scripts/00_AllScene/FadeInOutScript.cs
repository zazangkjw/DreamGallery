using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutScript : MonoBehaviour
{
    WaitForSeconds wait = new WaitForSeconds(0.005f);

    public void FadeIn(RawImage rawImage)
    {
        StartCoroutine(FadeInCoroutine(rawImage));
    }

    public void FadeOut(RawImage rawImage)
    {
        StartCoroutine(FadeOutCoroutine(rawImage));
    }

    // ���̵� �� �ڷ�ƾ
    IEnumerator FadeInCoroutine(RawImage rawImage)
    {
        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0f);
        while (rawImage.color.a < 1f)
        {
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, rawImage.color.a + ((2.55f / 255f) * 50f * Time.deltaTime));
            yield return wait;
        }
    }

    // ���̵� �ƿ� �ڷ�ƾ
    IEnumerator FadeOutCoroutine(RawImage rawImage)
    {
        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 1f);
        while (rawImage.color.a > 0f)
        {
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, rawImage.color.a - ((2.55f / 255f) * 50f * Time.deltaTime));
            yield return wait;
        }
    }
}
