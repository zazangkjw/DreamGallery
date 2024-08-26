using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public int team;

    public DefaultRaycast defaultRaycast;
    public Animator handAnim;
    public Collider[] cols;
    public bool isObtainable = true; // ȹ�� ��������

    protected WaitForSeconds delay = new WaitForSeconds(0.2f);

    public PutDialogScript putDialogScript; // Ŭ���� ��� ������ ���� ��� ��Ȱ��ȭ

    protected void OnEnable()
    {
        handAnim.SetFloat("ChargeTimer", 0f);
        handAnim.SetBool("isReady", false);
        handAnim.SetBool("isCanceled", false);
        handAnim.SetBool("isCharging", false);
        handAnim.SetBool("isCharged", false);
        handAnim.SetBool("isChargeAttack", false);
        handAnim.SetBool("isChargeAttackEnd", false);
        foreach (Collider col in cols)
        {
            col.enabled = false;
        }
    }
}
