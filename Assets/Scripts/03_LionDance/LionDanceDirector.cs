using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class LionDanceDirector : MonoBehaviour
{
    public LionDanceSceneManager lionDanceSceneManager;
    public Animator lionMonsterAnimator;
    public GameObject player;
    public GameObject directorCam;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;

    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트



    // 오프닝 컷신
    public PlayableDirector openingDirector;
    public AudioSource frying;

    public void OpeningDirector()
    {
        StartCoroutine(OpeningDirectorCoroutine());
    }

    IEnumerator OpeningDirectorCoroutine()
    {
        player.SetActive(false);
        directorCam.SetActive(true);
        mouseText.enabled = false;

        fadeInOutImage.color = new Color(0f, 0f, 0f, 1f);
        fadeInOutScript.FadeOut(fadeInOutImage);
        openingDirector.Play();
        frying.Play();

        yield return new WaitForSeconds(8f);

        player.SetActive(true);
        directorCam.SetActive(false);

        // 오프닝 대사
        putDialogScript.putDialogWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[0]["Content"],
                                                                                (string)GameManager.instance.textFileManager.dialog[1]["Content"]});
    }




    // 괴물이 윗층으로 올라가는 컷신
    public PlayableDirector lookMonsterDirector;
    public GameObject lookMonsterCam1;
    public GameObject lookMonsterCam2;
    public GameObject lookMonsterCam3;
    public AudioSource heartbeat;
    public AudioSource screaming;

    public void LookMonsterDirector()
    {
        StartCoroutine(LookMonsterDirectorCoroutine());
    }

    IEnumerator LookMonsterDirectorCoroutine()
    {
        player.SetActive(false);
        directorCam.SetActive(true);
        mouseText.enabled = false;

        lookMonsterDirector.Play();
        lionMonsterAnimator.Play("ClimbUp"); // 괴물이 윗층으로 올라가는 애니메이션 재생
        heartbeat.Play(); // 배경 심장소리 음악 재생

        yield return new WaitForSeconds(2.5f);

        player.SetActive(true);
        directorCam.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        screaming.Play(); // 윗층 비명소리 재생
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
        LoadSceneScript.LoadScene("02_ArtGallery");
    }
}
