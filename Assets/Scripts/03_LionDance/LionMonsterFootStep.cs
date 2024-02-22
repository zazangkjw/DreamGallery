using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionMonsterFootStep : MonoBehaviour
{
    public AudioSource footstep;

    void Footstep()
    {
        footstep.Play();
    }
}
