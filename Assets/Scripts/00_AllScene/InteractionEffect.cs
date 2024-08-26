using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionEffect : MonoBehaviour
{
    public GameObject target;
    public float distance;
    public float minDistance;
    public float maxDistance;
    float scale;

    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);

        if(distance > maxDistance)
        {
            transform.localScale = Vector3.zero;
        }
        else if(distance < minDistance)
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            transform.localScale = new Vector3(scale * (maxDistance - distance) / (maxDistance - minDistance), scale * (maxDistance - distance) / (maxDistance - minDistance), scale * (maxDistance - distance) / (maxDistance - minDistance));
        }
    }
}
