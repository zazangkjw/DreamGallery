using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownFootStep : MonoBehaviour
{
    public AudioSource[] squeakyShoes;
    int cnt;

    void SqueakyShoes()
    {
        squeakyShoes[cnt].Play();
        cnt++;
        if(cnt >= squeakyShoes.Length) { cnt = 0; }
    }
}
