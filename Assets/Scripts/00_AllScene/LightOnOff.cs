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

    void Start()
    {
        maxIntensity = myLight.intensity;
        myLight.intensity = 0f;
        myMesh = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(LightOn());
            if(myMesh != null) { myMesh.materials = emissionOnMats;  }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(LightOff());
            if (myMesh != null) { myMesh.materials = emissionOffMats; }
        }
    }

    IEnumerator LightOn()
    {
        flag_on = true;

        while (myLight.intensity < maxIntensity)
        {

            if (!flag_on)
            {
                break;
            }

            myLight.intensity += maxIntensity * (0.01f / seconds);

            if (myLight.intensity > maxIntensity)
            {
                myLight.intensity = maxIntensity;
                break;
            }

            yield return delay;
        }
    }

    IEnumerator LightOff()
    {
        flag_on = false;

        while (myLight.intensity > 0)
        {
            
            if (flag_on)
            {
                break;
            }
            
            myLight.intensity -= maxIntensity * (0.01f / seconds);

            if(myLight.intensity < 0)
            {
                myLight.intensity = 0;
                break;
            }

            yield return delay;
        }
    }
}
