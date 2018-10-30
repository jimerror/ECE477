using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyManager : MonoBehaviour {
	public PlayerStatus playerStatus;
	public GameObject bunny;
	public float respawnCD = 3f;
	public Transform[] location;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("Spawn", respawnCD, respawnCD);
	}

	void Spawn(){
		if (playerStatus.currHp <= 0f) {
			return;
		}
		int idx = Random.Range (0, location.Length);
		Instantiate (bunny, location [idx].position, location [idx].rotation);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
