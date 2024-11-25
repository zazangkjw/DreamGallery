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

    public AudioSource light_on_sound;
    public AudioSource light_off_sound;

    void Start()
    {
        maxIntensity = myLight.intensity;
        myLight.intensity = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "NPC")
        {
            if (!flag_on)
            {
                if (light_on_sound != null) { light_on_sound.Play(); }
                if (coroutine != null) { StopCoroutine(coroutine); }
                coroutine = LightOn();
                StartCoroutine(coroutine);
                if (myMesh != null) { myMesh.materials = emissionOnMats; }
                flag_on = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "NPC")
        {
            if (flag_on)
            {
                if (light_off_sound != null) { light_off_sound.Play(); }
                if (coroutine != null) { StopCoroutine(coroutine); }
                coroutine = LightOff();
                StartCoroutine(coroutine);
                if (myMesh != null) { myMesh.materials = emissionOffMats; }
                flag_on = false;
            }
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
