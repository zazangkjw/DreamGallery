using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public Animator handAnim;

    bool isCanceled; // ��¡ �غ� �Ϸ�
    float chargeTimer; // ��¡ �ð�

    WaitForSeconds delay = new WaitForSeconds(0.1f);




    private void OnEnable()
    {
        handAnim.SetBool("isObject", true);
        StartCoroutine(BatCoroutine());
    }




    IEnumerator BatCoroutine()
    {
        yield return delay; // ���� �ִϸ��̼����� ��ȯ�� ������ ����ϴ� ª�� �ð�(���� ��� �ð����� ���)
        while (handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        while (true)
        {
            // Ŭ��
            if (Input.GetMouseButtonDown(0))
            {
                isCanceled = false;
                chargeTimer = 0;
            }

            // Ŭ�� ����
            if (Input.GetMouseButton(0) && !isCanceled && !handAnim.GetBool("isSwing"))
            {
                handAnim.SetBool("isCharging", true);
                chargeTimer += Time.deltaTime;
            }

            // Ŭ�� ����
            if (Input.GetMouseButtonUp(0) && !isCanceled)
            {
                // ���� ����
                if (chargeTimer >= 0.2f && handAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    handAnim.SetBool("isSwing", true);
                    
                    // ���� ��� ���� ������ ���
                    yield return delay; // ���� �ִϸ��̼����� ��ȯ�� ������ ����ϴ� ª�� �ð�
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

            // ��Ŭ������ ��¡ ���
            if (Input.GetMouseButtonDown(1))
            {
                isCanceled = true;
                handAnim.SetBool("isCharging", false);
            }

            yield return null;
        }
    }
}
