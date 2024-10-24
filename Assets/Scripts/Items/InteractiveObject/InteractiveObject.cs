using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public IEnumerator currentCoroutine;

    public virtual void Action()
    {
        StartCoroutine(currentCoroutine = ActionCoroutine());
    }

    public virtual IEnumerator ActionCoroutine()
    {
        GetComponent<GetComponentScript>().outline.enabled = false; // 외곽선 비활성화
        yield return null;
    }
}
