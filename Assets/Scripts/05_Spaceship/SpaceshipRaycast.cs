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
    public RawImage fadeInOutImage; // ���̵�-��, �ƿ� �̹���
    public FadeInOutScript fadeInOutScript; // ���̵�-��, �ƿ� ��ũ��Ʈ

    // AudioSource
    // public AudioSource circusSong;

    public GameObject man001_check; // man001(��¯ �� ���) üũ��
    public GameObject guard001_check; // guard001 üũ��
    public GameObject[] items_check; // items üũ��



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

        // ���� �������� ���� ���� �����۰� �ٸ��� ��ü �� Ȱ��ȭ
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

        // ���� �������� ���������� Ȱ��ȭ
        if (currentItem.gameObject.activeSelf == false)
        {
            currentItem.gameObject.SetActive(true);
        }
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

    public void CheckObject()
    {
        isChecking = true;

        // ������
        if (isChecking)
        {
            foreach (GameObject i in items_check)
            {
                if (hitObject == i)
                {
                    if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    }

                    preObject = hitObject;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                    mouseText.text = GameManager.instance.textFileManager.ui[24]; // "��ȭ�ϱ�" �ؽ�Ʈ ����
                    mouseText.enabled = true;

                    // EŰ �Է� ��
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(PickUpItemCoroutine());
                    }

                    isChecking = false; // ������ �׸���� üũ���� ����

                    break;
                }
                else
                {
                    mouseText.enabled = false; // �ؽ�Ʈ ������

                    if (preObject != null)
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
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
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[24]; // "��ȭ�ϱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(man001Coroutine());
                }

                isChecking = false; // ������ �׸���� üũ���� ����
            }
            else
            {
                mouseText.enabled = false; // �ؽ�Ʈ ������

                if (preObject != null)
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    preObject = null;
                }
            }
        }

        // guard
        if (isChecking)
        {
            if (hitObject == guard001_check)
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[24]; // "��ȭ�ϱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(PickUpItemCoroutine());
                }

                isChecking = false; // ������ �׸���� üũ���� ����
            }
            else
            {
                mouseText.enabled = false; // �ؽ�Ʈ ������

                if (preObject != null)
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    preObject = null;
                }
            }
        }
    }




    // ������ ȹ��
    IEnumerator PickUpItemCoroutine()
    {
        // �ʿ��ϸ� "GameObject man001_this = hitObject" �����ؼ� ���

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ
        
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




    // man001 ��ȭ
    IEnumerator man001Coroutine()
    {
        // �ʿ��ϸ� "GameObject man001_this = hitObject" �����ؼ� ���

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ
        man001_check.GetComponent<HeadTracking>().isLooking = true;

        // ��ȭ
        if (true)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[19]["Content"], // "�� ����� �ΰ��� �츮 ������ ���� ���� ���ƹ��Ⱦ�"
                                                                   (string)GameManager.instance.textFileManager.dialog[20]["Content"], // "�Ʊ���� ��ӵǴ� ������ ���� ���� �� ����"
                                                                   (string)GameManager.instance.textFileManager.dialog[21]["Content"]}); // "�Ʒ����� ��ü ���� ���� ������ ����?"
        }

        yield return new WaitUntil(() => putDialogScript.isClickMode == false);

        man001_check.GetComponent<HeadTracking>().isLooking = false;
    }

    // guard001 ��ȭ
    IEnumerator guard001Coroutine()
    {
        // �ʿ��ϸ� "GameObject man001_this = hitObject" �����ؼ� ���

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ
        guard001_check.GetComponent<HeadTracking>().isLooking = true;

        if(currentItem.itemName.Equals("Gift"))
        {

        }
        else
        {
            // ��ȭ
            if (true)
            {
                putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[19]["Content"], // "�� ����� �ΰ��� �츮 ������ ���� ���� ���ƹ��Ⱦ�"
                                                                       (string)GameManager.instance.textFileManager.dialog[20]["Content"], // "�Ʊ���� ��ӵǴ� ������ ���� ���� �� ����"
                                                                       (string)GameManager.instance.textFileManager.dialog[21]["Content"]}); // "�Ʒ����� ��ü ���� ���� ������ ����?"
            }

            yield return new WaitUntil(() => putDialogScript.isClickMode == false);

            guard001_check.GetComponent<HeadTracking>().isLooking = false;
        }
    }
}
