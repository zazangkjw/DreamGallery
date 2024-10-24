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


    // 테스트용
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            team = 2;
            isShooting = !isShooting;
        }
    }
}
