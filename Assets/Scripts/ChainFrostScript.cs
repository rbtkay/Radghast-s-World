using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainFrostScript : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
    public int damage, hitCount;
    Rigidbody rbProjectile;
    [SerializeField] float projectileLifeTime;
    float projectileBirth;
    PlayerScript playerScript;
    // Start is called before the first frame update
    void Start()
    {
        hitCount = 5;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        rbProjectile = GetComponent<Rigidbody>();
        rbProjectile.velocity = transform.forward * projectileSpeed;
        projectileBirth = Time.timeSinceLevelLoad;
        damage = (int) (Random.Range((float)(playerScript.damage * 2.5) - 5, (float)(playerScript.damage * 2.5) + 5));
    }

    // Update is called once per frame
    void Update()
    {
        float projectileLife = Time.timeSinceLevelLoad - projectileBirth;
        if (projectileLife > projectileLifeTime || hitCount == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "PigTag")
        {
            hitCount--;
            other.gameObject.GetComponent<PigScript>().hitPoints -= damage;
        }

        else if (other.gameObject.tag == "QuestOneTag" || other.gameObject.tag == "QuestTwoTag")
        {
            hitCount--;
            other.gameObject.GetComponent<RoundedTowerScript>().hitPoints -= damage;
        }

        else if (other.gameObject.tag == "QuestThreeTag")
        {
            hitCount--;
            other.gameObject.GetComponent<DarkTowerScript>().hitPoints -= damage;
        }

        else if (other.gameObject.tag == "BullTag")
        {
            hitCount--;
            other.gameObject.GetComponent<BullScript>().hitPoints -= damage;
        }
    }
}
