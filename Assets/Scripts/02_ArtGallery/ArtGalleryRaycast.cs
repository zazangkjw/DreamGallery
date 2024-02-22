using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArtGalleryRaycast : MonoBehaviour
{
    public RaycastHit hitInfo;
    public GameObject hitObject;

    public Text mouseText;
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
            CheckDreamObject();
        }

        // ����� ��
        else
        {
            mouseText.text = ""; // �ؽ�Ʈ ������
        }
    }

    // �� ������Ʈ üũ �޼ҵ�
    private void CheckDreamObject()
    {
        if (hitObject == dreamObjects[0])
        {
            mouseText.text = GameManager.instance.textFileManager.ui[15]; // "�� ������ ����" �ؽ�Ʈ ����
            mouseText.enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                ArtGalleryDirector.selectedDream = 0;
                artGalleryDirector.LookVR();
            }
        }

        // ����� ��
        else
        {
            mouseText.enabled = false; // �ؽ�Ʈ ������
        }
    }
}
