using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefaultRaycast : MonoBehaviour
{
    public GameObject crosshair;
    public GameObject handObject;
    public Item empty;
    public GameObject itemCategory;
    //public List<GameObject> items_check = new List<GameObject>(); // items 체크용
    GameObject forInstantiate;

    public bool inventory_enabled = true;
    public static bool inventoryOnOff;
    public RawImage cursorImage;
    public GameObject inventory;
    public GameObject inventorySlot;
    public GameObject inventory_quickSlot;
    public GameObject quickSlot;

    public List<Slot> inventorySlots = new List<Slot>(); // 인벤토리
    public List<Slot> inventory_quickSlots = new List<Slot>(); // 인벤토리 퀵슬롯
    public List<Slot> quickSlots = new List<Slot>(); // 퀵슬롯
    public Item currentItem;

    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;
    public WaitForSeconds dialogDelay = new WaitForSeconds(3f);
    protected bool isChecking = true;

    public Canvas canvas;
    GraphicRaycaster graphicRaycaster;
    PointerEventData pointerEventData;
    List<RaycastResult> result = new List<RaycastResult>();

    public DefaultSceneManager defaultSceneManager;
    public PlayerController playerController;
    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트




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

                quickSlots[i].connectedSlot = inventory_quickSlots[i];
            }

            for (int i = 0; i < inventorySlot.transform.childCount; i++)
            {
                inventorySlots.Add(inventorySlot.transform.GetChild(i).GetComponent<Slot>());
                inventorySlots[i].item = empty;
                inventorySlots[i].index = i + quickSlots.Count;
            }

            Slot.cursorImage = cursorImage;
            Slot.currentIndex = 1;
            currentItem = inventory_quickSlots[Slot.currentIndex - 1].item;
            quickSlots[Slot.currentIndex - 1].backgroundImage.gameObject.SetActive(true);

            //items_check.AddRange(GameObject.FindGameObjectsWithTag("Item"));

            graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
            pointerEventData = new PointerEventData(null);
        }
    }

    public virtual void WhenUpdate()
    {
        SwitchItem();
        ShootRaycast();
        Inventory();
        CursorImageFollowMouse();
    }




    // 카메라에서 레이캐스트 쏘기
    public void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f) && !DefaultSceneManager.isPausing && !putDialogScript.isClickMode)
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
        if (inventory_enabled && Input.GetKeyDown(KeyCode.I) && inventory != null && !DefaultSceneManager.isPausing)
        {
            if (inventory.activeSelf) // 인벤토리 켜져있으면
            {
                inventoryOnOff = false;
                inventory.SetActive(false);
                playerController.isMouseLocked = false;
                Cursor.visible = false;

                Slot.selectedSlot = null;
                Slot.cursorImage.texture = null;
                Slot.cursorImage.gameObject.SetActive(false);
            }
            else // 인벤토리 꺼져있으면
            {
                inventoryOnOff = true;
                inventory.SetActive(true);
                playerController.isMouseLocked = true;
                Cursor.visible = true;
            }
        }
    }




    // 아이템 스위칭
    public void SwitchItem()
    {
        if (quickSlot != null && !DefaultSceneManager.isPausing)
        {
            // 현재 아이템 버리기
            if (currentItem != empty && Input.GetKeyDown(KeyCode.G))
            {
                currentItem.enabled = false;
                currentItem.transform.SetParent(itemCategory.transform);
                currentItem.transform.position = transform.position;
                foreach (GameObject o in currentItem.GetComponent<GetComponentScript>().objects)
                {
                    o.layer = LayerMask.NameToLayer("Default");
                }
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

            // 인벤토리가 켜있을 때
            if (inventoryOnOff)
            {
                // 마우스 좌클릭
                if (Input.GetMouseButtonDown(0))
                {
                    result.Clear();
                    pointerEventData.position = Input.mousePosition;
                    graphicRaycaster.Raycast(pointerEventData, result);

                    // 선택된 아이템이 있으면
                    if (Slot.selectedSlot != null)
                    {
                        // UI 밖을 클릭하면 버리기
                        if (result.Count == 0)
                        {
                            // 선택한 아이템이 지금 손에 들고 있는 아이템이면 손에 들고 있는 아이템을 empty로 바꾸기
                            if(Slot.selectedSlot.item == currentItem)
                            {
                                currentItem = empty;
                            }

                            Slot.selectedSlot.item.enabled = false;
                            Slot.selectedSlot.item.transform.SetParent(itemCategory.transform);
                            Slot.selectedSlot.item.transform.position = transform.position;
                            foreach (GameObject o in Slot.selectedSlot.item.GetComponent<GetComponentScript>().objects)
                            {
                                o.layer = LayerMask.NameToLayer("Default");
                            }
                            Slot.selectedSlot.item.GetComponent<Collider>().enabled = true;
                            Slot.selectedSlot.item.GetComponent<Rigidbody>().useGravity = true;
                            Slot.selectedSlot.item.GetComponent<Rigidbody>().isKinematic = false;
                            Slot.selectedSlot.item.gameObject.SetActive(true);

                            Slot.selectedSlot.item.handAnim.SetFloat("ChargeTimer", 0f);
                            Slot.selectedSlot.item.handAnim.SetBool("isReady", false);
                            Slot.selectedSlot.item.handAnim.SetBool("isCanceled", false);
                            Slot.selectedSlot.item.handAnim.SetBool("isCharging", false);
                            Slot.selectedSlot.item.handAnim.SetBool("isCharged", false);
                            Slot.selectedSlot.item.handAnim.SetBool("isChargeAttack", false);
                            Slot.selectedSlot.item.handAnim.SetBool("isChargeAttackEnd", false);
                            foreach (Collider col in Slot.selectedSlot.item.cols)
                            {
                                col.enabled = false;
                            }

                            Slot.selectedSlot.item = empty;
                            Slot.selectedSlot.slotImage.texture = null;
                            Slot.selectedSlot.slotImage.gameObject.SetActive(false);
                            Slot.selectedSlot = null;

                            cursorImage.texture = null;
                            cursorImage.gameObject.SetActive(false);
                        }
                    }
                }
            }

            // 휠로 퀵슬롯 바꾸기
            int preIndex = Slot.currentIndex;
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (wheelInput > 0)
            {
                // 휠을 밀어 돌렸을 때의 처리 ↑
                Slot.currentIndex = Slot.currentIndex <= 1 ? Slot.currentIndex : Slot.currentIndex - 1;
            }
            else if (wheelInput < 0)
            {
                // 휠을 당겨 올렸을 때의 처리 ↓   
                Slot.currentIndex = Slot.currentIndex >= quickSlots.Count ? Slot.currentIndex : Slot.currentIndex + 1;
            }

            // 숫자키로 퀵슬롯 바꾸기
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Slot.currentIndex = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Slot.currentIndex = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Slot.currentIndex = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Slot.currentIndex = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Slot.currentIndex = 5;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Slot.currentIndex = 6;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                Slot.currentIndex = 7;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                Slot.currentIndex = 8;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                Slot.currentIndex = 9;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Slot.currentIndex = 10;
            }

            // 퀵슬롯 적용
            if (preIndex != Slot.currentIndex)
            {
                foreach (Slot slot in quickSlots)
                {
                    slot.backgroundImage.gameObject.SetActive(false);
                }
                quickSlots[Slot.currentIndex - 1].backgroundImage.gameObject.SetActive(true);
            }

            // 현재 아이템이 현재 슬롯 아이템과 다르면 교체 및 활성화
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
                crosshair.SetActive(false);
                currentItem.gameObject.SetActive(true);
            }

            // 현재 아이템이 꺼져있으면 활성화
            if (currentItem.gameObject.activeSelf == false)
            {
                crosshair.SetActive(false);
                currentItem.gameObject.SetActive(true);
            }
        }
    }

    // 아이템 획득
    public void PickUpItemCoroutine()
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
                        if (!preObject.GetComponent<Item>().isStack)
                        {
                            inventory_quickSlots[i].item = preObject.GetComponent<Item>();
                        }
                        else
                        {
                            preObject.GetComponent<GetComponentScript>().outline.enabled = false;
                            forInstantiate = Instantiate(preObject.GetComponent<Item>().prefab);
                            preObject.GetComponent<GetComponentScript>().outline.enabled = true;
                            forInstantiate.GetComponent<Item>().isStack = false;
                            inventory_quickSlots[i].item = forInstantiate.GetComponent<Item>();
                        }

                        inventory_quickSlots[i].item.gameObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화
                        inventory_quickSlots[i].item.gameObject.SetActive(false);
                        inventory_quickSlots[i].item.gameObject.transform.SetParent(handObject.transform);
                        inventory_quickSlots[i].item.gameObject.transform.localPosition = Vector3.zero;
                        inventory_quickSlots[i].item.gameObject.transform.localRotation = Quaternion.identity;
                        inventory_quickSlots[i].item.gameObject.GetComponent<Rigidbody>().useGravity = false;
                        inventory_quickSlots[i].item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        foreach(GameObject o in inventory_quickSlots[i].item.gameObject.GetComponent<GetComponentScript>().objects)
                        {
                            o.layer = LayerMask.NameToLayer("Hand");
                        }
                        inventory_quickSlots[i].item.gameObject.GetComponent<Collider>().enabled = false;

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
                        if (!preObject.GetComponent<Item>().isStack)
                        {
                            inventorySlots[i - inventory_quickSlots.Count].item = preObject.GetComponent<Item>();
                        }
                        else
                        {
                            preObject.GetComponent<GetComponentScript>().outline.enabled = false;
                            forInstantiate = Instantiate(preObject.GetComponent<Item>().prefab);
                            preObject.GetComponent<GetComponentScript>().outline.enabled = true;
                            forInstantiate.GetComponent<Item>().isStack = false;
                            inventorySlots[i - inventory_quickSlots.Count].item = forInstantiate.GetComponent<Item>();
                        }

                        inventorySlots[i - inventory_quickSlots.Count].item.gameObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화
                        inventorySlots[i - inventory_quickSlots.Count].item.gameObject.SetActive(false);
                        inventorySlots[i - inventory_quickSlots.Count].item.gameObject.transform.SetParent(handObject.transform);
                        inventorySlots[i - inventory_quickSlots.Count].item.gameObject.transform.localPosition = Vector3.zero;
                        inventorySlots[i - inventory_quickSlots.Count].item.gameObject.transform.localRotation = Quaternion.identity;
                        inventorySlots[i - inventory_quickSlots.Count].item.gameObject.GetComponent<Rigidbody>().useGravity = false;
                        inventorySlots[i - inventory_quickSlots.Count].item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        foreach (GameObject o in inventorySlots[i - inventory_quickSlots.Count].item.gameObject.GetComponent<GetComponentScript>().objects)
                        {
                            o.layer = LayerMask.NameToLayer("Hand");
                        }
                        inventorySlots[i - inventory_quickSlots.Count].item.gameObject.GetComponent<Collider>().enabled = false;

                        inventorySlots[i - inventory_quickSlots.Count].item.enabled = true;
                        inventorySlots[i - inventory_quickSlots.Count].slotImage.texture = inventorySlots[i - inventory_quickSlots.Count].item.itemImage;
                        inventorySlots[i - inventory_quickSlots.Count].slotImage.gameObject.SetActive(true);

                        break;
                    }
                    else if (i == inventory_quickSlots.Count + inventorySlots.Count - 1) // 끝까지 돌았는데 공간이 없을 경우
                    {
                        putDialogScript.putDialog((string)GameManager.instance.textFileManager.dialog[26]["Content"], 1f); // "가방에 더 이상 공간이 없다"
                    }
                }
            }
        }
        else
        {
            // 획득 불가 아이템
        }
    }

    public void CursorImageFollowMouse()
    {
        if (cursorImage != null)
        {
            cursorImage.transform.position = Input.mousePosition;
        }
    }
}
