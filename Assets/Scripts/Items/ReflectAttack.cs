using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectAttack : MonoBehaviour
{
    public Item item;
    public GameObject playerCam;




    private void OnTriggerEnter(Collider other)
    {
        // Æ¨°Ü³»±â
        if (other.gameObject.tag == "RangedAttack")
        {
            if (other.gameObject.GetComponent<Item>().team != item.team)
            {
                other.gameObject.GetComponent<Item>().team = item.team;
                other.gameObject.GetComponent<Item>().myRigid.MovePosition(playerCam.transform.position + (playerCam.transform.forward));
                other.gameObject.transform.rotation = Quaternion.LookRotation(playerCam.transform.forward);
            }
        }
    }
}
