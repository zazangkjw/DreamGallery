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
    public List<GameObject> items_check = new List<GameObject>(); // items 체크용

    public GameObject inventory;
    public GameObject inventorySlot;
    public GameObject inventory_quickSlot;
    public GameObject quickSlot;

    public List<GameObject> inventorySlots = new List<GameObject>(); // 인벤토리
    public List<GameObject> inventory_quickSlots = new List<GameObject>(); // 인벤토리 퀵슬롯
    public List<GameObject> quickSlots = new List<GameObject>(); // 퀵슬롯

    public int currentSlot;
    public Item currentItem;
    public List<Item> myItems = new List<Item>(); // 보유 아이템들

    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;
    public WaitForSeconds dialogDelay = new WaitForSeconds(3f);
    protected bool isChecking = true;

    public DefaultSceneManager defaultSceneManager;
    public PlayerController playerController;
    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트




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




    // 카메라에서 레이캐스트 쏘기
    public void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f) && !defaultSceneManager.isPausing && !putDialogScript.isClickMode)
        {
            hitObject = hitInfo.collider.gameObject;
        }
        // 허공일 때
        else
        {
            hitObject = null;
        }

        CheckObject();
    }

    public virtual void CheckObject()
    {

    }




    // 인벤토리 메서드
    public void Inventory()
    {
        if (Input.GetKeyDown(KeyCode.I) && inventory != null && !defaultSceneManager.isPausing)
        {
            if (inventory.activeSelf) // 인벤토리 켜져있으면
            {
                inventory.SetActive(false);
                playerController.isMouseLocked = false;
                Cursor.visible = false;
            }
            else // 인벤토리 꺼져있으면
            {
                inventory.SetActive(true);
                playerController.isMouseLocked = true;
                Cursor.visible = true;
            }
        }
    }




    // 아이템 스위칭
    public void SwitchItem()
    {
        // 현재 아이템 버리기
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

        // 숫자키로 퀵슬롯 바꾸기
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

        // 현재 아이템이 현재 슬롯 아이템과 다르면 교체 및 활성화
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

        // 현재 아이템이 꺼져있으면 활성화
        if (currentItem.gameObject.activeSelf == false)
        {
            currentItem.gameObject.SetActive(true);
        }
    }

    // 아이템 획득
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
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화

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
                else if (i == myItems.Count - 1) // 끝까지 돌았는데 공간이 없을 경우
                {
                    putDialogScript.putDialog((string)GameManager.instance.textFileManager.dialog[26]["Content"], 1f); // "가방에 더 이상 공간이 없다"
                }
            }
        }
        else
        {
            // 획득 불가 아이템
        }
        
        yield return null;
    }
}
