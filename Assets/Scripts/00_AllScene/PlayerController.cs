using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public int team;

    // 플레이어 이동속도
    public float masterSpeed = 1f;
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
    private bool isAbove = false;
    private bool isTryStand = false;
    public bool isMouseLocked = false;
    private bool isSetDragToZero = false;
    public float knockbackTimer = 0;

    // 앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    public float crouchPosY;
    public float originPosY;
    private float applyCrouchPosY;
    public CapsuleCollider standCollider;
    public CapsuleCollider crouchCollider;

    // 땅 착지 여부
    int layerMask_NoTrigger;

    // 마우스 감도
    public float lookSensitivity;

    // 카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    // 필요한 컴포넌트
    public Camera theCamera;
    public Rigidbody myRigid;
    //[SerializeField]
    //private Animator animator;

    private float _moveDirX;
    private float _moveDirZ;

    public RaycastHit hitInfo;
    public RaycastHit hitInfo2;

    void Start()
    {
        // 초기화
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
        layerMask_NoTrigger = (-1) - LayerMask.GetMask("Trigger");
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
        CheckAbove();
        //ChangeDrag();
        KnockbackTimer();

        if (Input.GetKeyDown(KeyCode.H))
        {
            knockbackTimer = 0.5f;
            myRigid.AddForce((transform.forward * -10f) + (transform.right * 0f) + (transform.up * 5f), ForceMode.Impulse);
        }
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
            if (Input.GetKey(KeyCode.LeftControl) && isGround)
            {
                isTryStand = false;
                isCrouch = true;
                Crouch();
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {

                isTryStand = true;
            }

            if(isTryStand && !isAbove)
            {
                isTryStand = false;
                isCrouch = false;
                Crouch();
            }

            if (!isGround && isCrouch) // 공중에서는 앉기가 풀림
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
            yield return new WaitForSeconds(0.016f);
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }




    // 머리 위 체크. 머리 위에 물체가 있으면 못 일어남
    void CheckAbove()
    {
        Debug.DrawRay(transform.position - transform.up * 0.7f, transform.up * 1.65f, Color.blue);

        if (Physics.Raycast(transform.position - transform.up * 0.7f, transform.up, out hitInfo, 1.6f))
        {
            isAbove = true;
        }
        else
        {
            isAbove = false;
        }
    }

    // 지면 체크
    private void IsGround()
    {
        Debug.DrawRay(transform.position - transform.up * 0.7f, -transform.up * 0.2f, Color.red);

        if(Physics.Raycast(transform.position - transform.up * 0.7f, -transform.up, out hitInfo, 0.3f, layerMask_NoTrigger))
        {
            isGround = true;
            jumpVelocity = _velocity; // 점프하면 점프 직전의 이동 속도를 공중에서의 속도에도 적용
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
            //StartCoroutine(SetDragToZero());
            Jump();
        }
    }

    // 점프 동작
    private void Jump()
    {
        myRigid.velocity = new Vector3(myRigid.velocity.x, jumpForce, myRigid.velocity.z);
    }

    // 플레이어 rigidbody의 drag 바꾸기. drag(공기 저항)가 0이면 밀렸을 때 안 멈추고 계속 미끄러지는 현상이 있음
    private void ChangeDrag()
    {
        if (isGround && !isSetDragToZero) // 땅이면 drag 10
        {
            myRigid.drag = 10;
        }
        else if (!isGround && !isSetDragToZero) // 공중이면 drag 0
        {
            myRigid.drag = 0;
        }
    }

    // 점프키를 누르고 0.1초 동안은 drag를 0으로 유지. 이게 없으면 점프하자마자 땅이 감지되고 drag가 10이 되어서 점프를 못 함
    IEnumerator SetDragToZero()
    {
        isSetDragToZero = true;
        myRigid.drag = 0;

        yield return new WaitForSeconds(0.1f); // 점프 후 0.1초 동안은 drag가 0이고, 이후에 땅에 닿으면 다시 10으로 변환

        isSetDragToZero = false;
    }




    // 달리기 시도
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && isWalk)
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




    // 이동 스턴 타이머
    private void KnockbackTimer()
    {
        knockbackTimer = knockbackTimer > 0 ? knockbackTimer - Time.deltaTime : 0;
    }

    // 플레이어 이동
    private void TryMove()
    {
        _moveDirX = Input.GetAxisRaw("Horizontal"); // -1 0 1이 나옴
        _moveDirZ = Input.GetAxisRaw("Vertical"); // -1 0 1이 나옴
    }

    private void Move()
    {
        if(knockbackTimer == 0)
        {
            _moveHorizontal = transform.right * _moveDirX;
            _moveVertical = transform.forward * _moveDirZ;

            _velocity = (_moveHorizontal + _moveVertical).normalized * (applySpeed * masterSpeed); // normalized 벡터 길이를 1로 변화

            if (isGround) // 땅에서는 속도 그대로 움직임
            {
                //myRigid.MovePosition(transform.position + _velocity);
                myRigid.velocity = new Vector3(_velocity.x, myRigid.velocity.y, _velocity.z);
            }
            else // 공중에서는 선형 보간을 통해 방향과 속도가 서서히 바뀜
            {
                if (isWalk)
                {
                    jumpVelocity = Vector3.Lerp(jumpVelocity, _velocity, 0.1f); // 이동하는 방향으로 속도가 점점 늘어남
                }
                else
                {
                    jumpVelocity = Vector3.Lerp(jumpVelocity, jumpVelocity.normalized * walkSpeed * 0.5f, 0.1f);
                }

                //myRigid.MovePosition(transform.position + jumpVelocity);
                myRigid.velocity = new Vector3(jumpVelocity.x, myRigid.velocity.y, jumpVelocity.z);
            }

            MoveCheck(_velocity);
        }
        else
        {
            jumpVelocity = new Vector3(myRigid.velocity.x, 0, myRigid.velocity.z);
        }
    }

    private void MoveCheck(Vector3 _velocity) //(float MoveXZ)
    {
        if (!isCrouch)
        {
            if (_velocity.magnitude >= 0.01f) { isWalk = true; } // Vector3.magnitude 0,0,0부터 좌표까지 거리
            else if (_velocity.magnitude < 0.01f) { isWalk = false; }
        }
    }




    // 좌우 캐릭터 회전
    private void CharacterRotation()
    {
        if (!isMouseLocked)
        {
            float _yRotation = Input.GetAxisRaw("Mouse X");
            Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity * 0.1f;
            transform.localEulerAngles = transform.localEulerAngles + _characterRotationY;
        }
    }

    // 상하 카메라 회전
    private void CameraRotation()
    {
        if (!isMouseLocked)
        {
            float _xRotation = Input.GetAxisRaw("Mouse Y");
            float _cameraRotationX = _xRotation * lookSensitivity * 0.1f;
            currentCameraRotationX -= _cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }

    // 상하 카메라 회전값 설정
    public void SetCurrentCameraRotationX(float x)
    {
        currentCameraRotationX = x;
    }
}