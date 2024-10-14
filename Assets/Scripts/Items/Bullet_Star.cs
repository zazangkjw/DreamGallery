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


    // �׽�Ʈ��
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            team = 2;
            isShooting = !isShooting;
        }
    }
}
