using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int team;
    
    public Animator handAnim;
    public Collider[] cols;

    protected WaitForSeconds delay = new WaitForSeconds(0.2f);
    protected float chargeTimer; // Â÷Â¡ ½Ã°£
}
