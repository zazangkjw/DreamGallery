using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractiveObject_PitchingMachineButton : InteractiveObject
{
    public TextMeshProUGUI timerText;
    public List<Target> targets = new List<Target>();
    public List<Bullet> balls = new List<Bullet>();
    public GameObject pointStart; // �� �����
    public GameObject pointEnd; // �� ������
    IEnumerator currentShootingCoroutine;

    public MeshRenderer pitchingMachineMat;
    public Material idleMat, shootMat;

    float timer;
    public bool timerRunning;
    string displayText;
    WaitForSeconds startDelay = new WaitForSeconds(1f);
    WaitForSeconds displayDelay = new WaitForSeconds(0.5f);
    IEnumerator currentDisplayCoroutine;

    private void Start()
    {
        displayText = "HELLO_";
        StartCoroutine(currentDisplayCoroutine = DisplaySlideCoroutine());
    }

    private void FixedUpdate()
    {
        TimerCounter();
    }

    public override IEnumerator ActionCoroutine()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ

        StopCoroutine(currentDisplayCoroutine);

        // ���� �غ�
        timerText.text = "READY";
        yield return startDelay;
        // ��
        yield return startDelay;
        timerText.text = "START";
        // ��
        yield return startDelay;
        // ��
        timer = 30f;
        timerRunning = true;
        StartCoroutine(currentShootingCoroutine = ShootingCoroutine());

        // 1�ܰ�
        for (int i = 0; i < 3; i++)
        {
            targets[i].ResetTarget();
        }
        while (true)
        {
            yield return displayDelay;
            if (!targets[0].original.activeSelf && !targets[1].original.activeSelf && !targets[2].original.activeSelf)
            {
                break;
            }
        }

        // 2�ܰ�
        for (int i = 3; i < 8; i++)
        {
            targets[i].ResetTarget();
        }
        while (true)
        {
            yield return displayDelay;
            if (!targets[3].original.activeSelf && !targets[4].original.activeSelf && !targets[5].original.activeSelf && !targets[6].original.activeSelf && !targets[7].original.activeSelf)
            {
                break;
            }
        }

        // 3�ܰ�
        for (int i = 8; i < 10; i++)
        {
            targets[i].ResetTarget();
        }
        while (true)
        {
            yield return displayDelay;
            if (!targets[8].original.activeSelf && !targets[9].original.activeSelf)
            {
                break;
            }
        }

        // ����
        timerRunning = false;
        displayText = "CLEAR";
        StartCoroutine(currentDisplayCoroutine = DisplayBlinkCoroutine());
        GetComponent<Collider>().enabled = true;
        StopCoroutine(currentShootingCoroutine);
        pitchingMachineMat.material = idleMat;

        yield return null;
    }

    // �� �߻�
    IEnumerator ShootingCoroutine()
    {
        int num = 0;

        yield return startDelay;

        while (true)
        { 
            yield return startDelay;
            pitchingMachineMat.material = shootMat;
            balls[num].transform.position = pointStart.transform.position;
            balls[num].transform.rotation = Quaternion.LookRotation(pointEnd.transform.position - pointStart.transform.position);
            balls[num].team = 2;
            balls[num].gameObject.SetActive(true);
            num++;
            num %= balls.Count;
            yield return startDelay;
            pitchingMachineMat.material = idleMat;
        }
    }

    // Ÿ�̸�
    void TimerCounter()
    {
        if (timerRunning)
        {
            timer -= Time.fixedDeltaTime;
            timerText.text = ((int)timer).ToString();

            // 0�� �Ǹ� ����
            if (timer <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    targets[i].original.SetActive(false);
                }
                timerRunning = false;
                timerText.text = "FAIL";
                StopCoroutine(currentCoroutine);
                StopCoroutine(currentShootingCoroutine);
                GetComponent<Collider>().enabled = true;
                pitchingMachineMat.materials[0] = idleMat;
            }
        }
    }

    // �ؽ�Ʈ �����̵� ȿ��
    IEnumerator DisplaySlideCoroutine()
    {
        int n = 0;

        while (true)
        {
            yield return displayDelay;

            timerText.text = "";
            for (int i = n; i < (n + 5); i++)
            {
                timerText.text += displayText[i % displayText.Length];
            }
            n++;
            n %= displayText.Length ;
        }
    }

    // �ؽ�Ʈ ������ ȿ��
    IEnumerator DisplayBlinkCoroutine()
    {
        while (true)
        {
            timerText.text = displayText;
            yield return displayDelay;
            timerText.text = "";
            yield return displayDelay;
        }
    }
}
