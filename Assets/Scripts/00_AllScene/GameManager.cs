using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 싱글톤 //
    // instance라는 변수를 static으로 선언을 하여 다른 오브젝트 안의 스크립트에서도 instance를 불러올 수 있게 합니다 

    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때
        {
            instance = this; // 내자신을 instance로 넣어줍니다.
            DontDestroyOnLoad(gameObject); // OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지
        }
        else
        {
            if (instance != this) // instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
            {
                Destroy(this.gameObject); // 둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제
            }
        }
    }
    // 싱글톤 //

    // GamaManager 오브젝트에 있는 다른 스크립트들
    public FPS_Limit fps_Limit;
    public SaveManager saveManager;
    public TextFileManager textFileManager;

    // GamaManager 오브젝트에 있는 객체들
    public UniversalRendererData urpRenderer; // SSAO 컨트롤용 렌더러
}