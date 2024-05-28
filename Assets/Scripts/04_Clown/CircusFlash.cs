using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusFlash : MonoBehaviour
{
    [SerializeField]
    GameObject[] flashes;

    bool[] flashesNum;

    public float flashDelay; // ���� ������

    public bool isFlashOn; // ���� ��/����




    void Start()
    {
        flashes = new GameObject[transform.childCount];
        flashesNum = new bool[flashes.Length];
        flashDelay = 0f;
        isFlashOn = false;

        // �ڽ� ������Ʈ(����)�� �޾ƿ���
        for (int i = 0; i < transform.childCount; i++)
        {
            flashes[i] = transform.GetChild(i).gameObject;
            flashes[i].SetActive(false);
        }

        StartCoroutine(RandomCoroutine());
    }




    // �������� ���� ����
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




    // ���� ȿ��
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
