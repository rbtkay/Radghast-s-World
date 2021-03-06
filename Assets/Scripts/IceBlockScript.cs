﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlockScript : MonoBehaviour
{
    public float lifeTime, hitPoints, birthTime;
    // Start is called before the first frame update
    void Start()
    {
        hitPoints = 300f;
        birthTime = Time.timeSinceLevelLoad;
        lifeTime = 10f;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().manaPoints -= 30;
    }

    // Update is called once per frame
    void Update()
    {
        float life = Time.timeSinceLevelLoad - birthTime;
        if (GameObject.FindGameObjectWithTag("BasicAttack"))
        {
            foreach (Collider col in gameObject.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(col, GameObject.FindGameObjectWithTag("BasicAttack").GetComponent<Collider>());
            }
        }

        foreach (Collider col in gameObject.GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(col, GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Collider>());
        }

        if (life > lifeTime || hitPoints < 0)
        {
            Destroy(gameObject);
        }
    }
}
