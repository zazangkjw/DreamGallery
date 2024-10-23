using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    // �Ѿ� �̵�
    virtual public void Movement()
    {
        myRigid.MovePosition(myRigid.position + (transform.forward * speed));
    }




    // �ݶ��̴� ������ ��Ȱ��ȭ
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ignore")
        {

        }
        else if(other.gameObject.tag == "Target")
        {
            if(other.GetComponentInParent<Target>().team == team)
            {

            }
            else
            {
                other.GetComponentInParent<Target>().DestoryTarget();
                gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.tag == "Reflect")
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
