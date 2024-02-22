using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArtGalleryRaycast : MonoBehaviour
{
    public RaycastHit hitInfo;
    public GameObject hitObject;

    public Text mouseText;
    public PutDialogScript putDialogScript;

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
        Debug.DrawRay(transform.position, transform.forward * 1.5f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.5f) && !artGallerySceneManager.isPausing && !putDialogScript.isClickMode)
        {
            hitObject = hitInfo.collider.gameObject;
            CheckDreamObject();
        }

        // 허공일 때
        else
        {
            mouseText.text = ""; // 텍스트 없어짐
        }
    }

    // 꿈 오브젝트 체크 메소드
    private void CheckDreamObject()
    {
        if (hitObject == dreamObjects[0])
        {
            mouseText.text = GameManager.instance.textFileManager.ui[15]; // "꿈 속으로 들어가기" 텍스트 나옴
            mouseText.enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                ArtGalleryDirector.selectedDream = 0;
                artGalleryDirector.LookVR();
            }
        }

        // 허공일 때
        else
        {
            mouseText.enabled = false; // 텍스트 없어짐
        }
    }
}
