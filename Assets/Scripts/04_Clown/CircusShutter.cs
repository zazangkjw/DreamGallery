using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusShutter : MonoBehaviour
{
    [SerializeField]
    GameObject[] shutters;

    bool[] shuttersNum;

    public float shutterDelay = 1f; // 셔터 딜레이

    public bool isShutterOn; // 셔터 온/오프




    void Start()
    {
        shutters = new GameObject[transform.childCount];
        shuttersNum = new bool[shutters.Length];
        isShutterOn = false;

        // 자식 오브젝트(셔터)들 받아오기
        for (int i = 0; i < transform.childCount; i++)
        {
            shutters[i] = transform.GetChild(i).gameObject;
            shutters[i].SetActive(false);
        }

        StartCoroutine(RandomCoroutine());
    }




    // 랜덤으로 셔터 고르기
    IEnumerator RandomCoroutine()
    {
        while (true)
        {
            if (isShutterOn)
            {
                int rd = Random.Range(0, shutters.Length);

                if (!shuttersNum[rd])
                {
                    StartCoroutine(ShutterCoroutine(rd));
                    yield return new WaitForSeconds(shutterDelay);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }




    // 셔터 효과
    IEnumerator ShutterCoroutine(int rd)
    {
        shuttersNum[rd] = true;

        shutters[rd].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        shutters[rd].SetActive(false);
        yield return new WaitForSeconds(0.1f);
        shutters[rd].SetActive(true);
        yield return new WaitForSeconds(0.01f);
        shutters[rd].SetActive(false);

        shuttersNum[rd] = false;
    }
}
