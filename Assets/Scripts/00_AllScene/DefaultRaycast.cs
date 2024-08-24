using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefaultRaycast : MonoBehaviour
{
    public GameObject handObject;
    public Item empty;
    public GameObject itemCategory;
    public List<GameObject> items_check = new List<GameObject>(); // items üũ��

    public int inventorySize;
    public List<Item> inventory = new List<Item>(); // �κ��丮
    public int currentSlot;
    public Item currentItem;

    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;
    protected bool isChecking = true;

    public DefaultSceneManager defaultSceneManager;
    public RawImage fadeInOutImage; // ���̵�-��, �ƿ� �̹���
    public FadeInOutScript fadeInOutScript; // ���̵�-��, �ƿ� ��ũ��Ʈ




    public virtual void WhenStart()
    {
        currentSlot = 1;
        currentItem = empty;

        for (int i = 0; i < inventorySize; i++)
        {
            inventory.Add(empty);
        }

        items_check.AddRange(GameObject.FindGameObjectsWithTag("Item"));
    }

    public virtual void WhenUpdate()
    {
        SwitchItem();
        ShootRaycast();
    }




    // ī�޶󿡼� ����ĳ��Ʈ ���
    public void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f) && !defaultSceneManager.isPausing && !putDialogScript.isClickMode)
        {
            hitObject = hitInfo.collider.gameObject;
        }
        // ����� ��
        else
        {
            hitObject = null;
        }

        CheckObject();
    }

    public virtual void CheckObject()
    {

    }




    // ������ ����Ī
    public void SwitchItem()
    {
        if (currentItem != empty && Input.GetKeyDown(KeyCode.G))
        {
            currentItem.enabled = false;
            currentItem.transform.SetParent(itemCategory.transform);
            currentItem.transform.position = transform.position;
            currentItem.GetComponent<GetComponentScript>().outline.gameObject.layer = LayerMask.NameToLayer("Default");
            currentItem.GetComponent<Collider>().enabled = true;
            currentItem.GetComponent<Rigidbody>().useGravity = true;
            currentItem.GetComponent<Rigidbody>().isKinematic = false;

            currentItem.handAnim.SetFloat("ChargeTimer", 0f);
            currentItem.handAnim.SetBool("isReady", false);
            currentItem.handAnim.SetBool("isCanceled", false);
            currentItem.handAnim.SetBool("isCharging", false);
            currentItem.handAnim.SetBool("isCharged", false);
            currentItem.handAnim.SetBool("isChargeAttack", false);
            currentItem.handAnim.SetBool("isChargeAttackEnd", false);
            foreach (Collider col in currentItem.cols)
            {
                col.enabled = false;
            }

            currentItem = empty;
            inventory[currentSlot - 1] = empty;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentSlot = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentSlot = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentSlot = 3;
        }

        // ���� �������� ���� ���� �����۰� �ٸ��� ��ü �� Ȱ��ȭ
        if (currentItem != inventory[currentSlot - 1])
        {
            currentItem.gameObject.SetActive(false);

            currentItem.handAnim.SetFloat("ChargeTimer", 0f);
            currentItem.handAnim.SetBool("isReady", false);
            currentItem.handAnim.SetBool("isCanceled", false);
            currentItem.handAnim.SetBool("isCharging", false);
            currentItem.handAnim.SetBool("isCharged", false);
            currentItem.handAnim.SetBool("isChargeAttack", false);
            currentItem.handAnim.SetBool("isChargeAttackEnd", false);
            foreach (Collider col in currentItem.cols)
            {
                col.enabled = false;
            }

            currentItem = inventory[currentSlot - 1];
            currentItem.gameObject.SetActive(true);
        }

        // ���� �������� ���������� Ȱ��ȭ
        if (currentItem.gameObject.activeSelf == false)
        {
            currentItem.gameObject.SetActive(true);
        }
    }

    // ������ ȹ��
    protected IEnumerator PickUpItemCoroutine()
    {
        // �ʿ��ϸ� "GameObject man001_this = hitObject" �����ؼ� ���

        for (int i = 1; i < inventory.Count; i++)
        {
            if (inventory[i] == empty)
            {
                preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ

                preObject.SetActive(false);
                preObject.transform.SetParent(handObject.transform);
                preObject.transform.localPosition = Vector3.zero;
                preObject.transform.localRotation = Quaternion.identity;
                preObject.GetComponent<Rigidbody>().useGravity = false;
                preObject.GetComponent<Rigidbody>().isKinematic = true;
                preObject.GetComponent<GetComponentScript>().outline.gameObject.layer = LayerMask.NameToLayer("Hand");
                preObject.GetComponent<Collider>().enabled = false;

                inventory[i] = preObject.GetComponent<Item>();
                inventory[i].GetComponent<Item>().enabled = true;
                break;
            }
        }

        yield return null;
    }
}
