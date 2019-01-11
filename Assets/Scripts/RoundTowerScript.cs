using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTowerScript : MonoBehaviour
{
    [SerializeField] float hitPoints;
    [SerializeField] float maxHitPoints;
    [SerializeField] float triggerDistance;
    int firstTriggerCount;
    bool rTowerSpawning;
    bool firstTrigger;
    GameObject pig;
	public GameObject pigPrefab;



    GameObject player;
    // Use this for initialization
    void Start()
    {
		maxHitPoints = 300;
        hitPoints = maxHitPoints;
        firstTriggerCount = 0;
        rTowerSpawning = false;
        firstTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(player.transform.position, transform.position) < triggerDistance)
        {
            if (firstTriggerCount < 4)
            {
				FirstSpawn();
            }
        }
    }

    void FirstSpawn()
    {
        pig = Instantiate(pigPrefab, transform.position, Quaternion.identity);
        firstTriggerCount++;        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "BasicAttack")
        {
            hitPoints -= player.GetComponent<PlayerScript>().damage;
            if (hitPoints <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}