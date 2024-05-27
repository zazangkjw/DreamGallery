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
            circusFlash.isFlashOn = false;
        }

        // ��Ŀ�� ���� ���� Ʈ����
        foreach (Collider c in triggers_CircusSuccess)
        {
            if (other == c)
            {
                circusFlash.isFlashOn = true;
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
            myRigid.velocity = transform.up * 2;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                myRigid.velocity = transform.up * 4;
            }
        }
        else
        {
            myRigid.velocity = -transform.up * 2;
        }
    }
}
