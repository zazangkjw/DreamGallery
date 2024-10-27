using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicycleController : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    // �Ͼ��
    public CapsuleCollider standCollider;
    public CapsuleCollider crouchCollider;

    // ���콺 ����
    public float lookSensitivity;

    // ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit;
    public float currentCameraRotationX = 0;

    // ���� ���
    public GameObject bodyForUnity;
    public Vector3 originRotate;
    Vector3 balance;
    Vector3 balanceDir;
    public bool isBalancing;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCamera;
    public Rigidbody myRigid;
    public ClownRaycast clownRaycast;
    public ClownWorm clownWorm;
    public GameObject ladder;
    public GameObject clownWormMesh;
    public Collider clownWormCol;




    private void Start()
    {
        originRotate = bodyForUnity.transform.localEulerAngles;
        balance = Vector3.zero;
        balanceDir = Vector3.up * 0.02f;
    }

    void Update()
    {
        CameraRotation();
        CharacterRotation();
    }

    private void FixedUpdate()
    {
        Balance();
    }

    private void OnEnable()
    {
        SetCurrentCameraRotationX(0);
    }




    // ���� ���
    private void Balance()
    {
        if (isBalancing)
        {
            // ���� ���
            if (Input.GetKey(KeyCode.A))
            {
                balanceDir = Vector3.up * 0.1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                balanceDir = -Vector3.up * 0.1f;
            }

            balance = balance + balanceDir;
            bodyForUnity.transform.localEulerAngles = bodyForUnity.transform.localEulerAngles + balance;

            // ���� ��� ����
            if (clownRaycast.unicycle_current.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                isBalancing = false;
                clownRaycast.isRidingUnicycle = false;

                transform.SetParent(clownRaycast.Armatures.transform);
                transform.position = clownRaycast.successsPlatform.transform.position + Vector3.up * 3;
                transform.eulerAngles = new Vector3(0f, clownRaycast.unicycleSeat.transform.eulerAngles.y, 0f);
                playerController.SetCurrentCameraRotationX(60f);
                playerController.knockbackTimer = 0.5f;

                clownRaycast.unicycleClown.GetComponent<GetComponentScript>().animator.Play("Return", 0, 1f - clownRaycast.unicycleClown.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                clownRaycast.unicycleClown.GetComponent<GetComponentScript>().animator.Play("WheelTurnReverse", 1);

                clownRaycast.putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[16]["Content"], 5f); // "���߾��!"

                // ��Ʈ�ѷ� ��ü
                playerController.enabled = true;
                enabled = false;
                myRigid.isKinematic = false;
            }

            // ���� ��� ����
            if (bodyForUnity.transform.localEulerAngles.y > 90f && bodyForUnity.transform.localEulerAngles.y < 270f)
            {
                isBalancing = false;
                balance = Vector3.zero;
                balanceDir = Vector3.up * 0.02f;

                clownRaycast.life--;
                clownRaycast.lifeText.text = clownRaycast.life.ToString();
                clownRaycast.isRidingUnicycle = false;
                transform.SetParent(clownRaycast.Armatures.transform);
                transform.eulerAngles = new Vector3(0f, clownRaycast.unicycleSeat.transform.eulerAngles.y, 0f);
                playerController.SetCurrentCameraRotationX(currentCameraRotationX);

                // ��Ʈ�ѷ� ��ü
                playerController.enabled = true;
                enabled = false;
                myRigid.isKinematic = false;

                // ���� ȿ����
                clownRaycast.booing.Play();

                // ��ȸ ��� ����
                if (clownRaycast.life <= 0)
                {
                    clownWormMesh.gameObject.SetActive(true);
                    clownWormCol.enabled = true;
                    clownRaycast.circusSong.pitch = -0.5f;
                    clownRaycast.putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[17]["Content"], 5f); // "��� ������ �������� �Ǹ��߾��"
                    clownWorm.navMeshAgent.stoppingDistance = 0f;
                    clownWorm.deadTrigger.SetActive(true);
                    ladder.SetActive(false);
                }
                // ��ȸ ����
                else if (clownRaycast.life > 0)
                {
                    bodyForUnity.transform.localEulerAngles = originRotate;
                    clownRaycast.unicycle_current.GetComponent<GetComponentScript>().animator.Play("Return", 0, 1f - clownRaycast.unicycle_current.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                    clownRaycast.unicycle_current.GetComponent<GetComponentScript>().animator.Play("WheelTurnReverse", 1);
                    clownRaycast.bikeWheel.Play();
                    clownRaycast.unicycleClown.GetComponent<GetComponentScript>().animator.Play("Return", 0, 1f - clownRaycast.unicycleClown.GetComponent<GetComponentScript>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                    clownRaycast.unicycleClown.GetComponent<GetComponentScript>().animator.Play("WheelTurnReverse", 1);
                    clownRaycast.bikeWheelClown.Play();
                    clownRaycast.unicycle_current.GetComponent<Collider>().enabled = true; // �ݶ��̴� Ȱ��ȭ

                    if (clownRaycast.life == 2)
                    {
                        clownRaycast.putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[13]["Content"], 5f); // "�̰� ����켼��?"
                    }
                    else if (clownRaycast.life == 1)
                    {
                        clownRaycast.putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[14]["Content"], 5f); // "�Ϻη� �׷�����?"
                    }
                }
            }
        }
    }




    // �¿� ĳ���� ȸ��
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity * 0.1f;
        transform.localEulerAngles = transform.localEulerAngles + _characterRotationY;
    }

    // ���� ī�޶� ȸ��
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity * 0.1f;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    // ���� ī�޶� ȸ���� ����
    public void SetCurrentCameraRotationX(float x)
    {
        currentCameraRotationX = x;
    }




    // �Ͼ��
    public IEnumerator StandUpCoroutine()
    {
        standCollider.enabled = true;
        crouchCollider.enabled = false;

        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while (_posY != playerController.originPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, playerController.originPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0f);
            if (count > 15)
            {
                break;
            }
            yield return new WaitForSeconds(0.016f);
        }
        theCamera.transform.localPosition = new Vector3(0, playerController.originPosY, 0);
    }
}
