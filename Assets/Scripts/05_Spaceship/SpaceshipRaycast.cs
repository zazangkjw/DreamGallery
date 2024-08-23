using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipRaycast : MonoBehaviour
{
    public GameObject itemCategory;
    public Item currentItem;
    public int currentSlot;
    public GameObject handObject;
    public Item empty;
    public Item[] items;

    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;
    bool isChecking = true;

    public DefaultSceneManager defaultSceneManager;
    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트

    // AudioSource
    // public AudioSource circusSong;

    public GameObject man001_check; // man001(팔짱 낀 사람) 체크용
    public GameObject guard001_check; // guard001 체크용
    public GameObject[] items_check; // items 체크용



    void Start()
    {
        items = new Item[3];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = empty;
        }

        currentSlot = 1;
        currentItem = empty;
    }

    void Update()
    {
        SwitchItem();
        ShootRaycast();
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

            currentItem.chargeTimer = 0;
            currentItem.handAnim.SetBool("isReady", false);
            currentItem.handAnim.SetBool("isCanceled", false);
            currentItem.handAnim.SetBool("isCharging", false);
            currentItem.handAnim.SetBool("isCharged", false);
            currentItem.handAnim.SetBool("isChargeAttack", false);
            foreach (Collider col in currentItem.cols)
            {
                col.enabled = false;
            }

            currentItem = empty;
            items[currentSlot - 1] = empty;
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
        if (currentItem != items[currentSlot - 1])
        {
            currentItem.gameObject.SetActive(false);

            currentItem.chargeTimer = 0;
            currentItem.handAnim.SetBool("isReady", false);
            currentItem.handAnim.SetBool("isCanceled", false);
            currentItem.handAnim.SetBool("isCharging", false);
            currentItem.handAnim.SetBool("isCharged", false);
            currentItem.handAnim.SetBool("isChargeAttack", false);
            foreach(Collider col in currentItem.cols)
            {
                col.enabled = false;
            }
            
            currentItem = items[currentSlot - 1];
            currentItem.gameObject.SetActive(true);
        }

        // 현재 아이템이 꺼져있으면 활성화
        if (currentItem.gameObject.activeSelf == false)
        {
            currentItem.gameObject.SetActive(true);
        }
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

    public void CheckObject()
    {
        isChecking = true;

        // 아이템
        if (isChecking)
        {
            foreach (GameObject i in items_check)
            {
                if (hitObject == i)
                {
                    if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    }

                    preObject = hitObject;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                    mouseText.text = GameManager.instance.textFileManager.ui[24]; // "대화하기" 텍스트 나옴
                    mouseText.enabled = true;

                    // E키 입력 시
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(PickUpItemCoroutine());
                    }

                    isChecking = false; // 이후의 항목들은 체크하지 않음

                    break;
                }
                else
                {
                    mouseText.enabled = false; // 텍스트 없어짐

                    if (preObject != null)
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                        preObject = null;
                    }
                }
            }
        }

        // man001
        if (isChecking)
        {
            if (hitObject == man001_check)
            {
                if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                mouseText.text = GameManager.instance.textFileManager.ui[24]; // "대화하기" 텍스트 나옴
                mouseText.enabled = true;

                // E키 입력 시
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(man001Coroutine());
                }

                isChecking = false; // 이후의 항목들은 체크하지 않음
            }
            else
            {
                mouseText.enabled = false; // 텍스트 없어짐

                if (preObject != null)
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    preObject = null;
                }
            }
        }

        // guard
        if (isChecking)
        {
            if (hitObject == guard001_check)
            {
                if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                mouseText.text = GameManager.instance.textFileManager.ui[24]; // "대화하기" 텍스트 나옴
                mouseText.enabled = true;

                // E키 입력 시
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(PickUpItemCoroutine());
                }

                isChecking = false; // 이후의 항목들은 체크하지 않음
            }
            else
            {
                mouseText.enabled = false; // 텍스트 없어짐

                if (preObject != null)
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    preObject = null;
                }
            }
        }
    }




    // 아이템 획득
    IEnumerator PickUpItemCoroutine()
    {
        // 필요하면 "GameObject man001_this = hitObject" 생성해서 사용

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화
        
        preObject.SetActive(false);
        preObject.transform.SetParent(handObject.transform);
        preObject.transform.localPosition = Vector3.zero;
        preObject.transform.localRotation = Quaternion.identity;
        preObject.GetComponent<Rigidbody>().useGravity = false;
        preObject.GetComponent<Rigidbody>().isKinematic = true;
        preObject.GetComponent<GetComponentScript>().outline.gameObject.layer = LayerMask.NameToLayer("Hand");
        preObject.GetComponent<Collider>().enabled = false;

        for (int i = 1; i < items.Length; i++)
        {
            if (items[i] == empty)
            {
                items[i] = preObject.GetComponent<Item>();
                items[i].GetComponent<Item>().enabled = true;
                break;
            }
        }

        yield return null;
    }




    // man001 대화
    IEnumerator man001Coroutine()
    {
        // 필요하면 "GameObject man001_this = hitObject" 생성해서 사용

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화
        man001_check.GetComponent<HeadTracking>().isLooking = true;

        // 대화
        if (true)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[19]["Content"], // "이 물고기 인간이 우리 방으로 가는 길을 막아버렸어"
                                                                   (string)GameManager.instance.textFileManager.dialog[20]["Content"], // "아까부터 계속되는 진동에 겁을 먹은 것 같아"
                                                                   (string)GameManager.instance.textFileManager.dialog[21]["Content"]}); // "아래층에 대체 무슨 일이 벌어진 거지?"
        }

        yield return new WaitUntil(() => putDialogScript.isClickMode == false);

        man001_check.GetComponent<HeadTracking>().isLooking = false;
    }

    // guard001 대화
    IEnumerator guard001Coroutine()
    {
        // 필요하면 "GameObject man001_this = hitObject" 생성해서 사용

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화
        guard001_check.GetComponent<HeadTracking>().isLooking = true;

        if(currentItem.itemName.Equals("Gift"))
        {

        }
        else
        {
            // 대화
            if (true)
            {
                putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[19]["Content"], // "이 물고기 인간이 우리 방으로 가는 길을 막아버렸어"
                                                                       (string)GameManager.instance.textFileManager.dialog[20]["Content"], // "아까부터 계속되는 진동에 겁을 먹은 것 같아"
                                                                       (string)GameManager.instance.textFileManager.dialog[21]["Content"]}); // "아래층에 대체 무슨 일이 벌어진 거지?"
            }

            yield return new WaitUntil(() => putDialogScript.isClickMode == false);

            guard001_check.GetComponent<HeadTracking>().isLooking = false;
        }
    }
}
