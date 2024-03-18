using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        Debug.DrawRay(transform.position, transform.forward * 1.5f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.5f) && !artGallerySceneManager.isPausing && !putDialogScript.isClickMode)
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
        if (hitObject == dreamObjects[0]) // �� ������Ʈ�� ��
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

                ArtGalleryDirector.selectedDream = ArtGalleryDirector.Dreams.LionDance;
                artGalleryDirector.LookVR();
            }
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
