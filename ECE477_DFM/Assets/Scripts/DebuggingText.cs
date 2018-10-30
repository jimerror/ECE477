using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuggingText : MonoBehaviour {
	public static float x, y, z, a, b, c;
	Text text;
	GameObject cam;

	// Use this for initialization
	void Start () {
		text = GetComponent <Text> ();
		cam = GameObject.FindGameObjectWithTag ("MainCamera");

		x = 0; 
		y = 0; 
		z = 0;
		a = 0; 
		b = 0; 
		c = 0;
	}
	
	// Update is called once per frame
	void Update () {
//		a = cam.transform.rotation.x;
//		b = cam.transform.rotation.y;
//		c = cam.transform.rotation.z;

//		x = HUDControl.currEnemyAmount;
		text.text = "Enemy Left: " + (HUDControl.totalEnemyAmount - HUDControl.currEnemyAmount);
		byte[] byteArr = BitConverter.GetBytes ((uint)(HUDControl.currEnemyAmount * 16) + 1);
//		text.text = "breaks down to" + byteArr [0] + ", " + byteArr [1] + ", " + byteArr [2] + ", " + byteArr [3];
//		text.text = "Rot: " + x.ToString("N2") + ", " + y.ToString("N2") + ", " + z.ToString("N2") + ", " + a.ToString("N2") + ", " + b.ToString("N2") + ", " + c.ToString("N2");
//		text.text = "Rot: " + x.ToString("N2") + ", " + y.ToString("N2") + ", " + z.ToString("N2");
//		text.text = "Rot: " + x + ", " + y + ", " + z;
	}
}
