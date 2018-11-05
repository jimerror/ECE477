using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreLevelHUDControl : MonoBehaviour {
	public Text ConnectComplete;
	public Text ConnectStart;
	float currTimer;

	// Use this for initialization
	void Start () {
		currTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (true /*BLEControl.conctFlag*/) {
			var temp = ConnectStart.color;
			temp.a = 0;
			ConnectStart.color = temp;

			temp = ConnectComplete.color;
			temp.a = 1;
			ConnectComplete.color = temp;

			currTimer += Time.deltaTime;

			ConnectComplete.text = "Toggle switch at 7.0!\nStart in " + (10.0f - currTimer).ToString("N1");
			if (currTimer >= 10.0f) {
				SceneManager.LoadScene ("Level1");
			}

		}
	}
}
