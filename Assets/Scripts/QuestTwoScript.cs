using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class QuestTwoScript : MonoBehaviour
{
    Animator npcAnimator;
    NavMeshAgent npcAgent;
    public bool isActive;
    private bool isWalking;
    // Use this for initialization
    void Start()
    {
        isActive = false;
        npcAgent = GetComponent<NavMeshAgent>();
        npcAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (Vector3.Distance(transform.position, player.transform.position) < 40)
            {
                Walk();
            }
            else
            {
                npcStop();
            }
        }
    }

    private void Walk()
    {
        Vector3 destination = GameObject.FindGameObjectWithTag("DestinationQuestTwoTag").transform.position;
        npcAgent.speed = 1.0f;
        npcAgent.SetDestination(destination);
        npcAnimator.SetBool("Walk", true);

        if (Vector3.Distance(transform.position, destination) < 2.0f)
        {
			npcStop();
			isActive = false;
        }
    }

    private void npcStop()
    {
        npcAgent.speed = 0.0f;
        npcAnimator.SetBool("Walk", false);
    }
}
