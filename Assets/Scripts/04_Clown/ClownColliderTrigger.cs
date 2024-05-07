using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownColliderTrigger : MonoBehaviour
{
    [SerializeField]
    Collider[] trigger_Chase; // ���� �߰� ���� Ʈ���� �ݶ��̴�

    [SerializeField]
    Collider[] trigger_SafeZone; // ���� ���� Ʈ���� �ݶ��̴�

    [SerializeField]
    Clown_Chase clown_Chase;

    void Start()
    {

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (Collider col in trigger_SafeZone)
        {
            if (other == col)
            {
                clown_Chase.isSafe = true;
            }
        }
    }

    // �߰� ���� Ʈ���ſ� ���� ���� �߰� ����
    private void OnTriggerStay(Collider other)
    {
        foreach (Collider col in trigger_Chase)
        {
            if (other == col)
            {
                clown_Chase.isChasing = true;
            }
        }
    }

    // �߰� ���� Ʈ���ſ��� ������ ���� �߰� ����
    private void OnTriggerExit(Collider other)
    {
        foreach (Collider col in trigger_Chase)
        {
            if (other == col)
            {
                clown_Chase.isChasing = false;
            }
        }

        foreach (Collider col in trigger_SafeZone)
        {
            if (other == col)
            {
                clown_Chase.isSafe = false;
            }
        }
    }
}
