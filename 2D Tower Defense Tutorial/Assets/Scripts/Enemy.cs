using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField]
	private float speed;

	private Stack<AStarNode> walkingPath;

	public Point GridPosition { get; set; }

	private Vector3 currentDestination;

	public bool IsActive { get;	set; }

	private void Update(){
		Move ();
	}
		
	public void Spawn(){
		transform.position = LevelManager.Instance.SpawnPortal.transform.position;
		SetPath (LevelManager.Instance.EnemyPath);
		GetComponent<SpriteRenderer> ().sortingOrder = GridPosition.Y;
		IsActive = false;
		StartCoroutine(Scale(new Vector3(-0.1f, 0.1f), new Vector3(-1f, 1f), false));
	}

	public void Despawn(){
		StartCoroutine(Scale(new Vector3(-1f, 1f), new Vector3(-0.1f, 0.1f), true));
	}

	public IEnumerator Scale(Vector3 from, Vector3 to, bool remove){
		float progress = 0f;

		while (progress <= 1) {
			transform.localScale = Vector3.Lerp (from, to, progress);
			progress += Time.deltaTime;

			yield return null;
		}
			
		transform.localScale = to;

		IsActive = true;

		if (remove) {
			Release ();
		}
	}

	private void Move(){
		if (IsActive) {
			transform.position = Vector2.MoveTowards (transform.position, currentDestination, speed * Time.deltaTime);
			GetComponent<SpriteRenderer> ().sortingOrder = GridPosition.Y;

			//did we reach the destination?
			if (transform.position == currentDestination) {
				if (walkingPath != null && walkingPath.Count > 0) {
					Animate(GridPosition, walkingPath.Peek().gridPosition);
					GridPosition = walkingPath.Peek ().gridPosition;
					currentDestination = walkingPath.Pop ().WorldPosition;
				}
			}
		}
	}

	private void SetPath(Stack<AStarNode> path){
		if (path != null) {
			walkingPath = path;
			Animate(GridPosition, walkingPath.Peek().gridPosition);
			GridPosition = walkingPath.Peek ().gridPosition;
			currentDestination = walkingPath.Pop ().WorldPosition;
		}
	}

	private void Animate(Point currentPosition, Point newPosition){
		//moving right
		if (currentPosition.X < newPosition.X) {
			transform.localScale = new Vector3 (-1, 1, 1);
		} 
		//moving left
		else if(currentPosition.X > newPosition.X){
			transform.localScale = new Vector3 (1, 1, 1);
		}
	}
		
	private void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "GoalPortal") {
			Despawn ();
			GameManager.Instance.LifesLeft--;
		}
	}

	private void Release(){
		IsActive = false;
		GameManager.Instance.Pool.ReleaseObject (gameObject);
		GameManager.Instance.removeEnemy (this);
	}
}
