using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipRaycast : DefaultRaycast
{
    // AudioSource
    // public AudioSource circusSong;

    public GameObject man001_check; // man001(팔짱 낀 사람) 체크용
    public GameObject guard001_check; bool isInteractable_guard001 = true; // guard001 체크용
    public GameObject guard_check; // guard 체크용
    public DoorSensor doorSensor;




    private void Start()
    {
        WhenStart();
    }

    private void Update()
    {
        WhenUpdate();
    }




    public override void CheckObject()
    {
        isChecking = true;

        // 아이템
        if (isChecking)
        {
            foreach (GameObject i in items_check)
            {
                if (hitObject == i)
                {
                    if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    }

                    preObject = hitObject;
                    preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                    mouseText.text = GameManager.instance.textFileManager.ui[26]; // "줍기" 텍스트 나옴
                    mouseText.enabled = true;

                    // E키 입력 시
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(PickUpItemCoroutine());
                    }

                    isChecking = false; // 이후의 항목들은 체크하지 않음

                    break;
                }
                else
                {
                    mouseText.enabled = false; // 텍스트 없어짐

                    if (preObject != null)
                    {
                        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                        preObject = null;
                    }
                }
            }
        }

        // man001
        if (isChecking)
        {
            if (hitObject == man001_check)
            {
                if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                mouseText.text = GameManager.instance.textFileManager.ui[24]; // "대화하기" 텍스트 나옴
                mouseText.enabled = true;

                // E키 입력 시
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(Man001Coroutine());
                }

                isChecking = false; // 이후의 항목들은 체크하지 않음
            }
            else
            {
                mouseText.enabled = false; // 텍스트 없어짐

                if (preObject != null)
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    preObject = null;
                }
            }
        }

        // Guard001
        if (isChecking)
        {
            if (hitObject == guard001_check && isInteractable_guard001)
            {
                if (preObject != hitObject && preObject != null) // 전 오브젝트와 현재 오브젝트가 다를 때, 전 오브젝트 외곽선 끄기
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                }

                preObject = hitObject;
                preObject.GetComponent<GetComponentScript>().outline.enabled = true; // 외곽선 켜기

                if (currentItem.itemName.Equals("Gift"))
                {
                    mouseText.text = GameManager.instance.textFileManager.ui[27]; // "선물하기" 텍스트 나옴
                }
                else
                {
                    mouseText.text = GameManager.instance.textFileManager.ui[24]; // "대화하기" 텍스트 나옴
                }
                mouseText.enabled = true;

                // E키 입력 시
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(Guard001Coroutine());
                }

                isChecking = false; // 이후의 항목들은 체크하지 않음
            }
            else
            {
                mouseText.enabled = false; // 텍스트 없어짐

                if (preObject != null)
                {
                    preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 끄기
                    preObject = null;
                }
            }
        }
    }




    // Man001 대화
    IEnumerator Man001Coroutine()
    {
        // 필요하면 "GameObject man001_this = hitObject" 생성해서 사용

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화
        man001_check.GetComponent<HeadTracking>().isLooking = true;

        // 대화
        putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[19]["Content"], // "이 물고기 인간이 우리 방으로 가는 길을 막아버렸어"
                                                               (string)GameManager.instance.textFileManager.dialog[20]["Content"], // "아까부터 계속되는 진동에 겁을 먹은 것 같아"
                                                               (string)GameManager.instance.textFileManager.dialog[21]["Content"]}); // "아래층에 대체 무슨 일이 벌어진 거지?"

        yield return new WaitUntil(() => putDialogScript.isClickMode == false);

        man001_check.GetComponent<HeadTracking>().isLooking = false;
    }

    // guard001 대화
    IEnumerator Guard001Coroutine()
    {
        // 필요하면 "GameObject man001_this = hitObject" 생성해서 사용

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화
        guard001_check.GetComponent<HeadTracking>().isLooking = true;

        if (currentItem.itemName.Equals("Gift"))
        {
            // 경비가 선물 받음
            isInteractable_guard001 = false;
            guard001_check.GetComponent<GetComponentScript>().animator.Play("TakeGift");

            // 선물 주기
            Item gift = currentItem;
            items_check.Remove(gift.gameObject);
            gift.enabled = false;
            gift.transform.SetParent(itemCategory.transform);
            gift.GetComponent<Collider>().enabled = true;
            gift.GetComponent<GetComponentScript>().outline.gameObject.layer = LayerMask.NameToLayer("Default");
            gift.transform.position = new Vector3(19.104f, 9.427f, -83.348f);
            gift.transform.rotation = Quaternion.identity;
            currentItem = empty;
            inventory[currentSlot - 1] = empty;

            // 대사
            guard_check.GetComponent<GetComponentScript>().animator.SetBool("Able", false);
            guard_check.GetComponent<HeadTracking>().isLooking = true;
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[24]["Content"], 3f); // "아! 선물을 주셔서 정말 감사합니다"
            yield return dialogDelay;
            GameObject playerCam = guard001_check.GetComponent<HeadTracking>().target;
            guard001_check.GetComponent<HeadTracking>().target = gift.gameObject;
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[25]["Content"], 3f); // "빨리 안에 뭐가 들어 있는지 보고 싶어요!"
            yield return dialogDelay;

            gift.GetComponent<GetComponentScript>().animator.Play("Surprise"); 
            gift.GetComponent<Rigidbody>().useGravity = true;
            gift.GetComponent<Rigidbody>().isKinematic = false;
            gift.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.VelocityChange);

            yield return dialogDelay;

            guard001_check.GetComponent<HeadTracking>().lookSpeed = 0.025f;
            guard001_check.GetComponent<HeadTracking>().target = playerCam;

            yield return dialogDelay;
            guard_check.GetComponent<HeadTracking>().isLooking = false;
            guard001_check.GetComponent<HeadTracking>().isLooking = false;
            putDialogScript.putDialogPrint((string)GameManager.instance.textFileManager.dialog[26]["Content"], 3f); // "으아악!"

            // 경비 넘어짐
            guard001_check.GetComponent<Collider>().enabled = false;
            guard001_check.GetComponent<GetComponentScript>().animator.Play("FallDown");
            guard_check.GetComponent<GetComponentScript>().animator.Play("FallDown");

            yield return dialogDelay;

            doorSensor.isLock = false;
        }
        else
        {
            // 대화
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[22]["Content"], // "이 문 너머는 출입 금지 구역입니다"
                                                                   (string)GameManager.instance.textFileManager.dialog[23]["Content"]}); // "돌아가주세요"

            yield return new WaitUntil(() => putDialogScript.isClickMode == false);

            guard001_check.GetComponent<HeadTracking>().isLooking = false;
        }
    }
}
