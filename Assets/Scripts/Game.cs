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
    [SerializeField] Button btnResumeButton, btnNewGameButton;
    [SerializeField] GameObject spawnPoint;
    public GameObject roundTowerPrefab, darkTowerPrefab;
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
    public void NewGame()
    {
        ClearEnemies();
        player.transform.position = spawnPoint.transform.position;
        Unpause();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            ClearEnemies();
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("RoundTowerTag"))
            {
                Destroy(item);
            }

            foreach (GameObject item in GameObject.FindGameObjectsWithTag("QuestThreeTag"))
            {
                Destroy(item);
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            player.transform.position = new Vector3(save.saveX, save.saveY, save.saveZ);
            player.GetComponent<PlayerScript>().spawnPoint = new Vector3(save.saveX, save.saveY, save.saveZ);
            player.GetComponent<PlayerScript>().level = save.playerLevel;
            player.GetComponent<PlayerScript>().maxHitPoints = 95 + player.GetComponent<PlayerScript>().level * 5;
            player.GetComponent<PlayerScript>().hitPointsRegen = 0.15 + player.GetComponent<PlayerScript>().level * 0.05;
            player.GetComponent<PlayerScript>().maxManaPoints = 95 + player.GetComponent<PlayerScript>().level * 5;
            player.GetComponent<PlayerScript>().manaPointsRegen = 0.05 + player.GetComponent<PlayerScript>().level * 0.15;
            player.GetComponent<PlayerScript>().damage = 8 + player.GetComponent<PlayerScript>().level * 2;
            player.GetComponent<PlayerScript>().hitPoints = player.GetComponent<PlayerScript>().maxHitPoints;
            player.GetComponent<PlayerScript>().manaPoints = player.GetComponent<PlayerScript>().maxManaPoints;
            player.GetComponent<PlayerScript>().souls = save.playerSouls;
            player.GetComponent<PlayerScript>().maxHPPots = save.maxHPPots;
            player.GetComponent<PlayerScript>().maxMPPots = save.maxMPPots;
            player.GetComponent<PlayerScript>().HPPots = player.GetComponent<PlayerScript>().maxHPPots;
            player.GetComponent<PlayerScript>().MPPots = player.GetComponent<PlayerScript>().maxMPPots;
            

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

            foreach (string item in save.darkTowers)
            {
                GameObject temp = GameObject.Instantiate(darkTowerPrefab, new Vector3(float.Parse(item.Split(' ')[0]),
                                                         float.Parse(item.Split(' ')[1]),
                                                         float.Parse(item.Split(' ')[2])),
                                                         darkTowerPrefab.transform.rotation);
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
        btnNewGameButton.Select();
        btnResumeButton.Select();
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
}