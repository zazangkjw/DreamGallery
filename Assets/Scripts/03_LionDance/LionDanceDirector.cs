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
    public PutDialogScript putDialogScript;

    public RawImage fadeInOutImage; // ���̵�-��, �ƿ� �̹���
    public FadeInOutScript fadeInOutScript; // ���̵�-��, �ƿ� ��ũ��Ʈ

    // public Letterbox letterbox; // ���͹ڽ� ��ũ��Ʈ




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

        yield return new WaitForSeconds(2f);

        // letterbox.LetterboxOnImmediately();
        fadeInOutScript.FadeOut(fadeInOutImage);
        openingDirector.Play();
        frying.Play();

        yield return new WaitForSeconds(12f);

        GameManager.instance.urpRenderer.rendererFeatures[0].SetActive(true); // SSAO �ѱ�

        directorCam.SetActive(false);
        player.SetActive(true);

        // ������ ���
        putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[0]["Content"],
                                                               (string)GameManager.instance.textFileManager.dialog[1]["Content"]});

        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            if (!putDialogScript.isClickMode)
            {
                // letterbox.LetterboxOff();
                break;
            }
            yield return new WaitForSeconds(0.016f);
        }
    }




    // ������ �������� �ö󰡴� �ƽ�
    public PlayableDirector lookMonsterDirector;
    public GameObject lookMonsterCam1;
    public GameObject lookMonsterCam2;
    public GameObject lookMonsterCam3;
    public AudioSource screaming;

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
        lionMonsterAnimator.Play("ClimbUp"); // ������ �������� �ö󰡴� �ִϸ��̼� ���

        yield return new WaitForSeconds(2.5f);

        //player.SetActive(true);
        //directorCam.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        screaming.Play(); // ���� ���Ҹ� ���

        yield return new WaitForSeconds(3f);

        putDialogScript.putDialog((string)GameManager.instance.textFileManager.dialog[3]["Content"], 3f);
        StartCoroutine(lionDanceColliderTrigger.BalconyTimerCoroutine()); // ��� ������ 5�� �ڿ� ���ڴϷ� ���� ħ��
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
        LoadSceneScript.SuccessLoadScene("02_ArtGallery");
    }
}
