using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal_04_Clown : MonoBehaviour
{
    public AudioSource wind_sound;

    public void wind_sound_on()
    {
        StartCoroutine(AudioOnOffScript.VolumeCoroutine(wind_sound, true, 2f, 0.7f));
    }
}
