using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipRaycast : MonoBehaviour
{
    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;
    bool isChecking = true;

    public DefaultSceneManager defaultSceneManager;
    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트

    // AudioSource
    // public AudioSource circusSong;

    // man001(팔짱 낀 사람)
    public GameObject man001_check; // 체크용




    void Start()
    {
        
    }

    void Update()
    {
        ShootRaycast();
    }

    // 카메라에서 레이캐스트 쏘기
    public void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f) && !defaultSceneManager.isPausing && !putDialogScript.isClickMode)
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

        // man001
        if (isChecking)
        {
            if (hitObject == man001_check)
            {
                if (preObject != hitObject.GetComponent<GetComponentScript>().mesh && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                    preObject = null;
                }

                preObject = hitObject.GetComponent<GetComponentScript>().mesh;
                preObject.GetComponent<Outline>().enabled = true; // 외곽선 켜기

                mouseText.text = GameManager.instance.textFileManager.ui[24]; // "대화하기" 텍스트 나옴
                mouseText.enabled = true;

                // E키 입력 시
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(man001Coroutine());
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




    // man001 대화
    IEnumerator man001Coroutine()
    {
        // 필요하면 "GameObject man001_this = hitObject" 생성해서 사용

        preObject.GetComponent<Outline>().enabled = false; // 외곽선 비활성화
        man001_check.GetComponent<Man001HeadTracking>().isLooking = true;

        // 대화
        if (true)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[4]["Content"], // "첫 번째 도전입니다"
                                                                   (string)GameManager.instance.textFileManager.dialog[5]["Content"], // "이 외발자전거로 외줄을 건너서 반대편 타워까지 가세요"
                                                                   (string)GameManager.instance.textFileManager.dialog[6]["Content"], // "자전거는 자동으로 앞으로 갑니다. 그러니 좌우로 균형만 잘 잡아주세요"
                                                                   (string)GameManager.instance.textFileManager.dialog[7]["Content"] }); // "만약 중간에 떨어진다면 다시 여기로 와 주세요"
        }

        yield return new WaitUntil(() => putDialogScript.isClickMode == false);

        man001_check.GetComponent<Man001HeadTracking>().isLooking = false;
    }
}
