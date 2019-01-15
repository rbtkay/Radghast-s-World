using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] Button resumeButton;
    [SerializeField] Button newGameButton;
    [SerializeField] GameObject spawnPoint;
    public List<GameObject> savedRoundTowers, currentRoundTowers;
    public GameObject[] originalRoundTowers;
    public GameObject[] copyRoundTowers;
    public List<int> currentRoundTowersIndex;
    public GameObject roundTowerPrefab;
    public GameObject UI;
    GameObject player;
    public GameObject[] savePoints;
    public bool isPaused;

    private void Awake()
    {
        isPaused = false;
        Pause();
    }

    void Start()
    {
        // Cursor.visible = false;
        QualitySettings.vSyncCount = 2;
        copyRoundTowers = new GameObject[6];
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (Input.GetButtonDown("Start"))
        {
            // Debug.Log(isPaused);
            if (isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
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

    public void NewGame()
    {
        ClearEnemies();
        ResetPlayerPosition();
        Unpause();
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

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            ClearEnemies();
            currentRoundTowers = new List<GameObject>();
            currentRoundTowersIndex = new List<int>();
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("RoundTowerTag"))
            {
                Destroy(item);
                // currentRoundTowers.Add(item);
                // currentRoundTowersIndex.Add(item.GetComponent<TowerIndexScript>().index);
            }
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            player.transform.position = new Vector3(save.saveX, save.saveY, save.saveZ);
            player.GetComponent<PlayerScript>().hitPoints = player.GetComponent<PlayerScript>().maxHitPoints;
            player.GetComponent<PlayerScript>().manaPoints = player.GetComponent<PlayerScript>().maxManaPoints;

            foreach (string item in save.roundTowers)
            {
                Debug.Log("Tower bool" + item.Split(' ')[1]);
                GameObject temp = GameObject.Instantiate(roundTowerPrefab, new Vector3(float.Parse(item.Split(' ')[1]),
                                                         float.Parse(item.Split(' ')[2]),
                                                         float.Parse(item.Split(' ')[3])),
                                                         Quaternion.identity);
                // Debug.Log(temp.transform.position);

                temp.GetComponent<TowerIndexScript>().index = Int32.Parse(item.Split(' ')[0]);

                if (temp.GetComponent<TowerIndexScript>().index == 0
                || temp.GetComponent<TowerIndexScript>().index == 1
                || temp.GetComponent<TowerIndexScript>().index == 2)
                {
                    temp.transform.GetChild(4).tag = "QuestTwoTag";
                }
                else
                {
                    temp.transform.GetChild(4).tag = "QuestOneTag";
                }
            }
            Destroy(GameObject.FindGameObjectWithTag("npcTwoTag"));
            Destroy(GameObject.FindGameObjectWithTag("npcThreeTag"));
            Destroy(GameObject.FindGameObjectWithTag("ChestTag"));

            Debug.Log("Game Loaded");
            Unpause();
            GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<ScriptManager>().gameState = save.gameState;
        }
        else
        {
            Debug.Log("No Game Saved!");
        }
    }

    public void Pause()
    {
        menu.SetActive(true);
        newGameButton.Select();
        resumeButton.Select();
        isPaused = true;
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        menu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }

    private void ClearEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("PigTag"))
        {
            Destroy(enemy);
        }

        foreach (GameObject enemySpawn in GameObject.FindGameObjectsWithTag("EnemySpawnTag"))
        {
            enemySpawn.GetComponent<EnemySpawnScript>().countPig = 0;
        }
    }

    private void ResetPlayerPosition()
    {
        player.transform.position = spawnPoint.transform.position;
    }

}