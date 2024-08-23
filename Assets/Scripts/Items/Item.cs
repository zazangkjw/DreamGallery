using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public int team;
    
    public Animator handAnim;
    public Collider[] cols;

    protected WaitForSeconds delay = new WaitForSeconds(0.2f);
    public float chargeTimer; // ��¡ �ð�

    public PutDialogScript putDialogScript; // Ŭ���� ��� ������ ���� ��� ��Ȱ��ȭ

    public IEnumerator coroutine;

    protected void OnEnable()
    {
        chargeTimer = 0;
        handAnim.SetBool("isReady", false);
        handAnim.SetBool("isCanceled", false);
        handAnim.SetBool("isCharging", false);
        handAnim.SetBool("isCharged", false);
        handAnim.SetBool("isChargeAttack", false);
        foreach (Collider col in cols)
        {
            col.enabled = false;
        }
    }
}
