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
    public GameObject guard001_check; // guard001 체크용




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

                    mouseText.text = GameManager.instance.textFileManager.ui[24]; // "대화하기" 텍스트 나옴
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
                    StartCoroutine(man001Coroutine());
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

        // guard
        if (isChecking)
        {
            if (hitObject == guard001_check)
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
                    StartCoroutine(PickUpItemCoroutine());
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




    // man001 대화
    IEnumerator man001Coroutine()
    {
        // 필요하면 "GameObject man001_this = hitObject" 생성해서 사용

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화
        man001_check.GetComponent<HeadTracking>().isLooking = true;

        // 대화
        if (true)
        {
            putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[19]["Content"], // "이 물고기 인간이 우리 방으로 가는 길을 막아버렸어"
                                                                   (string)GameManager.instance.textFileManager.dialog[20]["Content"], // "아까부터 계속되는 진동에 겁을 먹은 것 같아"
                                                                   (string)GameManager.instance.textFileManager.dialog[21]["Content"]}); // "아래층에 대체 무슨 일이 벌어진 거지?"
        }

        yield return new WaitUntil(() => putDialogScript.isClickMode == false);

        man001_check.GetComponent<HeadTracking>().isLooking = false;
    }

    // guard001 대화
    IEnumerator guard001Coroutine()
    {
        // 필요하면 "GameObject man001_this = hitObject" 생성해서 사용

        preObject.GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화
        guard001_check.GetComponent<HeadTracking>().isLooking = true;

        if(currentItem.itemName.Equals("Gift"))
        {

        }
        else
        {
            // 대화
            if (true)
            {
                putDialogScript.putDialogPrintWithClick(new string[] { (string)GameManager.instance.textFileManager.dialog[19]["Content"], // "이 물고기 인간이 우리 방으로 가는 길을 막아버렸어"
                                                                       (string)GameManager.instance.textFileManager.dialog[20]["Content"], // "아까부터 계속되는 진동에 겁을 먹은 것 같아"
                                                                       (string)GameManager.instance.textFileManager.dialog[21]["Content"]}); // "아래층에 대체 무슨 일이 벌어진 거지?"
            }

            yield return new WaitUntil(() => putDialogScript.isClickMode == false);

            guard001_check.GetComponent<HeadTracking>().isLooking = false;
        }
    }
}
