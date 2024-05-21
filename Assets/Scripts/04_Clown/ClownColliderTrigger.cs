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
    Collider trigger_CircusSuccess;

    [SerializeField]
    CircusFlash circusFlash;




    private void OnTriggerEnter(Collider other)
    {
        // ���� ���� Ʈ����
        if (other == trigger_SafeZone)
        {
            clown_Chase.isSafe = true;
            circusFlash.isFlashOn = false;
        }

        // ��Ŀ�� ���� ���� Ʈ����
        if(other == trigger_CircusSuccess)
        {
            circusFlash.isFlashOn = true;
        }
    }

    // �߰� ���� Ʈ���ſ� ���� ���� �߰� ����
    private void OnTriggerStay(Collider other)
    {
        if (other == trigger_Chase)
        {
            clown_Chase.isChasing = true;
        }
    }

    // �߰� ���� Ʈ���ſ��� ������ ���� �߰� ����
    private void OnTriggerExit(Collider other)
    {
        if (other == trigger_Chase)
        {
            clown_Chase.isChasing = false;
        }

        if (other == trigger_SafeZone)
        {
            clown_Chase.isSafe = false;
        }
    }
}
