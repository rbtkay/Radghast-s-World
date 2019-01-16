using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class FireScript : MonoBehaviour
{
    [SerializeField] float distanceSave;
    [SerializeField] GameObject firePrefab, campfireMenu;
    [SerializeField] Button btnLvlUp, btnUpgradeHealth, btnUpgradeMana;
    bool isOn;
    public GameObject player;
    public GameObject Game;
    double currentSouls, lvlUpCost, upgradeHealthCost, upgradeManaCost;

    // Update is called once per frame
    void Start()
    {
        isOn = false;
        campfireMenu.SetActive(false);
    }
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lvlUpCost = player.GetComponent<PlayerScript>().level * 1;
        upgradeHealthCost = player.GetComponent<PlayerScript>().maxHPPots * 2500;
        upgradeManaCost = player.GetComponent<PlayerScript>().maxMPPots * 2500;
        if (player.GetComponent<PlayerScript>().souls < lvlUpCost)
        {
            btnLvlUp.interactable = false;
        }
        else
        {
            btnLvlUp.interactable = true;
        }

        if (player.GetComponent<PlayerScript>().souls < upgradeHealthCost)
        {
            btnUpgradeHealth.interactable = false;
        }
        else
        {
            btnUpgradeHealth.interactable = true;
        }

        if (player.GetComponent<PlayerScript>().souls < upgradeManaCost)
        {
            btnUpgradeMana.interactable = false;
        }
        else
        {
            btnUpgradeMana.interactable = true;
        }
        btnLvlUp.GetComponentInChildren<Text>().text = "Level Up\n\n (" + lvlUpCost + " Souls)";
        btnUpgradeHealth.GetComponentInChildren<Text>().text = "Upgrade HP Potion\n\n (" + (int)(upgradeHealthCost) + " Souls)";
        btnUpgradeMana.GetComponentInChildren<Text>().text = "Upgrade MP Potion\n\n (" + (int)(upgradeManaCost) + " Souls)";

        if (Vector3.Distance(player.transform.position, transform.position) < distanceSave)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("SPACE");
                GameObject fire = GameObject.Instantiate(firePrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                isOn = true;

                currentSouls = player.GetComponent<PlayerScript>().souls;
                campfireMenu.SetActive(true);
                btnUpgradeHealth.Select();
                btnLvlUp.Select();


                GameObject.FindGameObjectWithTag("GameTag").GetComponent<Game>().isPaused = true;
                Time.timeScale = 0;
            }
        }
    }

    public void LevelUp()
    {
        player.GetComponent<PlayerScript>().level += 1;
        player.GetComponent<PlayerScript>().maxHitPoints = 95 + player.GetComponent<PlayerScript>().level * 5;
        player.GetComponent<PlayerScript>().hitPointsRegen = 0.15 + player.GetComponent<PlayerScript>().level * 0.05;
        player.GetComponent<PlayerScript>().maxManaPoints = 95 + player.GetComponent<PlayerScript>().level * 5;
        player.GetComponent<PlayerScript>().manaPointsRegen = 0.05 + player.GetComponent<PlayerScript>().level * 0.15;
        player.GetComponent<PlayerScript>().damage = 8 + player.GetComponent<PlayerScript>().level * 2;
        player.GetComponent<PlayerScript>().souls -= lvlUpCost;
    }

    public void UpgradeHealth()
    {
        player.GetComponent<PlayerScript>().maxHPPots += 1;
        player.GetComponent<PlayerScript>().souls -= upgradeHealthCost;
    }

    public void UpgradeMana()
    {
        player.GetComponent<PlayerScript>().maxMPPots += 1;
        player.GetComponent<PlayerScript>().souls -= upgradeManaCost;
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        // savedRoundTowers = new List<GameObject>();        

        // foreach (GameObject enemy in liveEnemies)
        // {
        //     save.livingTargetPositions.Add(enemy.transform.position);
        // }

        save.saveX = player.transform.position.x;
        save.saveY = player.transform.position.y;
        save.saveZ = player.transform.position.z;

        save.playerLevel = player.GetComponent<PlayerScript>().level;

        save.playerSouls = player.GetComponent<PlayerScript>().souls;

        save.maxHPPots = player.GetComponent<PlayerScript>().maxHPPots;
        save.maxMPPots = player.GetComponent<PlayerScript>().maxMPPots;

        save.roundTowers = new List<string>();

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("RoundTowerTag"))
        {
            save.roundTowers.Add(item.GetComponent<TowerIndexScript>().index.ToString() + " " +
                                 item.transform.position.x + " " +
                                 item.transform.position.y + " " +
                                 item.transform.position.z);
        }

        save.listNPC = new List<string>();

        switch (GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<ScriptManager>().currentQuest)
        {
            case 1:
                save.gameState = ScriptManager.State.questOne;
                save.listNPC.Add("Quest2" + " " + GameObject.FindGameObjectWithTag("npcTwoTag").GetComponent<QuestTwoScript>().isActive);
                break;

            case 2:
                save.gameState = ScriptManager.State.questTwo;
                save.listNPC.Add("Quest3");
                break;

            case 3:
                save.gameState = ScriptManager.State.questThree;
                break;

            case 4:
                save.gameState = ScriptManager.State.sideQuest;
                save.listNPC.Add("Chest");
                break;

            default:
                break;
        }

        // on load : destroy all npcs and let gamestate decide which to create
        // if (GameObject.FindGameObjectWithTag("npcTwoTag").GetComponent<QuestTwoScript>().isActive)
        // {
        //     save.listNPC.Add("Quest2" + " " + GameObject.FindGameObjectWithTag("npcTwoTag").GetComponent<QuestTwoScript>().isActive);
        // }



        // foreach(GameObject item in GameObject.FindGameObjectsWithTag("DarkTowerTag"))
        // {
        //     save.darkTowers.Add(item);
        //     Destroy(item);
        // }

        return save;
    }

    public void SaveGame()
    {
        Save save = CreateSaveGameObject();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        player.GetComponent<PlayerScript>().hitPoints = player.GetComponent<PlayerScript>().maxHitPoints;
        player.GetComponent<PlayerScript>().manaPoints = player.GetComponent<PlayerScript>().maxManaPoints;

        Debug.Log("Game Saved at: " + Application.persistentDataPath + "/gamesave.save");
    }

    public void Resume()
    {
        campfireMenu.SetActive(false);
        player.GetComponent<PlayerScript>().HPPots = player.GetComponent<PlayerScript>().maxHPPots;
        player.GetComponent<PlayerScript>().MPPots = player.GetComponent<PlayerScript>().maxMPPots; 
        GameObject.FindGameObjectWithTag("GameTag").GetComponent<Game>().isPaused = false;
        Time.timeScale = 1;
    }
}
