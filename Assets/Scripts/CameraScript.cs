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
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform);
    }
}
