using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{
    Light flickeringLight;

    void Start()
    {
        flickeringLight = GetComponent<Light>();
        StartCoroutine(FlickeringCoroutine());
    }

    IEnumerator FlickeringCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            flickeringLight.intensity = Random.Range(3f, 4f);
        }
    }
}
