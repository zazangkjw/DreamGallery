using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimationScript : MonoBehaviour
{
    public Animator dogAnim;
    public GameObject player;
    public GameObject waist; // 개의 허리 Bone
    public Vector3 originalRotation; // 원래 로테이션값
    public bool isLookPlayer; // 플레이어를 바라볼지

    public AudioSource bark;

    void Start()
    {
        isLookPlayer = true;
        originalRotation = waist.transform.eulerAngles;
    }

    void Update()
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
        waist.transform.eulerAngles = originalRotation;
        isLookPlayer = false;
    }

    void LookPlayer() // 개가 플레이어 바라보기
    {
        if (isLookPlayer)
        {
            waist.transform.rotation = Quaternion.Lerp(waist.transform.rotation, Quaternion.LookRotation(new Vector3(-(player.transform.position.x - waist.transform.position.x), waist.transform.position.y, -(player.transform.position.z - waist.transform.position.z))), 0.05f);
        }
    }
}
