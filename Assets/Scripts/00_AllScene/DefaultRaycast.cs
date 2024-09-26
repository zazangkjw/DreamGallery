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

    public List<GameObject> inventorySlots = new List<GameObject>(); // �κ��丮
    public List<GameObject> inventory_quickSlots = new List<GameObject>(); // �κ��丮 ������
    public List<GameObject> quickSlots = new List<GameObject>(); // ������

    public int currentSlot;
    public Item currentItem;
    public List<Item> myItems = new List<Item>(); // ���� �����۵�

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
            for (int i = 0; i < inventorySlot.transform.childCount; i++)
            {
                inventorySlots.Add(inventorySlot.transform.GetChild(i).gameObject);
                myItems.Add(empty);
            }

            for (int i = 0; i < inventory_quickSlot.transform.childCount; i++)
            {
                inventory_quickSlots.Add(inventory_quickSlot.transform.GetChild(i).gameObject);
                quickSlots.Add(quickSlot.transform.GetChild(i).gameObject);
                myItems.Add(empty);
            }

            currentSlot = 1;
            currentItem = empty;
            quickSlots[currentSlot - 1].transform.GetChild(1).gameObject.SetActive(true);

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
            myItems[currentSlot - 1] = empty;
            inventory_quickSlots[currentSlot - 1].transform.GetChild(0).GetComponent<RawImage>().color = Color.clear;
            quickSlots[currentSlot - 1].transform.GetChild(0).GetComponent<RawImage>().color = Color.clear;
        }

        // ����Ű�� ������ �ٲٱ�
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentSlot = 1;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            quickSlots[currentSlot - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentSlot = 2;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            quickSlots[currentSlot - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentSlot = 3;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            quickSlots[currentSlot - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentSlot = 4;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            quickSlots[currentSlot - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentSlot = 5;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            quickSlots[currentSlot - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentSlot = 6;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            quickSlots[currentSlot - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentSlot = 7;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            quickSlots[currentSlot - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            currentSlot = 8;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            quickSlots[currentSlot - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            currentSlot = 9;
            for (int i = 0; i < quickSlots.Count; i++)
            {
                quickSlots[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            quickSlots[currentSlot - 1].transform.GetChild(1).gameObject.SetActive(true);
        }

        // ���� �������� ���� ���� �����۰� �ٸ��� ��ü �� Ȱ��ȭ
        if (currentItem != myItems[currentSlot - 1])
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

            currentItem = myItems[currentSlot - 1];
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
            for (int i = 0; i < myItems.Count; i++)
            {
                if (myItems[currentSlot - 1] == empty)
                {
                    i = currentSlot - 1;
                }

                if (myItems[i] == empty)
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

                    myItems[i] = preObject.GetComponent<Item>();
                    myItems[i].GetComponent<Item>().enabled = true;

                    if (i < quickSlots.Count)
                    {
                        inventory_quickSlots[i].transform.GetChild(0).GetComponent<RawImage>().texture = myItems[i].itemImage;
                        inventory_quickSlots[i].transform.GetChild(0).GetComponent<RawImage>().color = Color.white;
                        quickSlots[i].transform.GetChild(0).GetComponent<RawImage>().texture = myItems[i].itemImage;
                        quickSlots[i].transform.GetChild(0).GetComponent<RawImage>().color = Color.white;
                    }
                    else
                    {
                        inventorySlots[i - inventory_quickSlots.Count].transform.GetChild(0).GetComponent<RawImage>().texture = myItems[i].itemImage;
                        inventorySlots[i - inventory_quickSlots.Count].transform.GetChild(0).GetComponent<RawImage>().color = Color.white;
                    }

                    break;
                }
                else if (i == myItems.Count - 1) // ������ ���Ҵµ� ������ ���� ���
                {
                    putDialogScript.putDialog((string)GameManager.instance.textFileManager.dialog[26]["Content"], 1f); // "���濡 �� �̻� ������ ����"
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
