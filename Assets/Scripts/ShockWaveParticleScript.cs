using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveParticleScript : MonoBehaviour
{
    float birth;
    float life;
    [SerializeField] float lifeExpectancy;
    // Use this for initialization
    void Start()
    {
        birth = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        life = Time.timeSinceLevelLoad - birth;

        if (life > lifeExpectancy)
        {
            Destroy(gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerScript>().hitPoints -= Random.Range(1, 2);
        }

    }
}
