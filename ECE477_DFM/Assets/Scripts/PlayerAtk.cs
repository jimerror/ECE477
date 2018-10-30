using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtk : MonoBehaviour {
	public float attackCooldown = 0.5f;
	public int dps = 50;

	GameObject player,sword;
	PlayerStatus playerStatus;
	EnemyStatus enemyStatus;
	bool checkRange;
	float timer;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		sword = GameObject.Find("Medieval Sward");
		playerStatus = player.GetComponent <PlayerStatus> ();
		enemyStatus = GetComponent<EnemyStatus> ();
	}

	void OnTriggerEnter (Collider obj){
		if (obj.gameObject == sword) {
			checkRange = true;
		}
	}

	void OnTriggerExit (Collider obj){
		if (obj.gameObject == sword) {
			checkRange = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		timer = timer + Time.deltaTime;

		if (timer >= attackCooldown && checkRange && playerStatus.currHp > 0) {
			timer = 0f;
			if (enemyStatus.currHp > 0) {
				enemyStatus.DamageTaken (dps);
			}
		}
	}
}
