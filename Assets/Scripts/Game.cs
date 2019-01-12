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
        save.roundTowers = new List<string>();
        // savedRoundTowers = new List<GameObject>();        

        // foreach (GameObject enemy in liveEnemies)
        // {
        //     save.livingTargetPositions.Add(enemy.transform.position);
        // }

        save.saveX = player.transform.position.x;
        save.saveY = player.transform.position.y;
        save.saveZ = player.transform.position.z;

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("RoundTowerTag"))
        {
            save.roundTowers.Add(item.GetComponent<TowerIndexScript>().index.ToString() + " " +
                                 item.GetComponentInChildren<RoundedTowerScript>().isActive + " " +
                                 item.transform.position.x + " " +
                                 item.transform.position.y + " " +
                                 item.transform.position.z);

            // savedRoundTowers.Add(item);
        }

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

        // Debug.Log("Game Saved at: " + Application.persistentDataPath + "/gamesave.save");
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



            foreach (string item in save.roundTowers)
            {
                Debug.Log("Tower bool" + item.Split(' ')[1]);
                GameObject temp = GameObject.Instantiate(roundTowerPrefab, new Vector3(float.Parse(item.Split(' ')[2]),
                                                         float.Parse(item.Split(' ')[3]),
                                                         float.Parse(item.Split(' ')[4])),
                                                         Quaternion.identity);
                // Debug.Log(temp.transform.position);

                temp.GetComponent<TowerIndexScript>().index = Int32.Parse(item.Split(' ')[0]);
                temp.GetComponentInChildren<RoundedTowerScript>().isActive = Boolean.Parse(item.Split(' ')[1]);

                if (temp.GetComponent<TowerIndexScript>().index == 0
                || temp.GetComponent<TowerIndexScript>().index == 1
                || temp.GetComponent<TowerIndexScript>().index == 2)
                {
                    temp.transform.GetChild(3).tag = "QuestTwoTag";
                }
                else
                {
                    temp.transform.GetChild(3).tag = "QuestOneTag";
                }
                // Boolean.Parse(item.Split(' ')[1].ToLower())
                // foreach (GameObject rt in currentRoundTowers)
                // {
                //     if (rt.GetComponent<TowerIndexScript>().index == temp.GetComponent<TowerIndexScript>().index)
                //     {
                //         rt.GetComponent<RoundedTowerScript>().hitPoints = rt.GetComponent<RoundedTowerScript>().maxHitPoints;
                //         rt.GetComponent<RoundedTowerScript>().isActive = temp.GetComponent<RoundedTowerScript>().isActive;
                //     }
                //     else
                //     {
                //         Destroy(rt);
                //     }
                //     Destroy(temp);
                // }
                // temp.transform.position = new Vector3(float.Parse(item.Split(' ')[0]));
                // if (!currentRoundTowers.Contains())
            }

            // foreach(GameObject item in GameObject.FindGameObjectsWithTag("RoundTowerTag"))
            // {
            //     if (item.GetComponent<TowerIndexScript>().index == )
            // }


            // if (GameObject.FindGameObjectsWithTag("RoundTowerTag").Length <= save.roundTowers.Count)
            // {
            // foreach (string item in save.roundTowers)
            // {
            //     if (!currentRoundTowersIndex.Contains(item))
            //     {
            //         GameObject temp = GameObject.Instantiate(copyRoundTowers[item]);
            //     }
            //     else
            //     {
            //         originalRoundTowers[item] = copyRoundTowers[item];
            //     }
            // }
            // }
            // else
            // {
            // foreach (GameObject item in GameObject.FindGameObjectsWithTag("RoundTowerTag"))
            // {
            //     if (!save.roundTowers.Contains(item.GetComponent<TowerIndexScript>().index))
            //     {
            //         Destroy(item);
            //     }
            // else
            // {
            //     item.GetComponentInChildren<RoundedTowerScript>().hitPoints = item.GetComponentInChildren<RoundedTowerScript>().maxHitPoints;
            // }
            // }
            // }

            // foreach(GameObject item in GameObject.FindGameObjectsWithTag("RoundTowerTag"))
            // {
            //     if (save.roundTowers.Contains(item.GetComponent<TowerIndexScript>().index))
            //     {
            //         item.GetComponentInChildren<RoundedTowerScript>().hitPoints = item.GetComponentInChildren<RoundedTowerScript>().maxHitPoints;
            //     }
            // }
            // foreach(int item in save.roundTowers)
            // {
            //     if (!currentRoundTowersIndex.Contains(item))
            //     {
            //         GameObject.Instantiate(originalRoundTowers[item]);
            //     }
            //     else
            //     {
            //         foreach (GameObject tower in GameObject.FindGameObjectsWithTag("RoundTowerTag"))
            //         {
            //             if (tower.GetComponent<TowerIndexScript>().index == item)
            //             {
            //                 tower.GetComponentInChildren<RoundedTowerScript>().hitPoints = tower.GetComponentInChildren<RoundedTowerScript>().maxHitPoints;
            //             }
            //             else
            //             {
            //                 Destroy(tower);
            //             }
            //         }
            //     }
            // }
            // foreach(GameObject item in save.darkTowers)
            // {
            //     GameObject DarkTower = item;
            // }
            // player.transform.position = savePoints[save.savePointIndex].transform.position;
            Debug.Log("Game Loaded");
            Unpause();
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