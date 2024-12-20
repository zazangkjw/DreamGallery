using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClownWorm : MonoBehaviour
{
    public GameObject deadTrigger;

    public List<Rigidbody> bodies = new List<Rigidbody> ();
    public List<Vector3> positionHistory = new List<Vector3> ();
    public List<Quaternion> rotationHistory = new List<Quaternion>();
    public int gap = 10;

    //public float moveSpeed = 5;
    //public float steerSpeed = 5;

    public GameObject target;
    public NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }

    private void FixedUpdate()
    {
        //Vector3 vel = -transform.right * moveSpeed;
        //myRigid.velocity = new Vector3(vel.x, myRigid.velocity.y, vel.z);
        //transform.localEulerAngles = transform.localEulerAngles + (new Vector3(0f, Input.GetAxis("Horizontal"), 0f) * steerSpeed);

        positionHistory.Insert(0, transform.position);
        rotationHistory.Insert(0, transform.rotation);
        if (positionHistory.Count > bodies.Count * gap + 1) { positionHistory.RemoveAt(positionHistory.Count - 1); }
        if (rotationHistory.Count > bodies.Count * gap + 1) { rotationHistory.RemoveAt(rotationHistory.Count - 1); }

        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].MovePosition(positionHistory[Mathf.Min((i + 1) * gap, positionHistory.Count - 1)]);
            bodies[i].MoveRotation(rotationHistory[Mathf.Min((i + 1) * gap, rotationHistory.Count - 1)]);
        }
    }
}
