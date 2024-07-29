using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public Animator handAnim;

    bool isCanceled; // 차징 준비 완료
    float chargeTimer; // 차징 시간

    WaitForSeconds delay = new WaitForSeconds(0.1f);




    private void OnEnable()
    {
        handAnim.SetBool("isObject", true);
        StartCoroutine(BatCoroutine());
    }




    IEnumerator BatCoroutine()
    {
        yield return delay; // 다음 애니메이션으로 전환될 때까지 대기하는 짧은 시간(무기 드는 시간동안 대기)
        while (handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        while (true)
        {
            // 클릭
            if (Input.GetMouseButtonDown(0))
            {
                isCanceled = false;
                chargeTimer = 0;
            }

            // 클릭 유지
            if (Input.GetMouseButton(0) && !isCanceled && !handAnim.GetBool("isSwing"))
            {
                handAnim.SetBool("isCharging", true);
                chargeTimer += Time.deltaTime;
            }

            // 클릭 떼기
            if (Input.GetMouseButtonUp(0) && !isCanceled)
            {
                // 차지 공격
                if (chargeTimer >= 0.2f && handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    handAnim.SetBool("isSwing", true);
                    
                    // 스윙 모션 끝날 때까지 대기
                    yield return delay; // 다음 애니메이션으로 전환될 때까지 대기하는 짧은 시간
                    handAnim.SetBool("isCharging", false);
                    while (true)
                    {
                        if(handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                        {
                            break;
                        }
                        yield return null;
                    }

                    handAnim.SetBool("isSwing", false);
                }
                else
                {
                    handAnim.SetBool("isCharging", false);
                }
            }

            // 우클릭으로 차징 취소
            if (Input.GetMouseButtonDown(1))
            {
                isCanceled = true;
                handAnim.SetBool("isCharging", false);
            }

            yield return null;
        }
    }
}
