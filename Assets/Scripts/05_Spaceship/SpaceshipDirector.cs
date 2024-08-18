using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SpaceshipDirector : MonoBehaviour
{
    public GameObject player;
    public GameObject directorCam;
    public TextMeshProUGUI mouseText;
    public RawImage crosshair;
    public PutDialogScript putDialogScript;

    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트




    // 오프닝 컷신
    public PlayableDirector openingDirector;
    public AudioSource vibration;
    public AudioSource piano2;
    public Man001HeadTracking man001HeadTracking;
    public Animator elevatorAnim;

    public void OpeningDirector()
    {
        StartCoroutine(OpeningDirectorCoroutine());
    }

    IEnumerator OpeningDirectorCoroutine()
    {
        player.SetActive(false);
        directorCam.SetActive(true);
        mouseText.enabled = false;
        crosshair.enabled = false;
        fadeInOutImage.color = new Color(0f, 0f, 0f, 1f);

        yield return new WaitForSeconds(2f);

        openingDirector.Play();
        fadeInOutScript.FadeOut(fadeInOutImage);

        yield return new WaitForSeconds(41f);

        piano2.Play();

        yield return new WaitForSeconds(13f);

        putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[18]["Content"], 10f);

        yield return new WaitForSeconds(9f);

        fadeInOutScript.FadeIn(fadeInOutImage);

        yield return new WaitForSeconds(4f);

        fadeInOutScript.FadeOut(fadeInOutImage);
        man001HeadTracking.isLooking = true;

        directorCam.SetActive(false);
        player.SetActive(true);
        crosshair.enabled = true;

        yield return new WaitForSeconds(5f);

        elevatorAnim.Play("Open");
    }




    // 아트갤러리로 복귀

    public void BackToArtGallery()
    {
        StartCoroutine(BackToArtGalleryCoroutine());
    }

    public IEnumerator BackToArtGalleryCoroutine()
    {
        fadeInOutScript.FadeIn(fadeInOutImage);
        yield return new WaitForSeconds(2f);
        LoadSceneScript.SuccessLoadScene("02_ArtGallery");
    }
}
