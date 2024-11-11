using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackAttack : MonoBehaviour
{
    public Item item;
    public GameObject playerCam;

    public AudioSource hitSound;




    private void OnTriggerEnter(Collider other)
    {
        // ³Ë¹é
        if(other.gameObject.tag == "Player")
        {
            if (other.transform.GetComponentInParent<PlayerController>().team == item.team)
            {

            }
            else
            {
                other.transform.GetComponentInParent<PlayerController>().knockbackTimer = 0.5f;
                other.transform.GetComponentInParent<PlayerController>().myRigid.AddForce((playerCam.transform.forward * 10f) + (playerCam.transform.right * 0f) + (playerCam.transform.up * 5f), ForceMode.Impulse);
            }
        }
        else if(other.gameObject.tag == "Item")
        {

        }
        else if(other.gameObject.tag == "Target")
        {
            //other.GetComponentInParent<Target>().DestoryTarget();
        }
        else if(other.gameObject.tag == "Bullet")
        {
            hitSound.Play();
        }
        else
        {
            if (other.GetComponent<Rigidbody>() != null)
            {
                other.GetComponent<Rigidbody>().AddForce((playerCam.transform.forward * 10f) + (playerCam.transform.right * 0f) + (playerCam.transform.up * 5f), ForceMode.Impulse);
            }
        }
    }
}
