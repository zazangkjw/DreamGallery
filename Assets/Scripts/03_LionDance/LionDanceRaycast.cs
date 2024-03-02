using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LionDanceRaycast : MonoBehaviour
{
    public RaycastHit hitInfo;
    public GameObject hitObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;
    public GameObject[] doorAndWindow; // ��, â�� (��)������Ʈ �迭 �ڵ� ����
    public GameObject balconyWindow; // ���ڴ� â�� ������Ʈ
    public GameObject balconyDoor; // ���ڴ� �� ������Ʈ
    public GameObject kitchenBalconyWindow1; // �ξ� ���ڴ� â�� ������Ʈ1
    public GameObject kitchenBalconyWindow2; // �ξ� ���ڴ� â�� ������Ʈ2
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
        doorAndWindow = GameObject.FindGameObjectsWithTag("DoorAndWindow");
    }

    void Update()
    {
        ShootRaycast();
    }

    // ī�޶󿡼� ����ĳ��Ʈ ���
    public void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 1.5f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.5f) && !lionDanceSceneManager.isPausing && !putDialogScript.isClickMode)
        {
            hitObject = hitInfo.collider.gameObject;

            // step 0�� ��. ��, â�� �� ����
            if (lionDanceColliderTrigger.step == 0)
            {
                if (hitObject.tag == "DoorAndWindow")
                {
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
                            putDialogScript.putDialog((string)GameManager.instance.textFileManager.dialog[2]["Content"], 3f); // �ξ����� ���ư��ڴ� ��� ���
                            lionDanceSceneManager.FogOut();
                        }
                    }
                }
            }

            // step 3�� ��. ���ڴ� â�� �ݱ�
            else if (lionDanceColliderTrigger.step == 3)
            {
                if (hitObject == balconyWindow)
                {
                    mouseText.text = GameManager.instance.textFileManager.ui[14]; // "����/�ݱ�" �ؽ�Ʈ ����
                    mouseText.enabled = true;

                    // EŰ �Է� ��
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        balconyWindow.GetComponent<GetComponentScript>().animator.SetBool("Active", false);
                        balconyWindow.GetComponent<AudioSource>().Play();
                        mouseText.enabled = false;
                        lionMonsterAnimator.Play("Balcony");
                        lionDanceColliderTrigger.step = 4;

                        Debug.Log("���ڴ� â�� ����");
                    }
                }
            }

            // step 9�� ��. �ξ� ���ڴ� â�� �ݱ�
            else if (lionDanceColliderTrigger.step == 9)
            {
                if (hitObject == kitchenBalconyWindow1 || hitObject == kitchenBalconyWindow2)
                {
                    mouseText.text = GameManager.instance.textFileManager.ui[14]; // "����/�ݱ�" �ؽ�Ʈ ����
                    mouseText.enabled = true;

                    // EŰ �Է� ��
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        hitObject.GetComponent<GetComponentScript>().animator.SetBool("Active", false);
                        hitObject.GetComponent<AudioSource>().Play();
                        lionDanceColliderTrigger.step = 10;
                        mouseText.enabled = false;
                        Debug.Log("�ξ� â�� ����");

                        lionMonsterAnimator.Play("ClimbDown"); // ������ �ξ� â�� �ۿ��� �Ʒ������� �������� �ִϸ��̼� ���
                        Debug.Log("������ �ξ� â�� �ۿ��� �Ʒ������� �������� �ִϸ��̼� ���");

                        StartCoroutine(lionDanceColliderTrigger.SurviveTimerCoroutine(5f));
                    }
                }
            }
        }

        // ����� ��
        else
        {
            mouseText.enabled = false; // �ؽ�Ʈ ������
        }
    }
}
