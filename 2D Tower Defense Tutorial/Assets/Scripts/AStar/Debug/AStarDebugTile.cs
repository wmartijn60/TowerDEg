using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AStarDebugTile : MonoBehaviour {

	[SerializeField]
	private Text gValueText;
	public Text GValueText {
		get { return gValueText; }
		set { gValueText = value; }
	}

	[SerializeField]
	private Text hValueText;
	public Text HValueText {
		get { return hValueText; }
		set { hValueText = value; }
	}

	[SerializeField]
	private Text fValueText;
	public Text FValueText {
		get { return fValueText; }
		set { fValueText = value; }
	}
}
