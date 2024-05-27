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
        // 안전 구역 트리거
        if (other == trigger_SafeZone)
        {
            clown_Chase.isSafe = true;
            circusFlash.isFlashOn = false;
        }

        // 서커스 도전 성공 트리거
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
        // 추격 시작 트리거에 들어가면 광대 추격 시작
        if (other == trigger_Chase)
        {
            clown_Chase.isChasing = true;
        }

        // 사다리 트리거
        if (other == Ladder_Trigger)
        {
            UpLadder();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 추격 시작 트리거에서 나가면 광대 추격 종료
        if (other == trigger_Chase)
        {
            clown_Chase.isChasing = false;
        }
        if (other == trigger_SafeZone)
        {
            clown_Chase.isSafe = false;
        }
    }




    // 사다리 기능
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
