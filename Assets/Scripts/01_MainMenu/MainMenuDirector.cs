using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenuDirector : MonoBehaviour
{
    public MainMenuSceneManager mainmenuSceneManager;
    public GameObject mainCam;
    public GameObject directorCam;

    public RawImage fadeInOutImage; // ���̵�-��, �ƿ� �̹���
    public FadeInOutScript fadeInOutScript; // ���̵�-��, �ƿ� ��ũ��Ʈ

    


    // ������ ���� �ƽ�
    public Animator doorAnimator;
    public PlayableDirector enterGalleryDirector;

    public void EnterGallery()
    {
        StartCoroutine(EnterGalleryCoroutine());
    }
    
    IEnumerator EnterGalleryCoroutine()
    {
        Cursor.visible = false;

        enterGalleryDirector.Play();

        yield return new WaitForSeconds(3f);

        LoadSceneScript.LoadScene("02_ArtGallery");
    }
}
