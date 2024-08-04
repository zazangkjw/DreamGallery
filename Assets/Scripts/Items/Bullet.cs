using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Bullet : MonoBehaviour
{
    public int team;
    public float speed;

    public Rigidbody myRigid;
    public Collider[] cols;




    void FixedUpdate()
    {
        Movement();
    }



    // 총알 이동
    virtual public void Movement()
    {
        myRigid.MovePosition(myRigid.position + (transform.forward * speed));
    }




    // 콜라이더 닿으면 비활성화
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {

        }
        else if (other.gameObject.tag == "Bullet")
        {

        }
        else if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponentInParent<PlayerController>().team == team)
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
