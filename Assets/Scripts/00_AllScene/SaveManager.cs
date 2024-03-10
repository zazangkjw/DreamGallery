using System;
using System.IO;
using UnityEngine;

[Serializable] // 객체를 바이트 형태로 변환하는 직렬화를 할 수 있게 함. 통신할 때는 문자열로 직렬화하여 주고 받는 것이 안전. 직렬화 해야 json으로 바꿀 수 있는 듯
public struct SettingData
{
    public int language; // 언어
    public float mouseSens; // 마우스 감도
    public int resolution; // 해상도 설정 값
    public int width; // 화면 너비
    public int height; // 화면 높이
    public bool isFullScreen; // 화면 모드
    public float volume; // 사운드 크기
}

public class SaveManager : MonoBehaviour
{
    void Start()
    {
        // 처음 게임 켰을 때 언어는 한글, 기본 감도가 0.5f이고, 설정 파일이 존재하면 받아오기
        settingData.language = 0;
        settingData.mouseSens = 5f;
        settingData.resolution = 0;
        settingData.width = 1920;
        settingData.height = 1080;
        settingData.isFullScreen = true;
        settingData.volume = 0f;
        ReloadSettingData();
        GameManager.instance.textFileManager.Reload(settingData.language);
        Screen.SetResolution(settingData.width, settingData.height, settingData.isFullScreen);
    }

    ////////////////////////////
    ///// 유저 데이터 저장 /////
    ////////////////////////////






    ////////////////////////////
    ///// 설정 데이터 저장 /////
    ////////////////////////////
    public SettingData settingData;

    // 적용 버튼 OnClick()에 넣음. 데이터 저장하기
    public void SaveSettingData()
    {
        // json 파일로 저장. ToJson은 json 포맷으로 직렬화하기
        File.WriteAllText(Application.persistentDataPath + "/SettingData.json", JsonUtility.ToJson(settingData));
    }

    // 게임 시작 시 데이터 읽어오기
    public void ReloadSettingData()
    {
        // json 파일 읽어오기. FromJson은 다시 오브젝트로 전환하기
        if (File.Exists(Application.persistentDataPath + "/SettingData.json"))
        {
            settingData = JsonUtility.FromJson<SettingData>(File.ReadAllText(Application.persistentDataPath + "/SettingData.json"));  
        }
    }






    ////////////////////////////
    //// 그래픽 데이터 저장 ////
    ////////////////////////////

    // 해상도 값 넣기
    public void SetResolution(int value)
    {
        settingData.resolution = value;

        switch (value)
        {
            case 0:
                settingData.width = 1920;
                settingData.height = 1080;
                break;
            case 1:
                settingData.width = 1280;
                settingData.height = 720;
                break;
            default:
                break;
        }
    }
}
