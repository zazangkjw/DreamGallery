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

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f) && !artGallerySceneManager.isPausing && !putDialogScript.isClickMode)
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
        { }
            foreach (GameObject col in dreamObjects)
        {
            if (hitObject == col) // �� ������Ʈ�� ��
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                    preObject = null;
                }

                preObject = hitObject;
                preObject.GetComponent<Outline>().enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[15]; // "�� ������ ����" �ؽ�Ʈ ����
                mouseText.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                    preObject = null;

                    // ���� ���� �����ߴ���
                    if (hitObject == dreamObjects[0])
                    {
                        ArtGalleryDirector.selectedDream = ArtGalleryDirector.Dreams.LionDance;
                    }
                    else if(hitObject == dreamObjects[1])
                    {
                        ArtGalleryDirector.selectedDream = ArtGalleryDirector.Dreams.Clown;
                    }

                    artGalleryDirector.LookVR();
                }

                isChecking = false; // ������ �׸���� üũ���� ����

                break;
            }
            else
            {
                mouseText.enabled = false;

                if (preObject != null)
                {
                    preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                    preObject = null;
                }
            }
        }
    }
}
