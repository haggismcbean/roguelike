using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// we're creating a singleton here so we can access the game manager from anywhere in the game.
	public static GameManager instance = null;


	public BoardManager boardScript;

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
	
	// Update is called once per frame
	void Update () {
		
	}
}
