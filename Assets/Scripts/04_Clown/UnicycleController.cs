using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicycleController : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    // 일어나기
    public CapsuleCollider standCollider;
    public CapsuleCollider crouchCollider;

    // 마우스 감도
    public float lookSensitivity;

    // 카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    // 균형 잡기
    public GameObject bodyForUnity;
    public Vector3 originRotate;
    Vector3 balance;
    Vector3 balanceDir;

    // 필요한 컴포넌트
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




    // 균형 잡기
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

            // 컨트롤러 교체
            clownRaycast.player.GetComponent<PlayerController>().enabled = true;
            clownRaycast.player.GetComponent<UnicycleController>().enabled = false;
            clownRaycast.player.GetComponent<Rigidbody>().isKinematic = false;
        }
    }




    // 좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity * 0.1f;
        transform.localEulerAngles = transform.localEulerAngles + _characterRotationY;
    }

    // 상하 카메라 회전
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity * 0.1f;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    // 상하 카메라 회전값 설정
    public void SetCurrentCameraRotationX(float x)
    {
        currentCameraRotationX = x;
    }
}
