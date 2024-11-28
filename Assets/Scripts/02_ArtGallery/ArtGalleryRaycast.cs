using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArtGalleryRaycast : MonoBehaviour
{
    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;
    bool isChecking = true;

    public ArtGallerySceneManager artGallerySceneManager;
    public ArtGalleryDirector artGalleryDirector;

    public GameObject[] dreamObjects; // 꿈 속으로 들어가는 물건들

    public GameObject deskMan_check; // 안내데스크 npc 체크용
    public GameObject informationUI;
    public static bool is_pop_up;

    public PlayerController playerController;

    void Start()
    {

    }

    void Update()
    {
        ShootRaycast();
    }

    // 카메라에서 레이캐스트 쏘기
    private void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f) && !DefaultSceneManager.isPausing && !putDialogScript.isClickMode)
        {
            hitObject = hitInfo.collider.gameObject;
        }
        else
        {
            hitObject = null;
        }

        CheckObject();
    }

    // 레이캐스트 오브젝트 체크 메소드
    private void CheckObject()
    {
        isChecking = true;

        // 꿈 오브젝트
        if (isChecking)
        {
            foreach (GameObject col in dreamObjects)
            {
                if (hitObject == col) // 꿈 오브젝트일 떄
                {
                    if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    }

                    preObject = hitObject;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                    mouseText.text = GameManager.instance.textFileManager.ui[15]; // "꿈 속으로 들어가기" 텍스트 나옴
                    mouseText.enabled = true;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(SelectDreamCoroutine());
                    }

                    isChecking = false; // 이후의 항목들은 체크하지 않음

                    break;
                }
                else
                {
                    mouseText.enabled = false;

                    if (preObject != null)
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                        preObject = null;
                    }
                }
            }
        }

        // deskMan
        if (isChecking)
        {
            if (hitObject == deskMan_check) // 안내데스크 npc일 떄
            {
                if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                mouseText.text = GameManager.instance.textFileManager.ui[24]; // "대화하기" 텍스트 나옴
                mouseText.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(DeskManCoroutine());
                }

                isChecking = false; // 이후의 항목들은 체크하지 않음
            }
            else
            {
                mouseText.enabled = false;

                if (preObject != null)
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    preObject = null;
                }
            }
        }
    }

    // 꿈 선택
    IEnumerator SelectDreamCoroutine()
    {
        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기

        // 무슨 꿈을 선택했는지
        if (hitObject == dreamObjects[0])
        {
            ArtGalleryDirector.selectedDream = ArtGalleryDirector.Dreams.LionDance;
        }
        else if (hitObject == dreamObjects[1])
        {
            ArtGalleryDirector.selectedDream = ArtGalleryDirector.Dreams.Clown;
        }
        else if (hitObject == dreamObjects[2])
        {
            ArtGalleryDirector.selectedDream = ArtGalleryDirector.Dreams.Spaceship;
        }

        artGalleryDirector.LookVR();

        yield return null;
    }

    // 안내데스크 npc 대화
    IEnumerator DeskManCoroutine()
    {
        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기

        // 대화
        /*putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[27]["Content"], // "'꿈 전시관'에 오신 것을 환영합니다"
                                                               (string)GameManager.instance.textFileManager.dialog[28]["Content"], // "이곳에는 꿈과 관련된 다양한 물건들이 전시되어 있으며, 각 전시물 앞에는 VR 기기가 배치되어 있습니다"
                                                               (string)GameManager.instance.textFileManager.dialog[29]["Content"]}); // "VR 기기를 바라보고 '상호작용' 키(E)를 눌러 꿈을 체험하실 수 있습니다"*/

        informationUI.SetActive(true);
        playerController.enabled = false;
        playerController.myRigid.isKinematic = true;
        is_pop_up = true;
        Cursor.visible = true; // 마우스 커서 켜기

        yield return null;
    }

    public void PopUpControl(bool b)
    {
        is_pop_up = b;
    }

    public void CursorVisible(bool b)
    {
        Cursor.visible = b;
    }
}
