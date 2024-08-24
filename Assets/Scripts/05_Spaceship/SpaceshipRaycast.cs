using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipRaycast : DefaultRaycast
{
    // AudioSource
    // public AudioSource circusSong;

    public GameObject man001_check; // man001(��¯ �� ���) üũ��
    public GameObject guard001_check; // guard001 üũ��




    private void Start()
    {
        WhenStart();
    }

    private void Update()
    {
        WhenUpdate();
    }




    public override void CheckObject()
    {
        isChecking = true;

        // ������
        if (isChecking)
        {
            foreach (GameObject i in items_check)
            {
                if (hitObject == i)
                {
                    if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    }

                    preObject = hitObject;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                    mouseText.text = GameManager.instance.textFileManager.ui[24]; // "��ȭ�ϱ�" �ؽ�Ʈ ����
                    mouseText.enabled = true;

                    // EŰ �Է� ��
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(PickUpItemCoroutine());
                    }

                    isChecking = false; // ������ �׸���� üũ���� ����

                    break;
                }
                else
                {
                    mouseText.enabled = false; // �ؽ�Ʈ ������

                    if (preObject != null)
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                        preObject = null;
                    }
                }
            }
        }

        // man001
        if (isChecking)
        {
            if (hitObject == man001_check)
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[24]; // "��ȭ�ϱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(man001Coroutine());
                }

                isChecking = false; // ������ �׸���� üũ���� ����
            }
            else
            {
                mouseText.enabled = false; // �ؽ�Ʈ ������

                if (preObject != null)
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    preObject = null;
                }
            }
        }

        // guard
        if (isChecking)
        {
            if (hitObject == guard001_check)
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[24]; // "��ȭ�ϱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(PickUpItemCoroutine());
                }

                isChecking = false; // ������ �׸���� üũ���� ����
            }
            else
            {
                mouseText.enabled = false; // �ؽ�Ʈ ������

                if (preObject != null)
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    preObject = null;
                }
            }
        }
    }




    // man001 ��ȭ
    IEnumerator man001Coroutine()
    {
        // �ʿ��ϸ� "GameObject man001_this = hitObject" �����ؼ� ���

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ
        man001_check.GetComponent<HeadTracking>().isLooking = true;

        // ��ȭ
        if (true)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[19]["Content"], // "�� ����� �ΰ��� �츮 ������ ���� ���� ���ƹ��Ⱦ�"
                                                                   (string)GameManager.instance.textFileManager.dialog[20]["Content"], // "�Ʊ���� ��ӵǴ� ������ ���� ���� �� ����"
                                                                   (string)GameManager.instance.textFileManager.dialog[21]["Content"]}); // "�Ʒ����� ��ü ���� ���� ������ ����?"
        }

        yield return new WaitUntil(() => putDialogScript.isClickMode == false);

        man001_check.GetComponent<HeadTracking>().isLooking = false;
    }

    // guard001 ��ȭ
    IEnumerator guard001Coroutine()
    {
        // �ʿ��ϸ� "GameObject man001_this = hitObject" �����ؼ� ���

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ
        guard001_check.GetComponent<HeadTracking>().isLooking = true;

        if(currentItem.itemName.Equals("Gift"))
        {

        }
        else
        {
            // ��ȭ
            if (true)
            {
                putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[19]["Content"], // "�� ����� �ΰ��� �츮 ������ ���� ���� ���ƹ��Ⱦ�"
                                                                       (string)GameManager.instance.textFileManager.dialog[20]["Content"], // "�Ʊ���� ��ӵǴ� ������ ���� ���� �� ����"
                                                                       (string)GameManager.instance.textFileManager.dialog[21]["Content"]}); // "�Ʒ����� ��ü ���� ���� ������ ����?"
            }

            yield return new WaitUntil(() => putDialogScript.isClickMode == false);

            guard001_check.GetComponent<HeadTracking>().isLooking = false;
        }
    }
}
