using TMPro;
using UnityEngine;

public class LionDanceRaycast : MonoBehaviour
{
    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;

    public GameObject[] doorAndWindow; // ��, â�� (��)������Ʈ �迭 �ڵ� ����
    public GameObject balconyWindow; // ���ڴ� â�� ������Ʈ
    public GameObject balconyDoor; // ���ڴ� �� ������Ʈ
    public GameObject kitchenBalconyWindow1; // �ξ� ���ڴ� â�� ������Ʈ1
    public GameObject kitchenBalconyWindow2; // �ξ� ���ڴ� â�� ������Ʈ2
    public GameObject myRoomDoor;
    public GameObject sisRoomDoor;
    int openCount;

    public Animator lionMonsterAnimator;
    public LionDanceSceneManager lionDanceSceneManager;
    public LionDanceColliderTrigger lionDanceColliderTrigger;
    public PlayerController playerController;
    public Camera playerCam;
    public GameObject player;
    public Material fogMat;

    void Start()
    {
        openCount = 0;
    }

    void Update()
    {
        ShootRaycast();
    }

    // ī�޶󿡼� ����ĳ��Ʈ ���
    public void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f) && !lionDanceSceneManager.isPausing && !putDialogScript.isClickMode)
        {
            hitObject = hitInfo.collider.gameObject;
        }
        // ����� ��
        else
        {
            hitObject = null;
        }

        CheckObject();
    }

    public void CheckObject()
    {
        // step 0�� ��. ��, â�� �� ����
        if (lionDanceColliderTrigger.step == 0)
        {
            foreach(GameObject doorWindow in doorAndWindow)
            {
                if (hitObject == doorWindow)
                {
                    if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    }

                    preObject = hitObject;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                    mouseText.text = GameManager.instance.textFileManager.ui[14]; // "����/�ݱ�" �ؽ�Ʈ ����
                    mouseText.enabled = true;

                    // EŰ �Է� ��
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // ��, â�� ����
                        if (!hitObject.GetComponent<GetComponentScript>().animator.GetBool("Active")) // ���������� ����
                        {
                            hitObject.GetComponent<GetComponentScript>().animator.SetBool("Active", true);
                            openCount++;
                        }
                        else // ���������� �ݱ�
                        {
                            hitObject.GetComponent<GetComponentScript>().animator.SetBool("Active", false);
                            openCount--;
                        }
                        hitObject.GetComponent<AudioSource>().Play();

                        // ��, â�� �� ���ȴ��� üũ�ϰ� ���� ��������
                        if (openCount == 10)
                        {
                            lionMonsterAnimator.Play("Walking", 1);
                            lionDanceColliderTrigger.step = 1;
                            mouseText.enabled = false;
                            preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                            preObject = null;
                            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[2]["Content"], 3f); // �ξ����� ���ư��ڴ� ��� ���
                            lionDanceSceneManager.FogOut();
                        }
                    }
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

        // step 3�� ��. ���ڴ� â�� �ݱ�
        else if (lionDanceColliderTrigger.step == 3)
        {
            if (hitObject == balconyWindow)
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[14]; // "����/�ݱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    balconyWindow.GetComponent<GetComponentScript>().animator.SetBool("Active", false);
                    balconyWindow.GetComponent<AudioSource>().Play();
                    mouseText.enabled = false;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    preObject = null;
                    lionMonsterAnimator.Play("Balcony");
                    lionDanceColliderTrigger.step = 4;

                    Debug.Log("���ڴ� â�� ����");
                }
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

        // step 5�� ��. �� �� �� �ݱ�
        else if (lionDanceColliderTrigger.step == 5)
        {
            if (hitObject == myRoomDoor)
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[14]; // "����/�ݱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    lionMonsterAnimator.Play("MyRoom"); // ������ �� �� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���
                    lionDanceColliderTrigger.step = 7;
                    mouseText.enabled = false;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    preObject = null;
                    Debug.Log("������ �� �� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���");

                    myRoomDoor.GetComponent<GetComponentScript>().animator.SetBool("Active", false); // �� �� �� ������ �ִϸ��̼� ���
                    myRoomDoor.GetComponent<AudioSource>().Play();
                    StartCoroutine(lionDanceColliderTrigger.SisRoomTimerCoroutine()); // 5.5�� �ڿ� ���� ������ ���� ħ��
                    lionDanceColliderTrigger.dogAnimator.Play("LookSisRoom");
                    Debug.Log("�� �� �� ������ �ִϸ��̼� ���");
                }
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

        // step 7�� ��. ���� �� �� �ݱ�
        else if (lionDanceColliderTrigger.step == 7)
        {
            if (hitObject == sisRoomDoor)
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[14]; // "����/�ݱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    lionMonsterAnimator.Play("SisRoom"); // ������ ���� �� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���
                    lionDanceColliderTrigger.step = 9;
                    mouseText.enabled = false;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    preObject = null;
                    Debug.Log("������ ���� �� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���");

                    sisRoomDoor.GetComponent<GetComponentScript>().animator.SetBool("Active", false); // ���� �� �� ������ �ִϸ��̼� ���
                    sisRoomDoor.GetComponent<AudioSource>().Play();
                    StartCoroutine(lionDanceColliderTrigger.KitchenTimerCoroutine()); // 6�� �ڿ� �ξ����� ���� ħ��
                    lionDanceColliderTrigger.dogAnimator.Play("Kitchen");
                    Debug.Log("���� �� �� ������ �ִϸ��̼� ���. �ξ� â������ ������ ������ ī��Ʈ�ٿ� ����");
                }
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

        // step 9�� ��. �ξ� ���ڴ� â�� �ݱ�
        else if (lionDanceColliderTrigger.step == 9)
        {
            if (hitObject == kitchenBalconyWindow1 || hitObject == kitchenBalconyWindow2)
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[14]; // "����/�ݱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hitObject.GetComponent<GetComponentScript>().animator.SetBool("Active", false);
                    hitObject.GetComponent<AudioSource>().Play();
                    lionDanceColliderTrigger.step = 10;
                    mouseText.enabled = false;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    preObject = null;

                    Debug.Log("�ξ� â�� ����");

                    lionMonsterAnimator.Play("ClimbDown"); // ������ �ξ� â�� �ۿ��� �Ʒ������� �������� �ִϸ��̼� ���
                    Debug.Log("������ �ξ� â�� �ۿ��� �Ʒ������� �������� �ִϸ��̼� ���");

                    StartCoroutine(lionDanceColliderTrigger.SurviveTimerCoroutine(5f));
                }
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
}
