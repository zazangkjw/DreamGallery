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

        handAnim.Play("Bat_Up");
        StartCoroutine(coroutine = BatCoroutine());
    }




    IEnumerator BatCoroutine()
    {
        while (true)
        {
            if (!putDialogScript.isClickMode && handAnim.GetBool("isReady")) // Ŭ���� ��� ������ ���� ��� ��Ȱ��ȭ
            {
                // Ŭ��
                if (Input.GetMouseButtonDown(0))
                {
                    handAnim.SetBool("isCanceled", false);
                    chargeTimer = 0;
                }

                // Ŭ�� ����
                if (Input.GetMouseButton(0) && !handAnim.GetBool("isCanceled") && !handAnim.GetBool("isChargeAttack"))
                {
                    handAnim.SetBool("isCharging", true);
                    chargeTimer += Time.deltaTime;

                    if (chargeTimer >= 0.2f && handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                    {
                        handAnim.SetBool("isCharged", true);
                    }
                }

                // Ŭ�� ����
                if (Input.GetMouseButtonUp(0) && handAnim.GetBool("isCharging"))
                {
                    // ���� ����
                    if (handAnim.GetBool("isCharged"))
                    {
                        chargeTimer = 0;
                        handAnim.SetBool("isChargeAttack", true);
                        foreach (Collider col in cols)
                        {
                            col.enabled = true;
                        }

                        // ���� ��� ���� ������ ���
                        yield return delay; // ���� �ִϸ��̼����� ��ȯ�� ������ ����ϴ� ª�� �ð�(������ ��� -> �������� ���)
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

                // ��Ŭ������ ��¡ ���
                if (Input.GetMouseButtonDown(1))
                {
                    chargeTimer = 0;
                    handAnim.SetBool("isCanceled", true);
                    handAnim.SetBool("isCharging", false);
                    handAnim.SetBool("isCharged", false);
                }
            }
            else
            {
                // ���� �ִϸ��̼����� ��ȯ�� ������ ���(���� ��� ��� -> idle ���)
                while (handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                {
                    if(handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f) { handAnim.SetBool("isReady", true); break; }
                    
                    yield return null;
                }
            }

            yield return null;
        }
    }
}
