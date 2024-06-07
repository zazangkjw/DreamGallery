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
    private float currentCameraRotationX = 0;

    // ���� ���
    public GameObject bodyForUnity;
    public Vector3 originRotate;
    Vector3 balance;
    Vector3 balanceDir;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCamera;
    public Rigidbody myRigid;
    public ClownRaycast clownRaycast;




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




    // ���� ���
    private void Balance()
    {
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

        if(bodyForUnity.transform.localEulerAngles.y > 90f && bodyForUnity.transform.localEulerAngles.y < 270f)
        {
            balance = Vector3.zero;
            balanceDir = Vector3.up * 0.02f;

            clownRaycast.life--;
            clownRaycast.lifeText.text = clownRaycast.life.ToString();
            clownRaycast.isOnUnicycle = false;
            clownRaycast.player.transform.SetParent(clownRaycast.Objects.transform);
            clownRaycast.player.transform.eulerAngles = new Vector3(0f, clownRaycast.player.transform.eulerAngles.y, 0f);

            // ��Ʈ�ѷ� ��ü
            clownRaycast.player.GetComponent<PlayerController>().enabled = true;
            clownRaycast.player.GetComponent<UnicycleController>().enabled = false;
            clownRaycast.player.GetComponent<Rigidbody>().isKinematic = false;
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
}
