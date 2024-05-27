using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClownRaycast : MonoBehaviour
{
    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;

    bool isChecking = true;

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject Objects;

    [SerializeField]
    Animator[] elevatorAnims;

    [SerializeField]
    GameObject[] elevatorButtons;

    [SerializeField]
    GameObject elevator_Point_Player;

    GameObject button;

    [SerializeField]
    GameObject unicycle;

    [SerializeField]
    Animator unicycleAnim;




    void Start()
    {
        // ó���� �����־�� �ϴ� ���������� ����
        elevatorAnims[0].Play("Open");
        elevatorAnims[1].Play("Open");
        elevatorAnims[3].Play("Open");
    }

    void Update()
    {
        ShootRaycast();
    }

    // ī�޶󿡼� ����ĳ��Ʈ ���
    public void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 1.5f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.5f))
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

        // ���������� ��ư
        if (isChecking)
        {
            foreach (GameObject btn in elevatorButtons)
            {
                if (hitObject == btn)
                {
                    if (preObject != hitObject.GetComponent<GetComponentScript>().mesh && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                    {
                        preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                        preObject = null;
                    }

                    preObject = hitObject.GetComponent<GetComponentScript>().mesh;
                    preObject.GetComponent<Outline>().enabled = true; // �ܰ��� �ѱ�

                    // EŰ �Է� ��
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(ElevatorCoroutine());
                    }

                    isChecking = false; // �Ʒ��� �׸���� üũ���� ����

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

        // �ܹ�������
        if (isChecking)
        {
            if (hitObject == unicycle)
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ����
                    preObject = null;
                }

                preObject = hitObject;
                preObject.GetComponent<Outline>().enabled = true; // �ܰ��� �ѱ�

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    unicycleAnim.Play("Go");
                    hitObject.GetComponent<Collider>().enabled = false; // ��ư �ݶ��̴� ��Ȱ��ȭ
                    hitObject.GetComponent<Outline>().enabled = false; // ��ư �ܰ��� ��Ȱ��ȭ
                }

                isChecking = false; // �Ʒ��� �׸���� üũ���� ����
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

    // ���������� ��ư�� ������ ��
    IEnumerator ElevatorCoroutine()
    {
        button = hitObject;

        button.GetComponent<Collider>().enabled = false; // ��ư �ݶ��̴� ��Ȱ��ȭ
        button.GetComponent<Outline>().enabled = false; // ��ư �ܰ��� ��Ȱ��ȭ
        button.GetComponent<NextElevatorPoint>().thisElevatorAnim.Play("Close");

        elevator_Point_Player.transform.position = button.GetComponent<NextElevatorPoint>().thisPoint.transform.position; // elevator_Point_Player�� ���������� ����Ʈ�� �ű��, �÷��̾ �ڽ����� �־
        elevator_Point_Player.transform.rotation = button.GetComponent<NextElevatorPoint>().thisPoint.transform.rotation; // 2�� �Ŀ� ���� ���������� ����Ʈ�� �̵� �� �ڽ� ����
        player.transform.SetParent(elevator_Point_Player.transform);

        yield return new WaitForSeconds(2f);

        elevator_Point_Player.transform.position = button.GetComponent<NextElevatorPoint>().nextPoint.transform.position;
        elevator_Point_Player.transform.rotation = button.GetComponent<NextElevatorPoint>().nextPoint.transform.rotation;

        yield return new WaitForSeconds(2f);

        button.GetComponent<NextElevatorPoint>().nextElevatorAnim.Play("Open");
        player.transform.SetParent(Objects.transform);
    }
}
    
