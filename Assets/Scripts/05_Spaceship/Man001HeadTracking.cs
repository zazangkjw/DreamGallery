using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man001HeadTracking : MonoBehaviour
{
    [SerializeField]
    GameObject playerCam; // 플레이어 카메라

    [SerializeField]
    GameObject head;

    public bool isLooking;

    void Start()
    {
        
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
        if (isLooking)
        {
            head.transform.rotation = Quaternion.Lerp(head.transform.rotation, Quaternion.LookRotation(playerCam.transform.position - head.transform.position) * Quaternion.Euler(new Vector3(0f, 90f, 0f)), 0.1f);
        }
        else
        {
            head.transform.localRotation = Quaternion.Lerp(head.transform.localRotation, Quaternion.identity, 0.1f);
        }
    }
}
