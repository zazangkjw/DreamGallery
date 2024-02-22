using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LionDanceColliderTrigger : MonoBehaviour
{
    public GameObject balconyDoor;
    public GameObject myRoomDoor;
    public GameObject sisRoomDoor;
    public Animator lionMonsterAnimator;
    public Collider lionMonsterCol;
    public LionDanceDirector lionDanceDirector;
    public Collider[] ColliderTriggers; // 트리거 콜라이더들
    public PutDialogScript putDialogScript;

    enum Triggers
    {
        kitchen,            // 0
        livingRoom,         // 1
        balcony,            // 2
        frontOfBathroom,    // 3
        myRoom,             // 4
        sisRoom             // 5
    }

    public int step; // 지금 무슨 단계인지
                     // 0: 창문 다 열기
                     // 1: 부엌 가기
                     // 2: 거실 가기
                     // 3: 발코니 창문 닫기
                     // 4: 발코니 문 닫힘
                     // 5: 내 방 가기
                     // 6: 내 방 문 닫힘
                     // 7: 누나 방 가기
                     // 8: 누나 방 문 닫힘 + 부엌 가기
                     // 9: 부엌 창문 닫기
                     // 10: 생존 성공
                     // 11: 괴물 침입

    void Start()
    {
        step = 0;
        ColliderTriggers[1].enabled = false; // 문 열 때 레이캐스트를 막아서 잠시 비활성화 
        ColliderTriggers[2].enabled = false;
        ColliderTriggers[3].enabled = false;
        ColliderTriggers[4].enabled = false;
        ColliderTriggers[5].enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == lionMonsterCol) // 괴물과 접촉 시 사망
        {
            LoadSceneScript.failLoadScene("03_LionDance");
        }

        switch (step)
        {
            case 0: // 부엌 가기
                break;

            case 1: // 부엌 가기
                if (other == ColliderTriggers[(int)Triggers.kitchen]) // 부엌 트리거에 닿았을 때
                {
                    // 괴물 발소리 재생
                    putDialogScript.putDialog((string)GameManager.instance.textFileManager.dialog[2]["Content"], 3f); // 발코니 쪽에서 이상한 소리 난다는 대사 출력
                    step = 2;
                    ColliderTriggers[1].enabled = true;
                    ColliderTriggers[2].enabled = true;
                    ColliderTriggers[3].enabled = true;
                    ColliderTriggers[4].enabled = true;
                    ColliderTriggers[5].enabled = true;

                    Debug.Log("괴물 발소리 재생. 발코니 쪽에서 이상한 소리 난다는 대사 출력");
                }
                break;

            case 2: // 거실 가기
                if (other == ColliderTriggers[(int)Triggers.livingRoom]) // 거실 트리거에 닿았을 때
                {
                    step = 3;
                    lionDanceDirector.LookMonsterDirector();

                    Debug.Log("괴물이 윗층으로 올라가는 컷신 재생");
                }
                break;

            case 3: // 발코니 창문 닫기
                if (other == ColliderTriggers[(int)Triggers.kitchen] ||       // 부엌 트리거에 닿았을 때
                    other == ColliderTriggers[(int)Triggers.balcony] ||       // 발코니 트리거에 닿았을 때
                    other == ColliderTriggers[(int)Triggers.frontOfBathroom]) // 화장실 앞 트리거에 닿았을 때
                {
                    step = 11;
                    lionMonsterAnimator.Play("Balcony_All"); // 괴물이 발코니 창문으로 들어와 집을 돌아다니는 애니메이션 재생
                    StartCoroutine(SurviveTimerCoroutine(11f)); // 일정 시간 버티면 생존 엔딩
                    SetActiveFalseColliderTriggers();

                    Debug.Log("괴물이 발코니 창문으로 들어와 집을 돌아다니는 애니메이션 재생. 일정 시간 버티면 생존 엔딩");
                }
                break;

            case 4: // 발코니 문 닫힘
                if (other == ColliderTriggers[(int)Triggers.livingRoom]) // 거실 트리거에 닿았을 때
                {
                    balconyDoor.GetComponent<GetComponentScript>().animator.SetBool("Active", false); // 발코니 문 닫힘
                    balconyDoor.GetComponent<AudioSource>().Play();
                    step = 5;

                    Debug.Log("발코니 문 닫기");
                }
                break;

            case 5: // 내 방 가기
                if (other == ColliderTriggers[(int)Triggers.kitchen] || // 부엌 트리거에 닿았을 때
                    other == ColliderTriggers[(int)Triggers.sisRoom])   // 누나 방 트리거에 닿았을 때
                {
                    step = 11;
                    lionMonsterAnimator.Play("MyRoom_All"); // 괴물이 내 방 창문으로 들어와 집을 돌아다니는 애니메이션 재생
                    StartCoroutine(SurviveTimerCoroutine(12f)); // 일정 시간 버티면 생존 엔딩
                    SetActiveFalseColliderTriggers();

                    Debug.Log("괴물이 내 방 창문으로 들어와 집을 돌아다니는 애니메이션 재생. 일정 시간 버티면 생존 엔딩");
                }
                else if (other == ColliderTriggers[(int)Triggers.myRoom]) // 내 방 트리거에 닿았을 때
                {
                    lionMonsterAnimator.Play("MyRoom"); // 괴물이 내 방 창문으로 들어와 방을 돌아다니는 애니메이션 재생
                    step = 6;

                    Debug.Log("괴물이 내 방 창문으로 들어와 방을 돌아다니는 애니메이션 재생");
                }
                break;

            case 6: // 내 방 문 닫힘
                if (other == ColliderTriggers[(int)Triggers.frontOfBathroom]) // 내 방에서 나온 후에 화장실 앞 트리거에 닿았을 때 
                {
                    myRoomDoor.GetComponent<GetComponentScript>().animator.SetBool("Active", false); // 내 방 문 닫히는 애니메이션 재생
                    myRoomDoor.GetComponent<AudioSource>().Play();
                    step = 7;

                    Debug.Log("내 방 문 닫히는 애니메이션 재생");
                }
                break;

            case 7: // 누나 방 가기
                if (other == ColliderTriggers[(int)Triggers.kitchen] ||  // 부엌 트리거에 닿았을 때
                    other == ColliderTriggers[(int)Triggers.livingRoom]) // 거실 트리거에 닿았을 때
                {
                    step = 11;
                    lionMonsterAnimator.Play("SisRoom_All"); // 괴물이 누나 방 창문으로 들어와 집을 돌아다니는 애니메이션 재생
                    StartCoroutine(SurviveTimerCoroutine(11f)); // 일정 시간 버티면 생존 엔딩
                    SetActiveFalseColliderTriggers();

                    Debug.Log("괴물이 누나 방 창문으로 들어와 집을 돌아다니는 애니메이션 재생. 일정 시간 버티면 생존 엔딩");
                }
                else if (other == ColliderTriggers[(int)Triggers.sisRoom]) // 누나 방 트리거에 닿았을 때
                {
                    lionMonsterAnimator.Play("SisRoom"); // 괴물이 누나 방 창문으로 들어와 방을 돌아다니는 애니메이션 재생
                    step = 8;
                    
                    Debug.Log("괴물이 누나 방 창문으로 들어와 방을 돌아다니는 애니메이션 재생");
                }
                break;

            case 8: // 누나 방 문 닫힘 + 부엌 가기
                if (other == ColliderTriggers[(int)Triggers.frontOfBathroom]) // 누나 방에서 나온 후에 화장실 앞 트리거에 닿았을 때 
                {
                    sisRoomDoor.GetComponent<GetComponentScript>().animator.SetBool("Active", false); // 누나 방 문 닫히는 애니메이션 재생
                    sisRoomDoor.GetComponent<AudioSource>().Play();
                    StartCoroutine(TimerCoroutine()); // 부엌 창문으로 괴물이 들어오는 카운트다운 시작
                    step = 9;

                    Debug.Log("누나 방 문 닫히는 애니메이션 재생. 부엌 창문으로 괴물이 들어오는 카운트다운 시작");
                }
                break;

            default:
                break;
        }
    }

    // 부엌 창문으로 괴물이 들어오는 카운트다운 코루틴
    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(5f);
        if (step == 9)
        {
            step = 11;
            lionMonsterAnimator.Play("Kitchen_All"); // 괴물이 부엌 창문으로 들어와 집을 돌아다니는 애니메이션 재생
            Debug.Log("괴물이 부엌 창문으로 들어와 집을 돌아다니는 애니메이션 재생");
        }
    }

    // 트리거 콜라이더 비활성화 메소드
    public void SetActiveFalseColliderTriggers()
    {
        for (int i = 0; i < ColliderTriggers.Length; i++)
        {
            ColliderTriggers[i].enabled = false;
        }
    }

    // 일정 시간 버티면 생존 엔딩
    public IEnumerator SurviveTimerCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        if (step == 11 || step == 10)
        {
            step = 10; // 생존 성공
            lionDanceDirector.BackToArtGallery();
        }
    }
}
