using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimationScript : MonoBehaviour
{
    public Animator dogAnim;
    public GameObject player;
    public GameObject waist; // ���� �㸮 Bone
    public Vector3 originalRotation; // ���� �����̼ǰ�
    public bool isLookPlayer; // �÷��̾ �ٶ���

    public AudioSource bark;

    void Start()
    {
        isLookPlayer = true;
        originalRotation = waist.transform.eulerAngles;
    }

    void Update()
    {
        LookPlayer();
    }




    public void StartWalking() // �� �ȱ� ����
    {
        dogAnim.SetBool("IsWalking", true);
    }

    public void StopWalking() // �� �ȱ� ����
    {
        dogAnim.SetBool("IsWalking", false);
    }



    public void StartBarking() // �� ¢�� ����
    { 
        bark.Play();
    }

    public void StopBarking() // �� ¢�� ����
    {
        bark.Stop();
    }




    public void enableLookPlayer() // ���� �÷��̾� �ٶ󺸱� Ȱ��ȭ
    {
        isLookPlayer = true;
    }

    public void disableLookPlayer() // ���� �÷��̾� �ٶ󺸱� ��Ȱ��ȭ
    {
        waist.transform.eulerAngles = originalRotation;
        isLookPlayer = false;
    }

    void LookPlayer() // ���� �÷��̾� �ٶ󺸱�
    {
        if (isLookPlayer)
        {
            waist.transform.rotation = Quaternion.Lerp(waist.transform.rotation, Quaternion.LookRotation(new Vector3(-(player.transform.position.x - waist.transform.position.x), waist.transform.position.y, -(player.transform.position.z - waist.transform.position.z))), 0.05f);
        }
    }
}
