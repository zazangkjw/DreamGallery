using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{
    Light flickeringLight;
    float intensity;

    void Start()
    {
        flickeringLight = GetComponent<Light>();
        intensity = flickeringLight.intensity;
        StartCoroutine(FlickeringCoroutine());
    }

    IEnumerator FlickeringCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            flickeringLight.intensity = Random.Range(intensity - 1f, intensity);
        }
    }
}
