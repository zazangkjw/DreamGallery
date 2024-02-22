using System;
using System.IO;
using UnityEngine;

[Serializable] // ��ü�� ����Ʈ ���·� ��ȯ�ϴ� ����ȭ�� �� �� �ְ� ��. ����� ���� ���ڿ��� ����ȭ�Ͽ� �ְ� �޴� ���� ����. ����ȭ �ؾ� json���� �ٲ� �� �ִ� ��
public class SettingData
{
    public int language;
    public float mouseSens;
    //public int width; // ȭ�� �ʺ�
    //public int height; // ȭ�� ����
    //public bool fullScreen; // ��ü ȭ��
}

public class SaveManager : MonoBehaviour
{
    void Start()
    {
        // ó�� ���� ���� �� ���� �ѱ�, �⺻ ������ 0.5f�̰�, ���� ������ �����ϸ� �޾ƿ���
        settingData.language = 0;
        settingData.mouseSens = 0.5f;
        //settingData.width = 1920;
        //settingData.height = 1080;
        //settingData.fullScreen = true;
        ReloadSettingData();
        GameManager.instance.textFileManager.Reload(settingData.language);
    }

    ////////////////////////////
    ///// ���� ������ ���� /////
    ////////////////////////////






    ////////////////////////////
    ///// ���� ������ ���� /////
    ////////////////////////////
    public SettingData settingData = new SettingData();

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
}
