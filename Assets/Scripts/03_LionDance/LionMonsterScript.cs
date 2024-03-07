using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionMonsterScript : MonoBehaviour
{
    public AudioSource footstep;
    public AudioSource monsterLaughing;

    void Footstep()
    {
        footstep.Play();
    }

    void StartLaughing()
    {
        monsterLaughing.Play();
    }

    void StopLaughing()
    {
        monsterLaughing.Stop();
    }
}
