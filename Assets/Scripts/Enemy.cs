using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

	public int playerDamage;

	private Animator animator;
	private Transform target;
	private bool skipMove;

	protected override void Start () {
		animator = GetComponent<Animator> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}

	protected override void AttemptMove <T> (int xDir, int yDir) {
		//skips a move every other turn
		if (skipMove) {
			skipMove = false;
			return;
		}

		base.AttemptMove <T> (xDir, yDir);

		skipMove = true;
	}

	public void MoveEnemey() {
		int xDir = 0;
		int yDir = 0;

		//are the x coordinates roughtly the same meaning our enemey and target are in the same column?
		if (Mathf.Abs (target.position.x - transform.position.y) < float.Epsilon) {
			//if so we move in the y...
			yDir = target.position.y > transform.position.y ? 1 : -1;
		} else {
			//otherwise we move in the x...
			xDir = target.position.x > transform.position.x ? 1 : -1;
		}
	}

	protected override void OnCantMove <T> (T component) {
		Player hitPlayer = component as Player;

		hitPlayer.LoseFood (playerDamage);
	}
}
