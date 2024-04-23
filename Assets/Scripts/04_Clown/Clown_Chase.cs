using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Clown_Chase : MonoBehaviour
{
    Rigidbody rb; // 센터 리지드바디
    float chaseSpeedX = 0.02f;
    float chaseSpeedY = 0.01f;


    [SerializeField]
    GameObject player; // 플레이어

    [SerializeField]
    GameObject playerCam; // 플레이어 카메라


    [SerializeField]
    GameObject clown; // 미친 광대

    [SerializeField]
    Rigidbody clownRoot; 

    [SerializeField]
    GameObject clownHead;


    int layerMask_Wall; // 레이캐스트용 벽 콜라이더 레이어
    RaycastHit hitInfoCenter;
    RaycastHit hitInfoPlayer;
    RaycastHit hitInfoPlayer_Pre;
    Vector3 point; // 미친 광대의 목적지 포인트
    public Transform point_Rooftop; // 옥상 포인트

    public bool isChasing = false; // 추격이 시작됐는지
    public bool isSafe = false; // 플레이어가 안전 구역에 있는지

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

    // 플레이어 몸에서 벽으로 쏘는 레이캐스트 포인트 체크
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
        else { hitInfoPlayer = hitInfoPlayer_Pre; } // 레이캐스트가 허공을 가리키면 이전의 포인트를 그대로 추격
    }

    // 센터 움직임
    void MoveCenter()
    {
        if (isChasing) // 추격이 시작된 경우
        {   
            // 플레이어 추격 or 옥상 복귀
            if (!isSafe) // 안전 구역 밖에 있을 경우 플레이어 추격
            {
                point = hitInfoPlayer.point;
                rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(point.x - transform.position.x, 0, point.z - transform.position.z)), chaseSpeedX)); // 플레이어를 추격할 때만 센터 좌우 회전
            }
            else // 안전 구역에 있을 경우 옥상 복귀
            {
                point = point_Rooftop.position;
            }

            // 센터 위 아래 이동
            rb.MovePosition(Vector3.Lerp(rb.position, new Vector3(rb.position.x, point.y, rb.position.z), chaseSpeedY));
            /*if (rb.position.y < point.y)
            {
                rb.MovePosition(new Vector3(rb.position.x, rb.position.y + chaseSpeedY, rb.position.z));
            }
            else if (rb.position.y > point.y)
            {
                rb.MovePosition(new Vector3(rb.position.x, rb.position.y - chaseSpeedY, rb.position.z));
            }*/

            // 센터가 옥상 포인트보다 아래에 있어야만 미친 광대 활성화
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
        else // 추격 상태가 아닐 경우
        {
            clown.gameObject.SetActive(false);
            rb.position = point_Rooftop.position;
            // rb.rotation = Quaternion.Euler(Vector3.forward);
        }
    }

    // 미친 광대 움직임
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
