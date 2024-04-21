using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimationScript : MonoBehaviour
{
    public Animator dogAnim;
    public GameObject player;
    public GameObject rootForUnity; // ���� �㸮 Bone
    public Vector3 originalRotation; // ���� �����̼ǰ�
    public bool isLookPlayer; // �÷��̾ �ٶ���

    public AudioSource bark;

    void Start()
    {
        isLookPlayer = true;
        originalRotation = rootForUnity.transform.rotation.eulerAngles;
    }

    void FixedUpdate()
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
        rootForUnity.transform.rotation = Quaternion.Euler(originalRotation);
        isLookPlayer = false;
    }

    void LookPlayer() // ���� �÷��̾� �ٶ󺸱�
    {
        if (isLookPlayer)
        {
            // rootForUnity.transform.rotation = Quaternion.Lerp(rootForUnity.transform.rotation, Quaternion.LookRotation(new Vector3(-(player.transform.position.x - rootForUnity.transform.position.x), rootForUnity.transform.position.y, -(player.transform.position.z - rootForUnity.transform.position.z))), 0.05f);
            rootForUnity.transform.rotation = Quaternion.Lerp(rootForUnity.transform.rotation, Quaternion.LookRotation(new Vector3((player.transform.position.x - rootForUnity.transform.position.x), 0, (player.transform.position.z - rootForUnity.transform.position.z))) * Quaternion.Euler(new Vector3(-90f, 180f, 0)), 0.05f);
        }
    }
}
