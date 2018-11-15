using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

	private int currency;
	public int Currency {
		get {
			return currency;
		}
		set {
			currency = value;
			currencyText.text = value.ToString () + " <color=lime>$</color>";
		}
	}

	[SerializeField]
	private Text currencyText;

	public TowerButton ActiveTowerButton {
		get;
		private set;
	}

	public ObjectPool Pool {
		get;
		private set;
	}

	private int waveCount = 0;
	[SerializeField]
	private Text waveText;
	public int WaveCount {
		get {
			return waveCount;
		}
		private set{
			waveCount = value;
			waveText.text = "Wave: " + waveCount.ToString ();
		}
	}

	[SerializeField]
	private GameObject waveButton;
	[SerializeField]
	private GameObject towerPanel;

	private List<Enemy> activeEnemies = new List<Enemy> ();

	public bool WaveActive{
		get {
			return activeEnemies.Count > 0;
		}
	}

	private int lifesLeft;
	[SerializeField]
	private Text lifesText;
	public int LifesLeft {
		get{
			return lifesLeft;
		}
		set{
			lifesLeft = value;
			if (lifesLeft <= 0) {
				lifesLeft = 0;
				GameOver ();
			}
			lifesText.text = "Lives: <color=red>" + lifesLeft.ToString () + "</color>";
		}
	}

	private bool gameOver = false;
	[SerializeField]
	private GameObject gameOverPanel;

	private void Awake(){
		Pool = GetComponent<ObjectPool> ();
	}

	// Use this for initialization
	void Start () {
		Currency = 10000;
		WaveCount = 0;
		LifesLeft = 10;
		gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Escape)) {
			HandleEscape ();
		}

		if (Input.GetMouseButtonDown (1)) {
			HandleRightClick ();
		}
	}

	private void HandleEscape() {
		if (ActiveTowerButton != null) {
			unpickTower ();
		}
	}

	private void HandleRightClick(){
		if(ActiveTowerButton != null ){
			unpickTower ();
		}
	}

	public void pickTower(TowerButton towerButton){
		if (Currency >= towerButton.Price && !WaveActive) {
			ActiveTowerButton = towerButton;
			Hover.Instance.Activate (towerButton.HoverSprite);
		}
	}

	public void unpickTower () {
		ActiveTowerButton = null;
		Hover.Instance.Deactivate ();
	}

	public void BuyTower(){
		if (Currency >= ActiveTowerButton.Price) {
			Currency -= ActiveTowerButton.Price;
		}

		if (!Input.GetKey(KeyCode.LeftShift)){
			ActiveTowerButton = null;
			Hover.Instance.Deactivate ();
		}
	}

	public void StartWave(){
		WaveCount++;
		waveButton.SetActive (false);
		towerPanel.SetActive (false);
		StartCoroutine (SpawnWave ());
		unpickTower ();
	}

	private IEnumerator SpawnWave(){
		LevelManager.Instance.GeneratePath ();

		for (int i = 0; i < WaveCount; i++) {
			Enemy newEnemey = Pool.getObject ("Crocodile").GetComponent<Enemy> ();
			newEnemey.Spawn ();
			activeEnemies.Add (newEnemey);

			yield return new WaitForSeconds (1.5f);
		}
	}

	public void removeEnemy(Enemy enemy){
		activeEnemies.Remove (enemy);
		if (!WaveActive && !gameOver) {
			waveButton.SetActive (true);
			towerPanel.SetActive (true);
		}
	}

	public void GameOver(){
		if (!gameOver) {
			gameOver = true;
			gameOverPanel.SetActive (true);
		}
	}

	public void Restart(){
		Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void Quit(){
		Application.Quit();
	}
}
