using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    [SerializeField] float spawningDistance;
    GameObject player;
    public GameObject pigPrefab;
    GameObject pig;
    private bool isProducing;
    public bool isActive;

    public int countPig;


    void Start()
    {
        countPig = 0;
        isActive = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            pig = GameObject.Instantiate(pigPrefab, transform.position, Quaternion.identity);
            countPig++;
            if (countPig > 4)
            {
                isActive = false;
            }
        }
        // player = GameObject.FindGameObjectWithTag("Player");
        // if (Vector3.Distance(player.transform.position, transform.position) < spawningDistance)
        // {
        //     if (!isProducing)
        //     {
        //         pig = Instantiate(pigPrefab, transform.position, Quaternion.identity);
        //         countPig++;
        //         isProducing = true;
        //         if (countPig < 5)
        //         {
        //             Invoke("Deactivate", 5);
        //         }
        //     }
        // }
    }

    void Deactivate()
    {
        isProducing = false;
    }
}

