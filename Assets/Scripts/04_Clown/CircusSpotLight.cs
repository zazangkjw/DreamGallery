using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusSpotLight : MonoBehaviour
{
    float originX;
    float originY;

    [SerializeField]
    GameObject lightsParent;

    // Start is called before the first frame update
    void Start()
    {
        originX = transform.localEulerAngles.x;
        originY = transform.localEulerAngles.y;
        StartCoroutine(RotateLight());
        if (lightsParent != null )
        {
            StartCoroutine(RotateLightsParent());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RotateLight()
    {
        while (true)
        {
            while (transform.localEulerAngles.x > originX - 35f)
            {
                transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x - (35f / 5f) * Time.deltaTime, transform.localEulerAngles.y - (180f / 5f) * Time.deltaTime, 0);
                yield return null;
            }
            transform.localRotation = Quaternion.Euler(originX - 35f, originY - 180f, 0);

            yield return new WaitForSeconds(5f);

            while (transform.localEulerAngles.x < originX)
            {
                transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x + (35f / 5f) * Time.deltaTime, transform.localEulerAngles.y - (180f / 5f) * Time.deltaTime, 0);
                yield return null;
            }
            transform.localRotation = Quaternion.Euler(originX, originY, 0);
        }
    }

    IEnumerator RotateLightsParent()
    {
        while (true)
        {
            lightsParent.transform.localRotation = Quaternion.Euler(0, lightsParent.transform.localEulerAngles.y + (360f / 20f) * Time.deltaTime, 0);
            yield return null;
        }
    }
}
