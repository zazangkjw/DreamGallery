using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownColliderTrigger : MonoBehaviour
{
    [SerializeField]
    Collider trigger_Chase; // 광대 추격 시작 트리거 콜라이더

    [SerializeField]
    Collider trigger_SafeZone; // 안전 구역 트리거 콜라이더

    [SerializeField]
    Clown_Chase clown_Chase;

    [SerializeField]
    Collider trigger_CircusSuccess;

    [SerializeField]
    CircusFlash circusFlash;




    private void OnTriggerEnter(Collider other)
    {
        // 안전 구역 트리거
        if (other == trigger_SafeZone)
        {
            clown_Chase.isSafe = true;
            circusFlash.isFlashOn = false;
        }

        // 서커스 도전 성공 트리거
        if(other == trigger_CircusSuccess)
        {
            circusFlash.isFlashOn = true;
        }
    }

    // 추격 시작 트리거에 들어가면 광대 추격 시작
    private void OnTriggerStay(Collider other)
    {
        if (other == trigger_Chase)
        {
            clown_Chase.isChasing = true;
        }
    }

    // 추격 시작 트리거에서 나가면 광대 추격 종료
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
