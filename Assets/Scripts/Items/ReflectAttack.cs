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
        if (other.gameObject.tag == "Bullet")
        {
            if (other.gameObject.GetComponent<Bullet>().team != item.team)
            {
                other.gameObject.GetComponent<Bullet>().team = item.team;
                other.gameObject.GetComponent<Bullet>().myRigid.MovePosition(playerCam.transform.position + (playerCam.transform.forward));
                other.gameObject.transform.rotation = Quaternion.LookRotation(playerCam.transform.forward);
            }
        }
    }
}
