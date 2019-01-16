using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PigScript : MonoBehaviour
{
    NavMeshAgent pigAgent;
    GameObject player;
    public PlayerScript playerScript;
    [SerializeField] GameObject pigBlood;

    [SerializeField] float distanceCharge;
    float maxHitPoints;
    public float hitPoints;
    public int damage;
    int soulReward;
    int focusReward;
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
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        // maxHitPoints = (gameObject.tag == "PigOneTag" ? 60 : 150);
        maxHitPoints = 60; // << remove this when placed different tags for pigs >>

        hitPoints = maxHitPoints;

        // damage = (gameObject.tag == "PigOneTag" ? Random.Range(10, 15) : Random.Range(15, 25));
        damage = Random.Range(10, 15); // << remove >>

        // soulReward = (gameObject.tag == "PigOneTag" ? Random.Range(10, 15) + playerScript.level : Random.Range(20, 30) + playerScript.level);
        soulReward = Random.Range(10, 15) + playerScript.level; // << remove >> 

        // focusReward = (gameObject.tag == "PigOneTag" ? Random.Range(2, 4) : Random.Range(3, 6);
        focusReward = Random.Range(2, 4); // << remove >> 

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
            if (hitPoints <= 0)
            {
                Destroy(gameObject);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().souls += soulReward;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().focus += focusReward;
            }
            // GameObject blood = GameObject.Instantiate(pigBlood, transform.position + new Vector3(0, 5, 0), Quaternion.identity);
        }
    }
}
