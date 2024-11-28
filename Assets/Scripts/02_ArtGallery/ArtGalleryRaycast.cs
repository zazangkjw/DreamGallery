using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArtGalleryRaycast : MonoBehaviour
{
    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;
    bool isChecking = true;

    public ArtGallerySceneManager artGallerySceneManager;
    public ArtGalleryDirector artGalleryDirector;

    public GameObject[] dreamObjects; // �� ������ ���� ���ǵ�

    public GameObject deskMan_check; // �ȳ�����ũ npc üũ��
    public GameObject informationUI;
    public static bool is_pop_up;

    public PlayerController playerController;

    void Start()
    {

    }

    void Update()
    {
        ShootRaycast();
    }

    // ī�޶󿡼� ����ĳ��Ʈ ���
    private void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f) && !DefaultSceneManager.isPausing && !putDialogScript.isClickMode)
        {
            hitObject = hitInfo.collider.gameObject;
        }
        else
        {
            hitObject = null;
        }

        CheckObject();
    }

    // ����ĳ��Ʈ ������Ʈ üũ �޼ҵ�
    private void CheckObject()
    {
        isChecking = true;

        // �� ������Ʈ
        if (isChecking)
        {
            foreach (GameObject col in dreamObjects)
            {
                if (hitObject == col) // �� ������Ʈ�� ��
                {
                    if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    }

                    preObject = hitObject;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                    mouseText.text = GameManager.instance.textFileManager.ui[15]; // "�� ������ ����" �ؽ�Ʈ ����
                    mouseText.enabled = true;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(SelectDreamCoroutine());
                    }

                    isChecking = false; // ������ �׸���� üũ���� ����

                    break;
                }
                else
                {
                    mouseText.enabled = false;

                    if (preObject != null)
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                        preObject = null;
                    }
                }
            }
        }

        // deskMan
        if (isChecking)
        {
            if (hitObject == deskMan_check) // �ȳ�����ũ npc�� ��
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[24]; // "��ȭ�ϱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(DeskManCoroutine());
                }

                isChecking = false; // ������ �׸���� üũ���� ����
            }
            else
            {
                mouseText.enabled = false;

                if (preObject != null)
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    preObject = null;
                }
            }
        }
    }

    // �� ����
    IEnumerator SelectDreamCoroutine()
    {
        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����

        // ���� ���� �����ߴ���
        if (hitObject == dreamObjects[0])
        {
            ArtGalleryDirector.selectedDream = ArtGalleryDirector.Dreams.LionDance;
        }
        else if (hitObject == dreamObjects[1])
        {
            ArtGalleryDirector.selectedDream = ArtGalleryDirector.Dreams.Clown;
        }
        else if (hitObject == dreamObjects[2])
        {
            ArtGalleryDirector.selectedDream = ArtGalleryDirector.Dreams.Spaceship;
        }

        artGalleryDirector.LookVR();

        yield return null;
    }

    // �ȳ�����ũ npc ��ȭ
    IEnumerator DeskManCoroutine()
    {
        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����

        // ��ȭ
        /*putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[27]["Content"], // "'�� ���ð�'�� ���� ���� ȯ���մϴ�"
                                                               (string)GameManager.instance.textFileManager.dialog[28]["Content"], // "�̰����� �ް� ���õ� �پ��� ���ǵ��� ���õǾ� ������, �� ���ù� �տ��� VR ��Ⱑ ��ġ�Ǿ� �ֽ��ϴ�"
                                                               (string)GameManager.instance.textFileManager.dialog[29]["Content"]}); // "VR ��⸦ �ٶ󺸰� '��ȣ�ۿ�' Ű(E)�� ���� ���� ü���Ͻ� �� �ֽ��ϴ�"*/

        informationUI.SetActive(true);
        playerController.enabled = false;
        playerController.myRigid.isKinematic = true;
        is_pop_up = true;
        Cursor.visible = true; // ���콺 Ŀ�� �ѱ�

        yield return null;
    }

    public void PopUpControl(bool b)
    {
        is_pop_up = b;
    }

    public void CursorVisible(bool b)
    {
        Cursor.visible = b;
    }
}
