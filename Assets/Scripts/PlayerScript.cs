﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public class PlayerScript : MonoBehaviour
{
    /* [SerializeField] */
    GameObject game;
    ScriptManager sm;
    [SerializeField] float playerSpeed;
    [SerializeField] float playerRotation;
    double maxHitPoints, hitPoints, hitPointsRegen;
    double maxManaPoints, manaPoints, manaPointsRegen;
    double maxFocus, focus;
    public int level;
    public int damage;
    public double souls;
    public GameObject healthBar, healthText, manaBar, manaText, soulsText;
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
        level = 1;
        souls = 0;

        maxHitPoints = 95 + level * 5;
        hitPoints = maxHitPoints;
        hitPointsRegen = 0.15 + level * 0.05;

        maxManaPoints = 95 + level * 5;
        manaPoints = maxManaPoints;
        manaPointsRegen = 0.05 + level * 0.15;

        damage = 8 + level * 2;

        sm = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<ScriptManager>();
        maxFocus = 70 + sm.currentQuest * 30;
        focus = maxFocus;


        spawnPoint = transform.position;
        fireTime = Time.timeSinceLevelLoad;
        isFiring = false;
    }

    // Update is called once per frame
    void Update()
    {
        mageAnimator = GetComponent<Animator>();
        game = GameObject.FindGameObjectWithTag("GameTag");
        healthBar = GameObject.FindGameObjectWithTag("HealthBarTag");
        healthText = GameObject.FindGameObjectWithTag("HealthTextTag");
        manaBar = GameObject.FindGameObjectWithTag("ManaBarTag");
        manaText = GameObject.FindGameObjectWithTag("ManaTextTag");
        soulsText = GameObject.FindGameObjectWithTag("SoulsTextTag");

        healthText.GetComponent<Text>().text = ((int)(hitPoints)).ToString() + " / " + ((int)(maxHitPoints)).ToString();
        healthBar.GetComponent<Image>().fillAmount = (float)(hitPoints / maxHitPoints);

        manaText.GetComponent<Text>().text = ((int)(manaPoints)).ToString() + " / " + ((int)(maxManaPoints)).ToString();
        manaBar.GetComponent<Image>().fillAmount = (float)(manaPoints / maxManaPoints);

        soulsText.GetComponent<Text>().text = "Souls: " + souls.ToString();

        if (hitPoints < 0)
        {
            Destroy(gameObject);
        }

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
        Debug.Log("focus: " + focus);
        // CastSpell();
    }

    private void CastSpell()
    {
        Vector3 ballPosition = transform.position + transform.forward * 5f;
        GameObject basicAttack = GameObject.Instantiate(basicAttackPrefab, ballPosition + new Vector3(0, 5f, 0), transform.rotation);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "PigTag" && other.gameObject.GetComponent<PigScript>().pigState == PigScript.State.charging)
        {
            if (hitPoints > 0)
            {
                hitPoints -= other.gameObject.GetComponent<PigScript>().damage;
            }
            
            other.gameObject.GetComponent<PigScript>().pigState = PigScript.State.ready;
        }
    }

    void FootR() { }

    void FootL() { }

    void NewEvent() { }

    void Hit() { }
}
