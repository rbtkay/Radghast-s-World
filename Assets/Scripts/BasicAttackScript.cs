using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackScript : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
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
        if (other.gameObject.tag == "PigTag")
        {
            other.gameObject.GetComponent<PigScript>().hitPoints -= damage;
        }

        else if (other.gameObject.tag == "QuestOneTag" || other.gameObject.tag == "QuestTwoTag")
        {
            other.gameObject.GetComponent<RoundedTowerScript>().hitPoints -= damage;
        }

        else if (other.gameObject.tag == "QuestThreeTag")
        {
            other.gameObject.GetComponent<DarkTowerScript>().hitPoints -= damage;
        }

        else if (other.gameObject.tag == "BullTag")
        {
            other.gameObject.GetComponent<BullScript>().hitPoints -= damage;
        }
        Destroy(gameObject);
    }
}
