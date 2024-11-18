using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown_Rooftop : MonoBehaviour
{
    [SerializeField]
    GameObject playerCam; // 플레이어 카메라


    [SerializeField]
    GameObject clownHead;

    public Item switch_item;
    public bool is_hand_up_enabled = true;


    void Start()
    {
        GetComponent<Animator>().Play("Breathe");
    }

    // Update is called once per frame
    void Update()
    {
        // 스위치 먹으면 손 들기 끄기
        if (switch_item.enabled && is_hand_up_enabled)
        {
            is_hand_up_enabled = false;
            GetComponent<Animator>().SetBool("Hand_Up", false);
        }
    }

    void FixedUpdate()
    {
        LookPlayer();
    }

    void LookPlayer()
    {
        clownHead.transform.rotation = Quaternion.LookRotation(playerCam.transform.position - clownHead.transform.position) * Quaternion.Euler(new Vector3(0f, 90f, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && is_hand_up_enabled)
        {
            GetComponent<Animator>().SetBool("Hand_Up", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && is_hand_up_enabled)
        {
            GetComponent<Animator>().SetBool("Hand_Up", false);
        }
    }
}
