using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public GameObject playerPrefab;
    GameObject player;

    public enum State { noQuest, inQuest, questOne, questTwo, questThree, sideQuest, finalQuest };
    public State gameState;
    public int currentQuest;

    public GameObject initialSpawnPoint;

    [SerializeField] GameObject questOnePosition;
    [SerializeField] GameObject questTwoPosition;
    [SerializeField] GameObject questThreePosition;
    [SerializeField] GameObject sideQuestPosition;
    [SerializeField] GameObject finalQuestPosition;

    [SerializeField] GameObject questOne;
    [SerializeField] GameObject questTwo;
    [SerializeField] GameObject questThree;
    [SerializeField] GameObject chestPrefab;
    [SerializeField] GameObject bullPrefab;

    private string tagToFind;

    GameObject npcOne;
    GameObject npcTwo;
    GameObject npcThree;

    GameObject chest;

    bool isInteractionOneDone;
    public bool isInteractionTwoDone;
    public int interactionTwoCount;
    bool isInteractionThreeDone;
    bool isInteractionChestDone;

    void Start()
    {
        gameState = State.noQuest;


        player = GameObject.Instantiate(playerPrefab, initialSpawnPoint.transform.position, Quaternion.identity);

        npcOne = GameObject.Instantiate(questOne, questOnePosition.transform.position, Quaternion.identity);
        isInteractionOneDone = false;
        isInteractionTwoDone = false;
        interactionTwoCount = 0;
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
            interactionTwoCount = 0;
            isInteractionTwoDone = false;
            currentQuest = 1;
        }
        else if (gameState == State.questTwo)
        {
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("QuestTwoTag"))
            {
                item.GetComponent<RoundedTowerScript>().isActive = true;
            }
            npcThree = GameObject.Instantiate(questThree, questThreePosition.transform.position, Quaternion.identity);
            gameState = State.questThree;
            currentQuest = 2;
        }
        else if (gameState == State.questThree)
        {
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("QuestThreeTag"))
            {
                item.GetComponent<DarkTowerScript>().isActive = true;
            }
            gameState = State.inQuest;
            currentQuest = 3;
        }
        else if (gameState == State.sideQuest)
        {
            chest = GameObject.Instantiate(chestPrefab, sideQuestPosition.transform.position, sideQuestPosition.transform.rotation);
            gameState = State.inQuest;
            currentQuest = 4;
        }
        else if (gameState == State.finalQuest)
        {
            GameObject boss = GameObject.Instantiate(bullPrefab, finalQuestPosition.transform.position, finalQuestPosition.transform.rotation);
            gameState = State.inQuest;
            currentQuest = 4;
        }

        CheckInteraction();
    }

    public void CheckInteraction()
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

                    Destroy(GameObject.FindGameObjectWithTag("npcOneTag"));

                    npcTwo.GetComponent<QuestTwoScript>().isActive = true;

                    if (interactionTwoCount == 0)
                    {
                        foreach (GameObject item in GameObject.FindGameObjectsWithTag("EnemySpawnTag"))
                        {
                            item.GetComponent<EnemySpawnScript>().isActive = true;
                        }
                    }
                    else if (interactionTwoCount == 1)
                    {
                        gameState = State.sideQuest;
                        Debug.Log("interaction with quest 2");
                    }
                    interactionTwoCount++;
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
                    gameState = State.finalQuest;
                    Debug.Log("interaction with quest 4");
                }
            }
        }

        if (GameObject.FindGameObjectWithTag("ChestTag") != null && !isInteractionChestDone)
        {
            if (Vector3.Distance(player.transform.position, chest.transform.position) < 10.0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isInteractionChestDone = true;
                    gameState = State.questTwo;
                    Debug.Log("interaction with quest chest");
                }
            }
        }
    }
}
