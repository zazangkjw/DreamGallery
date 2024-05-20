using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CircusLight : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(Vector3.Lerp(rb.position, new Vector3(player.transform.position.x, rb.position.y, player.transform.position.z), 0.05f));
    }
}
