using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour {

	[SerializeField]
	private GameObject towerPrefab;

	[SerializeField]
	private Sprite hoverSprite;

	[SerializeField]
	private int price;
	public int Price{ 
		get{
			return price;
		}
	}

	[SerializeField]
	private Text priceText;

	private void Start(){
		priceText.text = Price.ToString () + " <color=lime>$</color>";
	}

	public GameObject TowerPrefab {
		get{
			return towerPrefab;
		}
	}

	public Sprite HoverSprite {
		get {
			return hoverSprite;
		}
	}
}
