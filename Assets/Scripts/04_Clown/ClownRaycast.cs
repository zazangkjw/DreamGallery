using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ClownRaycast : DefaultRaycast
{
    public GameObject player;
    [SerializeField]
    Rigidbody playerRigid;
    public GameObject Armatures;

    public ClownSceneManager clownSceneManager;

    // 엘리베이터
    public Animator[] elevatorAnims;
    [SerializeField]
    GameObject[] elevatorBtns_check; // 체크용
    GameObject elevatorBtn_current;
    [SerializeField]
    GameObject elevator_Point_Player;

    // AudioSource
    public AudioSource circusSong;
    public AudioSource applause;
    public AudioSource booing;
    public AudioSource yay;
    public AudioSource bikeWheel;
    public AudioSource bikeWheelClown;

    // 도전 기회
    public int life_max = 3;
    public int life;
    public TextMeshProUGUI lifeText;

    // 외줄타기 도전
    public GameObject unicycle_check; // 체크용
    public GameObject unicycle_current;
    public GameObject unicycleSeat;
    public GameObject successsPlatform;
    public bool isRidingUnicycle;
    public GameObject unicycleClown;
    public GameObject rope;
    public GameObject rope_fake;


    void Start()
    {
        WhenStart();

        isRidingUnicycle = false;
        rope.SetActive(false);
        rope_fake.SetActive(true);

        // 처음에 열려있어야 하는 엘리베이터 문들
        elevatorAnims[0].Play("Open");
        elevatorAnims[1].Play("Open");

        life = life_max;
    }

    void Update()
    {
        WhenUpdate();
        ShootRaycast();
        UnicycleIdle();
    }

    public override void CheckObject()
    {
        isChecking = true;

        // 상호작용 오브젝트
        if (isChecking)
        {
            if (hitObject != null && hitObject.tag == "InteractiveObject")
            {
                if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                mouseText.text = GameManager.instance.textFileManager.ui[26]; // "줍기" 텍스트 나옴
                mouseText.enabled = true;

                // E키 입력 시
                if (Input.GetKeyDown(KeyCode.E))
                {
                    preObject.GetComponent<InteractiveObject>().Action();
                }

                isChecking = false; // 이후의 항목들은 체크하지 않음
            }
            else
            {
                mouseText.enabled = false; // 텍스트 없어짐

                if (preObject != null)
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    preObject = null;
                }
            }
        }

        // 아이템
        if (isChecking)
        {
            if (hitObject != null && hitObject.tag == "Item")
            {
                if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                mouseText.text = GameManager.instance.textFileManager.ui[26]; // "줍기" 텍스트 나옴
                mouseText.enabled = true;

                // E키 입력 시
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItemCoroutine();
                }

                isChecking = false; // 이후의 항목들은 체크하지 않음
            }
            else
            {
                mouseText.enabled = false; // 텍스트 없어짐

                if (preObject != null)
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    preObject = null;
                }
            }
        }

        // 엘리베이터 버튼
        if (isChecking)
        {
            foreach (GameObject btn in elevatorBtns_check)
            {
                if (hitObject == btn)
                {
                    if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    }

                    preObject = hitObject;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                    mouseText.text = GameManager.instance.textFileManager.ui[28]; // "[E]" 텍스트 나옴
                    mouseText.enabled = true;

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
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                        preObject = null;
                    }
                }
            }
        }

        // 외발자전거
        if (isChecking)
        {
            if (hitObject == unicycle_check)
            {
                if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                mouseText.text = GameManager.instance.textFileManager.ui[24]; // "대화하기" 텍스트 나옴
                mouseText.enabled = true;

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
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    preObject = null;
                }
            }
        }
    }




    // 엘리베이터 버튼을 눌렀을 때
    IEnumerator ElevatorCoroutine()
    {
        elevatorBtn_current = hitObject;

        elevatorBtn_current.GetComponent<Collider>().enabled = false; // 콜라이더 비활성화
        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화

        elevatorBtn_current.GetComponent<NextElevatorPoint>().thisElevatorAnim.Play("Close");

        


        // 탈출 엘리베이터
        if(elevatorBtn_current.GetComponent<NextElevatorPoint>().goTo == "Exit")
        {
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(circusSong, false, 7f));
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(applause, false, 7f));

            yield return new WaitForSeconds(5f);

            // 아트갤러리로 복귀
            fadeInOutScript.FadeOut(fadeInOutImage);
            yield return new WaitForSeconds(2f);
            LoadSceneScript.SuccessLoadScene("02_ArtGallery");
        }
        else // 그 외 엘리베이터
        {
            elevator_Point_Player.transform.position = elevatorBtn_current.GetComponent<NextElevatorPoint>().thisPoint.transform.position; // elevator_Point_Player을 엘리베이터 포인트로 옮기고, 플레이어를 자식으로 넣어서
            elevator_Point_Player.transform.rotation = elevatorBtn_current.GetComponent<NextElevatorPoint>().thisPoint.transform.rotation; // n초 후에 다음 엘리베이터 포인트로 이동 후 자식 해제
            player.transform.SetParent(elevator_Point_Player.transform);

            yield return new WaitForSeconds(3f);

            elevator_Point_Player.transform.position = elevatorBtn_current.GetComponent<NextElevatorPoint>().nextPoint.transform.position;
            elevator_Point_Player.transform.rotation = elevatorBtn_current.GetComponent<NextElevatorPoint>().nextPoint.transform.rotation;

            // 서커스장 가는 엘리베이터
            if (elevatorBtn_current.GetComponent<NextElevatorPoint>().goTo == "Circus")
            {
                life = life_max;
                lifeText.text = life.ToString();
                StartCoroutine(AudioOnOffScript.VolumeCoroutine(circusSong, true, 15f, 0.4f));
            }

            // 이동 시간
            yield return new WaitForSeconds(6f);

            // 문 열림
            elevatorBtn_current.GetComponent<NextElevatorPoint>().next_elevator_ding.Play();
            yield return new WaitForSeconds(1f);
            elevatorBtn_current.GetComponent<NextElevatorPoint>().nextElevatorAnim.Play("Open");
            player.transform.SetParent(Armatures.transform);
        }
    }




    // 외발자전거를 눌렀을 때
    IEnumerator UnicycleCoroutine()
    {
        isRidingUnicycle = true;

        unicycle_current = hitObject;

        unicycle_current.GetComponent<Collider>().enabled = false; // 콜라이더 비활성화
        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화


        // 대화
        if (life == life_max)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[4]["Content"], // "도전에 대해 설명드리겠습니다"
                                                                   (string)GameManager.instance.textFileManager.dialog[5]["Content"], // "이 외발자전거로 외줄을 건너서 반대편 타워까지 가세요"
                                                                   (string)GameManager.instance.textFileManager.dialog[6]["Content"], // "자전거는 자동으로 앞으로 갑니다. 그러니 좌우로 균형만 잘 잡아주세요"
                                                                   (string)GameManager.instance.textFileManager.dialog[7]["Content"] }); // "만약 떨어진다면 다시 여기로 와 주세요"*/
        }

        if(life > 1)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[8]["Content"] + life + (string)GameManager.instance.textFileManager.dialog[9]["Content"] }); // "남은 도전 기회는 n번입니다"
            //putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[30]["Content"], "..." + life.ToString() + "..." }); // "......" "...n..."
        }
        else if(life == 1)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[15]["Content"] }); // "마지막 기회입니다"
            //putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[30]["Content"], "...1..."}); // "......" "...1..."
        }

        //putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[10]["Content"] }); // "저를 잘 따라오세요"
        
        yield return new WaitUntil(() => putDialogScript.isClickMode == false);
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        fadeInOutScript.FadeOut(fadeInOutImage, 1f);
        yield return new WaitForSeconds(2f);

        // 외발자전거 탑승
        player.transform.SetParent(unicycleSeat.transform);
        player.transform.position = unicycleSeat.transform.position;
        player.transform.rotation = unicycleSeat.transform.rotation;
        transform.localEulerAngles = Vector3.zero;

        // 외발자전거 컨트롤러 활성화
        player.GetComponent<UnicycleController>().enabled = true;
        player.GetComponent<UnicycleController>().lookSensitivity = player.GetComponent<PlayerController>().lookSensitivity;
        StartCoroutine(player.GetComponent<UnicycleController>().StandUpCoroutine()); // 앉아있는 상태일 때 일어나게 만들기

        // 위치 조정
        unicycleClown.GetComponent<GetComponentScript>().animator.Play("Go", 0, 0.02f);
        unicycleClown.GetComponent<GetComponentScript>().animator.speed = 0f;
        unicycle_current.GetComponent<GetComponentScript>().animator.Play("Go", 0, 0.01f);
        unicycle_current.GetComponent<GetComponentScript>().animator.speed = 0f;

        fadeInOutScript.FadeIn(fadeInOutImage, 1f);
        yield return new WaitForSeconds(2f);

        // 가짜 외줄 사라지고 진짜 외줄 나타남
        if(life == life_max)
        {
            yield return new WaitForSeconds(2f);
            //putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[11]["Content"], 3f); // "잠시만요"
            //yield return new WaitForSeconds(6f);
            rope.SetActive(true);
            rope_fake.SetActive(false);
            yield return new WaitForSeconds(1f);
            //putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[12]["Content"], 3f); // "너무 쉬울까봐 조금 수정했어요. 이제 출발합니다"
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[10]["Content"], 3f); // "저를 잘 따라오세요"
            yield return new WaitForSeconds(2f);
        }

        // 광대 출발
        unicycleClown.GetComponent<GetComponentScript>().animator.speed = 1f;
        unicycleClown.GetComponent<GetComponentScript>().animator.Play("WheelTurn", 1);
        bikeWheelClown.Play();

        // 플레이어 출발
        unicycle_current.GetComponent<GetComponentScript>().animator.speed = 1f;
        unicycle_current.GetComponent<GetComponentScript>().animator.Play("WheelTurn", 1);
        bikeWheel.Play();
        player.GetComponent<UnicycleController>().isBalancing = true;
    }

    // 외발자전거 멈추면 바퀴도 멈추기
    void UnicycleIdle()
    {
        if (unicycle_check.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            unicycle_check.GetComponent<GetComponentScript>().animator.Play("Idle", 1);
            bikeWheel.Stop();
        }

        if (unicycleClown.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            unicycleClown.GetComponent<GetComponentScript>().animator.Play("Idle", 1);
            bikeWheelClown.Stop();
        }
    }
}
    
































/*
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ClownRaycast : DefaultRaycast
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
    public GameObject Armatures;

    public ClownSceneManager clownSceneManager;
    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트

    // 엘리베이터
    public Animator[] elevatorAnims;
    [SerializeField]
    GameObject[] elevatorBtns_check; // 체크용
    GameObject elevatorBtn_current;
    [SerializeField]
    GameObject elevator_Point_Player;

    // AudioSource
    public AudioSource circusSong;
    public AudioSource applause;
    public AudioSource booing;
    public AudioSource yay;
    public AudioSource bikeWheel;
    public AudioSource bikeWheelClown;

    // 도전 기회
    public int life_max = 3;
    public int life;
    public TextMeshProUGUI lifeText;

    // 외줄타기 도전
    public GameObject unicycle_check; // 체크용
    public GameObject unicycle_current;
    public GameObject unicycleSeat;
    public GameObject successsPlatform;
    public bool isRidingUnicycle;
    public GameObject unicycleClown;
    public GameObject rope;
    public GameObject rope_fake;

    // 스위치
    public GameObject switch_check;


    void Start()
    {
        isRidingUnicycle = false;
        rope.SetActive(false);
        rope_fake.SetActive(true);

        // 처음에 열려있어야 하는 엘리베이터 문들
        elevatorAnims[0].Play("Open");
        elevatorAnims[1].Play("Open");

        life = life_max;
    }

    void Update()
    {
        ShootRaycast();
        UnicycleIdle();
    }

    // 카메라에서 레이캐스트 쏘기
    public void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f) && !clownSceneManager.isPausing && !putDialogScript.isClickMode)
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
            foreach (GameObject btn in elevatorBtns_check)
            {
                if (hitObject == btn)
                {
                    if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    }

                    preObject = hitObject;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                    // mouseText.text = GameManager.instance.textFileManager.ui[24]; // "대화하기" 텍스트 나옴
                    // mouseText.enabled = true;

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
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                        preObject = null;
                    }
                }
            }
        }

        // 외발자전거
        if (isChecking)
        {
            if (hitObject == unicycle_check)
            {
                if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                mouseText.text = GameManager.instance.textFileManager.ui[24]; // "대화하기" 텍스트 나옴
                mouseText.enabled = true;

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
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    preObject = null;
                }
            }
        }

        // 스위치
        if (isChecking)
        {
            if (hitObject == switch_check)
            {
                if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                mouseText.text = GameManager.instance.textFileManager.ui[26]; // "줍기" 텍스트 나옴
                mouseText.enabled = true;

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
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    preObject = null;
                }
            }
        }
    }




    // 엘리베이터 버튼을 눌렀을 때
    IEnumerator ElevatorCoroutine()
    {
        elevatorBtn_current = hitObject;

        elevatorBtn_current.GetComponent<Collider>().enabled = false; // 콜라이더 비활성화
        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화

        elevatorBtn_current.GetComponent<NextElevatorPoint>().thisElevatorAnim.Play("Close");




        // 탈출 엘리베이터
        if (elevatorBtn_current.GetComponent<NextElevatorPoint>().goTo == "Exit")
        {
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(circusSong, false, 7f));
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(applause, false, 7f));

            yield return new WaitForSeconds(5f);

            // 아트갤러리로 복귀
            fadeInOutScript.FadeOut(fadeInOutImage);
            yield return new WaitForSeconds(2f);
            LoadSceneScript.SuccessLoadScene("02_ArtGallery");
        }
        else // 그 외 엘리베이터
        {
            elevator_Point_Player.transform.position = elevatorBtn_current.GetComponent<NextElevatorPoint>().thisPoint.transform.position; // elevator_Point_Player을 엘리베이터 포인트로 옮기고, 플레이어를 자식으로 넣어서
            elevator_Point_Player.transform.rotation = elevatorBtn_current.GetComponent<NextElevatorPoint>().thisPoint.transform.rotation; // n초 후에 다음 엘리베이터 포인트로 이동 후 자식 해제
            player.transform.SetParent(elevator_Point_Player.transform);

            yield return new WaitForSeconds(3f);

            elevator_Point_Player.transform.position = elevatorBtn_current.GetComponent<NextElevatorPoint>().nextPoint.transform.position;
            elevator_Point_Player.transform.rotation = elevatorBtn_current.GetComponent<NextElevatorPoint>().nextPoint.transform.rotation;

            // 서커스장 가는 엘리베이터
            if (elevatorBtn_current.GetComponent<NextElevatorPoint>().goTo == "Circus")
            {
                life = life_max;
                lifeText.text = life.ToString();
                StartCoroutine(AudioOnOffScript.VolumeCoroutine(circusSong, true, 15f, 0.6f));
            }

            // 이동 시간
            yield return new WaitForSeconds(6f);

            // 문 열림
            elevatorBtn_current.GetComponent<NextElevatorPoint>().next_elevator_ding.Play();
            yield return new WaitForSeconds(1f);
            elevatorBtn_current.GetComponent<NextElevatorPoint>().nextElevatorAnim.Play("Open");
            player.transform.SetParent(Armatures.transform);
        }
    }




    // 외발자전거를 눌렀을 때
    IEnumerator UnicycleCoroutine()
    {
        isRidingUnicycle = true;

        unicycle_current = hitObject;

        unicycle_current.GetComponent<Collider>().enabled = false; // 콜라이더 비활성화
        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화


        // 대화
        if (life == life_max)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[4]["Content"], // "첫 번째 도전입니다"
                                                                   (string)GameManager.instance.textFileManager.dialog[5]["Content"], // "이 외발자전거로 외줄을 건너서 반대편 타워까지 가세요"
                                                                   (string)GameManager.instance.textFileManager.dialog[6]["Content"], // "자전거는 자동으로 앞으로 갑니다. 그러니 좌우로 균형만 잘 잡아주세요"
                                                                   (string)GameManager.instance.textFileManager.dialog[7]["Content"] }); // "만약 중간에 떨어진다면 다시 여기로 와 주세요"
        }

        if (life > 1)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[8]["Content"] + life + (string)GameManager.instance.textFileManager.dialog[9]["Content"] }); // "남은 도전 기회는 n번입니다"
        }
        else if (life == 1)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[15]["Content"] }); // "마지막 기회입니다. 이번에도 실패하면 관객들이 분노할 거예요"
        }

        putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[10]["Content"] }); // "제 뒤를 잘 따라오세요"

        yield return new WaitUntil(() => putDialogScript.isClickMode == false);
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        fadeInOutScript.FadeOut(fadeInOutImage, 1f);
        yield return new WaitForSeconds(2f);

        // 외발자전거 탑승
        player.transform.SetParent(unicycleSeat.transform);
        player.transform.position = unicycleSeat.transform.position;
        player.transform.rotation = unicycleSeat.transform.rotation;
        transform.localEulerAngles = Vector3.zero;

        // 외발자전거 컨트롤러 활성화
        player.GetComponent<UnicycleController>().enabled = true;
        player.GetComponent<UnicycleController>().lookSensitivity = player.GetComponent<PlayerController>().lookSensitivity;
        StartCoroutine(player.GetComponent<UnicycleController>().StandUpCoroutine()); // 앉아있는 상태일 때 일어나게 만들기

        // 위치 조정
        unicycleClown.GetComponent<GetComponentScript>().animator.Play("Go", 0, 0.02f);
        unicycleClown.GetComponent<GetComponentScript>().animator.speed = 0f;
        unicycle_current.GetComponent<GetComponentScript>().animator.Play("Go", 0, 0.01f);
        unicycle_current.GetComponent<GetComponentScript>().animator.speed = 0f;

        fadeInOutScript.FadeIn(fadeInOutImage, 1f);
        yield return new WaitForSeconds(2f);

        // 가짜 외줄 사라지고 진짜 외줄 나타남
        if (life == life_max)
        {
            yield return new WaitForSeconds(2f);
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[11]["Content"], 3f); // "잠시만요"
            yield return new WaitForSeconds(6f);
            rope.SetActive(true);
            rope_fake.SetActive(false);
            yield return new WaitForSeconds(3f);
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[12]["Content"], 3f); // "너무 쉬울까봐 조금 수정했어요. 이제 출발합니다"
            yield return new WaitForSeconds(5f);
        }

        // 광대 출발
        unicycleClown.GetComponent<GetComponentScript>().animator.speed = 1f;
        unicycleClown.GetComponent<GetComponentScript>().animator.Play("WheelTurn", 1);
        bikeWheelClown.Play();

        // 플레이어 출발
        unicycle_current.GetComponent<GetComponentScript>().animator.speed = 1f;
        unicycle_current.GetComponent<GetComponentScript>().animator.Play("WheelTurn", 1);
        bikeWheel.Play();
        player.GetComponent<UnicycleController>().isBalancing = true;
    }

    // 외발자전거 멈추면 바퀴도 멈추기
    void UnicycleIdle()
    {
        if (unicycle_check.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            unicycle_check.GetComponent<GetComponentScript>().animator.Play("Idle", 1);
            bikeWheel.Stop();
        }

        if (unicycleClown.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            unicycleClown.GetComponent<GetComponentScript>().animator.Play("Idle", 1);
            bikeWheelClown.Stop();
        }
    }
}

*/