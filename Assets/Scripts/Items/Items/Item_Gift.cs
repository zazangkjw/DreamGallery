using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Gift : Item
{
    

    void Update()
    {
        
    }

    private new void OnEnable()
    {
        base.OnEnable();

        handAnim.Play("Gift_Up");
        transform.localPosition = new Vector3(0, 0.1f, 0);
        transform.localEulerAngles = new Vector3(0, 0, -25);
    }
}
