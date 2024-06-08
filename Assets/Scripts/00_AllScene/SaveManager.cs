using System;
using System.IO;
using UnityEngine;

[Serializable] // ��ü�� ����Ʈ ���·� ��ȯ�ϴ� ����ȭ�� �� �� �ְ� ��. ����� ���� ���ڿ��� ����ȭ�Ͽ� �ְ� �޴� ���� ����. ����ȭ �ؾ� json���� �ٲ� �� �ִ� ��
public struct SettingData
{
    public int language; // ���
    public float mouseSens; // ���콺 ����
    public int resolution; // �ػ� ���� ��
    public int width; // ȭ�� �ʺ�
    public int height; // ȭ�� ����
    public bool isFullScreen; // ȭ�� ���
    public float volume; // ���� ũ��
    public int fpsLimit; // ������ ����
    public bool isDisplayFps; // ������ ǥ��
    public int vSync; // ��������ȭ
}

public class SaveManager : MonoBehaviour
{
    void Start()
    {
        // ó�� ���� ���� �� ���� �ѱ�, �⺻ ������ 15f�̰�, ���� ������ �����ϸ� �޾ƿ���
        settingData.language = 0;
        settingData.mouseSens = 15f;
        settingData.resolution = 3;
        settingData.width = 1920;
        settingData.height = 1080;
        settingData.isFullScreen = true;
        settingData.volume = 0f;
        settingData.fpsLimit = 100;
        settingData.isDisplayFps = false;
        settingData.vSync = 1;
        ReloadSettingData();
        GameManager.instance.textFileManager.Reload(settingData.language);
        Screen.SetResolution(settingData.width, settingData.height, settingData.isFullScreen);
        GameManager.instance.fps_Limit.setLimit();
        GameManager.instance.fps_Limit.setActive();
        QualitySettings.vSyncCount = settingData.vSync;
    }

    ////////////////////////////
    ///// ���� ������ ���� /////
    ////////////////////////////






    ////////////////////////////
    ///// ���� ������ ���� /////
    ////////////////////////////
    public SettingData settingData;

    // ���� ��ư OnClick()�� ����. ������ �����ϱ�
    public void SaveSettingData()
    {
        // json ���Ϸ� ����. ToJson�� json �������� ����ȭ�ϱ�
        File.WriteAllText(Application.persistentDataPath + "/SettingData.json", JsonUtility.ToJson(settingData));
    }

    // ���� ���� �� ������ �о����
    public void ReloadSettingData()
    {
        // json ���� �о����. FromJson�� �ٽ� ������Ʈ�� ��ȯ�ϱ�
        if (File.Exists(Application.persistentDataPath + "/SettingData.json"))
        {
            settingData = JsonUtility.FromJson<SettingData>(File.ReadAllText(Application.persistentDataPath + "/SettingData.json"));  
        }
    }






    ////////////////////////////
    //// �׷��� ������ ���� ////
    ////////////////////////////

    // �ػ� �� �ֱ�
    public void SetResolution(int value)
    {
        settingData.resolution = value;

        switch (value)
        {
            case 0:
                settingData.width = 3840;
                settingData.height = 2160;
                break;
            case 1:
                settingData.width = 3200;
                settingData.height = 1800;
                break;
            case 2:
                settingData.width = 2560;
                settingData.height = 1440;
                break;
            case 3:
                settingData.width = 1920;
                settingData.height = 1080;
                break;
            case 4:
                settingData.width = 1600;
                settingData.height = 900;
                break;
            case 5:
                settingData.width = 1366;
                settingData.height = 768;
                break;
            case 6:
                settingData.width = 1280;
                settingData.height = 720;
                break;
            case 7:
                settingData.width = 640;
                settingData.height = 360;
                break;
            default:
                break;
        }
    }
}
