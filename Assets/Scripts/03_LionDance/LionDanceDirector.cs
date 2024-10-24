using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LionDanceDirector : MonoBehaviour
{
    public LionDanceSceneManager lionDanceSceneManager;
    public LionDanceColliderTrigger lionDanceColliderTrigger;
    public Animator lionMonsterAnimator;
    public GameObject player;
    public GameObject directorCam;
    public TextMeshProUGUI mouseText;
    public GameObject crosshair;
    public PutDialogScript putDialogScript;

    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트

    // public Letterbox letterbox; // 레터박스 스크립트






    // 괴물이 윗층으로 올라가는 컷신
    public PlayableDirector lookMonsterDirector;
    public GameObject lookMonsterCam1;
    public GameObject lookMonsterCam2;
    public GameObject lookMonsterCam3;
    public AudioSource screaming;
    public AudioSource horrorSound;

    public void LookMonsterDirector()
    {
        StartCoroutine(LookMonsterDirectorCoroutine());
    }

    IEnumerator LookMonsterDirectorCoroutine()
    {
        //player.SetActive(false);
        //directorCam.SetActive(true);
        //mouseText.enabled = false;

        //lookMonsterDirector.Play();
        lionMonsterAnimator.Play("ClimbUp"); // 괴물이 윗층으로 올라가는 애니메이션 재생
        horrorSound.Play();

        yield return new WaitForSeconds(2.5f);

        //player.SetActive(true);
        //directorCam.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        screaming.Play(); // 윗층 비명소리 재생

        yield return new WaitForSeconds(3f);

        putDialogScript.putDialog((string)GameManager.instance.textFileManager.dialog[3]["Content"], 5f); // "밖에 무언가 있어... 바깥 창문을 닫아야 해"
        StartCoroutine(lionDanceColliderTrigger.BalconyTimerCoroutine()); // 대사 끝나고 3초 뒤에 발코니로 괴물 침입
    }




    // 아트갤러리로 복귀

    public void BackToArtGallery()
    {
        StartCoroutine(BackToArtGalleryCoroutine());
    }

    public IEnumerator BackToArtGalleryCoroutine()
    {
        fadeInOutScript.FadeOut(fadeInOutImage);
        yield return new WaitForSeconds(2f);
        LoadSceneScript.SuccessLoadScene("02_ArtGallery");
    }
}
