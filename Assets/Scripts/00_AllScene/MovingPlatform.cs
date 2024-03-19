using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // ��ũ��Ʈ�� �÷��̾ �����ϰ� �÷��� ������Ʈ�� �±׸� Platform���� ����. Ʈ���� �ȿ� ���� �����̴� �÷����� �θ�� ������

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Platform")
        {
            transform.SetParent(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            transform.SetParent(null);
        }
    }
}
