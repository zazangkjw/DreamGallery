using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ClownRaycast : MonoBehaviour
{
    public RaycastHit hitInfo;
    public GameObject hitObject;
    public GameObject preObject;
    public TextMeshProUGUI mouseText;
    public PutDialogScript putDialogScript;

    bool isChecking = true;

    public GameObject player;
    [SerializeField]
    Rigidbody playerRigid;
    public GameObject Objects;

    // ����������
    [SerializeField]
    Animator[] elevatorAnims;
    [SerializeField]
    GameObject[] elevatorButtons;
    [SerializeField]
    GameObject elevator_Point_Player;
    GameObject elevatorBtn;

    // AudioSource
    public AudioSource cheer;
    public AudioSource clap;
    public AudioSource booing;
    public AudioSource yay;

    // ���� ��ȸ
    public int life;
    public TextMeshProUGUI lifeText;

    // ����Ÿ�� ����
    [SerializeField]
    GameObject unicycle;
    [SerializeField]
    GameObject unicycleSeat;
    public GameObject successsPlatform;
    public bool isOnUnicycle;


    void Start()
    {
        isOnUnicycle = false;

        // ó���� �����־�� �ϴ� ���������� ����
        elevatorAnims[0].Play("Open");
        elevatorAnims[1].Play("Open");

        life = 3;
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

                    isChecking = false; // ������ �׸���� üũ���� ����

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
                    StartCoroutine(UnicycleCoroutine());
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




    // ���������� ��ư�� ������ ��
    IEnumerator ElevatorCoroutine()
    {
        elevatorBtn = hitObject;

        elevatorBtn.GetComponent<Collider>().enabled = false; // �ݶ��̴� ��Ȱ��ȭ
        preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ��Ȱ��ȭ

        elevatorBtn.GetComponent<NextElevatorPoint>().thisElevatorAnim.Play("Close");

        elevator_Point_Player.transform.position = elevatorBtn.GetComponent<NextElevatorPoint>().thisPoint.transform.position; // elevator_Point_Player�� ���������� ����Ʈ�� �ű��, �÷��̾ �ڽ����� �־
        elevator_Point_Player.transform.rotation = elevatorBtn.GetComponent<NextElevatorPoint>().thisPoint.transform.rotation; // 2�� �Ŀ� ���� ���������� ����Ʈ�� �̵� �� �ڽ� ����
        player.transform.SetParent(elevator_Point_Player.transform);

        yield return new WaitForSeconds(3f);

        elevator_Point_Player.transform.position = elevatorBtn.GetComponent<NextElevatorPoint>().nextPoint.transform.position;
        elevator_Point_Player.transform.rotation = elevatorBtn.GetComponent<NextElevatorPoint>().nextPoint.transform.rotation;

        // �߰� ��� //
        if (elevatorBtn.GetComponent<NextElevatorPoint>().goTo == "Circus")
        {
            life = 3;
            lifeText.text = life.ToString();
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(cheer, true, 5f));
        }
        else
        {
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(cheer, false, 5f));
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(clap, false, 5f));
        }
        //-----------//

        yield return new WaitForSeconds(6f);

        elevatorBtn.GetComponent<NextElevatorPoint>().nextElevatorAnim.Play("Open");
        player.transform.SetParent(Objects.transform);
    }




    // �ܹ������Ÿ� ������ ��
    IEnumerator UnicycleCoroutine()
    {
        isOnUnicycle = true;

        unicycle = hitObject;

        unicycle.GetComponent<Collider>().enabled = false; // �ݶ��̴� ��Ȱ��ȭ
        preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ��Ȱ��ȭ

        // �ɾ��ִ� ������ �� �Ͼ�� �����
        transform.localPosition = new Vector3(0, player.GetComponent<PlayerController>().originPosY, 0);
        player.GetComponent<PlayerController>().standCollider.enabled = true;
        player.GetComponent<PlayerController>().crouchCollider.enabled = false;

        // �ܹ������� ž��
        player.transform.SetParent(unicycleSeat.transform);
        player.transform.position = unicycleSeat.transform.position;
        player.transform.rotation = unicycleSeat.transform.rotation;

        // ��Ʈ�ѷ� ��ü
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<UnicycleController>().enabled = true;
        player.GetComponent<UnicycleController>().lookSensitivity = player.GetComponent<PlayerController>().lookSensitivity;
        player.GetComponent<Rigidbody>().isKinematic = true;

        unicycle.GetComponent<GetComponentScript>().animator.Play("Go");
        unicycle.GetComponent<GetComponentScript>().animator.Play("WheelTurn", 1);
        yield return new WaitForSeconds(8f);
        unicycle.GetComponent<GetComponentScript>().animator.Play("Idle", 1);


        // �ܹ������� ���� ����
        if (isOnUnicycle)
        {
            isOnUnicycle = false;

            player.transform.SetParent(Objects.transform);
            player.transform.position = successsPlatform.transform.position + Vector3.up * 3;
            player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y, 0f);
            elevatorAnims[3].Play("Open");
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(clap, true, 2f));
            yay.Play();

            // ��Ʈ�ѷ� ��ü
            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<UnicycleController>().enabled = false;
            player.GetComponent<Rigidbody>().isKinematic = false;
        }
        else if(life > 0)
        {
            player.GetComponent<UnicycleController>().bodyForUnity.transform.localEulerAngles = player.GetComponent<UnicycleController>().originRotate;
            unicycle.GetComponent<GetComponentScript>().animator.Play("Return");
            unicycle.GetComponent<GetComponentScript>().animator.Play("WheelTurnReverse", 1);
            yield return new WaitForSeconds(8f);
            unicycle.GetComponent<GetComponentScript>().animator.Play("Idle", 1);
            unicycle.GetComponent<Collider>().enabled = true; // �ݶ��̴� Ȱ��ȭ
        }
    }
}
    
