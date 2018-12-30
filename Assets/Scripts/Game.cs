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
        Cursor.visible = false;
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

        // foreach (GameObject enemy in liveEnemies)
        // {
        //     save.livingTargetPositions.Add(enemy.transform.position);
        // }

        save.savePointIndex = 0;

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
            // ResetPlayerPosition();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            player.transform.position = savePoints[save.savePointIndex].transform.position;
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
        Cursor.visible = false;
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
            enemySpawn.GetComponent<EnemySpawnScript>().isActive = false;
        }
    }

    private void ResetPlayerPosition()
    {
        player.transform.position = spawnPoint.transform.position;
    }

}