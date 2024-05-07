using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnOff : MonoBehaviour
{
    [SerializeField]
    Light myLight;

    [SerializeField]
    Material[] emissionOnMats;

    [SerializeField]
    Material[] emissionOffMats;

    MeshRenderer myMesh;

    void Start()
    {
        myMesh = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            myLight.enabled = true;
            myMesh.materials = emissionOnMats;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            myLight.enabled = false;
            myMesh.materials = emissionOffMats;
        }
    }
}
