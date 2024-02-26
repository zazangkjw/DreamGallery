using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenuDirector : MonoBehaviour
{
    public MainMenuSceneManager mainmenuSceneManager;

    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트




    // 갤러리 입장 컷신
    public Animator doorAnimator;

    public void EnterGallery()
    {
        StartCoroutine(EnterGalleryCoroutine());
    }
    
    IEnumerator EnterGalleryCoroutine()
    {
        doorAnimator.SetBool("Active", true);
        fadeInOutScript.FadeIn(fadeInOutImage);
        yield return new WaitForSeconds(2f);
        LoadSceneScript.LoadScene("02_ArtGallery");
    }
}
