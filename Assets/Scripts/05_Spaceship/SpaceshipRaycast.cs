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
    public GameObject guard001_check; bool isInteractable_guard001 = true; // guard001 üũ��
    public GameObject guard_check; // guard üũ��
    public DoorSensor doorSensor;




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

                    mouseText.text = GameManager.instance.textFileManager.ui[26]; // "�ݱ�" �ؽ�Ʈ ����
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
                    StartCoroutine(Man001Coroutine());
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

        // Guard001
        if (isChecking)
        {
            if (hitObject == guard001_check && isInteractable_guard001)
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                if (currentItem.itemName.Equals("Gift"))
                {
                    mouseText.text = GameManager.instance.textFileManager.ui[27]; // "�����ϱ�" �ؽ�Ʈ ����
                }
                else
                {
                    mouseText.text = GameManager.instance.textFileManager.ui[24]; // "��ȭ�ϱ�" �ؽ�Ʈ ����
                }
                mouseText.enabled = true;

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(Guard001Coroutine());
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




    // Man001 ��ȭ
    IEnumerator Man001Coroutine()
    {
        // �ʿ��ϸ� "GameObject man001_this = hitObject" �����ؼ� ���

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ
        man001_check.GetComponent<HeadTracking>().isLooking = true;

        // ��ȭ
        putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[19]["Content"], // "�� ����� �ΰ��� �츮 ������ ���� ���� ���ƹ��Ⱦ�"
                                                               (string)GameManager.instance.textFileManager.dialog[20]["Content"], // "�Ʊ���� ��ӵǴ� ������ ���� ���� �� ����"
                                                               (string)GameManager.instance.textFileManager.dialog[21]["Content"]}); // "�Ʒ����� ��ü ���� ���� ������ ����?"

        yield return new WaitUntil(() => putDialogScript.isClickMode == false);

        man001_check.GetComponent<HeadTracking>().isLooking = false;
    }

    // guard001 ��ȭ
    IEnumerator Guard001Coroutine()
    {
        // �ʿ��ϸ� "GameObject man001_this = hitObject" �����ؼ� ���

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ
        guard001_check.GetComponent<HeadTracking>().isLooking = true;

        if (currentItem.itemName.Equals("Gift"))
        {
            // ��� ���� ����
            isInteractable_guard001 = false;
            guard001_check.GetComponent<GetComponentScript>().animator.Play("TakeGift");

            // ���� �ֱ�
            Item gift = currentItem;
            items_check.Remove(gift.gameObject);
            gift.enabled = false;
            gift.transform.SetParent(itemCategory.transform);
            gift.GetComponent<Collider>().enabled = true;
            gift.GetComponent<GetComponentScript>().outline.gameObject.layer = LayerMask.NameToLayer("Default");
            gift.transform.position = new Vector3(19.104f, 9.427f, -83.348f);
            gift.transform.rotation = Quaternion.identity;
            currentItem = empty;
            inventory[currentSlot - 1] = empty;

            // ���
            guard_check.GetComponent<GetComponentScript>().animator.SetBool("Able", false);
            guard_check.GetComponent<HeadTracking>().isLooking = true;
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[24]["Content"], 3f); // "��! ������ �ּż� ���� �����մϴ�"
            yield return dialogDelay;
            GameObject playerCam = guard001_check.GetComponent<HeadTracking>().target;
            guard001_check.GetComponent<HeadTracking>().target = gift.gameObject;
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[25]["Content"], 3f); // "���� �ȿ� ���� ��� �ִ��� ���� �;��!"
            yield return dialogDelay;

            gift.GetComponent<GetComponentScript>().animator.Play("Surprise"); 
            gift.GetComponent<Rigidbody>().useGravity = true;
            gift.GetComponent<Rigidbody>().isKinematic = false;
            gift.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.VelocityChange);

            yield return dialogDelay;

            guard001_check.GetComponent<HeadTracking>().lookSpeed = 0.025f;
            guard001_check.GetComponent<HeadTracking>().target = playerCam;

            yield return dialogDelay;
            guard_check.GetComponent<HeadTracking>().isLooking = false;
            guard001_check.GetComponent<HeadTracking>().isLooking = false;
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[26]["Content"], 3f); // "���ƾ�!"

            // ��� �Ѿ���
            guard001_check.GetComponent<Collider>().enabled = false;
            guard001_check.GetComponent<GetComponentScript>().animator.Play("FallDown");
            guard_check.GetComponent<GetComponentScript>().animator.Play("FallDown");

            yield return dialogDelay;

            doorSensor.isLock = false;
        }
        else
        {
            // ��ȭ
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[22]["Content"], // "�� �� �ʸӴ� ���� ���� �����Դϴ�"
                                                                   (string)GameManager.instance.textFileManager.dialog[23]["Content"]}); // "���ư��ּ���"

            yield return new WaitUntil(() => putDialogScript.isClickMode == false);

            guard001_check.GetComponent<HeadTracking>().isLooking = false;
        }
    }
}
