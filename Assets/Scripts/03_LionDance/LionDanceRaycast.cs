using TMPro;
using UnityEngine;

public class LionDanceRaycast : MonoBehaviour
{
    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;

    public GameObject[] doorAndWindow; // ��, â�� (��)������Ʈ �迭 �ڵ� ����
    public GameObject balconyWindow; // ���ڴ� â�� ������Ʈ
    public GameObject balconyDoor; // ���ڴ� �� ������Ʈ
    public GameObject kitchenBalconyWindow1; // �ξ� ���ڴ� â�� ������Ʈ1
    public GameObject kitchenBalconyWindow2; // �ξ� ���ڴ� â�� ������Ʈ2
    int openCount;

    public Animator lionMonsterAnimator;
    public LionDanceSceneManager lionDanceSceneManager;
    public LionDanceColliderTrigger lionDanceColliderTrigger;
    public PlayerController playerController;
    public Camera playerCam;
    public GameObject player;
    public Material fogMat;

    void Start()
    {
        openCount = 0;
    }

    void Update()
    {
        ShootRaycast();
    }

    // ī�޶󿡼� ����ĳ��Ʈ ���
    public void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 1.5f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.5f) && !lionDanceSceneManager.isPausing && !putDialogScript.isClickMode)
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
        // step 0�� ��. ��, â�� �� ����
        if (lionDanceColliderTrigger.step == 0)
        {
            foreach(GameObject doorWindow in doorAndWindow)
            {
                if (hitObject == doorWindow)
                {
                    if (preObject != hitObject.GetComponent<GetComponentScript>().mesh && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                    {
                        preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                        preObject = null;
                    }

                    preObject = hitObject.GetComponent<GetComponentScript>().mesh;
                    preObject.GetComponent<Outline>().enabled = true; // �ܰ��� �ѱ�

                    mouseText.text = GameManager.instance.textFileManager.ui[14]; // "����/�ݱ�" �ؽ�Ʈ ����
                    mouseText.enabled = true;

                    // EŰ �Է� ��
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // ��, â�� ����
                        if (!hitObject.GetComponent<GetComponentScript>().animator.GetBool("Active")) // ���������� ����
                        {
                            hitObject.GetComponent<GetComponentScript>().animator.SetBool("Active", true);
                            openCount++;
                        }
                        else // ���������� �ݱ�
                        {
                            hitObject.GetComponent<GetComponentScript>().animator.SetBool("Active", false);
                            openCount--;
                        }
                        hitObject.GetComponent<AudioSource>().Play();

                        // ��, â�� �� ���ȴ��� üũ�ϰ� ���� ��������
                        if (openCount == 10)
                        {
                            lionMonsterAnimator.Play("Walking", 1);
                            lionDanceColliderTrigger.step = 1;
                            mouseText.enabled = false;
                            preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                            preObject = null;
                            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[2]["Content"], 3f); // �ξ����� ���ư��ڴ� ��� ���
                            lionDanceSceneManager.FogOut();
                        }
                    }
                    break;
                }
                else
                {
                    mouseText.enabled = false; // �ؽ�Ʈ ������

                    if (preObject != null)
                    {
                        preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                        preObject = null;
                    }
                }
            }
        }

        // step 3�� ��. ���ڴ� â�� �ݱ�
        else if (lionDanceColliderTrigger.step == 3)
        {
            if (hitObject == balconyWindow)
            {
                if (preObject != hitObject.GetComponent<GetComponentScript>().mesh && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                    preObject = null;
                }

                preObject = hitObject.GetComponent<GetComponentScript>().mesh;
                preObject.GetComponent<Outline>().enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[14]; // "����/�ݱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    balconyWindow.GetComponent<GetComponentScript>().animator.SetBool("Active", false);
                    balconyWindow.GetComponent<AudioSource>().Play();
                    mouseText.enabled = false;
                    preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                    preObject = null;
                    lionMonsterAnimator.Play("Balcony");
                    lionDanceColliderTrigger.step = 4;

                    Debug.Log("���ڴ� â�� ����");
                }
            }
            else
            {
                mouseText.enabled = false; // �ؽ�Ʈ ������

                if (preObject != null)
                {
                    preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                    preObject = null;
                }
            }
        }

        // step 9�� ��. �ξ� ���ڴ� â�� �ݱ�
        else if (lionDanceColliderTrigger.step == 9)
        {
            if (hitObject == kitchenBalconyWindow1 || hitObject == kitchenBalconyWindow2)
            {
                if (preObject != hitObject.GetComponent<GetComponentScript>().mesh && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                    preObject = null;
                }

                preObject = hitObject.GetComponent<GetComponentScript>().mesh;
                preObject.GetComponent<Outline>().enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[14]; // "����/�ݱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hitObject.GetComponent<GetComponentScript>().animator.SetBool("Active", false);
                    hitObject.GetComponent<AudioSource>().Play();
                    lionDanceColliderTrigger.step = 10;
                    mouseText.enabled = false;
                    preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                    preObject = null;

                    Debug.Log("�ξ� â�� ����");

                    lionMonsterAnimator.Play("ClimbDown"); // ������ �ξ� â�� �ۿ��� �Ʒ������� �������� �ִϸ��̼� ���
                    Debug.Log("������ �ξ� â�� �ۿ��� �Ʒ������� �������� �ִϸ��̼� ���");

                    StartCoroutine(lionDanceColliderTrigger.SurviveTimerCoroutine(5f));
                }
            }
            else
            {
                mouseText.enabled = false; // �ؽ�Ʈ ������

                if (preObject != null)
                {
                    preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                    preObject = null;
                }
            }
        }
    }
}
