using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicycleScript : MonoBehaviour
{
    [SerializeField]
    Animator unicycleAnim;

    public void StartTurn()
    {
        unicycleAnim.SetBool("Turn", true);
    }
    public void StopTurn()
    {
        unicycleAnim.SetBool("Turn", false);
    }
}
