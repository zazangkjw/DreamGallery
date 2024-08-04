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

    public RawImage fadeInOutImage; // ���̵�-��, �ƿ� �̹���
    public FadeInOutScript fadeInOutScript; // ���̵�-��, �ƿ� ��ũ��Ʈ

    // public Letterbox letterbox; // ���� ��ũ��Ʈ

    public GameObject[] vrCamPos; // VR ��� �� vr ī�޶� ������ �迭
    public GameObject[] vrPlayerPos; // VR ��� �� �÷��̾� ������ �迭

    public static bool isFromDream; // �޿��� �ٽ� ��Ʈ�������� ���� ������
    public enum Dreams // �� ��ȣ�� �� �̸�
    {
        LionDance,
        Clown
    }
    public static Dreams selectedDream;




    void Start()
    {
        if (isFromDream)
        {
            ExitVR(); // �޿��� ���Դٸ�, VR���� ������ �ƽ� ����
            isFromDream = false;
        }
        else
        {
            fadeInOutScript.FadeOut(fadeInOutImage);
        }
    }


    

    // VR ��� ���� �ƽ�
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




    // VR ��� ������ �ƽ�
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
