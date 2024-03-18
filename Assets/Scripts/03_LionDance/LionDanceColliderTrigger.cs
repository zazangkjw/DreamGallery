using System.Collections;
using TMPro;
using UnityEngine;

public class LionDanceColliderTrigger : MonoBehaviour
{
    public GameObject balconyDoor;
    public GameObject myRoomDoor;
    public GameObject sisRoomDoor;
    public GameObject balconyWindow;
    public GameObject kitchenWindow;
    public Animator lionMonsterAnimator;
    public Collider lionMonsterCol;
    public LionDanceDirector lionDanceDirector;
    public LionDanceRaycast lionDanceRaycast;
    public AudioSource monsterLaughing;
    public Animator dogAnimator;
    public DogAnimationScript dogAnimationScript;
    public Collider[] ColliderTriggers; // Ʈ���� �ݶ��̴���
    public TextMeshProUGUI mouseText;

    enum Triggers
    {
        kitchen,            // 0
        livingRoom,         // 1
        balcony,            // 2
        frontOfBathroom,    // 3
        myRoom,             // 4
        sisRoom             // 5
    }

    public int step; // ���� ���� �ܰ�����
                     // 0: â�� �� ����
                     // 1: �ξ� ����
                     // 2: �Ž� ����
                     // 3: ���ڴ� â�� �ݱ�
                     // 4: ���ڴ� �� ����
                     // 5: �� �� ����
                     // 6: �� �� �� ����
                     // 7: ���� �� ����
                     // 8: ���� �� �� ���� + �ξ� ����
                     // 9: �ξ� â�� �ݱ�
                     // 10: ���� ����
                     // 11: ���� ħ��

    void Start()
    {
        step = 0;
        ColliderTriggers[1].enabled = false; // �� �� �� ����ĳ��Ʈ�� ���Ƽ� ��� ��Ȱ��ȭ 
        ColliderTriggers[2].enabled = false;
        ColliderTriggers[3].enabled = false;
        ColliderTriggers[4].enabled = false;
        ColliderTriggers[5].enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == lionMonsterCol) // ������ ���� �� ���
        {
            LoadSceneScript.FailLoadScene("03_LionDance");
        }

        switch (step)
        {
            case 0: // �ξ� ����
                break;

            case 1: // �ξ� ����
                if (other == ColliderTriggers[(int)Triggers.kitchen]) // �ξ� Ʈ���ſ� ����� ��
                {
                    step = 2;
                    ColliderTriggers[1].enabled = true;
                    ColliderTriggers[2].enabled = true;
                    ColliderTriggers[3].enabled = true;
                    ColliderTriggers[4].enabled = true;
                    ColliderTriggers[5].enabled = true;

                    dogAnimationScript.disableLookPlayer();
                    dogAnimator.Play("Balcony");

                    Debug.Log("���� �߼Ҹ� ���. ���ڴ� �ʿ��� �̻��� �Ҹ� ���ٴ� ��� ���");
                }
                break;

            case 2: // �Ž� ����
                if (other == ColliderTriggers[(int)Triggers.livingRoom]) // �Ž� Ʈ���ſ� ����� ��
                {
                    step = 3;
                    lionDanceDirector.LookMonsterDirector();

                    Debug.Log("������ �������� �ö󰡴� �ƽ� ���");
                }
                break;

            case 3: // ���ڴ� â�� �ݱ�
                if (other == ColliderTriggers[(int)Triggers.kitchen] ||       // �ξ� Ʈ���ſ� ����� ��
                    other == ColliderTriggers[(int)Triggers.balcony] ||       // ���ڴ� Ʈ���ſ� ����� ��
                    other == ColliderTriggers[(int)Triggers.frontOfBathroom]) // ȭ��� �� Ʈ���ſ� ����� ��
                {
                    step = 11;
                    lionMonsterAnimator.Play("Balcony_All"); // ������ ���ڴ� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���
                    StartCoroutine(SurviveTimerCoroutine(11f)); // ���� �ð� ��Ƽ�� ���� ����
                    SetActiveFalseColliderTriggers();
                    dogAnimationScript.StopBarking();

                    Debug.Log("������ ���ڴ� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���. ���� �ð� ��Ƽ�� ���� ����");
                }
                break;

            case 4: // ���ڴ� �� ����
                if (other == ColliderTriggers[(int)Triggers.livingRoom]) // �Ž� Ʈ���ſ� ����� ��
                {
                    balconyDoor.GetComponent<GetComponentScript>().animator.SetBool("Active", false); // ���ڴ� �� ����
                    balconyDoor.GetComponent<AudioSource>().Play();
                    step = 5;
                    StartCoroutine(MyRoomTimerCoroutine()); // 8�� �ڿ� �� ������ ���� ħ��

                    dogAnimator.Play("LookMyRoom");

                    Debug.Log("���ڴ� �� �ݱ�");
                }
                break;

            case 5: // �� �� ����
                if (other == ColliderTriggers[(int)Triggers.kitchen] || // �ξ� Ʈ���ſ� ����� ��
                    other == ColliderTriggers[(int)Triggers.sisRoom])   // ���� �� Ʈ���ſ� ����� ��
                {
                    step = 11;
                    lionMonsterAnimator.Play("MyRoom_All"); // ������ �� �� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���
                    StartCoroutine(SurviveTimerCoroutine(12f)); // ���� �ð� ��Ƽ�� ���� ����
                    SetActiveFalseColliderTriggers();
                    dogAnimationScript.StopBarking();

                    Debug.Log("������ �� �� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���. ���� �ð� ��Ƽ�� ���� ����");
                }
                else if (other == ColliderTriggers[(int)Triggers.myRoom]) // �� �� Ʈ���ſ� ����� ��
                {
                    lionMonsterAnimator.Play("MyRoom"); // ������ �� �� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���
                    step = 6;

                    Debug.Log("������ �� �� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���");
                }
                break;

            case 6: // �� �� �� ����
                if (other == ColliderTriggers[(int)Triggers.frontOfBathroom]) // �� �濡�� ���� �Ŀ� ȭ��� �� Ʈ���ſ� ����� �� 
                {
                    myRoomDoor.GetComponent<GetComponentScript>().animator.SetBool("Active", false); // �� �� �� ������ �ִϸ��̼� ���
                    myRoomDoor.GetComponent<AudioSource>().Play();
                    step = 7;
                    StartCoroutine(SisRoomTimerCoroutine()); // 5.5�� �ڿ� ���� ������ ���� ħ��

                    dogAnimator.Play("LookSisRoom");

                    Debug.Log("�� �� �� ������ �ִϸ��̼� ���");
                }
                break;

            case 7: // ���� �� ����
                if (other == ColliderTriggers[(int)Triggers.kitchen] ||  // �ξ� Ʈ���ſ� ����� ��
                    other == ColliderTriggers[(int)Triggers.livingRoom]) // �Ž� Ʈ���ſ� ����� ��
                {
                    step = 11;
                    lionMonsterAnimator.Play("SisRoom_All"); // ������ ���� �� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���
                    StartCoroutine(SurviveTimerCoroutine(11f)); // ���� �ð� ��Ƽ�� ���� ����
                    SetActiveFalseColliderTriggers();
                    dogAnimationScript.StopBarking();

                    Debug.Log("������ ���� �� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���. ���� �ð� ��Ƽ�� ���� ����");
                }
                else if (other == ColliderTriggers[(int)Triggers.sisRoom]) // ���� �� Ʈ���ſ� ����� ��
                {
                    lionMonsterAnimator.Play("SisRoom"); // ������ ���� �� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���
                    step = 8;
                    
                    Debug.Log("������ ���� �� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���");
                }
                break;

            case 8: // ���� �� �� ���� + �ξ� ����
                if (other == ColliderTriggers[(int)Triggers.frontOfBathroom]) // ���� �濡�� ���� �Ŀ� ȭ��� �� Ʈ���ſ� ����� �� 
                {
                    sisRoomDoor.GetComponent<GetComponentScript>().animator.SetBool("Active", false); // ���� �� �� ������ �ִϸ��̼� ���
                    sisRoomDoor.GetComponent<AudioSource>().Play();
                    StartCoroutine(KitchenTimerCoroutine()); // 6�� �ڿ� �ξ����� ���� ħ��
                    step = 9;

                    dogAnimator.Play("Kitchen");

                    Debug.Log("���� �� �� ������ �ִϸ��̼� ���. �ξ� â������ ������ ������ ī��Ʈ�ٿ� ����");
                }
                break;

            default:
                break;
        }
    }




    // ���ڴϷ� ������ ������ ī��Ʈ�ٿ� �ڷ�ƾ
    public IEnumerator BalconyTimerCoroutine()
    {
        yield return new WaitForSeconds(8f);
        if (step == 3)
        {
            mouseText.enabled = false;
            balconyDoor.GetComponent<Outline>().enabled = false; // �ܰ��� ����
            step = 11;
            lionMonsterAnimator.Play("Balcony_All"); // ������ ���ڴϷ� ���� ���� ���ƴٴϴ� �ִϸ��̼� ���
            StartCoroutine(SurviveTimerCoroutine(11f)); // ���� �ð� ��Ƽ�� ���� ����
            SetActiveFalseColliderTriggers();
            dogAnimationScript.StopBarking();
            Debug.Log("������ ���ڴϷ� ���� ���� ���ƴٴϴ� �ִϸ��̼� ���");
        }
    }

    // �� ������ ������ ������ ī��Ʈ�ٿ� �ڷ�ƾ
    IEnumerator MyRoomTimerCoroutine()
    {
        yield return new WaitForSeconds(8f);
        if (step == 5)
        {
            step = 11;
            lionMonsterAnimator.Play("MyRoom_All"); // ������ �� ������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���
            StartCoroutine(SurviveTimerCoroutine(12f)); // ���� �ð� ��Ƽ�� ���� ����
            SetActiveFalseColliderTriggers();
            dogAnimationScript.StopBarking();
            Debug.Log("������ �� ������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���");
        }
    }

    // ���� ������ ������ ������ ī��Ʈ�ٿ� �ڷ�ƾ
    IEnumerator SisRoomTimerCoroutine()
    {
        yield return new WaitForSeconds(5.5f);
        if (step == 7)
        {
            step = 11;
            lionMonsterAnimator.Play("SisRoom_All"); // ������ ���� ������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���
            StartCoroutine(SurviveTimerCoroutine(11f)); // ���� �ð� ��Ƽ�� ���� ����
            SetActiveFalseColliderTriggers();
            dogAnimationScript.StopBarking();
            Debug.Log("������ ���� ������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���");
        }
    }

    // �ξ� â������ ������ ������ ī��Ʈ�ٿ� �ڷ�ƾ
    IEnumerator KitchenTimerCoroutine()
    {
        yield return new WaitForSeconds(6f);
        if (step == 9)
        {
            mouseText.enabled = false;
            kitchenWindow.GetComponent<Outline>().enabled = false; // �ܰ��� ����
            step = 11;
            lionMonsterAnimator.Play("Kitchen_All"); // ������ �ξ� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���
            StartCoroutine(SurviveTimerCoroutine(10f)); // ���� �ð� ��Ƽ�� ���� ����
            dogAnimator.Play("Desk");
            dogAnimationScript.enableLookPlayer();
            Debug.Log("������ �ξ� â������ ���� ���� ���ƴٴϴ� �ִϸ��̼� ���");
        }
    }




    // ���� �ð� ��Ƽ�� ���� ����
    public IEnumerator SurviveTimerCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        if (step == 11 || step == 10)
        {
            step = 10; // ���� ����
            lionDanceDirector.BackToArtGallery();
        }
    }




    // Ʈ���� �ݶ��̴� ��Ȱ��ȭ �޼ҵ�
    public void SetActiveFalseColliderTriggers()
    {
        for (int i = 0; i < ColliderTriggers.Length; i++)
        {
            ColliderTriggers[i].enabled = false;
        }
    }
}
