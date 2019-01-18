using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveParticleScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerScript>().hitPoints -= Random.Range(1, 2);

        }

    }
}
