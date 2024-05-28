using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusFlash : MonoBehaviour
{
    [SerializeField]
    GameObject[] flashes;

    bool[] flashesNum;

    public float flashDelay; // 셔터 딜레이

    public bool isFlashOn; // 셔터 온/오프




    void Start()
    {
        flashes = new GameObject[transform.childCount];
        flashesNum = new bool[flashes.Length];
        flashDelay = 0f;
        isFlashOn = false;

        // 자식 오브젝트(셔터)들 받아오기
        for (int i = 0; i < transform.childCount; i++)
        {
            flashes[i] = transform.GetChild(i).gameObject;
            flashes[i].SetActive(false);
        }

        StartCoroutine(RandomCoroutine());
    }




    // 랜덤으로 셔터 고르기
    IEnumerator RandomCoroutine()
    {
        while (true)
        {
            if (isFlashOn)
            {
                int rd = Random.Range(0, flashes.Length);

                if (!flashesNum[rd])
                {
                    StartCoroutine(FlashCoroutine(rd));
                    yield return new WaitForSeconds(flashDelay);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }




    // 셔터 효과
    IEnumerator FlashCoroutine(int rd)
    {
        flashesNum[rd] = true;

        flashes[rd].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        flashes[rd].SetActive(false);
        yield return new WaitForSeconds(0.1f);
        flashes[rd].SetActive(true);
        yield return new WaitForSeconds(0.01f);
        flashes[rd].SetActive(false);

        flashesNum[rd] = false;
    }
}
