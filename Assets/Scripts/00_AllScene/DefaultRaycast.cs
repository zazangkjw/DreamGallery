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

    public int inventorySize;
    public List<Item> inventory = new List<Item>(); // 인벤토리
    public int currentSlot;
    public Item currentItem;

    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;
    protected bool isChecking = true;

    public DefaultSceneManager defaultSceneManager;
    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트




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




    // 아이템 스위칭
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

        // 현재 아이템이 현재 슬롯 아이템과 다르면 교체 및 활성화
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

        // 현재 아이템이 꺼져있으면 활성화
        if (currentItem.gameObject.activeSelf == false)
        {
            currentItem.gameObject.SetActive(true);
        }
    }

    // 아이템 획득
    protected IEnumerator PickUpItemCoroutine()
    {
        // 필요하면 "GameObject man001_this = hitObject" 생성해서 사용

        for (int i = 1; i < inventory.Count; i++)
        {
            if (inventory[i] == empty)
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

                inventory[i] = preObject.GetComponent<Item>();
                inventory[i].GetComponent<Item>().enabled = true;
                break;
            }
        }

        yield return null;
    }
}
