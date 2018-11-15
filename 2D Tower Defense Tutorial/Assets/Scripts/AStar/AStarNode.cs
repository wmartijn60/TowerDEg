using UnityEngine;

public class AStarNode{
	public Point gridPosition { get; private set; }
	public Vector3 WorldPosition { get; private set; }
	public TileScript TileReference { get; private set; }
	public AStarNode Parent { get; private set;	}
	public int GScore { get; private set; }
	public int HScore { get; private set; }
	public int FScore { get; private set; }

	public AStarNode(TileScript tileReference){
		TileReference = tileReference;
		gridPosition = tileReference.GridPosition;
		WorldPosition = tileReference.WorldPosition;
	}

	public void CalcValues(AStarNode parent, AStarNode goal, int gScore){
		Parent = parent;

		GScore = parent.GScore + gScore;
		HScore = (Mathf.Abs (gridPosition.X - goal.gridPosition.X) + Mathf.Abs (gridPosition.Y - goal.gridPosition.Y)) * 10;

		FScore = GScore + HScore;
	}
}