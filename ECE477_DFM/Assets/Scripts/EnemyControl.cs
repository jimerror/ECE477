using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {
	GameObject player;
	UnityEngine.AI.NavMeshAgent nav;
	PlayerStatus playerStatus;
	EnemyStatus enemyStatus;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
		playerStatus = player.GetComponent <PlayerStatus> ();
		enemyStatus = GetComponent<EnemyStatus> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (enemyStatus.currHp > 0 && playerStatus.currHp > 0) {
			if (!nav.enabled) {
				nav.enabled = true;
			}
			nav.SetDestination (player.transform.position);

		} else {
			nav.enabled = false;
		}
	}
}
