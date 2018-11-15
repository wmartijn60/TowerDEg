using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager> {

	[SerializeField]
	private GameObject[] tilePrefabs;
	[SerializeField]
	private GameObject portalPrefab;

	[SerializeField]
	private CameraMovement cameraMovement;

	[SerializeField]
	private Transform map;

	private string[] mapData;

	private Point startPortal;
	private Point endPortal;

	private Stack<AStarNode> enemyPath;
	public Stack<AStarNode> EnemyPath {
		get{
			if (enemyPath == null) {
				GeneratePath ();
			}
			return new Stack<AStarNode> (new Stack<AStarNode>(enemyPath));
		}
	}

	public Dictionary<Point, TileScript> Tiles { get; set; }

	/// <summary>
	/// Gets the width of the tile.
	/// </summary>
	/// <value>The width of the tile.</value>
	public float tileWidth {
		get{
			return tilePrefabs[0].GetComponent<SpriteRenderer> ().sprite.bounds.size.x;	
		}
	}

	/// <summary>
	/// Gets the height of the tile.
	/// </summary>
	/// <value>The height of the tile.</value>
	public float tileHeight {
		get{
			return tilePrefabs[0].GetComponent<SpriteRenderer> ().sprite.bounds.size.y;	
		}
	}

	public Portal SpawnPortal {
		get;
		private set;
	}

	// Use this for initialization
	void Start () {
		Tiles = new Dictionary<Point, TileScript> ();

		loadLevel ("level1");
		createLevel ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	/// <summary>
	/// Loads the level.
	/// </summary>
	private void loadLevel(string levelName){
		TextAsset levelRes = Resources.Load ("Levels/" + levelName) as TextAsset;

		string levelString = levelRes.text.Replace (Environment.NewLine, string.Empty);
		mapData = levelString.Split ('-');
	}

	/// <summary>
	/// Creates the level.
	/// </summary>
	private void createLevel(){
		//calculates the world origin to be the top left corner of the camera
		Vector3 worldOrigin = Camera.main.ScreenToWorldPoint (new Vector3 (0, Screen.height));

		int mapGridX = mapData [0].ToCharArray ().Length;
		int mapGridY = mapData.Length;

		//place the tiles
		for (int y = 0; y < mapGridY; y++) {
			for (int x = 0; x < mapGridX; x++) {
				int tileType = int.Parse(mapData[y].ToCharArray()[x].ToString());
				placeTile (x, y, tileType, worldOrigin);
			}
		}

		Vector3 maxTilePositon = Tiles [new Point (mapGridX - 1, mapGridY - 1)].transform.position;
		cameraMovement.setLimits (new Vector3(maxTilePositon.x + tileWidth,
			maxTilePositon.y - tileHeight));

		createPortals ();
	}

	/// <summary>
	/// Places the tile.
	/// </summary>
	/// <param name="x">The x coordinate of the tile.</param>
	/// <param name="y">The y coordinate of the tile.</param>
	/// <param name="worldOrigin">The origin of the world.</param>
	private void placeTile(int x, int y, int tileType, Vector3 worldOrigin){
		//get the TileScript of the tile to place
		TileScript newTileScript = Instantiate (tilePrefabs[tileType]).GetComponent<TileScript>();

		//calculate the world position of the tile
		Vector3 worldPosition = new Vector3 (worldOrigin.x + (tileWidth * x), worldOrigin.y - (tileHeight * y), 0);

		//coordinate of the Tile in the grid
		Point gridPosition = new Point (x, y);

		//setup the new tile
		newTileScript.Setup (map, gridPosition, worldPosition);
	}

	private void createPortals(){
		startPortal = new Point (0, 1);
		endPortal = new Point (17, 9);

		GameObject tmp = Instantiate (portalPrefab, Tiles[startPortal].WorldPosition, Quaternion.identity);
		SpawnPortal = tmp.GetComponent<Portal> ();
		SpawnPortal.name = "SpawnPortal";

		GameObject goal = Instantiate (portalPrefab, Tiles[endPortal].WorldPosition, Quaternion.identity);
		goal.tag = "GoalPortal";
		goal.name = "GoalPortal";

		BoxCollider2D goalCollider = goal.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
		goalCollider.isTrigger = true;
		goalCollider.size = new Vector2 (1f, 1f);
	}

	public void GeneratePath(){
		enemyPath = AStar.GetPath (startPortal, endPortal);
	}
}