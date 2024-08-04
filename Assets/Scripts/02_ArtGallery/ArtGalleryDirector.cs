using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class ArtGalleryDirector : MonoBehaviour
{
    public ArtGallerySceneManager artGallerySceneManager;
    public GameObject player;
    public GameObject playerCam;
    public GameObject directorCam;
    public TextMeshProUGUI mouseText;
    public RawImage crosshair;
    public PutDialogScript putDialogScript;

    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트

    // public Letterbox letterbox; // 블랙바 스크립트

    public GameObject[] vrCamPos; // VR 기기 앞 vr 카메라 포지션 배열
    public GameObject[] vrPlayerPos; // VR 기기 앞 플레이어 포지션 배열

    public static bool isFromDream; // 꿈에서 다시 아트갤러리로 나온 것인지
    public enum Dreams // 꿈 번호와 씬 이름
    {
        LionDance,
        Clown
    }
    public static Dreams selectedDream;




    void Start()
    {
        if (isFromDream)
        {
            ExitVR(); // 꿈에서 나왔다면, VR에서 나오는 컷신 실행
            isFromDream = false;
        }
        else
        {
            fadeInOutScript.FadeOut(fadeInOutImage);
        }
    }


    

    // VR 기기 보는 컷신
    public PlayableDirector LookVRDirector;
    public GameObject lookVRCam;

    public void LookVR()
    {
        StartCoroutine(LookVRCoroutine());
    }

    IEnumerator LookVRCoroutine()
    {
        player.SetActive(false);
        directorCam.SetActive(true);
        mouseText.enabled = false;
        crosshair.enabled = false;

        lookVRCam.transform.position = vrCamPos[(int)selectedDream].transform.position;
        lookVRCam.transform.rotation = vrCamPos[(int)selectedDream].transform.rotation;

        LookVRDirector.Play();
        yield return new WaitForSeconds(0.5f);
        fadeInOutScript.FadeIn(fadeInOutImage);
        yield return new WaitForSeconds(2f);

        isFromDream = true;

        switch (selectedDream)
        {
            case Dreams.LionDance:
                LoadSceneScript.LoadScene("03_LionDance");
                break;
            case Dreams.Clown:
                LoadSceneScript.LoadScene("04_Clown");
                break;
            default:
                break;
        }
    }




    // VR 기기 나오는 컷신
    public PlayableDirector ExitVRDirector;
    public GameObject exitVRCam;

    public void ExitVR()
    {
        StartCoroutine(ExitVRCoroutine());
    }

    IEnumerator ExitVRCoroutine()
    {
        player.SetActive(false);
        directorCam.SetActive(true);
        mouseText.enabled = false;
        crosshair.enabled = false;

        exitVRCam.transform.position = vrCamPos[(int)selectedDream].transform.position;
        exitVRCam.transform.rotation = vrCamPos[(int)selectedDream].transform.rotation;
        player.transform.position = vrPlayerPos[(int)selectedDream].transform.position;
        player.transform.rotation = vrPlayerPos[(int)selectedDream].transform.rotation;

        ExitVRDirector.Play();
        fadeInOutScript.FadeOut(fadeInOutImage);
        yield return new WaitForSeconds(2.5f);

        player.SetActive(true);
        directorCam.SetActive(false);
        crosshair.enabled = true;
    }
}
