using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS_Limit : MonoBehaviour
{
    /*
    int fps;
    public int fpsLimit;
    public Text FPS_Text;

    IEnumerator FPSCoroutine() {
        while (true)
        {
            fps = (int)(1 / Time.deltaTime);
            // Debug.Log(1 / Time.deltaTime);
            FPS_Text.text = (fps + "fps");
            yield return new WaitForSeconds(1);
        }
    }

    void Start()
    {
        Application.targetFrameRate = fpsLimit;
        StartCoroutine(FPSCoroutine());
    }
    */

    bool active = true;

    //float deltaTime = 0.0f;

    void Update()
    {
        //deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        if (active)
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            // float msec = deltaTime * 1000.0f;
            //float fps = 1f / deltaTime;
            float fps = 1f / Time.unscaledDeltaTime;
            // string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, (Math.Round(fps) + "fps"), style);
        }
    }

    void Start()
    {
        
    }

    public void setLimit()
    {
        Application.targetFrameRate = GameManager.instance.saveManager.settingData.fpsLimit;
    }

    public void setActive()
    {
        active = GameManager.instance.saveManager.settingData.isDisplayFps;
    }
}