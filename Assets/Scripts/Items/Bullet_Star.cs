using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Star : Bullet
{
    public GameObject target;
    public bool isShooting;




    void OnEnable()
    {
        foreach (Collider col in cols)
        {
            col.enabled = false;
        }
    }




    override public void Movement()
    {
        // Ÿ�� �ٶ󺸱�
        if (!isShooting)
        {
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        }
        // �߻�
        else
        {
            foreach (Collider col in cols)
            {
                col.enabled = true;
            }
            myRigid.MovePosition(myRigid.position + (transform.forward * speed));
        }
    }
}
