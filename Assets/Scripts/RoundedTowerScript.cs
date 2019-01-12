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
    bool isFirstTriggerDone;

    // Use this for initialization
    void Start()
    {
        isActive = false;
        maxHitPoints = 300;
        firstTriggerCount = 0;
        isFirstTriggerDone = false;
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
                    Physics.IgnoreCollision(pig.GetComponentInChildren<Collider>(), GetComponent<Collider>());
                    firstTriggerCount++;
                    Debug.Log("Pig Created");
                }
                else
                {
                    isFirstTriggerDone = true;
                }

                if (!isProducing && isFirstTriggerDone)
                {
                    pig = Instantiate(pigPrefab, transform.position, Quaternion.identity);
                    Physics.IgnoreCollision(pig.GetComponentInChildren<Collider>(), GetComponent<Collider>());
                    Debug.Log("Pig Created");
                    isProducing = true;
                    Invoke("Deactivate", spawningFrequency);
                }
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
                    Destroy(transform.parent.gameObject);
                }
            }
            // if (other.gameObject.tag == "PigTag")
            // {
            //     Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
            // }
        }
    }
}
