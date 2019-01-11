using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    GameObject player;
    public Vector3 offset;
    float distance;
    Color mainColor;

    // Use this for initialization
    void Start()
    {
        // transform.position = player.transform.position + offset;
        // transform.LookAt(player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        distance = Vector3.Distance(transform.position, player.transform.position);
        RaycastHit hit;
        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform);
        Vector3 dir = player.transform.position - transform.position;
        int layerMask = 1 << 9;
        layerMask = ~layerMask;

        if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, dir, Color.red);
            if (hit.collider.tag != "Player" && hit.collider.tag != "MainWorldTag" && hit.collider.tag != "PigTag")
            {
                hit.collider.GetComponent<MeshRenderer>().enabled = false;
                hit.collider.tag = "CameraHitTag";
            }
            else if(GameObject.FindGameObjectWithTag("CameraHitTag") != null)
            {
                GameObject.FindGameObjectWithTag("CameraHitTag").GetComponent<MeshRenderer>().enabled = true;
                GameObject.FindGameObjectWithTag("CameraHitTag").tag = "Untagged";
            }
            // Color temp = mainColor = hit.transform.GetComponent<Material>().color;
            // temp.a = 0.5f;
            // hit.transform.GetComponent<Material>().color = temp;
            // GameObject.FindGameObjectWithTag("CameraHitTag").GetComponent<Material>().color = mainColor;
        }
    }
}
