using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OwlSentinelScript : MonoBehaviour {
	[SerializeField] float owlLifeTime;
	TrailRenderer trailRenderer;
	NavMeshAgent navMeshAgent;
	float time;
	// Use this for initialization
	void Start () {
		trailRenderer = GetComponent<TrailRenderer>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.SetDestination(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().spawnPoint);
		time = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
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
    
}
