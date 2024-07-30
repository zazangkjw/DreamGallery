using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public Item item;

    // 콜라이더 닿으면 비활성화
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MeleeAttack")
        {

        }
        else if (other.gameObject.tag == "RangedAttack")
        {

        }
        else if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponentInParent<PlayerController>().team == item.team)
            {

            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
