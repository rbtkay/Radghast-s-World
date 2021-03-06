﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// class Phase
// {
//     int phase;

//     Phase(int phaseCount)
//     {
//         phase = phaseCount;

//         Charging();
//     }

//     void Charging()
//     {
//         //some code
//         bullAnimator.SetBool("run", true);
//     }
// }
public class BullScript : MonoBehaviour
{
    [SerializeField] int phase;
    [SerializeField] float bullSpeed;
    float gameTime;
    NavMeshAgent bullAgent;
    Animator bullAnimator;
    Rigidbody bullBody;
    bool isCharging;
    int countPhase;

    int bullLife;

    //Spells
    bool isBusy;

    //Basic Attack
    public GameObject bulletPrefab;
    float timeBasicAttack;

    //multiBalls
    float timeMultiBalls;
    float timeCharge;

    //shcckWave
    public GameObject shockWave;

    //Summon Pigs
    float timeSummonPig;
    GameObject summonPigPositionOne;
    GameObject summonPigPositionTwo;

    string[] actions = { "BasicAttack", "multiBalls", "ShockWave", "SummonPigs" };

    //Moving
    public GameObject[] movementPositions;
    bool isMoving;
    public GameObject pigPrefab;
    Vector3 destination;
    public double maxHitPoints, hitPoints;
    public bool isActive;

    public GameObject wallPrefab;
    GameObject wallPosition;

    float timeShockWave;

    // Use this for initialization
    void Start()
    {
        gameTime = Time.timeSinceLevelLoad;
        bullBody = GetComponent<Rigidbody>();
        bullAgent = GetComponent<NavMeshAgent>();
        bullAnimator = GetComponent<Animator>();
        isBusy = false;
        timeBasicAttack = 0.0f;
        timeMultiBalls = 0.0f;
        timeSummonPig = 0.0f;
        maxHitPoints = 1500;
        phase = 1;
        hitPoints = maxHitPoints;

    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (hitPoints < 0)
        {
            Destroy(gameObject);
        }
        wallPosition = GameObject.FindGameObjectWithTag("WallPositionTag");


        if (isActive)
        {

            if (!isBusy/* && !isMoving */)
            {
                transform.LookAt(player.transform.position);

                float basicAttackPercentage = Random.Range(0.0f, 100.0f);
                float multiPercentage = Random.Range(0.0f, 75.0f);
                float shockWavePercentage = Random.Range(0.0f, 50.0f);
                float spawnPigPercentage = Random.Range(0.0f, 25.0f);


                // phase = 1;

                switch (phase)
                {
                    case 1:
                        transform.LookAt(player.transform.position);
                        if (basicAttackPercentage > multiPercentage && basicAttackPercentage > shockWavePercentage && basicAttackPercentage > spawnPigPercentage)
                        {
                            BasicAttack();
                        }
                        else if (multiPercentage > basicAttackPercentage && multiPercentage > shockWavePercentage && multiPercentage > spawnPigPercentage)
                        {
                            multiBalls();
                        }
                        else if (shockWavePercentage > basicAttackPercentage && shockWavePercentage > multiPercentage && shockWavePercentage > spawnPigPercentage)
                        {
                            ShockWave();
                        }
                        else if (spawnPigPercentage > basicAttackPercentage && spawnPigPercentage > multiPercentage && spawnPigPercentage > shockWavePercentage)
                        {
                            SummonPigs();
                        }
                        // phase = 2;
                        Debug.Log("in case 1");
                        break;
                    case 2:
                        BullMovement();
                        // phase = 1;
                        Debug.Log("in case 2");
                        break;
                    default:
                        bullAnimator.SetBool("idle", true);
                        break;
                }
            }
            else if (phase == 2)
            {
                isBusy = !CheckoutAnimationTime();
            }

            // if (isMoving)
            // {
            //     if (Vector3.Distance(transform.position, destination) < 1)
            //     {

            //     }

            // }
            // // bullAgent.SetDestination(player.transform.position);
            // transform.LookAt(player.transform.position);
            // if (Vector3.Distance(player.transform.position, transform.position) < 40 && !isCharging)
            // {
            //     // Debug.Log("in distance");

            //     Charging(player.transform.position);
            //     // isCharging = true;
            // }
        }
        else if (Vector3.Distance(player.transform.position, transform.position) < 20f)
        {
            isActive = true;
            GameObject wall = GameObject.Instantiate(wallPrefab, wallPosition.transform.position, wallPosition.transform.rotation);
        }
    }

    void FollowMage(Vector3 playerPosition)
    {
        bullAnimator.SetBool("Walk", true);
        bullAgent.speed = 6f;
        bullAgent.SetDestination(playerPosition);
    }

    void BullMovement()
    {
        bullAnimator.SetBool("walk", true);
        bullAgent.speed = 10f;

        do
        {
            int movementPosition = Random.Range(0, 3);
            destination = movementPositions[movementPosition].transform.position;
        }
        while (Vector3.Distance(transform.position, destination) < 0.5);

        transform.LookAt(destination);
        bullAgent.SetDestination(destination);
        isBusy = true;
    }

    // void Charging(Vector3 playerPosition)
    // {
    //     // bullAgent.isStopped = true;
    //     // transform.position = Vector3.MoveTowards(transform.position, playerPosition, bullSpeed * Time.deltaTime);
    //     // bullAnimator.SetBool("Run", true);
    //     transform.LookAt(playerPosition);
    //     bullBody.velocity = transform.forward * 50;
    //     bullAnimator.SetBool("run", true);
    //     timeCharge = Time.timeSinceLevelLoad;
    //     Debug.Log("Charging");
    //     // pigState = State.charging;
    //     isBusy = true;
    // }
    void BasicAttack()
    {
        timeBasicAttack = Time.timeSinceLevelLoad;
        bullAnimator.SetBool("attack_01", true);
        isBusy = true;
        Invoke("CastBasicAttack", 0.9f);
        // CastSpell();
    }


    void multiBalls()
    {
        timeMultiBalls = Time.timeSinceLevelLoad;
        bullAnimator.SetBool("attack_02", true);
        isBusy = true;
        Invoke("CastMultiBalls", 0.9f);
    }

    void ShockWave()
    {
        GameObject fire = GameObject.Instantiate(shockWave,
        new Vector3(transform.position.x + transform.forward.x, transform.position.y + transform.forward.y, +transform.position.z + transform.forward.z)
        , transform.rotation);
        timeShockWave = Time.timeSinceLevelLoad;
        bullAnimator.SetBool("attack_03", true);
        isBusy = true;
        Invoke("RefreshBull", 1.0f);
    }

    void ClearAllBool()
    {
        bullAnimator.SetBool("defy", false);
        bullAnimator.SetBool("idle", true);
        bullAnimator.SetBool("dizzy", false);
        bullAnimator.SetBool("walk", false);
        bullAnimator.SetBool("run", false);
        bullAnimator.SetBool("jump", false);
        bullAnimator.SetBool("die", false);
        bullAnimator.SetBool("jump_left", false);
        bullAnimator.SetBool("jump_right", false);
        bullAnimator.SetBool("attack_01", false);
        bullAnimator.SetBool("attack_03", false);
        bullAnimator.SetBool("attack_02", false);
        bullAnimator.SetBool("damage", false);
    }

    bool CheckoutAnimationTime()
    {
        // if ((timeBasicAttack != 0.0f) && (Time.timeSinceLevelLoad - timeBasicAttack > 1.0f))
        // {
        //     timeBasicAttack = 0.0f;
        //     ClearAllBool();
        //     // bullBody.velocity = new Vector3(0,0,0);
        //     return true;
        // }
        // if ((timeMultiBalls != 0.0f) && (Time.timeSinceLevelLoad - timeMultiBalls > 1.0f))
        // {
        //     timeMultiBalls = 0.0f;
        //     // bullBody.velocity = new Vector3(0, 0, 0);
        //     ClearAllBool();

        //     return true;
        // }
        // if ((timeSummonPig != 0.0f) && (Time.timeSinceLevelLoad - timeSummonPig > 1.0f))
        // {
        //     timeSummonPig = 0.0f;
        //     // bullBody.velocity = new Vector3(0, 0, 0);
        //     ClearAllBool();

        //     return true;
        // }
        // if ((timeShockWave != 0.0f) && (Time.timeSinceLevelLoad - timeShockWave > 1.0f))
        // {
        //     timeShockWave = 0.0f;
        //     // bullBody.velocity = new Vector3(0, 0, 0);
        //     ClearAllBool();

        //     return true;
        // }
        if (bullAgent.remainingDistance < 0.5)
        {
            bullAgent.speed = 0.0f;
            ClearAllBool();
            phase = 1;
            return true;
        }
        return false;
    }

    void SummonPigs()
    {
        timeSummonPig = Time.timeSinceLevelLoad;
        bullAnimator.SetBool("jump", true);
        isBusy = true;
        Invoke("CastSummonPig", 0.9f);
    }

    void CastSummonPig()
    {
        Debug.Log("Good to Go!!");
        summonPigPositionOne = GameObject.FindGameObjectWithTag("BullTowerTag");
        summonPigPositionTwo = GameObject.FindGameObjectWithTag("BullTowerOneTag");
        GameObject pigOne = GameObject.Instantiate(pigPrefab, summonPigPositionOne.transform.position, Quaternion.identity);
        GameObject pigTwo = GameObject.Instantiate(pigPrefab, summonPigPositionTwo.transform.position, Quaternion.identity);
        Physics.IgnoreCollision(pigOne.GetComponentInChildren<Collider>(), GameObject.FindGameObjectWithTag("BullTowerTag").GetComponent<Collider>());
        Physics.IgnoreCollision(pigTwo.GetComponentInChildren<Collider>(), GameObject.FindGameObjectWithTag("BullTowerOneTag").GetComponent<Collider>());

        RefreshBull();
    }

    void CastBasicAttack()
    {
        Vector3 ballPosition = transform.position + transform.forward * 5f;
        GameObject basicAttack = GameObject.Instantiate(bulletPrefab, ballPosition + new Vector3(0, 5f, 0), transform.rotation);
        RefreshBull();
    }

    private void RefreshBull()
    {
        ClearAllBool();
        isBusy = false;
        phase = 2;
    }

    void CastMultiBalls()
    {
        Vector3 centerPosition = transform.position + transform.forward * 5f;
        GameObject centerBall = GameObject.Instantiate(bulletPrefab, centerPosition + new Vector3(0, 5f, 0), transform.rotation);

        Vector3 between = (transform.forward + (-transform.right)).normalized;
        Vector3 leftPosition = transform.position + between * 5f;
        GameObject leftBall = GameObject.Instantiate(bulletPrefab, leftPosition + new Vector3(0, 5f, 0), transform.rotation);

        between = (transform.forward + transform.right).normalized;
        Vector3 rightPosition = transform.position + between * 5f;
        GameObject rightBall = GameObject.Instantiate(bulletPrefab, rightPosition + new Vector3(0, 5f, 0), transform.rotation);
        Physics.IgnoreCollision(rightBall.GetComponent<Collider>(), leftBall.GetComponent<Collider>());
        Physics.IgnoreCollision(rightBall.GetComponent<Collider>(), centerBall.GetComponent<Collider>());
        Physics.IgnoreCollision(centerBall.GetComponent<Collider>(), leftBall.GetComponent<Collider>());
        Physics.IgnoreCollision(centerBall.GetComponent<Collider>(), bullBody.GetComponent<Collider>());
        Physics.IgnoreCollision(leftBall.GetComponent<Collider>(), bullBody.GetComponent<Collider>());
        Physics.IgnoreCollision(rightBall.GetComponent<Collider>(), bullBody.GetComponent<Collider>());

        RefreshBull();
    }

}
