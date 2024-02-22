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

    public RawImage fadeInOutImage; // ���̵�-��, �ƿ� �̹���
    public FadeInOutScript fadeInOutScript; // ���̵�-��, �ƿ� ��ũ��Ʈ

    public static bool isFromDream; // �޿��� �ٽ� ��Ʈ�������� ���� ������
    public static int selectedDream; // ���õ� ��
    // �� ��ȣ�� �� �̸�
    // 0: LionDance




    void Start()
    {
        if (isFromDream)
        {
            // �޿��� ���Դٸ�, VR���� ������ �ƽ� ����
            isFromDream = false;
        }
    }


    

    // VR ��� ���� �ƽ�
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
