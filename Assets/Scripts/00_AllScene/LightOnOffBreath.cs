using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnOffBreath : MonoBehaviour
{
    public float seconds = 2f;
    public float secondsForStop = 1f;

    float maxIntensity;

    [SerializeField]
    Light myLight;

    [SerializeField]
    Material[] emissionOnMats;

    [SerializeField]
    Material[] emissionOffMats;

    [SerializeField]
    MeshRenderer myMesh;

    bool flag_on;

    WaitForSeconds delay = new WaitForSeconds(0.01f);
    WaitForSeconds delayForStop;

    void Start()
    {
        delayForStop = new WaitForSeconds(secondsForStop);
        maxIntensity = myLight.intensity;
        myLight.intensity = 0f;
        myMesh = GetComponent<MeshRenderer>();
        StartCoroutine(LightBreath());
    }

    IEnumerator LightBreath()
    {
        while (true)
        {
            if (myLight.intensity >= maxIntensity)
            {
                yield return delayForStop;
                flag_on = false;
            }
            else if (myLight.intensity <= 0)
            {
                yield return delayForStop;
                flag_on = true;
            }

            if (flag_on)
            {
                myLight.intensity += maxIntensity * (0.01f / seconds);
            }
            else
            {
                myLight.intensity -= maxIntensity * (0.01f / seconds);
            }

            yield return delay;
        }
    }
}
