using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Bat : Item
{
    

    void Update()
    {
        
    }

    private void OnEnable()
    {
        handAnim.Play("Bat_Up");
        StartCoroutine(BatCoroutine());
    }




    IEnumerator BatCoroutine()
    {
        // 다음 애니메이션으로 전환될 때까지 대기(무기 드는 모션 -> idle 모션)
        while (handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        while (true)
        {
            if (!putDialogScript.isClickMode) // 클릭형 대사 나오는 동안 기능 비활성화
            {
                // 클릭
                if (Input.GetMouseButtonDown(0))
                {
                    handAnim.SetBool("isCanceled", false);
                    chargeTimer = 0;
                }

                // 클릭 유지
                if (Input.GetMouseButton(0) && !handAnim.GetBool("isCanceled") && !handAnim.GetBool("isChargeAttack"))
                {
                    handAnim.SetBool("isCharging", true);
                    chargeTimer += Time.deltaTime;

                    if (chargeTimer >= 0.2f && handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                    {
                        handAnim.SetBool("isCharged", true);
                    }
                }

                // 클릭 떼기
                if (Input.GetMouseButtonUp(0) && handAnim.GetBool("isCharging"))
                {
                    // 차지 공격
                    if (handAnim.GetBool("isCharged"))
                    {
                        handAnim.SetBool("isChargeAttack", true);
                        foreach (Collider col in cols)
                        {
                            col.enabled = true;
                        }

                        // 스윙 모션 끝날 때까지 대기
                        yield return delay; // 다음 애니메이션으로 전환될 때까지 대기하는 짧은 시간(차지드 모션 -> 차지어택 모션)
                        handAnim.SetBool("isCharging", false);
                        handAnim.SetBool("isCharged", false);
                        while (true)
                        {
                            if (handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                            {
                                break;
                            }
                            yield return null;
                        }

                        handAnim.SetBool("isChargeAttack", false);
                        foreach (Collider col in cols)
                        {
                            col.enabled = false;
                        }
                    }
                    else
                    {
                        handAnim.SetBool("isCharging", false);
                    }
                }

                // 우클릭으로 차징 취소
                if (Input.GetMouseButtonDown(1))
                {
                    handAnim.SetBool("isCanceled", true);
                    handAnim.SetBool("isCharging", false);
                    handAnim.SetBool("isCharged", false);
                }
            }
            yield return null;
        }
    }
}
