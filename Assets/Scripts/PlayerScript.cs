using System.Collections;
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
    public double maxHitPoints, hitPoints, hitPointsRegen;
    public double maxManaPoints, manaPoints, manaPointsRegen;
    public Sprite fullHPPot, halfHPPot, emptyHPPot, fullMPPot, halfMPPot, emptyMPPot;
    public int maxHPPots, HPPots, maxMPPots, MPPots;
    public GameObject healthPotion, manaPotion;
    public double maxFocus;
    public double focus;
    public int level;
    public int damage;
    public double souls;
    public GameObject healthBar, healthText, manaBar, manaText, soulsText, focusBar, levelText;
    public GameObject basicAttackPrefab, chainFrostPrefab;
    bool castingOwl;
    [SerializeField] GameObject owlSentinelPrefab;
    [SerializeField] GameObject owlGuidePrefab;

    [SerializeField] GameObject wallPlayerPrefab;
    float owlLife;
    bool testingBool;
    public Vector3 spawnPoint;
    bool isFiring;
    Animator mageAnimator;
    float fireTime;
    float regenTime;

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

        regenTime = Time.timeSinceLevelLoad;
        spawnPoint = transform.position;
        fireTime = Time.timeSinceLevelLoad;
        isFiring = false;
        mageAnimator = GetComponent<Animator>();

        maxHPPots = 1;
        HPPots = maxHPPots;
        maxMPPots = 1;
        MPPots = maxMPPots;
    }

    // Update is called once per frame
    void Update()
    {
        SetUp();

        PlayerRegen();

        if (hitPoints < 0)
        {
            game.GetComponent<Game>().LoadGame();
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
            if (!GameObject.FindGameObjectWithTag("OwlTag"))
            {
                GameObject owlSentinel = GameObject.Instantiate(owlSentinelPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Destroy(GameObject.FindGameObjectWithTag("OwlTag"));
            }
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button8) || Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("owl");
            if (!GameObject.FindGameObjectWithTag("OwlGuideTag"))
            {
                GameObject owlGuide = GameObject.Instantiate(owlGuidePrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Destroy(GameObject.FindGameObjectWithTag("OwlGuideTag"));
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            level += 1;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            souls += 10000;
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            UseHealthPotion();
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            UseManaPotion();
        }
        // if (Input.GetAxis("Potion") == 1)
        // {
        //     UseHealthPotion();
        // }

        // if (Input.GetAxis("Potion") == -1)
        // {
        //     UseManaPotion();
        // }
        if (!isFiring)
        {
            if (Input.GetAxis("Fire1") == -1)
            {
                BasicAttack();
            }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button2) && manaPoints > 20 && level >= 3)
            {
                ChainFrost();
            }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button1) && manaPoints > 30 && level >= 7)
            {
                Debug.Log("B");
                IceBlock();
            }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button0) && manaPoints > 8 && level >= 5)
            {
                Debug.Log("A");
                Blink();
            }
        }

        if (Input.GetKeyDown(KeyCode.P) && !testingBool)
        {
            testingBool = true;
        }

        else if (Input.GetKeyDown(KeyCode.P) && testingBool)
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

        if (Time.timeSinceLevelLoad - fireTime > 1.2f && isFiring)
        {
            isFiring = false;
        }
        Input.ResetInputAxes();
    }

    void Blink()
    {
        if (Physics.Raycast(transform.position, transform.forward, 20.0f))
        {
            return;
        }
        else
        {
            transform.position = transform.position + transform.forward * 20.0f;
        }
    }


    void IceBlock()
    {
        fireTime = Time.timeSinceLevelLoad;
        mageAnimator.SetTrigger("Attack1Trigger");
        isFiring = true;
        Invoke("CastIceBlock", 0.9f);
        // CastBasicAttack();
    }
    void CastIceBlock()
    {
        Vector3 blockPosition = transform.position + transform.forward * 10f;
        GameObject basicAttack = GameObject.Instantiate(wallPlayerPrefab, blockPosition, transform.rotation);
    }

    void PlayerRegen()
    {
        if (Time.timeSinceLevelLoad - regenTime > 1)
        {
            if (hitPoints < maxHitPoints)
            {
                hitPoints += hitPointsRegen;
            }
            if (manaPoints < maxManaPoints)
            {
                manaPoints += manaPointsRegen;
            }
            regenTime = Time.timeSinceLevelLoad;
        }
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
        Invoke("CastBasicAttack", 0.9f);
        // CastBasicAttack();
    }

    private void CastBasicAttack()
    {
        Vector3 ballPosition = transform.position + transform.forward * 5f;
        GameObject basicAttack = GameObject.Instantiate(basicAttackPrefab, ballPosition + new Vector3(0, 5f, 0), transform.rotation);
    }

    void ChainFrost()
    {
        fireTime = Time.timeSinceLevelLoad;
        mageAnimator.SetTrigger("Attack1Trigger");
        isFiring = true;
        Invoke("CastChainFrost", 0.9f);
        // CastBasicAttack();
    }
    void CastChainFrost()
    {
        Vector3 ballPosition = transform.position + transform.forward * 5f;
        GameObject basicAttack = GameObject.Instantiate(chainFrostPrefab, ballPosition + new Vector3(0, 5f, 0), transform.rotation);
    }

    // private void 

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
        if (other.gameObject.tag == "ShockWave")
        {
            Debug.Log("burning");
        }
    }

    void SetUp()
    {
        game = GameObject.FindGameObjectWithTag("GameTag");
        healthBar = GameObject.FindGameObjectWithTag("HealthBarTag");
        healthText = GameObject.FindGameObjectWithTag("HealthTextTag");
        manaBar = GameObject.FindGameObjectWithTag("ManaBarTag");
        manaText = GameObject.FindGameObjectWithTag("ManaTextTag");
        soulsText = GameObject.FindGameObjectWithTag("SoulsTextTag");
        focusBar = GameObject.FindGameObjectWithTag("FocusBarTag");
        levelText = GameObject.FindGameObjectWithTag("LevelTextTag");
        healthPotion = GameObject.FindGameObjectWithTag("ImgHealthPot");
        manaPotion = GameObject.FindGameObjectWithTag("ImgManaPot");

        healthText.GetComponent<Text>().text = ((int)(hitPoints)).ToString() + " / " + ((int)(maxHitPoints)).ToString();
        healthBar.GetComponent<Image>().fillAmount = (float)(hitPoints / maxHitPoints);

        manaText.GetComponent<Text>().text = ((int)(manaPoints)).ToString() + " / " + ((int)(maxManaPoints)).ToString();
        manaBar.GetComponent<Image>().fillAmount = (float)(manaPoints / maxManaPoints);

        soulsText.GetComponent<Text>().text = "Souls: " + souls.ToString();

        focusBar.GetComponent<Image>().fillAmount = (float)(focus / maxFocus);

        levelText.GetComponent<Text>().text = "Level: " + level.ToString();

        maxFocus = 70 + sm.currentQuest * 30;
        if (HPPots == maxHPPots)
            healthPotion.GetComponent<Image>().sprite = fullHPPot;
        else if (HPPots < maxHPPots && HPPots != 0)
            healthPotion.GetComponent<Image>().sprite = halfHPPot;
        else
            healthPotion.GetComponent<Image>().sprite = emptyHPPot;

        if (MPPots == maxMPPots)
            manaPotion.GetComponent<Image>().sprite = fullMPPot;
        else if (MPPots < maxMPPots && MPPots != 0)
            manaPotion.GetComponent<Image>().sprite = halfMPPot;
        else
            manaPotion.GetComponent<Image>().sprite = emptyMPPot;

        healthPotion.GetComponentInChildren<Text>().text = HPPots + "/" + maxHPPots;
        manaPotion.GetComponentInChildren<Text>().text = MPPots + "/" + maxMPPots;
    }

    void UseHealthPotion()
    {
        if (HPPots > 0 && hitPoints < maxHitPoints)
        {
            HPPots -= 1;
            double amountToHeal = 0.3 * maxHitPoints;
            hitPoints += amountToHeal;
            if (hitPoints >= maxHitPoints)
                hitPoints = maxHitPoints;
        }
    }

    void UseManaPotion()
    {
        if (MPPots > 0 && manaPoints < maxManaPoints)
        {
            MPPots -= 1;
            double amountToHeal = 0.3 * maxManaPoints;
            manaPoints += amountToHeal;
            if (manaPoints >= maxManaPoints)
                manaPoints = maxManaPoints;
        }
    }
    void FootR() { }

    void FootL() { }

    void NewEvent() { }

    void Hit() { }
}
