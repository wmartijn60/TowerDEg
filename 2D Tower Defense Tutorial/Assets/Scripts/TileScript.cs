using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour {

	private Color32 fullColor = new Color32(150, 10, 10, 255);
	private Color32 emptyColor = new Color32(10, 150, 10, 255);

	private SpriteRenderer spriteRenderer;

	/// <summary>
	/// Gets or sets the grid position.
	/// </summary>
	/// <value>The grid position.</value>
	public Point GridPosition { get; private set; }

	public bool IsEmpty { get; private set; }

	public bool IsWalkable {
		get;
		set;
	}

	public Vector2 WorldPosition {
		get{
			Vector3 spriteSize = GetComponent<SpriteRenderer> ().bounds.size;
			return new Vector2 (transform.position.x + spriteSize.x / 2, transform.position.y - spriteSize.y / 2);
		}
	}

	// Use this for initialization
	void Start () {
		spriteRenderer  = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Raises the mouse over event.
	/// </summary>
	private void OnMouseOver(){
		if (!EventSystem.current.IsPointerOverGameObject () && GameManager.Instance.ActiveTowerButton != null) {
			if (IsEmpty) {
				spriteRenderer.color = emptyColor;
			} else {
				spriteRenderer.color = fullColor;
			}

			if (Input.GetMouseButtonDown (0)) {
				PlaceTower ();
			}
		}
	}

	private void OnMouseExit(){
		spriteRenderer.color = Color.white;
	}

	public void Setup(Transform parent, Point gridPosition, Vector3 worldPosition){
		transform.parent = parent;
		GridPosition = gridPosition;
		transform.position = worldPosition;

		IsEmpty = true;
		IsWalkable = true;

		LevelManager.Instance.Tiles.Add (gridPosition, this);
	}

	private void PlaceTower(){
		if (IsEmpty) {
			GameObject newTower = Instantiate (GameManager.Instance.ActiveTowerButton.TowerPrefab, transform.position, Quaternion.identity);
			newTower.GetComponent<SpriteRenderer> ().sortingOrder = GridPosition.Y;
			newTower.transform.SetParent (transform);

			IsEmpty = false;
			IsWalkable = false;
			spriteRenderer.color = Color.white;

			GameManager.Instance.BuyTower ();
		}
	}
}
