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
        GetComponent<GetComponentScript>().outline.enabled = false; // �ܰ��� ��Ȱ��ȭ
        yield return null;
    }
}
