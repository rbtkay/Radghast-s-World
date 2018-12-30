using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    [SerializeField] float distanceSave;
    [SerializeField] GameObject firePrefab;
    bool isOn;
    GameObject player;

    // Update is called once per frame
    void Start()
    {
        isOn = false;

    }
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(player.transform.position, transform.position) < distanceSave)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject fire = GameObject.Instantiate(firePrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                isOn = true;
            }
        }
    }
}
