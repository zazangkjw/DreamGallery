using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownColliderTrigger : MonoBehaviour
{
    [SerializeField]
    Collider[] trigger_Chase; // ���� �߰� ���� Ʈ���� �ݶ��̴�

    [SerializeField]
    Clown_Chase clown_Chase;

    void Start()
    {
        
    }

    void Update()
    {
        
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
    }
}
