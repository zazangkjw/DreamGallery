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

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f) && !artGallerySceneManager.isPausing && !putDialogScript.isClickMode)
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
        { }
            foreach (GameObject col in dreamObjects)
        {
            if (hitObject == col) // 꿈 오브젝트일 떄
            {
                if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                    preObject = null;
                }

                preObject = hitObject;
                preObject.GetComponent<Outline>().enabled = true; // 외곽선 켜기

                mouseText.text = GameManager.instance.textFileManager.ui[15]; // "꿈 속으로 들어가기" 텍스트 나옴
                mouseText.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                    preObject = null;

                    // 무슨 꿈을 선택했는지
                    if (hitObject == dreamObjects[0])
                    {
                        ArtGalleryDirector.selectedDream = ArtGalleryDirector.Dreams.LionDance;
                    }
                    else if(hitObject == dreamObjects[1])
                    {
                        ArtGalleryDirector.selectedDream = ArtGalleryDirector.Dreams.Clown;
                    }

                    artGalleryDirector.LookVR();
                }

                isChecking = false; // 이후의 항목들은 체크하지 않음

                break;
            }
            else
            {
                mouseText.enabled = false;

                if (preObject != null)
                {
                    preObject.GetComponent<Outline>().enabled = false; // 외곽선 끄기
                    preObject = null;
                }
            }
        }
    }
}
