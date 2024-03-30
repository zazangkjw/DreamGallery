using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class TextFileManager : MonoBehaviour
{
    // CSV ���� ��簡 ����Ǵ� ����Ʈ
    public List<Dictionary<string, object>> dialog; 
    string DialogFileName;
    
    // UI �ؽ�Ʈ�� ����Ǵ� ����Ʈ
    public List<string> ui; // ��ü UI ���ڿ�
    string UIFileName;
    string readLine;
    StringReader stringReader;

    // ���, UI �ؽ�Ʈ ���� �о����
    public void Reload(int language)
    {
        switch (language)
        {
            case 0:
                DialogFileName = "/Dialog_Text_KR.csv";
                UIFileName = "/UI_Text_KR.txt";
                break;

            case 1:
                DialogFileName = "/Dialog_Text_US.csv";
                UIFileName = "/UI_Text_US.txt";
                break;

            default:
                break;
        }

        // ��� ���� �о����
        dialog = CSVReader.Read(DialogFileName);
      
        // UI ���� �о����
        stringReader = new StringReader(File.ReadAllText(Application.streamingAssetsPath + UIFileName));
        ui.Clear();
        for (int i = 0; i < 10000; i++)
        {
            readLine = stringReader.ReadLine();
            if (readLine == null || readLine == string.Empty)
            {
                break;
            }
            ui.Add(readLine);
        }
    }
}