using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundedTowerScript : MonoBehaviour
{
    GameObject player;
    GameObject pig;
    [SerializeField] int spawningFrequency;
    [SerializeField] GameObject pigPrefab;
    private bool isProducing;
    public bool isActive;

    [SerializeField] int towerLife;
    // Use this for initialization
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            player = GameObject.FindGameObjectWithTag("Player");

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
                towerLife--;
            }
        }
    }
}
