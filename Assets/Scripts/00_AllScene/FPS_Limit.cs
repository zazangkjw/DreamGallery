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

    float deltaTime = 0.0f;

    public int fpsLimit;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
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
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            // float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            // string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            string text = (Math.Round(fps) + "fps");
            GUI.Label(rect, text, style);
        }
    }

    void Start()
    {
        setLimit();
    }

    public void setLimit()
    {
        Application.targetFrameRate = fpsLimit;
    }

    public void setActive(bool b)
    {
        active = b;
    }
}