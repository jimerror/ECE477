using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtk : MonoBehaviour {
	public float attackCooldown = 0.5f;
	public int dps = 10;

	Animator anim;
	GameObject player;
	PlayerStatus playerStatus;
	EnemyStatus enemyStatus;
	bool checkRange;
	float timer;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerStatus = player.GetComponent <PlayerStatus> ();
		enemyStatus = GetComponent <EnemyStatus> ();
		anim = GetComponent <Animator>();
	}

	void OnTriggerEnter (Collider obj){
		if (obj.gameObject == player) {
			checkRange = true;
		}
	}

	void OnTriggerExit (Collider obj){
		if (obj.gameObject == player) {
			checkRange = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		timer = timer + Time.deltaTime;

		if (timer >= attackCooldown && checkRange && enemyStatus.currHp > 0) {
			timer = 0f;
			if (playerStatus.currHp > 0) {
				playerStatus.DamageTaken (dps);
			}
		}
		if (playerStatus.currHp <= 0) {
			anim.SetTrigger ("PlayerDead");
		}
	}
}
