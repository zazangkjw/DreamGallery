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

    public RawImage fadeInOutImage; // ���̵�-��, �ƿ� �̹���
    public FadeInOutScript fadeInOutScript; // ���̵�-��, �ƿ� ��ũ��Ʈ



    // ������ �ƽ�
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

        // ������ ���
        putDialogScript.putDialogWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[0]["Content"],
                                                                                (string)GameManager.instance.textFileManager.dialog[1]["Content"]});
    }




    // ������ �������� �ö󰡴� �ƽ�
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
        lionMonsterAnimator.Play("ClimbUp"); // ������ �������� �ö󰡴� �ִϸ��̼� ���
        heartbeat.Play(); // ��� ����Ҹ� ���� ���

        yield return new WaitForSeconds(2.5f);

        player.SetActive(true);
        directorCam.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        screaming.Play(); // ���� ���Ҹ� ���
    }




    // ��Ʈ�������� ����
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
