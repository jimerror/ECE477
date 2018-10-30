using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	private bool GyroEn;
	public	int rate = 2;
//	private Gyroscope gyro;

	// Use this for initialization
	void Start () {
//		GetComponent<Camera>().transform.position = new Vector3(0, 0, 0);
		GyroEn = EnableGyro ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GyroEn) {
//			GyroModifyCamera ();
			//transform.Rotate(-Input.gyro.rotationRateUnbiased.x * rate, -Input.gyro.rotationRateUnbiased.y * rate, 0);
			Vector3 previousEulerAngles = transform.eulerAngles;
			Vector3 gyroInput = -Input.gyro.rotationRateUnbiased;

			Vector3 targetEulerAngles = previousEulerAngles + gyroInput * Time.deltaTime * Mathf.Rad2Deg;
			targetEulerAngles.z = 0.0f;

			transform.eulerAngles = targetEulerAngles;
		}
		DebuggingText.x = transform.eulerAngles.x;
		DebuggingText.y = transform.eulerAngles.y;
		DebuggingText.z = transform.eulerAngles.z;
	}

	private bool EnableGyro(){
		if (SystemInfo.supportsGyroscope) {
//			gyro = Input.gyro;
			Input.gyro.enabled = true;

			return true;
		}
		return false; 
	}
}