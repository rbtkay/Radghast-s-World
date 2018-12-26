using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float playerSpeed;
    [SerializeField] float playerRotation;

    public GameObject basicAttackPrefab;

    bool isFiring;

    Animator mageAnimator;

    float fireTime;
    // Animation animationRun;

    // last
    // Use this for initialization
    void Start()
    {
        fireTime = Time.timeSinceLevelLoad;
        isFiring = false;
        mageAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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

        if (isFiring)
            playerSpeed = 0f;
        else
            playerSpeed = 0.22f;


        PlayerMove();

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

    void ContinueMoving()
    {
        playerSpeed = 0.22f;
    }

    void BasicAttack()
    {
        fireTime = Time.timeSinceLevelLoad;
        mageAnimator.SetTrigger("Attack1Trigger");
        Debug.Log("Firing");
        isFiring = true;
        Invoke("CastSpell", 0.9f);
        // CastSpell();
    }

    private void CastSpell()
    {
        Vector3 ballPosition = transform.position + transform.forward * 5f;
        GameObject basicAttack = GameObject.Instantiate(basicAttackPrefab, ballPosition + new Vector3(0, 5f, 0), transform.rotation);
    }

    void FootR()
    {
        // print("do nothing");
    }

    void FootL()
    {
        // print("do nothing");
    }

    void NewEvent()
    {

    }

    void Hit()
    {

    }
}
