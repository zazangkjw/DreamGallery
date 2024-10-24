using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Bat : Item
{
    void Update()
    {
        
    }

    private new void OnEnable()
    {
        base.OnEnable();

        crosshair.SetActive(true);
        handAnim.Play("Bat_Up");
        StartCoroutine(BatCoroutine());
    }





    IEnumerator BatCoroutine()
    {
        while (true)
        {
            if (!putDialogScript.isClickMode && handAnim.GetBool("isReady") && defaultRaycast.currentItem == this && !DefaultRaycast.inventoryOnOff) // 클릭형 대사 나오는 동안 기능 비활성화
            {
                // 클릭
                if (Input.GetMouseButtonDown(0))
                {
                    handAnim.SetBool("isCanceled", false);
                    handAnim.SetFloat("ChargeTimer", 0f);
                    chargedSoundPlayed = false;
                }

                // 클릭 유지
                if (Input.GetMouseButton(0) && !handAnim.GetBool("isCanceled") && !handAnim.GetBool("isChargeAttack") && !handAnim.GetBool("isChargeAttackEnd"))
                {
                    handAnim.SetBool("isCharging", true);
                    handAnim.SetFloat("ChargeTimer", handAnim.GetFloat("ChargeTimer") + Time.deltaTime);

                    if (handAnim.GetBool("isCharged") && !chargedSoundPlayed)
                    {
                        chargedSoundPlayed = true;
                        //chargedSound.Play();
                    }
                }

                // 클릭 떼기
                if (Input.GetMouseButtonUp(0) && handAnim.GetBool("isCharging"))
                {
                    // 차지 공격
                    if (handAnim.GetBool("isCharged"))
                    {
                        handAnim.SetFloat("ChargeTimer", 0f);
                        handAnim.SetBool("isChargeAttack", true);
                        foreach (Collider col in cols)
                        {
                            col.enabled = true;
                        }
                    }
                    else
                    {
                        handAnim.SetFloat("ChargeTimer", 0f);
                        handAnim.SetBool("isCharging", false);
                    }
                }

                // 차지어택 끝나면 콜라이더 비활성화
                if (!handAnim.GetBool("isChargeAttack"))
                {
                    foreach (Collider col in cols)
                    {
                        col.enabled = false;
                    }
                }

                // 우클릭으로 차징 취소
                if (Input.GetMouseButtonDown(1))
                {
                    handAnim.SetFloat("ChargeTimer", 0f);
                    handAnim.SetBool("isCanceled", true);
                    handAnim.SetBool("isCharging", false);
                    handAnim.SetBool("isCharged", false);
                }
            }

            yield return null;
        }
    }
}
