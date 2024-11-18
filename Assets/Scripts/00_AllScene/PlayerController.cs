using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public int team;

    // �÷��̾� �̵��ӵ�
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

    // ���� ����
    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;
    private bool isAbove = false;
    private bool isTryStand = false;
    public bool isMouseLocked = false;
    private bool isSetDragToZero = false;
    public float knockbackTimer = 0;

    // �ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    public float crouchPosY;
    public float originPosY;
    private float applyCrouchPosY;
    public CapsuleCollider standCollider;
    public CapsuleCollider crouchCollider;

    // �� ���� ����
    int layerMask_NoTrigger;

    // ���콺 ����
    public float lookSensitivity;

    // ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    // �ʿ��� ������Ʈ
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
        // �ʱ�ȭ
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




    // �ɱ� �õ�
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

            if (!isGround && isCrouch) // ���߿����� �ɱⰡ Ǯ��
            {
                isCrouch = false;
                Crouch();
            }
        }
    }

    // �ɱ� ����
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

    // �ε巯�� �ɱ� ����
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




    // �Ӹ� �� üũ. �Ӹ� ���� ��ü�� ������ �� �Ͼ
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

    // ���� üũ
    private void IsGround()
    {
        Debug.DrawRay(transform.position - transform.up * 0.7f, -transform.up * 0.2f, Color.red);

        if(Physics.Raycast(transform.position - transform.up * 0.7f, -transform.up, out hitInfo, 0.3f, layerMask_NoTrigger))
        {
            isGround = true;
            jumpVelocity = _velocity; // �����ϸ� ���� ������ �̵� �ӵ��� ���߿����� �ӵ����� ����
        }
        else
        {
            isGround = false;
        }
    }




    // ���� �õ�
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            //StartCoroutine(SetDragToZero());
            Jump();
        }
    }

    // ���� ����
    private void Jump()
    {
        myRigid.velocity = new Vector3(myRigid.velocity.x, jumpForce, myRigid.velocity.z);
    }

    // �÷��̾� rigidbody�� drag �ٲٱ�. drag(���� ����)�� 0�̸� �з��� �� �� ���߰� ��� �̲������� ������ ����
    private void ChangeDrag()
    {
        if (isGround && !isSetDragToZero) // ���̸� drag 10
        {
            myRigid.drag = 10;
        }
        else if (!isGround && !isSetDragToZero) // �����̸� drag 0
        {
            myRigid.drag = 0;
        }
    }

    // ����Ű�� ������ 0.1�� ������ drag�� 0���� ����. �̰� ������ �������ڸ��� ���� �����ǰ� drag�� 10�� �Ǿ ������ �� ��
    IEnumerator SetDragToZero()
    {
        isSetDragToZero = true;
        myRigid.drag = 0;

        yield return new WaitForSeconds(0.1f); // ���� �� 0.1�� ������ drag�� 0�̰�, ���Ŀ� ���� ������ �ٽ� 10���� ��ȯ

        isSetDragToZero = false;
    }




    // �޸��� �õ�
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

    // �޸��� ����
    private void Running()
    {
        if (!isCrouch)
        {
            isRun = true;
            applySpeed = runSpeed;
            //animator.SetBool("Running", true);
        }
    }

    // �޸��� ���
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
    // �ִϸ��̼�
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




    // �̵� ���� Ÿ�̸�
    private void KnockbackTimer()
    {
        knockbackTimer = knockbackTimer > 0 ? knockbackTimer - Time.deltaTime : 0;
    }

    // �÷��̾� �̵�
    private void TryMove()
    {
        _moveDirX = Input.GetAxisRaw("Horizontal"); // -1 0 1�� ����
        _moveDirZ = Input.GetAxisRaw("Vertical"); // -1 0 1�� ����
    }

    private void Move()
    {
        if(knockbackTimer == 0)
        {
            _moveHorizontal = transform.right * _moveDirX;
            _moveVertical = transform.forward * _moveDirZ;

            _velocity = (_moveHorizontal + _moveVertical).normalized * (applySpeed * masterSpeed); // normalized ���� ���̸� 1�� ��ȭ

            if (isGround) // �������� �ӵ� �״�� ������
            {
                //myRigid.MovePosition(transform.position + _velocity);
                myRigid.velocity = new Vector3(_velocity.x, myRigid.velocity.y, _velocity.z);
            }
            else // ���߿����� ���� ������ ���� ����� �ӵ��� ������ �ٲ�
            {
                if (isWalk)
                {
                    jumpVelocity = Vector3.Lerp(jumpVelocity, _velocity, 0.1f); // �̵��ϴ� �������� �ӵ��� ���� �þ
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
            if (_velocity.magnitude >= 0.01f) { isWalk = true; } // Vector3.magnitude 0,0,0���� ��ǥ���� �Ÿ�
            else if (_velocity.magnitude < 0.01f) { isWalk = false; }
        }
    }




    // �¿� ĳ���� ȸ��
    private void CharacterRotation()
    {
        if (!isMouseLocked)
        {
            float _yRotation = Input.GetAxisRaw("Mouse X");
            Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity * 0.1f;
            transform.localEulerAngles = transform.localEulerAngles + _characterRotationY;
        }
    }

    // ���� ī�޶� ȸ��
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

    // ���� ī�޶� ȸ���� ����
    public void SetCurrentCameraRotationX(float x)
    {
        currentCameraRotationX = x;
    }
}