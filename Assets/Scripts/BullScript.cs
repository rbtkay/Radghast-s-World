using System.Collections;
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
    bool isAttacking;

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
    // Use this for initialization
    void Start()
    {
        gameTime = Time.timeSinceLevelLoad;
        bullBody = GetComponent<Rigidbody>();
        bullAgent = GetComponent<NavMeshAgent>();
        bullAnimator = GetComponent<Animator>();
        isAttacking = false;
        timeBasicAttack = 0.0f;
        timeMultiBalls = 0.0f;
        timeSummonPig = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        transform.LookAt(player.transform.position);
        if (!isAttacking)
        {
            switch (phase)
            {
                case 1:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        //basic attack
                        //multi balls
                        ShockWave();
                        // SummonPigs();
                        // Debug.Log("Got KEY");
                        // Charging(player.transform.position);
                    }
                    break;
                case 2:
                    // bullAnimator.SetBool("dizzy", true);
                    multiBalls();
                    break;
                case 3:
                    bullAnimator.SetBool("walk", true);
                    break;
                default:
                    bullAnimator.SetBool("idle", true);
                    break;
            }
        }
        else
        {
            isAttacking = !CheckoutAnimationTime();
        }

        // // bullAgent.SetDestination(player.transform.position);
        // transform.LookAt(player.transform.position);
        // if (Vector3.Distance(player.transform.position, transform.position) < 40 && !isCharging)
        // {
        //     // Debug.Log("in distance");

        //     Charging(player.transform.position);
        //     // isCharging = true;
        // }
    }

    void FollowMage(Vector3 playerPosition)
    {
        bullAnimator.SetBool("Walk", true);
        bullAgent.speed = 3.5f;
        bullAgent.SetDestination(playerPosition);
    }

    void Charging(Vector3 playerPosition)
    {
        // bullAgent.isStopped = true;
        // transform.position = Vector3.MoveTowards(transform.position, playerPosition, bullSpeed * Time.deltaTime);
        // bullAnimator.SetBool("Run", true);
        transform.LookAt(playerPosition);
        bullBody.velocity = transform.forward * 50;
        bullAnimator.SetBool("run", true);
        timeCharge = Time.timeSinceLevelLoad;
        Debug.Log("Charging");
        // pigState = State.charging;
        isAttacking = true;
    }
    void BasicAttack()
    {
        timeBasicAttack = Time.timeSinceLevelLoad;
        bullAnimator.SetBool("attack_01", true);
        isAttacking = true;
        Invoke("CastBasicAttack", 0.9f);
        // CastSpell();
    }


    void multiBalls()
    {
        timeMultiBalls = Time.timeSinceLevelLoad;
        bullAnimator.SetBool("attack_02", true);
        isAttacking = true;
        Invoke("CastMultiBalls", 0.9f);
    }

    void ShockWave()
    {
        GameObject fire = GameObject.Instantiate(shockWave,
        new Vector3(transform.position.x + transform.forward.x, transform.position.y + transform.forward.y, +transform.position.z + transform.forward.z)
        , transform.rotation);
    }

    void ClearAllBool()
    {
        bullAnimator.SetBool("defy", false);
        Debug.Log("Idling");
        Debug.Log(bullBody.velocity);
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
        if ((timeBasicAttack != 0.0f) && (Time.timeSinceLevelLoad - timeBasicAttack > 1.0f))
        {
            Debug.Log("inBasicAttack");
            timeBasicAttack = 0.0f;
            ClearAllBool();
            // bullBody.velocity = new Vector3(0,0,0);
            return true;
        }
        if ((timeMultiBalls != 0.0f) && (Time.timeSinceLevelLoad - timeCharge > 2.0f))
        {
            Debug.Log("inCharge");
            timeMultiBalls = 0.0f;
            // bullBody.velocity = new Vector3(0, 0, 0);
            ClearAllBool();

            return true;
        }
        if ((timeSummonPig != 0.0f) && (Time.timeSinceLevelLoad - timeSummonPig > 1.0f))
        {
            Debug.Log("inSummon");
            timeSummonPig = 0.0f;
            // bullBody.velocity = new Vector3(0, 0, 0);
            ClearAllBool();

            return true;
        }
        return false;
    }

    void SummonPigs()
    {
        timeSummonPig = Time.timeSinceLevelLoad;
        bullAnimator.SetBool("jump", true);
        isAttacking = true;
        Invoke("CastSummonPig", 0.9f);
    }

    void CastSummonPig()
    {
        Debug.Log("Good to Go!!");
    }

    void CastBasicAttack()
    {
        Vector3 ballPosition = transform.position + transform.forward * 5f;
        GameObject basicAttack = GameObject.Instantiate(bulletPrefab, ballPosition + new Vector3(0, 5f, 0), transform.rotation);
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
    }

}
