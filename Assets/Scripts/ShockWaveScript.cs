using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveScript : MonoBehaviour
{

    // Use this for initialization
    Rigidbody rbProjectile;
    float projectileBirth;
    float projectileLifeTime = 5;
    [SerializeField] float projectileSpeed;
    public GameObject shockWaveParticles;

    Vector3 spawnPosition;
    void Start()
    {
        rbProjectile = GetComponent<Rigidbody>();
        rbProjectile.velocity = transform.forward * projectileSpeed;
        projectileBirth = Time.timeSinceLevelLoad;
        spawnPosition = transform.position;
        Debug.Log("connected");

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, spawnPosition) > 1)
        {
            GameObject particles = GameObject.Instantiate(shockWaveParticles, transform.position, transform.rotation);
            spawnPosition = transform.position;
            Debug.Log("particle created");
        }
        float projectileLife = Time.timeSinceLevelLoad - projectileBirth;
        if (projectileLife > projectileLifeTime)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}