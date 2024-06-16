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

    public ClownSceneManager clownSceneManager;
    public RawImage fadeInOutImage; // ���̵�-��, �ƿ� �̹���
    public FadeInOutScript fadeInOutScript; // ���̵�-��, �ƿ� ��ũ��Ʈ

    // ����������
    public Animator[] elevatorAnims;
    [SerializeField]
    GameObject[] elevatorButtons;
    [SerializeField]
    GameObject elevator_Point_Player;
    GameObject elevatorBtn;

    // AudioSource
    public AudioSource circusSong;
    public AudioSource applause;
    public AudioSource booing;
    public AudioSource yay;
    public AudioSource bikeWheel;
    public AudioSource bikeWheelClown;

    // ���� ��ȸ
    public int life_max = 3;
    public int life;
    public TextMeshProUGUI lifeText;

    // ����Ÿ�� ����
    public GameObject unicycle;
    public GameObject unicycleSeat;
    public GameObject successsPlatform;
    public bool isRidingUnicycle;
    public GameObject unicycleClown;
    public GameObject rope;
    public GameObject rope_fake;


    void Start()
    {
        isRidingUnicycle = false;
        rope.SetActive(false);
        rope_fake.SetActive(true);

        // ó���� �����־�� �ϴ� ���������� ����
        elevatorAnims[0].Play("Open");
        elevatorAnims[1].Play("Open");

        life = life_max;
    }

    void Update()
    {
        ShootRaycast();
        UnicycleIdle();
    }

    // ī�޶󿡼� ����ĳ��Ʈ ���
    public void ShootRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 2f) && !clownSceneManager.isPausing && !putDialogScript.isClickMode)
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

                    isChecking = false; // ������ �׸���� üũ���� ���� (��: �ܹ�������)

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

                mouseText.text = GameManager.instance.textFileManager.ui[24]; // "��ȭ�ϱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

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
            life = life_max;
            lifeText.text = life.ToString();
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(circusSong, true, 5f, 0.5f));
        }
        else
        {
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(circusSong, false, 5f));
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(applause, false, 5f));
        }
        //-----------//

        yield return new WaitForSeconds(6f);

        elevatorBtn.GetComponent<NextElevatorPoint>().nextElevatorAnim.Play("Open");
        player.transform.SetParent(Objects.transform);
    }




    // �ܹ������Ÿ� ������ ��
    IEnumerator UnicycleCoroutine()
    {
        isRidingUnicycle = true;

        unicycle = hitObject;

        unicycle.GetComponent<Collider>().enabled = false; // �ݶ��̴� ��Ȱ��ȭ
        preObject.GetComponent<Outline>().enabled = false; // �ܰ��� ��Ȱ��ȭ


        // ��ȭ
        if (life == life_max)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[4]["Content"], // "����� �밨���� ������ �� �ִ� ù ��° �����Դϴ�"
                                                                   (string)GameManager.instance.textFileManager.dialog[5]["Content"], // "�� �ܹ������ŷ� ������ �ǳʼ� �ݴ��� Ÿ������ ������"
                                                                   (string)GameManager.instance.textFileManager.dialog[6]["Content"], // "�����Ŵ� �˾Ƽ� ������ ���ϱ� �¿�� ������ �� ����ֽø� �˴ϴ�"
                                                                   (string)GameManager.instance.textFileManager.dialog[7]["Content"] }); // "���� �߰��� �������ٸ� �ٽ� ����� �� �ּ���"
        }

        if(life > 1)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[8]["Content"] + life + (string)GameManager.instance.textFileManager.dialog[9]["Content"] }); // "���� ���� ��ȸ�� n���Դϴ�"
        }
        else if(life == 1)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[15]["Content"] }); // "������ ��ȸ�Դϴ�. �̹����� �����ϸ� �������� �г��� �ſ���"
        }

        putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[10]["Content"] }); // "�� �ڸ� �� ���������"
        
        yield return new WaitUntil(() => putDialogScript.isClickMode == false);
        player.GetComponent<PlayerController>().enabled = false;
        fadeInOutScript.FadeIn(fadeInOutImage, 1f);
        yield return new WaitForSeconds(2f);

        // �ܹ������� ž��
        player.transform.SetParent(unicycleSeat.transform);
        player.transform.position = unicycleSeat.transform.position;
        player.transform.rotation = unicycleSeat.transform.rotation;
        transform.localEulerAngles = Vector3.zero;
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;

        // �ܹ������� ��Ʈ�ѷ� Ȱ��ȭ
        player.GetComponent<UnicycleController>().enabled = true;
        player.GetComponent<UnicycleController>().lookSensitivity = player.GetComponent<PlayerController>().lookSensitivity;
        StartCoroutine(player.GetComponent<UnicycleController>().StandUpCoroutine()); // �ɾ��ִ� ������ �� �Ͼ�� �����

        // ���� ��ġ ����
        unicycleClown.GetComponent<GetComponentScript>().animator.Play("Go", 0, 0.016f);
        unicycleClown.GetComponent<GetComponentScript>().animator.speed = 0f;

        fadeInOutScript.FadeOut(fadeInOutImage, 1f);
        yield return new WaitForSeconds(2f);

        // ��¥ ���� ������� ��¥ ���� ��Ÿ��
        if(life == life_max)
        {
            yield return new WaitForSeconds(2f);
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[11]["Content"], 3f); // "��ø���"
            yield return new WaitForSeconds(6f);
            rope.SetActive(true);
            rope_fake.SetActive(false);
            yield return new WaitForSeconds(3f);
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[12]["Content"], 3f); // "�ʹ� ������ ���� �����߾��. ���� ����մϴ�"
            yield return new WaitForSeconds(3f);
        }

        // ���� ���
        unicycleClown.GetComponent<GetComponentScript>().animator.speed = 1f;
        unicycleClown.GetComponent<GetComponentScript>().animator.Play("WheelTurn", 1);
        bikeWheelClown.Play();

        // �÷��̾� ���
        unicycle.GetComponent<GetComponentScript>().animator.Play("Go");
        unicycle.GetComponent<GetComponentScript>().animator.Play("WheelTurn", 1);
        bikeWheel.Play();
        player.GetComponent<UnicycleController>().isBalancing = true;
    }

    // �ܹ������� ���߸� ������ ���߱�
    void UnicycleIdle()
    {
        if (unicycle.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            unicycle.GetComponent<GetComponentScript>().animator.Play("Idle", 1);
            bikeWheel.Stop();
        }

        if (unicycleClown.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            unicycleClown.GetComponent<GetComponentScript>().animator.Play("Idle", 1);
            bikeWheelClown.Stop();
        }
    }
}
    
