using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The abstract keyword enables us to create classes & class members that are incomplete and must be implemented in the derived class
public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1f;

	//collision detection
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2D;
	private float inverseMoveTime;

	// Protected virtual functions can be overridden by their inheriting classes
	protected virtual void Start () {
		boxCollider = GetComponent<BoxCollider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		inverseMoveTime = 1f / moveTime;
	}

	protected bool Move (int xDir, int yDir, out RaycastHit2D hit) {
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);

		//checking if there were any collisions
		boxCollider.enabled = false;
		//this 'linecast' draws a line between the start and end, and checks whether there is anything solid in between.
		hit = Physics2D.Linecast (start, end, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null) {
			//i guess coroutine is some sort of async call we don't have to wait for it to finish before returning.
			StartCoroutine (SmoothMovement (end));
			return true;
		}

		//hit.transform == true so we can't move
		return false;
	}

	protected IEnumerator SmoothMovement (Vector3 end) {
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon) {
			//newPosition is the position of the object once it's moved inverseMoveTime * Time.deltaTime units towards its target (end)
			Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition (newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;

			//wait for a frame before reevaluating the condition of the loop
			yield return null;
		}
	}

	protected virtual void AttemptMove <T> (int xDir, int yDir)
		where T : Component
	{
		RaycastHit2D hit;

		//the 'out' parameter means it's updated in the Move function and new value passed to here!
		bool canMove = Move (xDir, yDir, out hit);

		if (hit.transform == null) {
			return;
		}

		//if something was hit...
		T hitComponent = hit.transform.GetComponent<T>();

		if (!canMove && hitComponent != null) {
			OnCantMove(hitComponent);
		}

	}

	//abstract modifier indicates that the thing being modified has a missing or incomplete function - this is gonna be overwritten in its inherited classes
	protected abstract void OnCantMove <T>(T Component)
		where T : Component;
}
