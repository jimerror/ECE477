using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {
	public int initHp = 100;
	public int currHp;
	public Slider hpSlider;
	public Image damageImage;
	public AudioClip deathSource;
	public float flashSpeed = 5f;
	public Color flashColor = new Color (1f, 0f, 0f, 0.1f);

	private bool PlayerDead;
	private bool PlayerDamaged;

	// Use this for initialization
	void Start () {
		currHp = initHp;
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerDamaged) {
			damageImage.color = flashColor;
		} else {
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		PlayerDamaged = false;
	}

	public void DamageTaken (int dps){
		PlayerDamaged = true;
		currHp -= dps;
		hpSlider.value = currHp;

		if (currHp <= 0 && !PlayerDead) {
			Death ();
		}
	}

	private void Death(){
		PlayerDead = true;
	}
}
