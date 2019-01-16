using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OwlGuideScript : MonoBehaviour
{
    // public GameObject owlGuidePrefab;
    [SerializeField] float owlLifeTime;
    TrailRenderer trailRenderer;
    NavMeshAgent navMeshAgent;
    float time;
    // Use this for initialization
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(GameObject.FindGameObjectWithTag(GetNpc()).transform.position);
        time = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad - time > 0.5)
        {
            time = Time.timeSinceLevelLoad;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().focus -= 5;
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().focus <= 0)
        {
            Destroy(gameObject);
        }
    }

    string GetNpc()
    {
        Debug.Log("petit debug");
        if (GameObject.FindGameObjectWithTag("npcThreeTag"))
        {
            return GameObject.FindGameObjectWithTag("npcThreeTag").tag;
        }
        else if (GameObject.FindGameObjectWithTag("npcTwoTag"))
        {
            return GameObject.FindGameObjectWithTag("npcTwoTag").tag;
        }
        else if (GameObject.FindGameObjectWithTag("ChestTag"))
        {
            return GameObject.FindGameObjectWithTag("ChestTag").tag;
        }
        else if (GameObject.FindGameObjectWithTag("npcOneTag"))
        {
            return GameObject.FindGameObjectWithTag("npcOneTag").tag;
        }

        return string.Empty;
    }
}
