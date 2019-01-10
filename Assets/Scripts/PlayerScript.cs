using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    /* [SerializeField] */ GameObject game;

    [SerializeField] float playerSpeed;
    [SerializeField] float playerRotation;
    [SerializeField] int health;

    public GameObject basicAttackPrefab;
    bool castingOwl;
    [SerializeField] GameObject owlSentinelPrefab;
    float owlLife;
    bool testingBool;

    public Vector3 spawnPoint;
    bool isFiring;

    Animator mageAnimator;

    float fireTime;

    void Start()
    {
        spawnPoint = transform.position;
        fireTime = Time.timeSinceLevelLoad;
        isFiring = false;
        mageAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        game = GameObject.FindGameObjectWithTag("GameTag");

        if (Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
            if (!GameObject.FindGameObjectWithTag("OwlTag"))
            {
                Debug.Log("Clicked");
                GameObject owlSentinel = GameObject.Instantiate(owlSentinelPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Destroy(GameObject.FindGameObjectWithTag("OwlTag"));
            }
        }


        if (Time.timeSinceLevelLoad - fireTime > 1.2f && isFiring)
        {
            isFiring = false;
        }

        if (Input.GetAxis("Fire1") == -1)
        {
            if (!isFiring)
            {
                BasicAttack();
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button0) && !testingBool)
        {
            testingBool = true;
        }

        else if (Input.GetKeyDown(KeyCode.Joystick1Button0) && testingBool)
        {
            testingBool = false;
        }


        if (isFiring)
            playerSpeed = 0f;
        else
        {
            if (!testingBool)
                playerSpeed = 0.45f;

            else if (testingBool)
                playerSpeed = 1.5f;
        }

        if (!game.GetComponent<Game>().isPaused)
        {
            // Debug.Log(game.GetComponent<Game>().IsGamePaused());
            PlayerMove();
        }


        Input.ResetInputAxes();
    }
    void PlayerMove()
    {
        float movementX = Input.GetAxis("Horizontal");
        float movementZ = Input.GetAxis("Vertical");

        float rotationX = Input.GetAxis("HorizontalRight");
        float rotationZ = -Input.GetAxis("VerticalRight");

        Vector3 toLookAt = transform.position + new Vector3(rotationX, 0, rotationZ);

        if (rotationX > 0.8f || rotationX < -0.8f)
        {
            transform.LookAt(toLookAt);
        }
        else if (rotationZ > 0.8f || rotationZ < -0.8f)
        {
            transform.LookAt(toLookAt);
        }

        if (movementX > 0.65f || movementX < -0.65f)
        {
            transform.position += new Vector3(movementX * playerSpeed, 0, movementZ * playerSpeed);
            mageAnimator.SetBool("Moving", true);
        }

        else if (movementZ > 0.65f || movementZ < -0.65f)
        {
            transform.position += new Vector3(movementX * playerSpeed, 0, movementZ * playerSpeed);
            mageAnimator.SetBool("Moving", true);
        }

        else
        {
            mageAnimator.SetBool("Moving", false);
        }

        // transform.position += new Vector3(movementX * playerSpeed, 0, movementZ * playerSpeed);


        /////////////////////////////////////////////////////////////////
        /// //float rotation = Input.GetAxis("HorizontalRight");
        // if (rotation > 0.5f)
        // {
        //     rotation = 1.0f;
        // }
        // else if (rotation < -0.5f)
        // {
        //     rotation = -1.0f;
        // }
        // else
        // {
        //     rotation = 0.0f;
        // }

        // transform.Rotate(0, rotation * playerRotation, 0, Space.World);
        /////////////////////////////////////////////////////////////////////
    }

    void BasicAttack()
    {
        fireTime = Time.timeSinceLevelLoad;
        mageAnimator.SetTrigger("Attack1Trigger");
        isFiring = true;
        Invoke("CastSpell", 0.9f);
        // CastSpell();
    }

    private void CastSpell()
    {
        Vector3 ballPosition = transform.position + transform.forward * 5f;
        GameObject basicAttack = GameObject.Instantiate(basicAttackPrefab, ballPosition + new Vector3(0, 5f, 0), transform.rotation);
    }

    private void OnCollisionEnter(Collision other)
    {
        // other.gameObject.GetComponent<PigScript>().pigState
        if (other.gameObject.tag == "PigTag" && other.gameObject.GetComponent<PigScript>().pigState == PigScript.State.charging)
        {
            if (health > 0)
            {
                Debug.Log(health--);
            }
            else
            {
                Destroy(gameObject);
            }
            other.gameObject.GetComponent<PigScript>().pigState = PigScript.State.ready;
        }
    }

    void FootR() { }

    void FootL() { }

    void NewEvent() { }

    void Hit() { }
}
