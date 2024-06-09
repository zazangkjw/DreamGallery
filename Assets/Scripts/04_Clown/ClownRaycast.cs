using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ClownRaycast : MonoBehaviour
{
    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;

    bool isChecking = true;

    public GameObject player;
    [SerializeField]
    Rigidbody playerRigid;
    public GameObject Objects;

    // 엘리베이터
    [SerializeField]
    Animator[] elevatorAnims;
    [SerializeField]
    GameObject[] elevatorButtons;
    [SerializeField]
    GameObject elevator_Point_Player;
    GameObject elevatorBtn;

    // AudioSource
    public AudioSource cheer;
    public AudioSource clap;
    public AudioSource booing;
    public AudioSource yay;

    // 도전 기회
    public int life;
    public TextMeshProUGUI lifeText;

    // 외줄타기 도전
    [SerializeField]
    GameObject unicycle;
    [SerializeField]
    GameObject unicycleSeat;
    public GameObject successsPlatform;
    public bool isOnUnicycle;


    void Start()
    {
        isOnUnicycle = false;

        // 처음에 열려있어야 하는 엘리베이터 문들
        elevatorAnims[0].Play("Open");
        elevatorAnims[1].Play("Open");

        life = 3;
    }

    void Update()
    {
        ShootRaycast();
    }

    // 카메라에서 레이캐스트 쏘기
    public void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 1.5f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.5f))
        {
            hitObject = hitInfo.collider.gameObject;
        }
        // 허공일 때
        else
        {
            hitObject = null;
        }

        CheckObject();
    }

    public void CheckObject()
    {
        isChecking = true;

        // 엘리베이터 버튼
        if (isChecking)
        {
            foreach (GameObject btn in elevatorButtons)
            {
                if (hitObject == btn)
                {
                    if (preObject != hitObject.GetComponent<GetComponentScript>().mesh && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                    {
                        preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                        preObject = null;
                    }

                    preObject = hitObject.GetComponent<GetComponentScript>().mesh;
                    preObject.GetComponent<Outline>().enabled = true; // 외곽선 켜기

                    // E키 입력 시
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(ElevatorCoroutine());
                    }

                    isChecking = false; // 이후의 항목들은 체크하지 않음

                    break;
                }
                else
                {
                    mouseText.enabled = false; // 텍스트 없어짐

                    if (preObject != null)
                    {
                        preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                        preObject = null;
                    }
                }
            }
        }

        // 외발자전거
        if (isChecking)
        {
            if (hitObject == unicycle)
            {
                if (preObject != hitObject.GetComponent<GetComponentScript>().mesh && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                    preObject = null;
                }

                preObject = hitObject.GetComponent<GetComponentScript>().mesh;
                preObject.GetComponent<Outline>().enabled = true; // 외곽선 켜기

                // E키 입력 시
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(UnicycleCoroutine());
                }

                isChecking = false; // 이후의 항목들은 체크하지 않음
            }
            else
            {
                mouseText.enabled = false; // 텍스트 없어짐

                if (preObject != null)
                {
                    preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                    preObject = null;
                }
            }
        }
    }




    // 엘리베이터 버튼을 눌렀을 때
    IEnumerator ElevatorCoroutine()
    {
        elevatorBtn = hitObject;

        elevatorBtn.GetComponent<Collider>().enabled = false; // 콜라이더 비활성화
        preObject.GetComponent<Outline>().enabled = false; // 외곽선 비활성화

        elevatorBtn.GetComponent<NextElevatorPoint>().thisElevatorAnim.Play("Close");

        elevator_Point_Player.transform.position = elevatorBtn.GetComponent<NextElevatorPoint>().thisPoint.transform.position; // elevator_Point_Player을 엘리베이터 포인트로 옮기고, 플레이어를 자식으로 넣어서
        elevator_Point_Player.transform.rotation = elevatorBtn.GetComponent<NextElevatorPoint>().thisPoint.transform.rotation; // 2초 후에 다음 엘리베이터 포인트로 이동 후 자식 해제
        player.transform.SetParent(elevator_Point_Player.transform);

        yield return new WaitForSeconds(3f);

        elevator_Point_Player.transform.position = elevatorBtn.GetComponent<NextElevatorPoint>().nextPoint.transform.position;
        elevator_Point_Player.transform.rotation = elevatorBtn.GetComponent<NextElevatorPoint>().nextPoint.transform.rotation;

        // 추가 기능 //
        if (elevatorBtn.GetComponent<NextElevatorPoint>().goTo == "Circus")
        {
            life = 3;
            lifeText.text = life.ToString();
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(cheer, true, 5f));
        }
        else
        {
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(cheer, false, 5f));
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(clap, false, 5f));
        }
        //-----------//

        yield return new WaitForSeconds(6f);

        elevatorBtn.GetComponent<NextElevatorPoint>().nextElevatorAnim.Play("Open");
        player.transform.SetParent(Objects.transform);
    }




    // 외발자전거를 눌렀을 때
    IEnumerator UnicycleCoroutine()
    {
        isOnUnicycle = true;

        unicycle = hitObject;

        unicycle.GetComponent<Collider>().enabled = false; // 콜라이더 비활성화
        preObject.GetComponent<Outline>().enabled = false; // 외곽선 비활성화

        // 앉아있는 상태일 때 일어나게 만들기
        transform.localPosition = new Vector3(0, player.GetComponent<PlayerController>().originPosY, 0);
        player.GetComponent<PlayerController>().standCollider.enabled = true;
        player.GetComponent<PlayerController>().crouchCollider.enabled = false;

        // 외발자전거 탑승
        player.transform.SetParent(unicycleSeat.transform);
        player.transform.position = unicycleSeat.transform.position;
        player.transform.rotation = unicycleSeat.transform.rotation;

        // 컨트롤러 교체
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<UnicycleController>().enabled = true;
        player.GetComponent<UnicycleController>().lookSensitivity = player.GetComponent<PlayerController>().lookSensitivity;
        player.GetComponent<Rigidbody>().isKinematic = true;

        unicycle.GetComponent<GetComponentScript>().animator.Play("Go");
        unicycle.GetComponent<GetComponentScript>().animator.Play("WheelTurn", 1);
        yield return new WaitForSeconds(8f);
        unicycle.GetComponent<GetComponentScript>().animator.Play("Idle", 1);


        // 외발자전거 도전 성공
        if (isOnUnicycle)
        {
            isOnUnicycle = false;

            player.transform.SetParent(Objects.transform);
            player.transform.position = successsPlatform.transform.position + Vector3.up * 3;
            player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
            elevatorAnims[3].Play("Open");
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(clap, true, 2f));
            yay.Play();

            // 컨트롤러 교체
            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<UnicycleController>().enabled = false;
            player.GetComponent<Rigidbody>().isKinematic = false;
        }
        else if(life > 0)
        {
            player.GetComponent<UnicycleController>().bodyForUnity.transform.localEulerAngles = player.GetComponent<UnicycleController>().originRotate;
            unicycle.GetComponent<GetComponentScript>().animator.Play("Return");
            unicycle.GetComponent<GetComponentScript>().animator.Play("WheelTurnReverse", 1);
            yield return new WaitForSeconds(8f);
            unicycle.GetComponent<GetComponentScript>().animator.Play("Idle", 1);
            unicycle.GetComponent<Collider>().enabled = true; // 콜라이더 활성화
        }
    }
}
    
