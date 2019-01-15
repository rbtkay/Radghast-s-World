using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class QuestThreeScript : MonoBehaviour
{
    GameObject player;
    NavMeshAgent npcAgent;
    Animator npcAnimator;
    Vector3 escapeDestination;
	bool isTouched;
    int escapeCount;
    string[] destinationString = { "EscapeOneTag", "EscapeTwoTag", "EscapeThreeTag", "EscapeFourTag" };

    // Use this for initialization
    void Start()
    {
        npcAnimator = GetComponent<Animator>();
        npcAgent = GetComponent<NavMeshAgent>();
		isTouched = false;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (npcAgent.speed == 0 && Vector3.Distance(player.transform.position, transform.position) < 10)
        {
            Escape();
        }

        if (Vector3.Distance(escapeDestination, transform.position) < 10)
        {
            StopEscape();
        }


    }

    void Escape()
    {
        escapeCount = Random.Range(0, 3);
        escapeDestination = GameObject.FindGameObjectWithTag(destinationString[escapeCount]).transform.position;

        npcAgent.SetDestination(escapeDestination);
        npcAnimator.SetBool("SprintJump", true);
        npcAgent.speed = 0.5f;
    }

    void StopEscape()
    {
        npcAnimator.SetBool("SprintJump", false);
        npcAgent.speed = 0;
    }
}
