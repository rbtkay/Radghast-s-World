using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackScript : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
    float damageValue;
    Rigidbody rbProjectile;
    float projectileBirth;
    [SerializeField] float projectileLifeTime;
    PlayerScript playerScript;
    public int damage;

    // Use this for initialization
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        rbProjectile = GetComponent<Rigidbody>();
        rbProjectile.velocity = transform.forward * projectileSpeed;
        projectileBirth = Time.timeSinceLevelLoad;
        damage = Random.Range(playerScript.damage - 2, playerScript.damage + 2);
    }

    // Update is called once per frame
    void Update()
    {
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
