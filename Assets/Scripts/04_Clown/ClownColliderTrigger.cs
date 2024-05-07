using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownColliderTrigger : MonoBehaviour
{
    [SerializeField]
    Collider[] trigger_Chase; // 광대 추격 시작 트리거 콜라이더

    [SerializeField]
    Collider[] trigger_SafeZone; // 안전 구역 트리거 콜라이더

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

    // 추격 시작 트리거에 들어가면 광대 추격 시작
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

    // 추격 시작 트리거에서 나가면 광대 추격 종료
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
