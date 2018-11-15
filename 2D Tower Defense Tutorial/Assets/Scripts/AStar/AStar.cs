using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar {

	private static Dictionary<Point, AStarNode> nodes;

	/// <summary>
	/// Creates the nodes on which we will perform the A* Algorithm.
	/// </summary>
	private static void CreateNodes(){
		nodes = new Dictionary<Point, AStarNode> ();

		foreach (TileScript tile in LevelManager.Instance.Tiles.Values) {
			nodes.Add (tile.GridPosition, new AStarNode(tile));
		}
	}

	public static Stack<AStarNode> GetPath(Point start, Point goal){
		if (nodes == null) CreateNodes ();

		HashSet<AStarNode> openList = new HashSet<AStarNode> ();
		HashSet<AStarNode> closedList = new HashSet<AStarNode> ();
		Stack<AStarNode> path = new Stack<AStarNode> ();

		AStarNode currentNode = nodes [start];
		openList.Add (currentNode);

		while (openList.Count > 0) {
			//add neighbouring nodes from starting node to the openlist
			for (int x = -1; x <= 1; x++) {
				for (int y = -1; y <= 1; y++) {
					Point neighbourPos = new Point (currentNode.gridPosition.X - x, currentNode.gridPosition.Y - y);
						
					//same as the node we look at?
					if (neighbourPos == currentNode.gridPosition)
						continue;
					//is a tile at the calculated neighbour position? (boundary check)
					if (!LevelManager.Instance.Tiles.ContainsKey (neighbourPos))
						continue; 
					//is the tile walkable?
					if (!LevelManager.Instance.Tiles [neighbourPos].IsWalkable)
						continue;

					AStarNode neighbour = nodes [neighbourPos];

					//calculate the costs for moving to the current neighbour
					int gCost = 0;
					if (Mathf.Abs (x - y) == 1) {
						gCost = 10;
					} else {
						if (!ConnectedDiagonally (currentNode, neighbour)) {
							continue;
						}
						gCost = 14;
					}

					if (openList.Contains (neighbour)) {
						if (currentNode.GScore + gCost < neighbour.GScore) {
							neighbour.CalcValues (currentNode, nodes [goal], gCost);	
						}	
					} else if (!closedList.Contains (neighbour)) {
						openList.Add (neighbour);
						neighbour.CalcValues (currentNode, nodes [goal], gCost);
					}
				}
			}

			openList.Remove (currentNode);
			closedList.Add (currentNode);

			if (openList.Count > 0) {
				currentNode = openList.OrderBy (n => n.FScore).First ();
			}

			if (currentNode == nodes [goal]) {
				path.Push (currentNode);
				while (currentNode.Parent != null) {
					path.Push (currentNode.Parent);
					currentNode = currentNode.Parent;
				}

				break;
			}
				
		}

		//Debug the Neighbours of the Starting Node
		//GameObject.Find ("Debug").GetComponent<AStarDebug> ().DebugPath (new HashSet<AStarNode> (openList), new HashSet<AStarNode> (closedList), path);

		return path;
	}

	private static bool ConnectedDiagonally(AStarNode currentNode, AStarNode neighbor){
		Point direction = neighbor.gridPosition - currentNode.gridPosition;

		Point first = new Point (currentNode.gridPosition.X + direction.X, currentNode.gridPosition.Y);
		Point second = new Point (currentNode.gridPosition.X, currentNode.gridPosition.Y + direction.Y);

		if (LevelManager.Instance.Tiles.ContainsKey (first) && !LevelManager.Instance.Tiles [first].IsWalkable) {
			return false;
		}
		if (LevelManager.Instance.Tiles.ContainsKey (second) && !LevelManager.Instance.Tiles [second].IsWalkable) {
			return false;
		}

		return true;
	}
}