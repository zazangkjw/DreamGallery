using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimationScript : MonoBehaviour
{
    public Animator dogAnim;
    public GameObject player;
    public GameObject rootForUnity; // 개의 허리 Bone
    public Vector3 originalRotation; // 원래 로테이션값
    public bool isLookPlayer; // 플레이어를 바라볼지

    public AudioSource bark;

    void Start()
    {
        isLookPlayer = true;
        originalRotation = rootForUnity.transform.rotation.eulerAngles;
    }

    void FixedUpdate()
    {
        LookPlayer();
    }




    public void StartWalking() // 개 걷기 시작
    {
        dogAnim.SetBool("IsWalking", true);
    }

    public void StopWalking() // 개 걷기 종료
    {
        dogAnim.SetBool("IsWalking", false);
    }



    public void StartBarking() // 개 짖기 시작
    { 
        bark.Play();
    }

    public void StopBarking() // 개 짖기 시작
    {
        bark.Stop();
    }




    public void enableLookPlayer() // 개가 플레이어 바라보기 활성화
    {
        isLookPlayer = true;
    }

    public void disableLookPlayer() // 개가 플레이어 바라보기 비활성화
    {
        rootForUnity.transform.rotation = Quaternion.Euler(originalRotation);
        isLookPlayer = false;
    }

    void LookPlayer() // 개가 플레이어 바라보기
    {
        if (isLookPlayer)
        {
            // rootForUnity.transform.rotation = Quaternion.Lerp(rootForUnity.transform.rotation, Quaternion.LookRotation(new Vector3(-(player.transform.position.x - rootForUnity.transform.position.x), rootForUnity.transform.position.y, -(player.transform.position.z - rootForUnity.transform.position.z))), 0.05f);
            rootForUnity.transform.rotation = Quaternion.Lerp(rootForUnity.transform.rotation, Quaternion.LookRotation(new Vector3((player.transform.position.x - rootForUnity.transform.position.x), 0, (player.transform.position.z - rootForUnity.transform.position.z))) * Quaternion.Euler(new Vector3(-90f, 180f, 0)), 0.05f);
        }
    }
}
