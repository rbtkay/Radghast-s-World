using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public GameObject playerPrefab;
    GameObject player;

    public GameObject initialSpawnPoint;
    void Start()
    {
        player = GameObject.Instantiate(playerPrefab, initialSpawnPoint.transform.position, Quaternion.identity);
    }
}
