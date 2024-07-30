using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int team;
    public Collider[] cols;
    public WaitForSeconds delay = new WaitForSeconds(0.1f);

    // �ٰŸ�
    public Animator handAnim;
    public float chargeTimer; // ��¡ �ð�
    
    // ���Ÿ�
    public GameObject target;
    public float speed;
    public Rigidbody myRigid;
    public bool isShooting;
}
