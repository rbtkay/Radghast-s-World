using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkTowerScript : MonoBehaviour
{
    public bool isActive;
    GameObject player;
    [SerializeField] GameObject bulletPrefab;
    GameObject bullet;
    float currentTime;
    public float hitPoints, maxHitPoints;
    // Use this for initialization
    void Start()
    {
        maxHitPoints = 250;
        hitPoints = maxHitPoints;
        isActive = false;
        currentTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (isActive)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 40 && (Time.timeSinceLevelLoad - currentTime > 2))
            {
                Fire();
            }
        }
    }

    void Fire()
    {
        currentTime = Time.timeSinceLevelLoad;
        bullet = GameObject.Instantiate(bulletPrefab, transform.position + new Vector3(0, 25, 0), Quaternion.identity);
        Physics.IgnoreCollision(GetComponent<Collider>(), bullet.GetComponent<Collider>());
    }
}
