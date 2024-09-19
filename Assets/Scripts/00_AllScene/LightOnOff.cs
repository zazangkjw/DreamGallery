using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnOff : MonoBehaviour
{
    public float seconds = 1f;

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

    IEnumerator coroutine;

    void Start()
    {
        maxIntensity = myLight.intensity;
        myLight.intensity = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "NPC")
        {
            if(coroutine != null) { StopCoroutine(coroutine); }
            coroutine = LightOn();
            StartCoroutine(coroutine);
            if(myMesh != null) { myMesh.materials = emissionOnMats;  }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "NPC")
        {
            if (coroutine != null) { StopCoroutine(coroutine); }
            coroutine = LightOff();
            StartCoroutine(coroutine);
            if (myMesh != null) { myMesh.materials = emissionOffMats; }
        }
    }

    IEnumerator LightOn()
    {
        seconds = seconds == 0 ? 0.01f : seconds;

        while (myLight.intensity < maxIntensity)
        {
            myLight.intensity += maxIntensity * (1f / seconds) * Time.deltaTime;

            if (myLight.intensity > maxIntensity)
            {
                myLight.intensity = maxIntensity;
                break;
            }

            yield return null;
        }
    }

    IEnumerator LightOff()
    {
        seconds = seconds == 0 ? 0.01f : seconds;

        while (myLight.intensity > 0)
        {
            myLight.intensity -= maxIntensity * (1f / seconds) * Time.deltaTime;

            if(myLight.intensity < 0)
            {
                myLight.intensity = 0;
                break;
            }

            yield return null;
        }
    }
}
