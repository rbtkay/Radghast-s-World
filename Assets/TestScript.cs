using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestScript : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] GameObject player;
    // Update is called once per frame
    void Update()
    {

        agent.SetDestination(player.transform.position);

    }
}
