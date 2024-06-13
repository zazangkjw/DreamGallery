using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown_Unicycle : MonoBehaviour
{
    [SerializeField]
    GameObject playerCam; // 플레이어 카메라


    [SerializeField]
    GameObject clownHead;


    void Start()
    {
        //GetComponent<Animator>().Play("Breathe");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        LookPlayer();
    }

    void LookPlayer()
    {
        clownHead.transform.rotation = Quaternion.LookRotation(playerCam.transform.position - clownHead.transform.position) * Quaternion.Euler(new Vector3(0f, 90f, 0f));
    }
}
