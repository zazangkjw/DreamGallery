using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int team;
    public Collider[] cols;
    public WaitForSeconds delay = new WaitForSeconds(0.1f);

    // 근거리
    public Animator handAnim;
    public float chargeTimer; // 차징 시간
    
    // 원거리
    public GameObject target;
    public float speed;
    public Rigidbody myRigid;
    public bool isShooting;
}
