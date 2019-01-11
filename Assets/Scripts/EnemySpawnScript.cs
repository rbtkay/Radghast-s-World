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

    public int countPig;


    void Start()
    {
        countPig = 0;
    }


    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(player.transform.position, transform.position) < spawningDistance)
        {
            if (!isProducing)
            {
                pig = Instantiate(pigPrefab, transform.position, Quaternion.identity);
                countPig++;
                isProducing = true;
                if (countPig < 5)
                {
                    Invoke("Deactivate", 5);
                }
            }
        }
    }

    void Deactivate()
    {
        isProducing = false;
    }
}

