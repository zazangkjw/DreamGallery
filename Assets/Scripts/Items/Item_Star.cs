using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Star : Item
{
    void OnEnable()
    {
        foreach (Collider col in cols)
        {
            col.enabled = false;
        }
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        Movement();
    }




    void Movement()
    {
        // 타겟 바라보기
        if (!isShooting)
        {
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        }
        // 발사
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
