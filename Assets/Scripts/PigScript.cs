using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PigScript : MonoBehaviour
{
    NavMeshAgent pigAgent;
    GameObject player;
    [SerializeField] GameObject pigBlood;
    [SerializeField] float distanceCharge;
    [SerializeField] int health;
    Animator pigAnimator;
    Rigidbody pigBody;
    bool isCharging;
    public enum State { ready, walking, charging };
    public State pigState;
    float chargeTime;

    bool isMoving;
    // Update is called once per frame

    void Start()
    {
        pigAnimator = GetComponent<Animator>();
        pigAgent = GetComponent<NavMeshAgent>();
        pigBody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        chargeTime = 0f;
        
        // pigAnimator.SetBool("New State 0", true);
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad - chargeTime > 2)
        {
            pigState = State.ready;
            pigBody.velocity = new Vector3(0, 0, 0);
            transform.LookAt(player.transform.position);
            if (Vector3.Distance(player.transform.position, transform.position) > distanceCharge && !isCharging)
            {
                pigState = State.walking;
                FollowMage();
            }
            else if (!isCharging)
            {
                pigAgent.speed = 0.0f;
                pigAnimator.SetFloat("a", 3f);
                isCharging = true;
                transform.LookAt(player.transform.position);
                Invoke("Charging", 2);
            }
        }
        else
        {
            FollowMage();
        }
    }

    void FollowMage()
    {
        pigAnimator.SetFloat("a", 1.5f);
        pigAgent.speed = 3.5f;
        pigAgent.SetDestination(new Vector3(player.transform.position.x, 0.5f, player.transform.position.z));
    }

    void Charging()
    {
        pigBody.velocity = transform.forward * 50;
        pigAnimator.SetFloat("a", 1.5f);
        pigState = State.charging;
        isCharging = false;
        chargeTime = Time.timeSinceLevelLoad;
    }

    // public void ReduceHealth()
    // {
    //     Debug.Log("health reduced");
    //     health--;
    // }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            pigBody.velocity = new Vector3(0, 0, 0);
            transform.LookAt(player.transform.position);
            // isCharging = false;
        }
        else if (other.gameObject.tag == "BasicAttack")
        {
            health--;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
            // GameObject blood = GameObject.Instantiate(pigBlood, transform.position + new Vector3(0, 5, 0), Quaternion.identity);
        }
    }
}
