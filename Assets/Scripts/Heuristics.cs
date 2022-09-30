using UnityEngine;

public static class Heuristics
{
	public static float GetEuclideanDistance(Cell first, Cell second)
	{
		return (first.gridPosition - second.gridPosition).sqrMagnitude;
	}

	public static float GetManhattanDistance(Cell first, Cell second)
	{
		return Mathf.Abs(first.gridPosition.x - second.gridPosition.x) +
			Mathf.Abs(first.gridPosition.y - second.gridPosition.y);
	}
}
