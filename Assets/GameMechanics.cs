﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.SceneManagement;
using System;

public class GameMechanics : MonoBehaviour {

	public float spawn_x = 0.0f;
	public float spawn_y = 0.0f;
	public int lives = 10;
	public float player_x = 0.0f;
	public float player_y = 0.0f;
	public string next_level = "level2";
	public float healthLossRate = 1.0f; // loss of health per second
	public bool level_won = false;

	public string level_description = "";
	private float active_player_health = 100.0f;

	private GameObject ActivePlayer;
	public GameObject NewPlayer;
	public GameObject DeadPlayer;
	public GameObject ExitDoor;

	private GameObject Camera;
	private Camera2DFollow camera_logic;

	public Color[] Colors = new Color[0];
	private Int32 currentColorIndex = 0;

	// Use this for initialization
	void Awake () {
		ActivePlayer = GameObject.FindGameObjectWithTag("Player");
		Camera = GameObject.FindGameObjectWithTag ("MainCamera");
		ExitDoor = GameObject.FindGameObjectWithTag ("ExitDoor");
		level_won = false;
		SetupLevel ();
//		NewPlayer = Resources.Load ("Player") as GameObject;
//		DeadPlayer = Resources.Load("DeadPlayer") as GameObject;
	}

	void SetupLevel() {
		Scene scene = SceneManager.GetActiveScene();
		if (scene.name == "level1") {
			lives = 2;
			healthLossRate = 0.2f;
			level_description = "Stand on the shoulders of giants. Or grab the box with Q.";
			next_level = "level2";
		}
		if (scene.name == "level2") {
			lives = 3;
			healthLossRate = 0.5f;
			level_description = "Let the efforts of the past generation guide you forward. Press 'K' to give your life to help.";
			next_level = "level3";
		}
		if (scene.name == "level3") {
			lives = 5;
			healthLossRate = 0.1f;
			level_description = "One generations pain is another generations gain.";
			next_level = "level4";
		}
		if (scene.name == "level4") {
			lives = 25;
			healthLossRate = 0.60f;
			spawn_x = 0.0f;
			spawn_y = 85.0f;
			level_description = "Generations into the void.";
			next_level = "menu _end_happy";
		}
	}

	void KillPlayer() {
		if (lives == 0) {
			// TODO GameOVer logic
			Destroy (ActivePlayer);
			Debug.Log ("Game Over");
			Debug.Log (lives);
			SceneManager.LoadScene("menu _end");
		} else {

			player_x = ActivePlayer.transform.position.x;
			player_y = ActivePlayer.transform.position.y;
			Vector2 dead_pos = new Vector2 (player_x, player_y);
			Vector2 spawn_pos = new Vector2 (spawn_x, spawn_y);
			Color dead_color = ActivePlayer.GetComponentInChildren<PoseSwitcher>().Color;
			Instantiate (NewPlayer, spawn_pos, Quaternion.identity);
			Destroy (ActivePlayer);
			Instantiate (DeadPlayer, dead_pos, Quaternion.identity);
			ActivePlayer = GameObject.FindGameObjectWithTag("Player"); // update active player
			ActivePlayer.GetComponent<PlayerProperties>().health = 100.0f;

			
		//	ActivePlayer.GetComponentInChildren<PoseSwitcher>().Color = Colors[currentColorIndex % Colors.Length];

			camera_logic = Camera.GetComponent<Camera2DFollow>();
			camera_logic.target = ActivePlayer.transform;
			lives = lives - 1;

			//var deadPoseSwitcher = DeadPlayer.GetComponentInChildren<PoseSwitcher>();

			//deadPoseSwitcher.Color = dead_color;
			//deadPoseSwitcher.CurrentType = PoseType.death;
			currentColorIndex++;
		}
		Debug.Log (lives);
		//GameObject.FindGameObjectWithTag("Player"); // always refresh who the player is incase it dies
	}

	void NextLevel(){
		Debug.Log ("next level");
		SceneManager.LoadScene(next_level);
		level_won = false;		
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log("OnTriggerEnter");
		Debug.Log(other.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		ActivePlayer = GameObject.FindGameObjectWithTag("Player");
		Camera = GameObject.FindGameObjectWithTag ("MainCamera");
		active_player_health = ActivePlayer.GetComponent<PlayerProperties> ().health;
		// kill a player instantly with a kill button (testing only maybe)
		if (ActivePlayer != null) {
			if (Input.GetKeyDown ("k") || active_player_health <= 0.0f) {
				// get player location.. kill it, place a dead player in its place
				KillPlayer ();
			}
			if (ActivePlayer.transform.position.y < -20.0f) {
				KillPlayer ();
			}

		}
			

		Debug.Log (level_won);
		if (level_won == true) {
			NextLevel ();
		}


	}
}
