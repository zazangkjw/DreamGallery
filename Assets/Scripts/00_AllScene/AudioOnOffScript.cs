using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOnOffScript : MonoBehaviour
{
    // 소리 점점 커지고 작아짐
    public static IEnumerator VolumeCoroutine(AudioSource audio, bool turnOn ,float time)
    {
        time = time == 0 ? 0.01f : time;

        if (turnOn)
        {
            audio.Play();
            while (audio.volume < 1)
            {
                audio.volume += (1f / time) * Time.deltaTime;
                yield return null;
            }
            audio.volume = 1;
        }
        else
        {
            while (audio.volume > 0)
            {
                audio.volume -= (1f / time) * Time.deltaTime;
                yield return null;
            }
            audio.volume = 0;
            audio.Stop();
        }
    }
}
