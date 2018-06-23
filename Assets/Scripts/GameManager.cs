using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// we're creating a singleton here so we can access the game manager from anywhere in the game.
	public static GameManager instance = null;


	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	private int level = 3;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}

		//IMPORTANT - This makes things persist between scenes.
		DontDestroyOnLoad(gameObject);

		//This 'getComponent' gets a reference to our board manager script.
		boardScript = GetComponent<BoardManager>();

		InitGame();
	}

	void InitGame() {
		boardScript.SetupScene(level);
	}

	public void GameOver() {
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
