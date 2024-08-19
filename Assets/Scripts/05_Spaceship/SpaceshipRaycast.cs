using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipRaycast : MonoBehaviour
{
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

    // man001(��¯ �� ���)
    public GameObject man001_check; // üũ��




    void Start()
    {
        
    }

    void Update()
    {
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

    public void CheckObject()
    {
        isChecking = true;

        // man001
        if (isChecking)
        {
            if (hitObject == man001_check)
            {
                if (preObject != hitObject.GetComponent<GetComponentScript>().mesh && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                    preObject = null;
                }

                preObject = hitObject.GetComponent<GetComponentScript>().mesh;
                preObject.GetComponent<Outline>().enabled = true; // �ܰ��� �ѱ�

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
                    preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                    preObject = null;
                }
            }
        }
    }




    // man001 ��ȭ
    IEnumerator man001Coroutine()
    {
        // �ʿ��ϸ� "GameObject man001_this = hitObject" �����ؼ� ���

        preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ��Ȱ��ȭ
        man001_check.GetComponent<Man001HeadTracking>().isLooking = true;

        // ��ȭ
        if (true)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[4]["Content"], // "ù ��° �����Դϴ�"
                                                                   (string)GameManager.instance.textFileManager.dialog[5]["Content"], // "�� �ܹ������ŷ� ������ �ǳʼ� �ݴ��� Ÿ������ ������"
                                                                   (string)GameManager.instance.textFileManager.dialog[6]["Content"], // "�����Ŵ� �ڵ����� ������ ���ϴ�. �׷��� �¿�� ������ �� ����ּ���"
                                                                   (string)GameManager.instance.textFileManager.dialog[7]["Content"] }); // "���� �߰��� �������ٸ� �ٽ� ����� �� �ּ���"
        }

        yield return new WaitUntil(() => putDialogScript.isClickMode == false);

        man001_check.GetComponent<Man001HeadTracking>().isLooking = false;
    }
}
