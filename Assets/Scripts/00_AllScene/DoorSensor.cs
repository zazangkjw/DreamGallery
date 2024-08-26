using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSensor : MonoBehaviour
{
    public bool isDetected;
    public bool isLock;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isDetected = true;

            if (!isLock)
            {
                animator.SetBool("Active", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isDetected = false;

            if (!isLock)
            {
                animator.SetBool("Active", false);
            }
        }
    }
}
