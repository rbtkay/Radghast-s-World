using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBulletScript : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    Vector3 destination;
    // Use this for initialization
    void Start()
    {
        destination = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        // if (other.gameObject.tag == "QuestThreeTag")
        // {

        // }
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "MainWorldTag")
        {
            Destroy(gameObject);
        }
    }
}
