using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BLEControl : MonoBehaviour {

	// Bluetooth Device Parameters
	[Tooltip("Enter BLE Device Name")]
	public string DeviceName = "SH-HC-08";
	[Tooltip("Enter service UUID")]
	public string ServiceUUID = "FFE0";
	[Tooltip("Enter characteristic UUID")]
	public string SerialCharacteristic = "FFE1";

	GameObject SwordContainer;
	public static bool conctFlag;

	// Bluetooth Controller States
	enum States
	{
		None,
		Scan,
		ScanRSSI,
		Connect,
		Subscribe,
		Unsubscribe,
		Disconnect,
	}

	private bool _transmit_bug = false;

	// Bluetooth Controller Parameters
	private bool _connected = false;
	private float _timeout = 0f;
	private States _state = States.None;
	private string _deviceAddress;
	private bool _foundSerialID = false;
	private byte[] _dataBytes = null;
	private bool _rssiOnly = false;
	private int _rssi = 0;
	private byte[] byteArr;


//	public Text debugText;

	public static UInt64 teststring;
	public static byte byte1;
	public static byte byte2;
	public static byte byte3;
	public static byte byte4;
	public static byte byte5;
	public static byte byte6;
	public static byte byte7;
	public static byte byte8;
	public static byte sendTest = 0;


	// BLE command
	public static byte command;

	void BleReset ()
	{

		// Reset the Bluetooth Controller Parameters
		_connected = false;
		_timeout = 0f;
		_state = States.None;
		_deviceAddress = null;
		_foundSerialID = false;
		_dataBytes = null;
		_rssi = 0;
	}

	// Change the Bluetooth controller state
	void BleSetState (States newState, float timeout)
	{
		_state = newState;
		_timeout = timeout;
	}

	void BleStartProcess ()
	{
		BleReset ();
		BluetoothLEHardwareInterface.Initialize (true, false, () => {
//			FoundDeviceListScript.DeviceAddressList = new List<DeviceObject> ();

			BleSetState (States.Scan, 0.1f);

		}, (error) => {

			BluetoothLEHardwareInterface.Log ("Error during initialize: " + error);
		});
	}

	string BleFullUUID (string uuid)
	{
		return "0000" + uuid + "-0000-1000-8000-00805f9b34fb";
	}

	bool BleIsEqual(string uuid1, string uuid2)
	{
		if (uuid1.Length == 4)
			uuid1 = BleFullUUID (uuid1);
		if (uuid2.Length == 4)
			uuid2 = BleFullUUID (uuid2);

		return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0);
	}

	public void BleSendByte (byte value)
	{
		byte[] data = new byte[] { value };
		bool writeWithResponse = false;
		BluetoothLEHardwareInterface.WriteCharacteristic (_deviceAddress, ServiceUUID, SerialCharacteristic, data, data.Length, writeWithResponse, (characteristicUUID) => {

			BluetoothLEHardwareInterface.Log ("Write Succeeded");
		});
		command = value;
		Debug.Log ("Bluetooth Send: " + value);
	}

	string BleReadData()
	{
		return System.Text.Encoding.ASCII.GetString(_dataBytes);
	}

	// Parse raw data received from the BLE
	/*
	void BleParseInputData(string rawData)
	{
		try
		{
			string[] vec = rawData.Split(',');
			adxl_x = int.Parse(vec[0]);
			adxl_y = int.Parse(vec[1]);
			adxl_z = int.Parse(vec[2]);
			_transmit_bug = false;
		}catch (System.Exception e ){
			_transmit_bug = true;
			Debug.Log (e);
		}
	}
	*/


	void BleParseInputData()
	{
		try{
			teststring = System.BitConverter.ToUInt64(_dataBytes, 0);
			byte1 = _dataBytes[0];
			byte2 = _dataBytes[1];
			byte3 = _dataBytes[2];
			byte4 = _dataBytes[3];
			byte5 = _dataBytes[4];
			byte6 = _dataBytes[5];
			byte7 = _dataBytes[6];
			byte8 = _dataBytes[7];
			//			teststring = _dataBytes.Length.ToString("D");


		} catch (System.Exception e){
			Debug.Log (e);
		}
	}

	// scale value
	static public float MapValue(float data_in, float min_in, float max_in, float min_out, float max_out)
	{
		return (data_in - min_in) * (max_out - min_out) / (max_in - min_in) + min_out;
	}

	void Awake ()
	{
		conctFlag = false;
		DontDestroyOnLoad (transform.gameObject);
	}

	// Use this for initialization
	void Start ()
	{
		BleStartProcess ();
		StartCoroutine (BlePingUserInput());
	}

	// Update is called once per frame
	void Update ()
	{
		// Refer to the correspond sword container in different scene
		if (SceneManager.GetActiveScene ().name != "PreLevel") {
			SwordContainer = GameObject.FindGameObjectWithTag ("GameController");
		}

		if (_timeout > 0f)
		{
			_timeout -= Time.deltaTime;
			if (_timeout <= 0f)
			{
				_timeout = 0f;

				switch (_state)
				{
				case States.None:
					break;

				case States.Scan:
					BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => {

						// if your device does not advertise the rssi and manufacturer specific data
						// then you must use this callback because the next callback only gets called
						// if you have manufacturer specific data

						if (!_rssiOnly)
						{
							if (name.Contains(DeviceName))
							{
								BluetoothLEHardwareInterface.StopScan ();
//								FoundDeviceListScript.DeviceAddressList.Add (new DeviceObject (address, name));
								// found a device with the name we want
								// this example does not deal with finding more than one
								_deviceAddress = address;
								BleSetState (States.Connect, 0.5f);
							}
						}

					}, (address, name, rssi, bytes) => {

						// use this one if the device responses with manufacturer specific data and the rssi

						if (name.Contains(DeviceName))
						{
							if (_rssiOnly)
							{
								_rssi = rssi;
							}
							else
							{
								BluetoothLEHardwareInterface.StopScan ();
//								FoundDeviceListScript.DeviceAddressList.Add (new DeviceObject (address, name));
								// found a device with the name we want
								// this example does not deal with finding more than one
								_deviceAddress = address;
								BleSetState (States.Connect, 0.5f);
							}
						}

					}, _rssiOnly); // this last setting allows BLE to send RSSI without having manufacturer data

					if (_rssiOnly)
						BleSetState (States.ScanRSSI, 0.5f);
					break;

				case States.ScanRSSI:
					break;

				case States.Connect:
					// set these flags
					_foundSerialID = false;

					// note that the first parameter is the address, not the name. I have not fixed this because
					// of backwards compatiblity.
					// also note that I am note using the first 2 callbacks. If you are not looking for specific characteristics you can use one of
					// the first 2, but keep in mind that the device will enumerate everything and so you will want to have a timeout
					// large enough that it will be finished enumerating before you try to subscribe or do any other operations.
					BluetoothLEHardwareInterface.ConnectToPeripheral (_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) => {

						if (BleIsEqual (serviceUUID, ServiceUUID))
						{
							_foundSerialID = _foundSerialID || BleIsEqual (characteristicUUID, SerialCharacteristic);

							// if we have found both characteristics that we are waiting for
							// set the state. make sure there is enough timeout that if the
							// device is still enumerating other characteristics it finishes
							// before we try to subscribe
							if (_foundSerialID)
							{
								_connected = true;
								conctFlag = true;
								BleSetState (States.Subscribe, 2f);
							}
						}
					});
					break;

				case States.Subscribe:
					BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (_deviceAddress, ServiceUUID, SerialCharacteristic, null, (address, characteristicUUID, bytes) => {

						_state = States.None;

						// data received from the device
						_dataBytes = bytes;



					});
					break;

				case States.Unsubscribe:
					BluetoothLEHardwareInterface.UnSubscribeCharacteristic (_deviceAddress, ServiceUUID, SerialCharacteristic, null);
					BleSetState (States.Disconnect, 4f);
					break;

				case States.Disconnect:
					if (_connected)
					{
						BluetoothLEHardwareInterface.DisconnectPeripheral (_deviceAddress, (address) => {
							BluetoothLEHardwareInterface.DeInitialize (() => {

								_connected = false;
								_state = States.None;
							});
						});
					}
					else
					{
						BluetoothLEHardwareInterface.DeInitialize (() => {

							_state = States.None;
						});
					}
					break;
				}
			}
		}
	}



	IEnumerator BlePingUserInput(){
		while (true) {
			yield return new WaitForSeconds (0.01f);
//
//			float currTimer = 0;
//
//			while (currTimer < 0.01f) {
//				currTimer += Time.unscaledDeltaTime;
//			}

			if (_connected) {
				BleParseInputData ();

				if (byte1 == 0x55 && byte2 == 0x53) {

					float x = (float)((short)byte4 << 8 | (short)byte3) * 180 / 32768; 
					float y = (float)((short)byte8 << 8 | (short)byte7) * 180 / 32768; 
					float z = (float)((short)byte6 << 8 | (short)byte5) * 180 / 32768;

					DebuggingText.x = x;
					DebuggingText.y = y;
					DebuggingText.z = z;

					if (SceneManager.GetActiveScene ().name != "PreLevel") {
						SwordContainer.transform.rotation = Quaternion.Euler (new Vector3 (-x, -y, -z));
					}
				}

				string currScene = SceneManager.GetActiveScene ().name;
//				byte[] byteArr = BitConverter.GetBytes (HUDControl.EnemyAmount*16);

				if (currScene == "Level1") {
					byteArr = BitConverter.GetBytes ((uint)(HUDControl.currEnemyAmount*16)+1);
					sendTest = (byteArr[0]);
				} else if (currScene == "Level2") {
					byteArr = BitConverter.GetBytes ((uint)(HUDControl.currEnemyAmount*16)+2);
					sendTest = (byteArr[0]);
				} else if (currScene == "Level3" && (!HUDControl.endFlag)) {
					byteArr = BitConverter.GetBytes ((uint)(HUDControl.currEnemyAmount*16)+3);
					sendTest = (byteArr[0]);
				} else if (currScene == "Level3" && HUDControl.endFlag) {
					byteArr = BitConverter.GetBytes ((uint)(HUDControl.currEnemyAmount*16)+4);
					sendTest = (byteArr[0]);
				} else {
					sendTest = 0x00;
				}
				BleSendByte (sendTest);
			}
		}
	}

	void OnGUI()
	{
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.skin.button.alignment = TextAnchor.MiddleCenter;
		GUI.skin.label.fontSize = 40;
		GUI.skin.button.fontSize = 15;

		if (_connected) {


			//			#if !UNITY_EDITOR
			//			GUI.Label (new Rect (10, 10, Screen.width - 10, Screen.height - 10), string(teststring));
			//			#endif

//			debugText.text = "Value: " + teststring + "," + byte1.ToString("X2") + "," + byte2.ToString("X2") + "," + byte3.ToString("X2");
			//			debugText.text = "Value: " + teststring + "," + ((int)Char.GetNumericValue(byte1)).ToString("X") + "," + ((int)Char.GetNumericValue(byte2)).ToString("X") + "," + ((int)Char.GetNumericValue(byte3)).ToString("X");

			_state = States.None;
			//			BleSendByte(sendTest);
			_state = States.Subscribe;

			// debugging pushbuttons on the screen
			//			if (GUI.Button (new Rect (10, 610, 90, 100), "VH F")) {
			//				BleSendByte ((byte)(HapticManager.HCmdVibHigh << 4 | HapticManager.HCmdTorqueForward));
			//			} else if (GUI.Button (new Rect (110, 610, 90, 100), "VM F")) {
			//				BleSendByte ((byte)(HapticManager.HCmdVibMid << 4 | HapticManager.HCmdTorqueForward));
			//			} else if (GUI.Button (new Rect (210, 610, 90, 100), "VL F")) {
			//				BleSendByte ((byte)(HapticManager.HCmdVibLow << 4 | HapticManager.HCmdTorqueForward));
			//			} else if (GUI.Button (new Rect (310, 610, 90, 100), "VH B")) {
			//				BleSendByte ((byte)(HapticManager.HCmdVibHigh << 4 | HapticManager.HCmdTorqueBackward));
			//			} else if (GUI.Button (new Rect (410, 610, 90, 100), "VM B")) {
			//				BleSendByte ((byte)(HapticManager.HCmdVibMid << 4 | HapticManager.HCmdTorqueBackward));
			//			} else if (GUI.Button (new Rect (510, 610, 90, 100), "VL B")) {
			//				BleSendByte ((byte)(HapticManager.HCmdVibLow << 4 | HapticManager.HCmdTorqueBackward));
			//			} else {
			//				_state = States.Subscribe;
			//			}

		} else {
			GUI.skin.label.fontSize = 40;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			#if !UNITY_EDITOR
			GUI.Label (new Rect (10, 10, Screen.width - 10, Screen.height - 10), "Device Not Found");
			#endif
		}
	}
}