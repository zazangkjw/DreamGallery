using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // 스크립트를 플레이어에 적용하고 플랫폼 오브젝트의 태그를 Platform으로 설정. 트리거 안에 들어가면 움직이는 플랫폼이 부모로 설정됨

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Platform")
        {
            transform.SetParent(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            transform.SetParent(null);
        }
    }
}
