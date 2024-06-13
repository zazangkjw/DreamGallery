using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownColliderTrigger : MonoBehaviour
{
    [SerializeField]
    Collider trigger_Chase; // ���� �߰� ���� Ʈ���� �ݶ��̴�

    [SerializeField]
    Collider trigger_SafeZone; // ���� ���� Ʈ���� �ݶ��̴�

    [SerializeField]
    Clown_Chase clown_Chase;

    [SerializeField]
    Collider[] triggers_CircusSuccess;

    [SerializeField]
    CircusFlash circusFlash;

    [SerializeField]
    Collider Ladder_Trigger;

    Rigidbody myRigid;

    [SerializeField]
    ClownRaycast clownRaycast;

    public bool[] isSuccess = new bool[1];




    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
    }


    private void OnTriggerEnter(Collider other)
    {
        // ���� ���� Ʈ����
        if (other == trigger_SafeZone)
        {
            clown_Chase.isSafe = true;

        }

        // �߰� ���� Ʈ���ſ� ���� �÷��� OFF
        if (other == trigger_Chase)
        {
            circusFlash.isFlashOn = false;
        }

        // ��Ŀ�� ���� ���� Ʈ����
        for(int i = 0; i < triggers_CircusSuccess.Length; i++)
        {
            if (!isSuccess[i] && other == triggers_CircusSuccess[i] && clownRaycast.life > 0)
            {
                isSuccess[i] = true;
                circusFlash.isFlashOn = true;
                StartCoroutine(AudioOnOffScript.VolumeCoroutine(clownRaycast.applause, true, 2f, 0.5f));
                clownRaycast.yay.Play();
                clownRaycast.circusSong.Stop();
                clownRaycast.elevatorAnims[3].Play("Open");
            }
        }
    }

    private void OnTriggerStay(Collider other)

    {
        // �߰� ���� Ʈ���ſ� ���� ���� �߰� ����
        if (other == trigger_Chase)
        {
            clown_Chase.isChasing = true;
        }

        // ��ٸ� Ʈ����
        if (other == Ladder_Trigger)
        {
            UpLadder();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �߰� ���� Ʈ���ſ��� ������ ���� �߰� ����
        if (other == trigger_Chase)
        {
            clown_Chase.isChasing = false;
        }
        if (other == trigger_SafeZone)
        {
            clown_Chase.isSafe = false;
        }
    }




    // ��ٸ� ���
    private void UpLadder()
    {
        if (Input.GetKey(KeyCode.W))
        {
            myRigid.velocity = new Vector3(myRigid.velocity.x, 2.5f, myRigid.velocity.z);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                myRigid.velocity = new Vector3(myRigid.velocity.x, 5f, myRigid.velocity.z);
            }
        }
        else
        {
            myRigid.velocity = new Vector3(myRigid.velocity.x, -2.5f, myRigid.velocity.z);
        }
    }
}
