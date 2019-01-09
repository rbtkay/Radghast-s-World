using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    GameObject player;
    public Vector3 offset;
    // Use this for initialization
    void Start()
    {
        // transform.position = player.transform.position + offset;
        // transform.LookAt(player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform);
        Vector3 dir = player.transform.position - transform.position;

        while (Physics.Raycast(transform.position, dir, out hit, Vector3.Distance(transform.position, player.transform.position)))
        {
            Debug.DrawRay(transform.position, dir, Color.red);
            if (hit.collider.tag != "Player")
            {
                Color temp = hit.transform.GetComponent<Material>().color;
                temp.a = 0.5f;
                hit.transform.GetComponent<Material>().color = temp;

            }
            else
            {
                break;
                // temp.a = 1;
                // hit.transform.GetComponent<Material>().color = temp;
            }
        }
    }
}
