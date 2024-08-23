using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Empty : Item
{
    

    void Update()
    {
        
    }

    private new void OnEnable()
    {
        handAnim.Play("Empty");
    }
}
