using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �÷��̾� �̵��ӵ�
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

    // �ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // �� ���� ����
    public CapsuleCollider standCollider;
    public CapsuleCollider crouchCollider;

    // ���콺 ����
    public float lookSensitivity;

    // ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    // �ʿ��� ������Ʈ
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
        // �ʱ�ȭ
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

    // �ɱ� �õ�
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

            if (!isGround & isCrouch) // ���߿����� �ɱⰡ Ǯ��
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
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    // ���� üũ
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

    // ���� �õ�
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            Jump();
        }
    }

    // ���� ����
    private void Jump()
    {
        myRigid.velocity = transform.up * jumpForce;
    }


    // �޸��� �õ�
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

    private void TryMove()
    {
        _moveDirX = Input.GetAxisRaw("Horizontal"); // -1 0 1�� ����
        _moveDirZ = Input.GetAxisRaw("Vertical"); // -1 0 1�� ����
    }


    // �÷��̾� �̵�
    private void Move()
    {
        _moveHorizontal = transform.right * _moveDirX;
        _moveVertical = transform.forward * _moveDirZ;

        _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; // normalized ���� ���̸� 1�� ��ȭ

        if (isGround) // �������� �ӵ� �״�� ������
        {
            myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);  // Time.deltaTime�� ���� 60������ ���� �� 0.016�̴�.
        }
        else // ���߿����� ���� ������ ���� ����� �ӵ��� ������ �ٲ�
        {
            if (isWalk) 
            {
                jumpVelocity = Vector3.Lerp(jumpVelocity, _velocity, 0.1f); // �̵��ϴ� �������� �ӵ��� ���� �þ
            }
            else
            {
                jumpVelocity = Vector3.Lerp(jumpVelocity, Vector3.zero, 0.1f); // �̵����� ������ �ӵ��� ���� �پ��
            }
            
            myRigid.MovePosition(transform.position + jumpVelocity * Time.deltaTime);
        }

        MoveCheck(_velocity);
    }

    private void MoveCheck(Vector3 _velocity) //(float MoveXZ)
    {
        if (!isCrouch)
        {
            if (_velocity.magnitude >= 0.1f) { isWalk = true; } // Vector3.magnitude 0,0,0���� ��ǥ���� �Ÿ�
            else if (_velocity.magnitude < 0.1f) { isWalk = false; }

        }
    }

    // �¿� ĳ���� ȸ��
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity * 10f;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    // ���� ī�޶� ȸ��
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity * 10f;
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