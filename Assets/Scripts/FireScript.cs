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
    GameObject player;
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
        if (Vector3.Distance(player.transform.position, transform.position) < distanceSave)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject fire = GameObject.Instantiate(firePrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                isOn = true;

                currentSouls = player.GetComponent<PlayerScript>().souls;
                lvlUpCost = player.GetComponent<PlayerScript>().level * 1250;
                upgradeHealthCost = player.GetComponent<PlayerScript>().level * 2500;
                upgradeManaCost = player.GetComponent<PlayerScript>().level * 2500;
                campfireMenu.SetActive(true);
                btnUpgradeHealth.Select();
                btnLvlUp.Select();
                btnLvlUp.GetComponentInChildren<Text>().text = "Level Up\n\n (" + lvlUpCost + " Souls)";
                btnUpgradeHealth.GetComponentInChildren<Text>().text = "Upgrade HP Potion\n\n (" + (int)(upgradeHealthCost) + " Souls)";
                btnUpgradeMana.GetComponentInChildren<Text>().text = "Upgrade MP Potion\n\n (" + (int)(upgradeManaCost) + " Souls)";

                GameObject.FindGameObjectWithTag("GameTag").GetComponent<Game>().isPaused = true;
                Time.timeScale = 0;
            }
        }
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

        Debug.Log("Game Saved at: " + Application.persistentDataPath + "/gamesave.save");
    }

    public void Resume()
    {
        campfireMenu.SetActive(false);
        GameObject.FindGameObjectWithTag("Game").GetComponent<Game>().isPaused = false;
        Time.timeScale = 1;
    }
}
