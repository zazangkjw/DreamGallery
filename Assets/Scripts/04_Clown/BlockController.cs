using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public Animator[] anims;
    float time;

    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = Random.Range(1, 3);
            for (int i = 0; i < 50; i++)
            {
                int n = Random.Range(0, anims.Length);
                if (!anims[n].GetCurrentAnimatorStateInfo(0).IsName("Active"))
                {
                    anims[n].Play("Active");
                    break;
                }
            }
        }
    }
}
