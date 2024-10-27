using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Clown_Chase : MonoBehaviour
{
    Rigidbody rb; // ���� ������ٵ�
    float chaseSpeedX = 0.02f; // 0.02f
    float chaseSpeedY = 0.01f; // 0.01f


    [SerializeField]
    GameObject player; // �÷��̾�

    [SerializeField]
    GameObject playerCam; // �÷��̾� ī�޶�

    [SerializeField]
    GameObject clownMesh;

    [SerializeField]
    Rigidbody clownRoot;

    [SerializeField]
    GameObject clownRootForUnity;

    [SerializeField]
    GameObject clownHead;


    int layerMask_Wall; // ����ĳ��Ʈ�� �� �ݶ��̴� ���̾�
    RaycastHit hitInfoCenter;
    RaycastHit hitInfoPlayer;
    RaycastHit hitInfoPlayer_Pre;
    Vector3 point; // ��ģ ������ ������ ����Ʈ
    public Transform point_Rooftop; // ���� ����Ʈ

    public bool isChasing = false; // �߰��� ���۵ƴ���
    public bool isSafe = false; // �÷��̾ ���� ������ �ִ���

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        layerMask_Wall = LayerMask.GetMask("Wall"); // layerMask_Wall = 1 << LayerMask.NameToLayer("Wall");
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        CheckWall();
        MoveCenter();
    }

    // �÷��̾� ������ ������ ��� ����ĳ��Ʈ ����Ʈ üũ
    void CheckWall()
    {
        Debug.DrawRay(player.transform.position, Vector3.forward * 2f, Color.green);
        Debug.DrawRay(player.transform.position, Vector3.back * 2f, Color.green);
        Debug.DrawRay(player.transform.position, Vector3.left * 2f, Color.green);
        Debug.DrawRay(player.transform.position, Vector3.right * 2f, Color.green);
        Debug.DrawRay(player.transform.position, Vector3.Normalize(Vector3.forward + Vector3.right) * 2f, Color.green);
        Debug.DrawRay(player.transform.position, Vector3.Normalize(Vector3.forward + Vector3.left) * 2f, Color.green);
        Debug.DrawRay(player.transform.position, Vector3.Normalize(Vector3.back + Vector3.right) * 2f, Color.green);
        Debug.DrawRay(player.transform.position, Vector3.Normalize(Vector3.back + Vector3.left) * 2f, Color.green);

        if (Physics.Raycast(player.transform.position, Vector3.forward, out hitInfoPlayer, 2f, layerMask_Wall)) { hitInfoPlayer_Pre = hitInfoPlayer; }
        else if (Physics.Raycast(player.transform.position, Vector3.back, out hitInfoPlayer, 2f, layerMask_Wall)) { hitInfoPlayer_Pre = hitInfoPlayer; }
        else if (Physics.Raycast(player.transform.position, Vector3.left, out hitInfoPlayer, 2f, layerMask_Wall)) { hitInfoPlayer_Pre = hitInfoPlayer; }
        else if (Physics.Raycast(player.transform.position, Vector3.right, out hitInfoPlayer, 2f, layerMask_Wall)) { hitInfoPlayer_Pre = hitInfoPlayer; }
        else if (Physics.Raycast(player.transform.position, Vector3.Normalize(Vector3.forward + Vector3.right), out hitInfoPlayer, 2f, layerMask_Wall)) { hitInfoPlayer_Pre = hitInfoPlayer; }
        else if (Physics.Raycast(player.transform.position, Vector3.Normalize(Vector3.forward + Vector3.left), out hitInfoPlayer, 2f, layerMask_Wall)) { hitInfoPlayer_Pre = hitInfoPlayer; }
        else if (Physics.Raycast(player.transform.position, Vector3.Normalize(Vector3.back + Vector3.right), out hitInfoPlayer, 2f, layerMask_Wall)) { hitInfoPlayer_Pre = hitInfoPlayer; }
        else if (Physics.Raycast(player.transform.position, Vector3.Normalize(Vector3.back + Vector3.left), out hitInfoPlayer, 2f, layerMask_Wall)) { hitInfoPlayer_Pre = hitInfoPlayer; }
        else { hitInfoPlayer = hitInfoPlayer_Pre; } // ����ĳ��Ʈ�� ����� ����Ű�� ������ ����Ʈ�� �״�� �߰�
    }

    // ���� ������
    void MoveCenter()
    {
        if (isChasing) // �߰��� ���۵� ���
        {   
            // �÷��̾� �߰� or ���� ����
            if (!isSafe) // ���� ���� �ۿ� ���� ��� �÷��̾� �߰�
            {
                point = hitInfoPlayer.point;
                rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(point.x - transform.position.x, 0, point.z - transform.position.z)), chaseSpeedX)); // �÷��̾ �߰��� ���� ���� �¿� ȸ��
                /*if (Quaternion.LookRotation(new Vector3(point.x - transform.position.x, 0, point.z - transform.position.z)).eulerAngles.y > rb.rotation.eulerAngles.y)
                {
                    rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + (Vector3.up * chaseSpeedX)));
                }
                else if (Quaternion.LookRotation(new Vector3(point.x - transform.position.x, 0, point.z - transform.position.z)).eulerAngles.y < rb.rotation.eulerAngles.y)
                {
                    rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles - (Vector3.up * chaseSpeedX)));
                }*/
            }
            else // ���� ������ ���� ��� ���� ����
            {
                point = point_Rooftop.position;
            }

            // ���� �� �Ʒ� �̵�
            rb.MovePosition(Vector3.Lerp(rb.position, new Vector3(rb.position.x, point.y, rb.position.z), chaseSpeedY));
            /*if(point.y < rb.position.y)
            {
                rb.MovePosition(rb.position - (Vector3.up * chaseSpeedY));
            }
            else if(point.y > rb.position.y)
            {
                rb.MovePosition(rb.position + (Vector3.up * chaseSpeedY));
            }*/

            // ���Ͱ� ���� ����Ʈ���� �Ʒ��� �־�߸� ��ģ ���� Ȱ��ȭ
            if (rb.position.y < point_Rooftop.position.y - 1)
            {
                clownMesh.SetActive(true);
                MoveClown();
            }
            else if(rb.position.y >= point_Rooftop.position.y - 1)
            {
                clownMesh.SetActive(false);
            }
        }
        else // �߰� ���°� �ƴ� ���
        {
            clownMesh.SetActive(false);
            rb.position = point_Rooftop.position;
        }
    }

    //public float a, b, c;

    // ��ģ ���� ������
    void MoveClown()
    {
        Debug.DrawRay(transform.position, transform.forward * 20f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfoCenter, 20f, layerMask_Wall))
        {
            clownRoot.MovePosition(hitInfoCenter.point + (-hitInfoCenter.normal * 0.55f) + (Vector3.down * 0.95f));
            clownRoot.rotation = Quaternion.LookRotation(hitInfoCenter.normal) * Quaternion.Euler(new Vector3(0f, 90f, 90f));
            //clownRootForUnity.transform.rotation = Quaternion.LookRotation(playerCam.transform.position - clownRootForUnity.transform.position);// ���� �÷��̾� �������� ȸ��
            //clownRootForUnity.transform.localEulerAngles = new Vector3(0, clownRootForUnity.transform.localEulerAngles.y, 0); // ���� �÷��̾� �������� ȸ��
            clownHead.transform.rotation = Quaternion.LookRotation(playerCam.transform.position - clownHead.transform.position) * Quaternion.Euler(new Vector3(0f, 90f, 0f));

            // �׽�Ʈ
            /*clownRoot.MovePosition(hitInfoCenter.point);
            clownRoot.rotation = Quaternion.LookRotation(hitInfoCenter.normal) * Quaternion.Euler(new Vector3(0f, 90f, -90f));
            clownRootForUnity.transform.rotation = Quaternion.LookRotation(playerCam.transform.position - clownRootForUnity.transform.position);// ���� �÷��̾� �������� ȸ��
            clownRootForUnity.transform.localEulerAngles = new Vector3(0, clownRootForUnity.transform.localEulerAngles.y, 0) + Quaternion.Euler(new Vector3(0f, 90f, 0f)).eulerAngles; // ���� �÷��̾� �������� ȸ��
            clownHead.transform.rotation = Quaternion.LookRotation(playerCam.transform.position - clownHead.transform.position) * Quaternion.Euler(new Vector3(0f, 90f, 0f));*/
        }
    } 
}
