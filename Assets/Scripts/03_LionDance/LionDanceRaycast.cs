using TMPro;
using UnityEngine;

public class LionDanceRaycast : MonoBehaviour
{
    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;

    public GameObject[] doorAndWindow; // 문, 창문 (본)오브젝트 배열 자동 생성
    public GameObject balconyWindow; // 발코니 창문 오브젝트
    public GameObject balconyDoor; // 발코니 문 오브젝트
    public GameObject kitchenBalconyWindow1; // 부엌 발코니 창문 오브젝트1
    public GameObject kitchenBalconyWindow2; // 부엌 발코니 창문 오브젝트2
    int openCount;

    public Animator lionMonsterAnimator;
    public LionDanceSceneManager lionDanceSceneManager;
    public LionDanceColliderTrigger lionDanceColliderTrigger;
    public PlayerController playerController;
    public Camera playerCam;
    public GameObject player;
    public Material fogMat;

    void Start()
    {
        openCount = 0;
    }

    void Update()
    {
        ShootRaycast();
    }

    // 카메라에서 레이캐스트 쏘기
    public void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 1.5f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.5f) && !lionDanceSceneManager.isPausing && !putDialogScript.isClickMode)
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
        // step 0일 때. 문, 창문 다 열기
        if (lionDanceColliderTrigger.step == 0)
        {
            foreach(GameObject doorWindow in doorAndWindow)
            {
                if (hitObject == doorWindow)
                {
                    if (preObject != hitObject.GetComponent<GetComponentScript>().mesh && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                    {
                        preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                        preObject = null;
                    }

                    preObject = hitObject.GetComponent<GetComponentScript>().mesh;
                    preObject.GetComponent<Outline>().enabled = true; // 외곽선 켜기

                    mouseText.text = GameManager.instance.textFileManager.ui[14]; // "열기/닫기" 텍스트 나옴
                    mouseText.enabled = true;

                    // E키 입력 시
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // 문, 창문 열기
                        if (!hitObject.GetComponent<GetComponentScript>().animator.GetBool("Active")) // 닫혀있으면 열기
                        {
                            hitObject.GetComponent<GetComponentScript>().animator.SetBool("Active", true);
                            openCount++;
                        }
                        else // 열려있으면 닫기
                        {
                            hitObject.GetComponent<GetComponentScript>().animator.SetBool("Active", false);
                            openCount--;
                        }
                        hitObject.GetComponent<AudioSource>().Play();

                        // 문, 창문 다 열렸는지 체크하고 다음 스텝으로
                        if (openCount == 10)
                        {
                            lionMonsterAnimator.Play("Walking", 1);
                            lionDanceColliderTrigger.step = 1;
                            mouseText.enabled = false;
                            preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                            preObject = null;
                            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[2]["Content"], 3f); // 부엌으로 돌아가자는 대사 출력
                            lionDanceSceneManager.FogOut();
                        }
                    }
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

        // step 3일 때. 발코니 창문 닫기
        else if (lionDanceColliderTrigger.step == 3)
        {
            if (hitObject == balconyWindow)
            {
                if (preObject != hitObject.GetComponent<GetComponentScript>().mesh && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                    preObject = null;
                }

                preObject = hitObject.GetComponent<GetComponentScript>().mesh;
                preObject.GetComponent<Outline>().enabled = true; // 외곽선 켜기

                mouseText.text = GameManager.instance.textFileManager.ui[14]; // "열기/닫기" 텍스트 나옴
                mouseText.enabled = true;

                // E키 입력 시
                if (Input.GetKeyDown(KeyCode.E))
                {
                    balconyWindow.GetComponent<GetComponentScript>().animator.SetBool("Active", false);
                    balconyWindow.GetComponent<AudioSource>().Play();
                    mouseText.enabled = false;
                    preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                    preObject = null;
                    lionMonsterAnimator.Play("Balcony");
                    lionDanceColliderTrigger.step = 4;

                    Debug.Log("발코니 창문 닫음");
                }
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

        // step 9일 때. 부엌 발코니 창문 닫기
        else if (lionDanceColliderTrigger.step == 9)
        {
            if (hitObject == kitchenBalconyWindow1 || hitObject == kitchenBalconyWindow2)
            {
                if (preObject != hitObject.GetComponent<GetComponentScript>().mesh && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                    preObject = null;
                }

                preObject = hitObject.GetComponent<GetComponentScript>().mesh;
                preObject.GetComponent<Outline>().enabled = true; // 외곽선 켜기

                mouseText.text = GameManager.instance.textFileManager.ui[14]; // "열기/닫기" 텍스트 나옴
                mouseText.enabled = true;

                // E키 입력 시
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hitObject.GetComponent<GetComponentScript>().animator.SetBool("Active", false);
                    hitObject.GetComponent<AudioSource>().Play();
                    lionDanceColliderTrigger.step = 10;
                    mouseText.enabled = false;
                    preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                    preObject = null;

                    Debug.Log("부엌 창문 닫음");

                    lionMonsterAnimator.Play("ClimbDown"); // 괴물이 부엌 창문 밖에서 아래층으로 내려가는 애니메이션 재생
                    Debug.Log("괴물이 부엌 창문 밖에서 아래층으로 내려가는 애니메이션 재생");

                    StartCoroutine(lionDanceColliderTrigger.SurviveTimerCoroutine(5f));
                }
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
}
