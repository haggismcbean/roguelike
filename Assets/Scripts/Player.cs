using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// note we're inheriting from MovingObject not just from a default nothing class!!!!
public class Player : MovingObject {

	public int wallDamage = 1;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;

	public float restartLevelDelay = 1f;

	//used to store a reference to our animator component
	private Animator animator;

	private int food;



	// Protected override because we have a different Start function here than in MovingObject
	protected override void Start () {
		animator = GetComponent<Animator> ();

		food = GameManager.instance.playerFoodPoints;

		//this calls the start function of the base class (MovingObject.Start)
		base.Start ();
	}

	//part of the unity api and will be called when the player is disabed.
	private void OnDisabled() {
		GameManager.instance.playerFoodPoints = food;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.playersTurn) {
			return;
		}

		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");

		//NOTE - THIS STOPS PLAYERS MOVING DIAGONALLY
		if (horizontal != 0) {
			vertical = 0;
		}

		if (horizontal != 0 || vertical != 0) {
			//passing in the generic parameter wall, meaning we're expecting the player may move into a wall which is an object the player can interact with.
			AttemptMove<Wall> (horizontal, vertical);
		}
	}

	//gives player ability to interract with other objects on the board (exit, soda, food)
	// OnTriggerEnter2D is a unity api that is called when a GameObject collides with another GameObject
	private void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Exit") {
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		} else if (other.tag == "Food") {
			food += pointsPerFood;
			other.gameObject.SetActive (false);
		} else if (other.tag == "Soda") {
			food += pointsPerSoda;
			other.gameObject.SetActive (false);
		}
	}

	protected override void OnCantMove <T> (T component) {
		Wall hitWall = component as Wall;
		hitWall.DamageWall (wallDamage);

		animator.SetTrigger ("playerChop");
	}

	private void Restart() {
		Application.LoadLevel (Application.loadedLevel);
	}

	public void LoseFood (int loss) {
		animator.SetTrigger ("playerHit");
		food -= loss;
		CheckIfGameOver ();
	}

	protected override void AttemptMove<T> (int xDir, int yDir) {
		//every time they move they lose 1 food point
		food--;

		base.AttemptMove <T> (xDir, yDir);

		//RaycastHit2D hit;

		CheckIfGameOver();

		GameManager.instance.playersTurn = false;
	}


	private void CheckIfGameOver() {
		if (food <= 0) {
			GameManager.instance.GameOver();
		}
	}
}
