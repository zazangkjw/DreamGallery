using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutScript : MonoBehaviour
{
    // ���̵� ��
    public void FadeIn(RawImage rawImage)
    {
        StartCoroutine(FadeInCoroutine(rawImage, 2));
    }
    // ���̵� ��(�ð� �Է�)
    public void FadeIn(RawImage rawImage, float time)
    {
        StartCoroutine(FadeInCoroutine(rawImage, time));
    }
    IEnumerator FadeInCoroutine(RawImage rawImage, float time)
    {
        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 1f);
        while (rawImage.color.a > 0f)
        {
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, rawImage.color.a - ((1f / time) * Time.deltaTime));
            yield return null;
        }
    }




    // ���̵� �ƿ�
    public void FadeOut(RawImage rawImage)
    {
        StartCoroutine(FadeOutCoroutine(rawImage, 2));
    }

    // ���̵� �ƿ�(�ð� �Է�)
    public void FadeOut(RawImage rawImage, float time)
    {
        StartCoroutine(FadeOutCoroutine(rawImage, time));
    }
    IEnumerator FadeOutCoroutine(RawImage rawImage, float time)
    {
        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0f);
        while (rawImage.color.a < 1f)
        {
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, rawImage.color.a + ((1f / time) * Time.deltaTime));
            yield return null;
        }
    }
}
