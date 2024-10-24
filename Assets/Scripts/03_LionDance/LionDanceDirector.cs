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

    public RawImage fadeInOutImage; // ���̵�-��, �ƿ� �̹���
    public FadeInOutScript fadeInOutScript; // ���̵�-��, �ƿ� ��ũ��Ʈ

    // public Letterbox letterbox; // ���͹ڽ� ��ũ��Ʈ






    // ������ �������� �ö󰡴� �ƽ�
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
        lionMonsterAnimator.Play("ClimbUp"); // ������ �������� �ö󰡴� �ִϸ��̼� ���
        horrorSound.Play();

        yield return new WaitForSeconds(2.5f);

        //player.SetActive(true);
        //directorCam.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        screaming.Play(); // ���� ���Ҹ� ���

        yield return new WaitForSeconds(3f);

        putDialogScript.putDialog((string)GameManager.instance.textFileManager.dialog[3]["Content"], 5f); // "�ۿ� ���� �־�... �ٱ� â���� �ݾƾ� ��"
        StartCoroutine(lionDanceColliderTrigger.BalconyTimerCoroutine()); // ��� ������ 3�� �ڿ� ���ڴϷ� ���� ħ��
    }




    // ��Ʈ�������� ����

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
