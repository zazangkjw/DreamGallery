using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

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




    void Start()
    {
        maxIntensity = myLight.intensity;
        myMesh = GetComponent<MeshRenderer>();
        StartCoroutine(LightBreath());
    }

    IEnumerator LightBreath()
    {
        while (true)
        {
            if (myLight.intensity >= maxIntensity)
            {
                yield return new WaitForSeconds(secondsForStop);
                flag_on = false;
            }
            else if (myLight.intensity <= 0)
            {
                yield return new WaitForSeconds(secondsForStop);
                flag_on = true;
            }

            if (flag_on)
            {
                myLight.intensity += maxIntensity * (1f / seconds) * Time.deltaTime;
            }
            else
            {
                myLight.intensity -= maxIntensity * (1f / seconds) * Time.deltaTime;
            }

            yield return null;
        }
    }
}
