using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBulletScript : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    Vector3 destination;
    GameObject terrain;
    public ParticleSystem particleSystem;
    // Use this for initialization
    void Start()
    {

        destination = GameObject.FindGameObjectWithTag("Player").transform.position;
        terrain = GameObject.FindGameObjectWithTag("MainWorldTag");
        Physics.IgnoreCollision(GetComponent<Collider>(), terrain.GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, bulletSpeed * Time.deltaTime);
        if (transform.position.y <= 0.5f)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    void Explode()
    {
       /*  GameObject boom = */ GameObject.Instantiate(particleSystem, transform.position, Quaternion.identity);
        // boom.GetComponent<ParticleSystem>().Play();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
