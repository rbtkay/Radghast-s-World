using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public GameObject playerPrefab;
    GameObject player;

    public enum State { noQuest, inQuest, questOne, questTwo, questThree, finalQuest };
    public State gameState;

    public GameObject initialSpawnPoint;

    [SerializeField] GameObject questOnePosition;
    [SerializeField] GameObject questTwoPosition;
    [SerializeField] GameObject questThreePosition;

    [SerializeField] GameObject questOne;
    [SerializeField] GameObject questTwo;
    [SerializeField] GameObject questThree;

    private string tagToFind;

    GameObject npcOne;
    GameObject npcTwo;
    GameObject npcThree;

    bool isInteractionOneDone;
    bool isInteractionTwoDone;
    bool isInteractionThreeDone;

    void Start()
    {
        gameState = State.noQuest;

        player = GameObject.Instantiate(playerPrefab, initialSpawnPoint.transform.position, Quaternion.identity);

        npcOne = GameObject.Instantiate(questOne, questOnePosition.transform.position, Quaternion.identity);
        isInteractionOneDone = false;
        isInteractionTwoDone = false;
        isInteractionThreeDone = false;
    }

    void Update()
    {
        if (gameState == State.questOne)
        {
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("QuestOneTag"))
            {
                item.GetComponent<RoundedTowerScript>().isActive = true;
            }
            npcTwo = GameObject.Instantiate(questTwo, questTwoPosition.transform.position, Quaternion.identity);
            gameState = State.inQuest;
        }
        else if (gameState == State.questTwo)
        {
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("QuestTwoTag"))
            {
                item.GetComponent<RoundedTowerScript>().isActive = true;
            }
            npcThree = GameObject.Instantiate(questThree, questThreePosition.transform.position, Quaternion.identity);
            gameState = State.inQuest;
        }
        else if (gameState == State.questThree)
        {
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("QuestThreeTag"))
            {
                item.GetComponent<RoundedTowerScript>().isActive = true;
            }
            gameState = State.inQuest;
        }

        CheckInteraction();
    }

    void CheckInteraction()
    {
        if (GameObject.FindGameObjectWithTag("npcOneTag") != null && !isInteractionOneDone)
        {
            if (Vector3.Distance(player.transform.position, npcOne.transform.position) < 10.0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isInteractionOneDone = true;
                    gameState = State.questOne;
                    Debug.Log("interaction with quest 1");
                }
            }
        }

        if (GameObject.FindGameObjectWithTag("npcTwoTag") != null && !isInteractionTwoDone)
        {
            if (Vector3.Distance(player.transform.position, npcTwo.transform.position) < 10.0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isInteractionTwoDone = true;
                    gameState = State.questTwo;
                    
                    Destroy(GameObject.FindGameObjectWithTag("npcOneTag"));

                    npcTwo.GetComponent<QuestTwoScript>().isActive = true;

                    foreach (GameObject item in GameObject.FindGameObjectsWithTag("EnemySpawnTag"))
                    {
                        item.GetComponent<EnemySpawnScript>().isActive = true;                        
                    }
                }
            }
        }

        if (GameObject.FindGameObjectWithTag("npcThreeTag") != null && !isInteractionThreeDone)
        {
            if (Vector3.Distance(player.transform.position, npcThree.transform.position) < 10.0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isInteractionThreeDone = true;
                    gameState = State.questThree;
                }
            }
        }
    }
}
