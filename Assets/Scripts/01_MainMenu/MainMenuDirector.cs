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

    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트

    


    // 갤러리 입장 컷신
    public Animator doorAnimator;
    public PlayableDirector enterGalleryDirector;

    public void EnterGallery()
    {
        StartCoroutine(EnterGalleryCoroutine());
    }
    
    IEnumerator EnterGalleryCoroutine()
    {
        Cursor.visible = false;
        mainCam.SetActive(false);
        directorCam.SetActive(true);

        doorAnimator.SetBool("Active", true);
        enterGalleryDirector.Play();

        // yield return new WaitForSeconds(0.5f);

        fadeInOutScript.FadeOut(fadeInOutImage);

        yield return new WaitForSeconds(2.5f);

        LoadSceneScript.LoadScene("02_ArtGallery");
    }
}
