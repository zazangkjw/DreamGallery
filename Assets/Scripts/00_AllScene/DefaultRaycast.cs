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

    public GameObject inventory;
    public GameObject inventorySlot;
    public GameObject inventory_quickSlot;
    public GameObject quickSlot;

    public List<Slot> inventorySlots = new List<Slot>(); // �κ��丮
    public List<Slot> inventory_quickSlots = new List<Slot>(); // �κ��丮 ������
    public List<Slot> quickSlots = new List<Slot>(); // ������
    public Item currentItem;

    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;
    public WaitForSeconds dialogDelay = new WaitForSeconds(3f);
    protected bool isChecking = true;

    public DefaultSceneManager defaultSceneManager;
    public PlayerController playerController;
    public RawImage fadeInOutImage; // ���̵�-��, �ƿ� �̹���
    public FadeInOutScript fadeInOutScript; // ���̵�-��, �ƿ� ��ũ��Ʈ




    public virtual void WhenStart()
    {
        if(quickSlot != null)
        {
            for (int i = 0; i < inventory_quickSlot.transform.childCount; i++)
            {
                inventory_quickSlots.Add(inventory_quickSlot.transform.GetChild(i).GetComponent<Slot>());
                inventory_quickSlots[i].item = empty;
                inventory_quickSlots[i].index = i;

                quickSlots.Add(quickSlot.transform.GetChild(i).GetComponent<Slot>());
                quickSlots[i].item = empty;
                quickSlots[i].index = i;
            }

            for (int i = 0; i < inventorySlot.transform.childCount; i++)
            {
                inventorySlots.Add(inventorySlot.transform.GetChild(i).GetComponent<Slot>());
                inventorySlots[i].item = empty;
                inventorySlots[i].index = i + quickSlots.Count;
            }

            Slot.currentIndex = 1;
            currentItem = inventory_quickSlots[Slot.currentIndex - 1].item;
            quickSlots[Slot.currentIndex - 1].backgroundImage.gameObject.SetActive(true);

            items_check.AddRange(GameObject.FindGameObjectsWithTag("Item"));
        }
    }

    public virtual void WhenUpdate()
    {
        SwitchItem();
        ShootRaycast();
        Inventory();
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




    // �κ��丮 �޼���
    public void Inventory()
    {
        if (Input.GetKeyDown(KeyCode.I) && inventory != null && !defaultSceneManager.isPausing)
        {
            if (inventory.activeSelf) // �κ��丮 ����������
            {
                inventory.SetActive(false);
                playerController.isMouseLocked = false;
                Cursor.visible = false;
            }
            else // �κ��丮 ����������
            {
                inventory.SetActive(true);
                playerController.isMouseLocked = true;
                Cursor.visible = true;
            }
        }
    }




    // ������ ����Ī
    public void SwitchItem()
    {
        // ���� ������ ������
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

            inventory_quickSlots[Slot.currentIndex - 1].item = empty;
            inventory_quickSlots[Slot.currentIndex - 1].slotImage.texture = null;
            inventory_quickSlots[Slot.currentIndex - 1].slotImage.gameObject.SetActive(false);

            quickSlots[Slot.currentIndex - 1].item = empty;
            quickSlots[Slot.currentIndex - 1].slotImage.texture = null;
            quickSlots[Slot.currentIndex - 1].slotImage.gameObject.SetActive(false);
        }

        // ����Ű�� ������ �ٲٱ�
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Slot.currentIndex = 1;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].backgroundImage.gameObject.SetActive(false);
            }
            quickSlots[Slot.currentIndex - 1].backgroundImage.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Slot.currentIndex = 2;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].backgroundImage.gameObject.SetActive(false);
            }
            quickSlots[Slot.currentIndex - 1].backgroundImage.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Slot.currentIndex = 3;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].backgroundImage.gameObject.SetActive(false);
            }
            quickSlots[Slot.currentIndex - 1].backgroundImage.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Slot.currentIndex = 4;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].backgroundImage.gameObject.SetActive(false);
            }
            quickSlots[Slot.currentIndex - 1].backgroundImage.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Slot.currentIndex = 5;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].backgroundImage.gameObject.SetActive(false);
            }
            quickSlots[Slot.currentIndex - 1].backgroundImage.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Slot.currentIndex = 6;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].backgroundImage.gameObject.SetActive(false);
            }
            quickSlots[Slot.currentIndex - 1].backgroundImage.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Slot.currentIndex = 7;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].backgroundImage.gameObject.SetActive(false);
            }
            quickSlots[Slot.currentIndex - 1].backgroundImage.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Slot.currentIndex = 8;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].backgroundImage.gameObject.SetActive(false);
            }
            quickSlots[Slot.currentIndex - 1].backgroundImage.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Slot.currentIndex = 9;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].backgroundImage.gameObject.SetActive(false);
            }
            quickSlots[Slot.currentIndex - 1].backgroundImage.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Slot.currentIndex = 10;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].backgroundImage.gameObject.SetActive(false);
            }
            quickSlots[Slot.currentIndex - 1].backgroundImage.gameObject.SetActive(true);
        }

        // ���� �������� ���� ���� �����۰� �ٸ��� ��ü �� Ȱ��ȭ
        if (currentItem != inventory_quickSlots[Slot.currentIndex - 1].item)
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

            currentItem = inventory_quickSlots[Slot.currentIndex - 1].item;
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
        if (preObject.GetComponent<Item>().isObtainable)
        {
            for (int i = 0; i < inventory_quickSlots.Count + inventorySlots.Count; i++)
            {
                if (inventory_quickSlots[Slot.currentIndex - 1].item == empty)
                {
                    i = Slot.currentIndex - 1;
                }

                if (i < inventory_quickSlots.Count)
                {
                    if (inventory_quickSlots[i].item == empty)
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

                        inventory_quickSlots[i].item = preObject.GetComponent<Item>();
                        inventory_quickSlots[i].item.enabled = true;
                        inventory_quickSlots[i].slotImage.texture = inventory_quickSlots[i].item.itemImage;
                        inventory_quickSlots[i].slotImage.gameObject.SetActive(true);

                        quickSlots[i].slotImage.texture = inventory_quickSlots[i].item.itemImage;
                        quickSlots[i].slotImage.gameObject.SetActive(true);

                        break;
                    }
                }
                else
                {
                    if (inventorySlots[i - inventory_quickSlots.Count].item == empty)
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

                        inventorySlots[i - inventory_quickSlots.Count].item = preObject.GetComponent<Item>();
                        inventorySlots[i - inventory_quickSlots.Count].item.enabled = true;
                        inventorySlots[i - inventory_quickSlots.Count].slotImage.texture = inventorySlots[i - inventory_quickSlots.Count].item.itemImage;
                        inventorySlots[i - inventory_quickSlots.Count].slotImage.gameObject.SetActive(true);

                        break;
                    }
                    else if (i == inventory_quickSlots.Count + inventorySlots.Count - 1) // ������ ���Ҵµ� ������ ���� ���
                    {
                        putDialogScript.putDialog((string)GameManager.instance.textFileManager.dialog[26]["Content"], 1f); // "���濡 �� �̻� ������ ����"
                    }
                }
            }
        }
        else
        {
            // ȹ�� �Ұ� ������
        }
        
        yield return null;
    }
}
