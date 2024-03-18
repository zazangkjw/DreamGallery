using System.Collections;
using TMPro;
using UnityEngine;

public class LionDanceColliderTrigger : MonoBehaviour
{
    public GameObject balconyDoor;
    public GameObject myRoomDoor;
    public GameObject sisRoomDoor;
    public GameObject balconyWindow;
    public GameObject kitchenWindow;
    public Animator lionMonsterAnimator;
    public Collider lionMonsterCol;
    public LionDanceDirector lionDanceDirector;
    public LionDanceRaycast lionDanceRaycast;
    public AudioSource monsterLaughing;
    public Animator dogAnimator;
    public DogAnimationScript dogAnimationScript;
    public Collider[] ColliderTriggers; // 트리거 콜라이더들
    public TextMeshProUGUI mouseText;

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
            LoadSceneScript.FailLoadScene("03_LionDance");
        }

        switch (step)
        {
            case 0: // 부엌 가기
                break;

            case 1: // 부엌 가기
                if (other == ColliderTriggers[(int)Triggers.kitchen]) // 부엌 트리거에 닿았을 때
                {
                    step = 2;
                    ColliderTriggers[1].enabled = true;
                    ColliderTriggers[2].enabled = true;
                    ColliderTriggers[3].enabled = true;
                    ColliderTriggers[4].enabled = true;
                    ColliderTriggers[5].enabled = true;

                    dogAnimationScript.disableLookPlayer();
                    dogAnimator.Play("Balcony");

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
                    dogAnimationScript.StopBarking();

                    Debug.Log("괴물이 발코니 창문으로 들어와 집을 돌아다니는 애니메이션 재생. 일정 시간 버티면 생존 엔딩");
                }
                break;

            case 4: // 발코니 문 닫힘
                if (other == ColliderTriggers[(int)Triggers.livingRoom]) // 거실 트리거에 닿았을 때
                {
                    balconyDoor.GetComponent<GetComponentScript>().animator.SetBool("Active", false); // 발코니 문 닫힘
                    balconyDoor.GetComponent<AudioSource>().Play();
                    step = 5;
                    StartCoroutine(MyRoomTimerCoroutine()); // 8초 뒤에 내 방으로 괴물 침입

                    dogAnimator.Play("LookMyRoom");

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
                    dogAnimationScript.StopBarking();

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
                    StartCoroutine(SisRoomTimerCoroutine()); // 5.5초 뒤에 누나 방으로 괴물 침입

                    dogAnimator.Play("LookSisRoom");

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
                    dogAnimationScript.StopBarking();

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
                    StartCoroutine(KitchenTimerCoroutine()); // 6초 뒤에 부엌으로 괴물 침입
                    step = 9;

                    dogAnimator.Play("Kitchen");

                    Debug.Log("누나 방 문 닫히는 애니메이션 재생. 부엌 창문으로 괴물이 들어오는 카운트다운 시작");
                }
                break;

            default:
                break;
        }
    }




    // 발코니로 괴물이 들어오는 카운트다운 코루틴
    public IEnumerator BalconyTimerCoroutine()
    {
        yield return new WaitForSeconds(8f);
        if (step == 3)
        {
            mouseText.enabled = false;
            balconyDoor.GetComponent<Outline>().enabled = false; // 외곽선 끄기
            step = 11;
            lionMonsterAnimator.Play("Balcony_All"); // 괴물이 발코니로 들어와 집을 돌아다니는 애니메이션 재생
            StartCoroutine(SurviveTimerCoroutine(11f)); // 일정 시간 버티면 생존 엔딩
            SetActiveFalseColliderTriggers();
            dogAnimationScript.StopBarking();
            Debug.Log("괴물이 발코니로 들어와 집을 돌아다니는 애니메이션 재생");
        }
    }

    // 내 방으로 괴물이 들어오는 카운트다운 코루틴
    IEnumerator MyRoomTimerCoroutine()
    {
        yield return new WaitForSeconds(8f);
        if (step == 5)
        {
            step = 11;
            lionMonsterAnimator.Play("MyRoom_All"); // 괴물이 내 방으로 들어와 집을 돌아다니는 애니메이션 재생
            StartCoroutine(SurviveTimerCoroutine(12f)); // 일정 시간 버티면 생존 엔딩
            SetActiveFalseColliderTriggers();
            dogAnimationScript.StopBarking();
            Debug.Log("괴물이 내 방으로 들어와 집을 돌아다니는 애니메이션 재생");
        }
    }

    // 누나 방으로 괴물이 들어오는 카운트다운 코루틴
    IEnumerator SisRoomTimerCoroutine()
    {
        yield return new WaitForSeconds(5.5f);
        if (step == 7)
        {
            step = 11;
            lionMonsterAnimator.Play("SisRoom_All"); // 괴물이 누나 방으로 들어와 집을 돌아다니는 애니메이션 재생
            StartCoroutine(SurviveTimerCoroutine(11f)); // 일정 시간 버티면 생존 엔딩
            SetActiveFalseColliderTriggers();
            dogAnimationScript.StopBarking();
            Debug.Log("괴물이 누나 방으로 들어와 집을 돌아다니는 애니메이션 재생");
        }
    }

    // 부엌 창문으로 괴물이 들어오는 카운트다운 코루틴
    IEnumerator KitchenTimerCoroutine()
    {
        yield return new WaitForSeconds(6f);
        if (step == 9)
        {
            mouseText.enabled = false;
            kitchenWindow.GetComponent<Outline>().enabled = false; // 외곽선 끄기
            step = 11;
            lionMonsterAnimator.Play("Kitchen_All"); // 괴물이 부엌 창문으로 들어와 집을 돌아다니는 애니메이션 재생
            StartCoroutine(SurviveTimerCoroutine(10f)); // 일정 시간 버티면 생존 엔딩
            dogAnimator.Play("Desk");
            dogAnimationScript.enableLookPlayer();
            Debug.Log("괴물이 부엌 창문으로 들어와 집을 돌아다니는 애니메이션 재생");
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




    // 트리거 콜라이더 비활성화 메소드
    public void SetActiveFalseColliderTriggers()
    {
        for (int i = 0; i < ColliderTriggers.Length; i++)
        {
            ColliderTriggers[i].enabled = false;
        }
    }
}
