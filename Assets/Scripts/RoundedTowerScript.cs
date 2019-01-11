using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundedTowerScript : MonoBehaviour
{
    GameObject player;
    GameObject pig;
    [SerializeField] float hitPoints;
    [SerializeField] float maxHitPoints;
    [SerializeField] float triggerDistance;
    [SerializeField] int spawningFrequency;
    [SerializeField] GameObject pigPrefab;
    private bool isProducing;
    public bool isActive;
    int firstTriggerCount;
    bool firstTrigger;

    // Use this for initialization
    void Start()
    {
        isActive = false;
        maxHitPoints = 300;
        firstTriggerCount = 0;
        firstTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            player = GameObject.FindGameObjectWithTag("Player");

            if (Vector3.Distance(player.transform.position, transform.position) < triggerDistance)
            {
                if (firstTriggerCount < 4)
                {
                    pig = Instantiate(pigPrefab, transform.position, Quaternion.identity);
                    firstTriggerCount++;
                }
            }
            if (!isProducing)
            {
                pig = Instantiate(pigPrefab, transform.position, Quaternion.identity);
                isProducing = true;
                Invoke("Deactivate", spawningFrequency);
            }
        }
    }

    void Deactivate()
    {
        isProducing = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isActive)
        {
            if (other.gameObject.tag == "BasicAttack")
            {
                hitPoints -= other.gameObject.GetComponent<BasicAttackScript>().damage;
                if (hitPoints <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
