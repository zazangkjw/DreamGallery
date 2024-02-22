using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 플레이어 이동속도
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float applySpeed;
    public float jumpForce;
    private Vector3 _velocity;
    private Vector3 jumpVelocity;
    private Vector3 _moveHorizontal;
    private Vector3 _moveVertical;

    // 상태 변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    // 앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // 땅 착지 여부
    public CapsuleCollider standCollider;
    public CapsuleCollider crouchCollider;

    // 마우스 감도
    public float lookSensitivity;

    // 카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;
    public Rigidbody myRigid;
    //[SerializeField]
    //private Animator animator;

    private float _moveDirX;
    private float _moveDirZ;

    public RaycastHit hitInfo;

    void Start()
    {
        // 초기화
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        //Walking();
        CameraRotation();
        CharacterRotation();
        TryMove();
    }

    void FixedUpdate()
    {
        Move();
    }

    // 앉기 시도
    private void TryCrouch()
    {
        if (!isRun)
        {
            if (Input.GetKey(KeyCode.LeftControl) & isGround)
            {
                isCrouch = true;
                Crouch();
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                isCrouch = false;
                Crouch();
            }

            if (!isGround & isCrouch) // 공중에서는 앉기가 풀림
            {
                isCrouch = false;
                Crouch();
            }
        }
    }

    // 앉기 동작
    private void Crouch()
    {
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
            standCollider.enabled = false;
            crouchCollider.enabled = true;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
            standCollider.enabled = true;
            crouchCollider.enabled = false;
        }

        StartCoroutine(CrouchCoroutine());
        //animator.SetBool("Crouching", isCrouch);
    }

    // 부드러운 앉기 동작
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while (_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0f);
            if (count > 15)
            {
                break;
            }
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    // 지면 체크
    private void IsGround()
    {
        Debug.DrawRay(transform.position - transform.up * 0.1f, -transform.up * 0.8f, Color.red);

        if(Physics.Raycast(transform.position - transform.up * 0.1f, -transform.up, out hitInfo, 0.8f))
        {
            isGround = true;
            jumpVelocity = _velocity;
        }
        else
        {
            isGround = false;
        }
    }

    // 점프 시도
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            Jump();
        }
    }

    // 점프 동작
    private void Jump()
    {
        myRigid.velocity = transform.up * jumpForce;
    }


    // 달리기 시도
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && isWalk)
        {
            Running();
        }
        else // if (Input.GetKeyUp(KeyCode.LeftShift) || !isWalk)
        {
            RunningCancel();
        }
    }

    // 달리기 동작
    private void Running()
    {
        if (!isCrouch)
        {
            isRun = true;
            applySpeed = runSpeed;
            //animator.SetBool("Running", true);
        }
    }

    // 달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        if (isCrouch == true)
        {
            applySpeed = crouchSpeed;
        }
        else
        {
            applySpeed = walkSpeed;
        }

        //animator.SetBool("Running", false);
    }

    /*
    // 애니메이션
    private void Walking()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("WalkingF", true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("WalkingF", false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("WalkingB", true);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("WalkingB", false);
        }
    }
    */

    private void TryMove()
    {
        _moveDirX = Input.GetAxisRaw("Horizontal"); // -1 0 1이 나옴
        _moveDirZ = Input.GetAxisRaw("Vertical"); // -1 0 1이 나옴
    }


    // 플레이어 이동
    private void Move()
    {
        _moveHorizontal = transform.right * _moveDirX;
        _moveVertical = transform.forward * _moveDirZ;

        _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; // normalized 벡터 길이를 1로 변화

        if (isGround) // 땅에서는 속도 그대로 움직임
        {
            myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);  // Time.deltaTime의 값은 60프레임 기준 약 0.016이다.
        }
        else // 공중에서는 선형 보간을 통해 방향과 속도가 서서히 바뀜
        {
            if (isWalk) 
            {
                jumpVelocity = Vector3.Lerp(jumpVelocity, _velocity, 0.1f); // 이동하는 방향으로 속도가 점점 늘어남
            }
            else
            {
                jumpVelocity = Vector3.Lerp(jumpVelocity, Vector3.zero, 0.1f); // 이동하지 않으면 속도가 점점 줄어듦
            }
            
            myRigid.MovePosition(transform.position + jumpVelocity * Time.deltaTime);
        }

        MoveCheck(_velocity);
    }

    private void MoveCheck(Vector3 _velocity) //(float MoveXZ)
    {
        if (!isCrouch)
        {
            if (_velocity.magnitude >= 0.1f) { isWalk = true; } // Vector3.magnitude 0,0,0부터 좌표까지 거리
            else if (_velocity.magnitude < 0.1f) { isWalk = false; }

        }
    }

    // 좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity * 10f;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    // 상하 카메라 회전
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity * 10f;
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