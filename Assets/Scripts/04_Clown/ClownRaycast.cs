using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ClownRaycast : DefaultRaycast
{
    public GameObject player;
    [SerializeField]
    Rigidbody playerRigid;
    public GameObject Armatures;

    public ClownSceneManager clownSceneManager;

    // ����������
    public Animator[] elevatorAnims;
    [SerializeField]
    GameObject[] elevatorBtns_check; // üũ��
    GameObject elevatorBtn_current;
    [SerializeField]
    GameObject elevator_Point_Player;

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
    public GameObject unicycle_check; // üũ��
    public GameObject unicycle_current;
    public GameObject unicycleSeat;
    public GameObject successsPlatform;
    public bool isRidingUnicycle;
    public GameObject unicycleClown;
    public GameObject rope;
    public GameObject rope_fake;


    void Start()
    {
        WhenStart();

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
        WhenUpdate();
        ShootRaycast();
        UnicycleIdle();
    }

    public override void CheckObject()
    {
        isChecking = true;

        // ��ȣ�ۿ� ������Ʈ
        if (isChecking)
        {
            if (hitObject != null && hitObject.tag == "InteractiveObject")
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[26]; // "�ݱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    preObject.GetComponent<InteractiveObject>().Action();
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

        // ������
        if (isChecking)
        {
            if (hitObject != null && hitObject.tag == "Item")
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[26]; // "�ݱ�" �ؽ�Ʈ ����
                mouseText.enabled = true;

                // EŰ �Է� ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItemCoroutine();
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

        // ���������� ��ư
        if (isChecking)
        {
            foreach (GameObject btn in elevatorBtns_check)
            {
                if (hitObject == btn)
                {
                    if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    }

                    preObject = hitObject;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                    mouseText.text = GameManager.instance.textFileManager.ui[28]; // "[E]" �ؽ�Ʈ ����
                    mouseText.enabled = true;

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
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                        preObject = null;
                    }
                }
            }
        }

        // �ܹ�������
        if (isChecking)
        {
            if (hitObject == unicycle_check)
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
                    StartCoroutine(UnicycleCoroutine());
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




    // ���������� ��ư�� ������ ��
    IEnumerator ElevatorCoroutine()
    {
        elevatorBtn_current = hitObject;

        elevatorBtn_current.GetComponent<Collider>().enabled = false; // �ݶ��̴� ��Ȱ��ȭ
        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ

        elevatorBtn_current.GetComponent<NextElevatorPoint>().thisElevatorAnim.Play("Close");

        


        // Ż�� ����������
        if(elevatorBtn_current.GetComponent<NextElevatorPoint>().goTo == "Exit")
        {
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(circusSong, false, 7f));
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(applause, false, 7f));

            yield return new WaitForSeconds(5f);

            // ��Ʈ�������� ����
            fadeInOutScript.FadeOut(fadeInOutImage);
            yield return new WaitForSeconds(2f);
            LoadSceneScript.SuccessLoadScene("02_ArtGallery");
        }
        else // �� �� ����������
        {
            elevator_Point_Player.transform.position = elevatorBtn_current.GetComponent<NextElevatorPoint>().thisPoint.transform.position; // elevator_Point_Player�� ���������� ����Ʈ�� �ű��, �÷��̾ �ڽ����� �־
            elevator_Point_Player.transform.rotation = elevatorBtn_current.GetComponent<NextElevatorPoint>().thisPoint.transform.rotation; // n�� �Ŀ� ���� ���������� ����Ʈ�� �̵� �� �ڽ� ����
            player.transform.SetParent(elevator_Point_Player.transform);

            yield return new WaitForSeconds(3f);

            elevator_Point_Player.transform.position = elevatorBtn_current.GetComponent<NextElevatorPoint>().nextPoint.transform.position;
            elevator_Point_Player.transform.rotation = elevatorBtn_current.GetComponent<NextElevatorPoint>().nextPoint.transform.rotation;

            // ��Ŀ���� ���� ����������
            if (elevatorBtn_current.GetComponent<NextElevatorPoint>().goTo == "Circus")
            {
                life = life_max;
                lifeText.text = life.ToString();
                StartCoroutine(AudioOnOffScript.VolumeCoroutine(circusSong, true, 15f, 0.4f));
            }

            // �̵� �ð�
            yield return new WaitForSeconds(6f);

            // �� ����
            elevatorBtn_current.GetComponent<NextElevatorPoint>().next_elevator_ding.Play();
            yield return new WaitForSeconds(1f);
            elevatorBtn_current.GetComponent<NextElevatorPoint>().nextElevatorAnim.Play("Open");
            player.transform.SetParent(Armatures.transform);
        }
    }




    // �ܹ������Ÿ� ������ ��
    IEnumerator UnicycleCoroutine()
    {
        isRidingUnicycle = true;

        unicycle_current = hitObject;

        unicycle_current.GetComponent<Collider>().enabled = false; // �ݶ��̴� ��Ȱ��ȭ
        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ


        // ��ȭ
        if (life == life_max)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[4]["Content"], // "������ ���� ����帮�ڽ��ϴ�"
                                                                   (string)GameManager.instance.textFileManager.dialog[5]["Content"], // "�� �ܹ������ŷ� ������ �ǳʼ� �ݴ��� Ÿ������ ������"
                                                                   (string)GameManager.instance.textFileManager.dialog[6]["Content"], // "�����Ŵ� �ڵ����� ������ ���ϴ�. �׷��� �¿�� ������ �� ����ּ���"
                                                                   (string)GameManager.instance.textFileManager.dialog[7]["Content"] }); // "���� �������ٸ� �ٽ� ����� �� �ּ���"*/
        }

        if(life > 1)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[8]["Content"] + life + (string)GameManager.instance.textFileManager.dialog[9]["Content"] }); // "���� ���� ��ȸ�� n���Դϴ�"
            //putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[30]["Content"], "..." + life.ToString() + "..." }); // "......" "...n..."
        }
        else if(life == 1)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[15]["Content"] }); // "������ ��ȸ�Դϴ�"
            //putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[30]["Content"], "...1..."}); // "......" "...1..."
        }

        //putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[10]["Content"] }); // "���� �� ���������"
        
        yield return new WaitUntil(() => putDialogScript.isClickMode == false);
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        fadeInOutScript.FadeOut(fadeInOutImage, 1f);
        yield return new WaitForSeconds(2f);

        // �ܹ������� ž��
        player.transform.SetParent(unicycleSeat.transform);
        player.transform.position = unicycleSeat.transform.position;
        player.transform.rotation = unicycleSeat.transform.rotation;
        transform.localEulerAngles = Vector3.zero;

        // �ܹ������� ��Ʈ�ѷ� Ȱ��ȭ
        player.GetComponent<UnicycleController>().enabled = true;
        player.GetComponent<UnicycleController>().lookSensitivity = player.GetComponent<PlayerController>().lookSensitivity;
        StartCoroutine(player.GetComponent<UnicycleController>().StandUpCoroutine()); // �ɾ��ִ� ������ �� �Ͼ�� �����

        // ��ġ ����
        unicycleClown.GetComponent<GetComponentScript>().animator.Play("Go", 0, 0.02f);
        unicycleClown.GetComponent<GetComponentScript>().animator.speed = 0f;
        unicycle_current.GetComponent<GetComponentScript>().animator.Play("Go", 0, 0.01f);
        unicycle_current.GetComponent<GetComponentScript>().animator.speed = 0f;

        fadeInOutScript.FadeIn(fadeInOutImage, 1f);
        yield return new WaitForSeconds(2f);

        // ��¥ ���� ������� ��¥ ���� ��Ÿ��
        if(life == life_max)
        {
            yield return new WaitForSeconds(2f);
            //putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[11]["Content"], 3f); // "��ø���"
            //yield return new WaitForSeconds(6f);
            rope.SetActive(true);
            rope_fake.SetActive(false);
            yield return new WaitForSeconds(1f);
            //putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[12]["Content"], 3f); // "�ʹ� ������ ���� �����߾��. ���� ����մϴ�"
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[10]["Content"], 3f); // "���� �� ���������"
            yield return new WaitForSeconds(2f);
        }

        // ���� ���
        unicycleClown.GetComponent<GetComponentScript>().animator.speed = 1f;
        unicycleClown.GetComponent<GetComponentScript>().animator.Play("WheelTurn", 1);
        bikeWheelClown.Play();

        // �÷��̾� ���
        unicycle_current.GetComponent<GetComponentScript>().animator.speed = 1f;
        unicycle_current.GetComponent<GetComponentScript>().animator.Play("WheelTurn", 1);
        bikeWheel.Play();
        player.GetComponent<UnicycleController>().isBalancing = true;
    }

    // �ܹ������� ���߸� ������ ���߱�
    void UnicycleIdle()
    {
        if (unicycle_check.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            unicycle_check.GetComponent<GetComponentScript>().animator.Play("Idle", 1);
            bikeWheel.Stop();
        }

        if (unicycleClown.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            unicycleClown.GetComponent<GetComponentScript>().animator.Play("Idle", 1);
            bikeWheelClown.Stop();
        }
    }
}
    
































/*
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ClownRaycast : DefaultRaycast
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
    public GameObject Armatures;

    public ClownSceneManager clownSceneManager;
    public RawImage fadeInOutImage; // ���̵�-��, �ƿ� �̹���
    public FadeInOutScript fadeInOutScript; // ���̵�-��, �ƿ� ��ũ��Ʈ

    // ����������
    public Animator[] elevatorAnims;
    [SerializeField]
    GameObject[] elevatorBtns_check; // üũ��
    GameObject elevatorBtn_current;
    [SerializeField]
    GameObject elevator_Point_Player;

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
    public GameObject unicycle_check; // üũ��
    public GameObject unicycle_current;
    public GameObject unicycleSeat;
    public GameObject successsPlatform;
    public bool isRidingUnicycle;
    public GameObject unicycleClown;
    public GameObject rope;
    public GameObject rope_fake;

    // ����ġ
    public GameObject switch_check;


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
            foreach (GameObject btn in elevatorBtns_check)
            {
                if (hitObject == btn)
                {
                    if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    }

                    preObject = hitObject;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                    // mouseText.text = GameManager.instance.textFileManager.ui[24]; // "��ȭ�ϱ�" �ؽ�Ʈ ����
                    // mouseText.enabled = true;

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
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                        preObject = null;
                    }
                }
            }
        }

        // �ܹ�������
        if (isChecking)
        {
            if (hitObject == unicycle_check)
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
                    StartCoroutine(UnicycleCoroutine());
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

        // ����ġ
        if (isChecking)
        {
            if (hitObject == switch_check)
            {
                if (preObject != hitObject && preObject != null) // �� ������Ʈ�� ���� ������Ʈ�� �ٸ� ��, �� ������Ʈ �ܰ��� ����
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // �ܰ��� �ѱ�

                mouseText.text = GameManager.instance.textFileManager.ui[26]; // "�ݱ�" �ؽ�Ʈ ����
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
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ����
                    preObject = null;
                }
            }
        }
    }




    // ���������� ��ư�� ������ ��
    IEnumerator ElevatorCoroutine()
    {
        elevatorBtn_current = hitObject;

        elevatorBtn_current.GetComponent<Collider>().enabled = false; // �ݶ��̴� ��Ȱ��ȭ
        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ

        elevatorBtn_current.GetComponent<NextElevatorPoint>().thisElevatorAnim.Play("Close");




        // Ż�� ����������
        if (elevatorBtn_current.GetComponent<NextElevatorPoint>().goTo == "Exit")
        {
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(circusSong, false, 7f));
            StartCoroutine(AudioOnOffScript.VolumeCoroutine(applause, false, 7f));

            yield return new WaitForSeconds(5f);

            // ��Ʈ�������� ����
            fadeInOutScript.FadeOut(fadeInOutImage);
            yield return new WaitForSeconds(2f);
            LoadSceneScript.SuccessLoadScene("02_ArtGallery");
        }
        else // �� �� ����������
        {
            elevator_Point_Player.transform.position = elevatorBtn_current.GetComponent<NextElevatorPoint>().thisPoint.transform.position; // elevator_Point_Player�� ���������� ����Ʈ�� �ű��, �÷��̾ �ڽ����� �־
            elevator_Point_Player.transform.rotation = elevatorBtn_current.GetComponent<NextElevatorPoint>().thisPoint.transform.rotation; // n�� �Ŀ� ���� ���������� ����Ʈ�� �̵� �� �ڽ� ����
            player.transform.SetParent(elevator_Point_Player.transform);

            yield return new WaitForSeconds(3f);

            elevator_Point_Player.transform.position = elevatorBtn_current.GetComponent<NextElevatorPoint>().nextPoint.transform.position;
            elevator_Point_Player.transform.rotation = elevatorBtn_current.GetComponent<NextElevatorPoint>().nextPoint.transform.rotation;

            // ��Ŀ���� ���� ����������
            if (elevatorBtn_current.GetComponent<NextElevatorPoint>().goTo == "Circus")
            {
                life = life_max;
                lifeText.text = life.ToString();
                StartCoroutine(AudioOnOffScript.VolumeCoroutine(circusSong, true, 15f, 0.6f));
            }

            // �̵� �ð�
            yield return new WaitForSeconds(6f);

            // �� ����
            elevatorBtn_current.GetComponent<NextElevatorPoint>().next_elevator_ding.Play();
            yield return new WaitForSeconds(1f);
            elevatorBtn_current.GetComponent<NextElevatorPoint>().nextElevatorAnim.Play("Open");
            player.transform.SetParent(Armatures.transform);
        }
    }




    // �ܹ������Ÿ� ������ ��
    IEnumerator UnicycleCoroutine()
    {
        isRidingUnicycle = true;

        unicycle_current = hitObject;

        unicycle_current.GetComponent<Collider>().enabled = false; // �ݶ��̴� ��Ȱ��ȭ
        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ


        // ��ȭ
        if (life == life_max)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[4]["Content"], // "ù ��° �����Դϴ�"
                                                                   (string)GameManager.instance.textFileManager.dialog[5]["Content"], // "�� �ܹ������ŷ� ������ �ǳʼ� �ݴ��� Ÿ������ ������"
                                                                   (string)GameManager.instance.textFileManager.dialog[6]["Content"], // "�����Ŵ� �ڵ����� ������ ���ϴ�. �׷��� �¿�� ������ �� ����ּ���"
                                                                   (string)GameManager.instance.textFileManager.dialog[7]["Content"] }); // "���� �߰��� �������ٸ� �ٽ� ����� �� �ּ���"
        }

        if (life > 1)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[8]["Content"] + life + (string)GameManager.instance.textFileManager.dialog[9]["Content"] }); // "���� ���� ��ȸ�� n���Դϴ�"
        }
        else if (life == 1)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[15]["Content"] }); // "������ ��ȸ�Դϴ�. �̹����� �����ϸ� �������� �г��� �ſ���"
        }

        putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[10]["Content"] }); // "�� �ڸ� �� ���������"

        yield return new WaitUntil(() => putDialogScript.isClickMode == false);
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        fadeInOutScript.FadeOut(fadeInOutImage, 1f);
        yield return new WaitForSeconds(2f);

        // �ܹ������� ž��
        player.transform.SetParent(unicycleSeat.transform);
        player.transform.position = unicycleSeat.transform.position;
        player.transform.rotation = unicycleSeat.transform.rotation;
        transform.localEulerAngles = Vector3.zero;

        // �ܹ������� ��Ʈ�ѷ� Ȱ��ȭ
        player.GetComponent<UnicycleController>().enabled = true;
        player.GetComponent<UnicycleController>().lookSensitivity = player.GetComponent<PlayerController>().lookSensitivity;
        StartCoroutine(player.GetComponent<UnicycleController>().StandUpCoroutine()); // �ɾ��ִ� ������ �� �Ͼ�� �����

        // ��ġ ����
        unicycleClown.GetComponent<GetComponentScript>().animator.Play("Go", 0, 0.02f);
        unicycleClown.GetComponent<GetComponentScript>().animator.speed = 0f;
        unicycle_current.GetComponent<GetComponentScript>().animator.Play("Go", 0, 0.01f);
        unicycle_current.GetComponent<GetComponentScript>().animator.speed = 0f;

        fadeInOutScript.FadeIn(fadeInOutImage, 1f);
        yield return new WaitForSeconds(2f);

        // ��¥ ���� ������� ��¥ ���� ��Ÿ��
        if (life == life_max)
        {
            yield return new WaitForSeconds(2f);
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[11]["Content"], 3f); // "��ø���"
            yield return new WaitForSeconds(6f);
            rope.SetActive(true);
            rope_fake.SetActive(false);
            yield return new WaitForSeconds(3f);
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[12]["Content"], 3f); // "�ʹ� ������ ���� �����߾��. ���� ����մϴ�"
            yield return new WaitForSeconds(5f);
        }

        // ���� ���
        unicycleClown.GetComponent<GetComponentScript>().animator.speed = 1f;
        unicycleClown.GetComponent<GetComponentScript>().animator.Play("WheelTurn", 1);
        bikeWheelClown.Play();

        // �÷��̾� ���
        unicycle_current.GetComponent<GetComponentScript>().animator.speed = 1f;
        unicycle_current.GetComponent<GetComponentScript>().animator.Play("WheelTurn", 1);
        bikeWheel.Play();
        player.GetComponent<UnicycleController>().isBalancing = true;
    }

    // �ܹ������� ���߸� ������ ���߱�
    void UnicycleIdle()
    {
        if (unicycle_check.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            unicycle_check.GetComponent<GetComponentScript>().animator.Play("Idle", 1);
            bikeWheel.Stop();
        }

        if (unicycleClown.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            unicycleClown.GetComponent<GetComponentScript>().animator.Play("Idle", 1);
            bikeWheelClown.Stop();
        }
    }
}

*/