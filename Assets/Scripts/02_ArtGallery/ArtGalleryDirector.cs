using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class ArtGalleryDirector : MonoBehaviour
{
    public ArtGallerySceneManager artGallerySceneManager;
    public GameObject player;
    public GameObject playerCam;
    public GameObject directorCam;
    public Text mouseText;
    public PutDialogScript putDialogScript;

    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트

    public static bool isFromDream; // 꿈에서 다시 아트갤러리로 나온 것인지
    public static int selectedDream; // 선택된 꿈
    // 꿈 번호와 씬 이름
    // 0: LionDance




    void Start()
    {
        if (isFromDream)
        {
            // 꿈에서 나왔다면, VR에서 나오는 컷신 실행
            isFromDream = false;
        }
    }


    

    // VR 기기 보는 컷신
    public PlayableDirector LookVRDirector;
    public GameObject lookVRCam;
    public GameObject[] vrCamPos;

    public void LookVR()
    {
        StartCoroutine(LookVRCoroutine());
    }

    IEnumerator LookVRCoroutine()
    {
        player.SetActive(false);
        directorCam.SetActive(true);
        mouseText.enabled = false;

        lookVRCam.transform.position = vrCamPos[selectedDream].transform.position;
        lookVRCam.transform.rotation = vrCamPos[selectedDream].transform.rotation;

        LookVRDirector.Play();
        yield return new WaitForSeconds(0.5f);
        fadeInOutScript.FadeIn(fadeInOutImage);
        yield return new WaitForSeconds(2f);

        switch (selectedDream)
        {
            case 0:
                LoadSceneScript.LoadScene("03_LionDance");
                break;

            default:
                break;
        }
    }
}
