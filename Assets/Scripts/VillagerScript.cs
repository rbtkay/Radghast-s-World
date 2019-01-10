using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerScript : MonoBehaviour
{
    float sensorLength = 0.50f;
    float speed = 2.0f;
    float directionSign = 1.0f;
    float turnValue = 0.0f;
    float turnSpeed = 90.0f;
    Animator villagerAnimator;
    Collider collider;

    // Use this for initialization
    void Start()
    {
        collider = transform.GetComponent<Collider>();
        villagerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        villagerAnimator.SetBool("Walk", true);
        RaycastHit hit;
        int flag = 0;
        if (Physics.Raycast(transform.position, transform.right, out hit, sensorLength + transform.localScale.x))
        {
            turnValue -= 1;
            flag++;
        }

        if (Physics.Raycast(transform.position, -transform.right, out hit, sensorLength + transform.localScale.x))
        {
            turnValue += 1;
            flag++;
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, sensorLength + transform.localScale.z))
        {
            if (directionSign == 1.0f)
            {
                directionSign = -1.0f;
            }
            else
            {
                directionSign = 1.0f;
            }
            flag++;
        }

        if (Physics.Raycast(transform.position, -transform.forward, out hit, sensorLength + transform.localScale.z))
        {
            if (directionSign == -1.0f)
            {
                directionSign = 1.0f;
            }
            flag++;
        }

        if (flag == 0)
        {
            turnValue = 0;
        }
        transform.Rotate(Vector3.up, turnSpeed * turnValue * Time.deltaTime);
        transform.position += transform.forward * speed * directionSign * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        // return;
        Gizmos.DrawRay(transform.position, transform.forward * (sensorLength + transform.localScale.z));
        Gizmos.DrawRay(transform.position, -transform.forward * (sensorLength + transform.localScale.z));
        Gizmos.DrawRay(transform.position, transform.right * (sensorLength + transform.localScale.x));
        Gizmos.DrawRay(transform.position, -transform.right * (sensorLength + transform.localScale.x));
    }
}
