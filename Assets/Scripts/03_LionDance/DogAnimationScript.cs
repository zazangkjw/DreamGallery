using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimationScript : MonoBehaviour
{
    public Animator dogAnim;

    public GameObject player;
    public GameObject root;
    public Vector3 originalRotation;
    public bool isLookPlayer;

    void Start()
    {
        isLookPlayer = true;
        originalRotation = root.transform.localEulerAngles;
    }

    void Update()
    {
        LookPlayer();
    }

    void StartWalking()
    {
        dogAnim.SetBool("IsWalking", true);
    }

    void StopWalking()
    {
        dogAnim.SetBool("IsWalking", false);
    }

    public void disableLookPlayer()
    {
        root.transform.localEulerAngles = originalRotation;
        isLookPlayer = false;
    }

    public void enableLookPlayer()
    {
        isLookPlayer = true;
    }

    void LookPlayer()
    {
        if (isLookPlayer)
        {
            root.transform.rotation = Quaternion.Lerp(root.transform.rotation, Quaternion.LookRotation(new Vector3(-(player.transform.position.x - root.transform.position.x), root.transform.position.y, -(player.transform.position.z - root.transform.position.z))), 0.05f);
        }
    }
}
