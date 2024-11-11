using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // �⺻ �Ӽ�
    public string itemName;
    public Texture itemImage;
    public GameObject prefab;
    public int team;
    public Collider[] cols;
    public bool isObtainable = true; // ȹ�� ��������
    public bool isStack = false; // ��� ȹ�� ������ ����������(������ ����)

    // �巡�׷� �߰�
    public DefaultRaycast defaultRaycast;
    public Animator handAnim;
    public PutDialogScript putDialogScript; // Ŭ���� ��� ������ ���� ��� ��Ȱ��ȭ
    public GameObject crosshair;

    // ��Ÿ
    protected WaitForSeconds delay = new WaitForSeconds(0.2f);

    


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
