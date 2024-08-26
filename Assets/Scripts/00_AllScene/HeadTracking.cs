using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTracking : MonoBehaviour
{
    public GameObject target; // Å¸°Ù

    [SerializeField]
    GameObject head;

    public Vector3 correction;
    public float lookSpeed = 0.1f;
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
        LookTarget();
    }

    void LookTarget()
    {
        if (isLooking)
        {
            head.transform.rotation = Quaternion.Lerp(head.transform.rotation, Quaternion.LookRotation(target.transform.position - head.transform.position) * Quaternion.Euler(correction), lookSpeed);
        }
        else
        {
            head.transform.localRotation = Quaternion.Lerp(head.transform.localRotation, Quaternion.identity, lookSpeed);
        }
    }
}
