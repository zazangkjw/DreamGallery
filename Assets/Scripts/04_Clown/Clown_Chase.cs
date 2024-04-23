using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Clown_Chase : MonoBehaviour
{
    Rigidbody rb; // ���� ������ٵ�
    float chaseSpeedX = 0.02f;
    float chaseSpeedY = 0.01f;


    [SerializeField]
    GameObject player; // �÷��̾�

    [SerializeField]
    GameObject playerCam; // �÷��̾� ī�޶�


    [SerializeField]
    GameObject clown; // ��ģ ����

    [SerializeField]
    Rigidbody clownRoot; 

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
        clown.GetComponent<Animator>().Play("Climb");
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
            }
            else // ���� ������ ���� ��� ���� ����
            {
                point = point_Rooftop.position;
            }

            // ���� �� �Ʒ� �̵�
            rb.MovePosition(Vector3.Lerp(rb.position, new Vector3(rb.position.x, point.y, rb.position.z), chaseSpeedY));
            /*if (rb.position.y < point.y)
            {
                rb.MovePosition(new Vector3(rb.position.x, rb.position.y + chaseSpeedY, rb.position.z));
            }
            else if (rb.position.y > point.y)
            {
                rb.MovePosition(new Vector3(rb.position.x, rb.position.y - chaseSpeedY, rb.position.z));
            }*/

            // ���Ͱ� ���� ����Ʈ���� �Ʒ��� �־�߸� ��ģ ���� Ȱ��ȭ
            if (rb.position.y < point_Rooftop.position.y - 1)
            {
                clown.gameObject.SetActive(true);
                MoveClown();
            }
            else if(rb.position.y >= point_Rooftop.position.y - 1)
            {
                clown.gameObject.SetActive(false);
            }
        }
        else // �߰� ���°� �ƴ� ���
        {
            clown.gameObject.SetActive(false);
            rb.position = point_Rooftop.position;
            // rb.rotation = Quaternion.Euler(Vector3.forward);
        }
    }

    // ��ģ ���� ������
    void MoveClown()
    {
        Debug.DrawRay(transform.position, transform.forward * 20f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hitInfoCenter, 20f, layerMask_Wall))
        {
            clownRoot.MovePosition(hitInfoCenter.point + (-hitInfoCenter.normal * 0.55f) + (Vector3.down * 1.1f));
            clownRoot.rotation = Quaternion.LookRotation(hitInfoCenter.normal) * Quaternion.Euler(new Vector3(0, 90f, 90f));
            clownHead.transform.rotation = Quaternion.LookRotation(playerCam.transform.position - clownHead.transform.position) * Quaternion.Euler(new Vector3(0f, 90f, 0f));        
        }
    } 
}
