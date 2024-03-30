using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class TextFileManager : MonoBehaviour
{
    // CSV 파일 대사가 저장되는 리스트
    public List<Dictionary<string, object>> dialog; 
    string DialogFileName;
    
    // UI 텍스트가 저장되는 리스트
    public List<string> ui; // 전체 UI 문자열
    string UIFileName;
    string readLine;
    StringReader stringReader;

    // 대사, UI 텍스트 파일 읽어오기
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

        // 대사 파일 읽어오기
        dialog = CSVReader.Read(DialogFileName);
      
        // UI 파일 읽어오기
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