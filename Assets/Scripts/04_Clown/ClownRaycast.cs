using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClownRaycast : MonoBehaviour
{
    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;

    bool isChecking = true;

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject Objects;

    [SerializeField]
    Animator[] elevatorAnims;

    [SerializeField]
    GameObject[] elevatorButtons;

    [SerializeField]
    GameObject elevator_Point_Player;

    GameObject button;

    [SerializeField]
    GameObject unicycle;

    [SerializeField]
    Animator unicycleAnim;




    void Start()
    {
        // 처음에 열려있어야 하는 엘리베이터 문들
        elevatorAnims[0].Play("Open");
        elevatorAnims[1].Play("Open");
        elevatorAnims[3].Play("Open");
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

                    isChecking = false; // 아래의 항목들은 체크하지 않음

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
                if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                    preObject = null;
                }

                preObject = hitObject;
                preObject.GetComponent<Outline>().enabled = true; // 외곽선 켜기

                // E키 입력 시
                if (Input.GetKeyDown(KeyCode.E))
                {
                    unicycleAnim.Play("Go");
                    hitObject.GetComponent<Collider>().enabled = false; // 버튼 콜라이더 비활성화
                    hitObject.GetComponent<Outline>().enabled = false; // 버튼 외곽선 비활성화
                }

                isChecking = false; // 아래의 항목들은 체크하지 않음
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
        button = hitObject;

        button.GetComponent<Collider>().enabled = false; // 버튼 콜라이더 비활성화
        button.GetComponent<Outline>().enabled = false; // 버튼 외곽선 비활성화
        button.GetComponent<NextElevatorPoint>().thisElevatorAnim.Play("Close");

        elevator_Point_Player.transform.position = button.GetComponent<NextElevatorPoint>().thisPoint.transform.position; // elevator_Point_Player을 엘리베이터 포인트로 옮기고, 플레이어를 자식으로 넣어서
        elevator_Point_Player.transform.rotation = button.GetComponent<NextElevatorPoint>().thisPoint.transform.rotation; // 2초 후에 다음 엘리베이터 포인트로 이동 후 자식 해제
        player.transform.SetParent(elevator_Point_Player.transform);

        yield return new WaitForSeconds(2f);

        elevator_Point_Player.transform.position = button.GetComponent<NextElevatorPoint>().nextPoint.transform.position;
        elevator_Point_Player.transform.rotation = button.GetComponent<NextElevatorPoint>().nextPoint.transform.rotation;

        yield return new WaitForSeconds(2f);

        button.GetComponent<NextElevatorPoint>().nextElevatorAnim.Play("Open");
        player.transform.SetParent(Objects.transform);
    }
}
    
