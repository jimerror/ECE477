using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour {
	public int initHp = 100;
	public int currHp = 0;
	public float animSpeed = 2.5f;
	public int point = 10;
	public AudioClip deathSource;

	Animator anim;
	AudioSource enemyAudio;
	CapsuleCollider capsuleCollider;
	bool Dead;
	bool Sink;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		enemyAudio = GetComponent <AudioSource> ();
		capsuleCollider = GetComponent <CapsuleCollider> ();

		currHp = initHp;
	}
	
	// Update is called once per frame
	void Update () {
		if (Sink) {
			transform.Translate (-Vector3.up * animSpeed * Time.deltaTime);
		}
	}

	public void DamageTaken(int dps){
		if (Dead) {
			return;
		}

		enemyAudio.Play ();

		currHp -= dps;

		if (currHp <= 0) {
			// Enemy is dying
			Dead = true;
			capsuleCollider.isTrigger = true;

			HUDControl.currEnemyAmount += 1;

			anim.SetTrigger ("Dead");
			enemyAudio.clip = deathSource;
			enemyAudio.Play ();
		}
	}

	// Sinking animation trigger event
	public void Sinking(){
		GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
		GetComponent <Rigidbody> ().isKinematic = true;

		Sink = true;

		Destroy (gameObject, 2f);
	}
}
