using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public partial class Grid
{
	private class CellHolder : System.IComparable<CellHolder>
	{
		public Cell cell;
		public List<Cell> path;
		public float distanceCost;
		public float heuristicCost;
		public float totalCost;

		public CellHolder(Cell cell, float distanceCost, float heuristicCost)
		{
			this.cell = cell;
			this.distanceCost = distanceCost;
			this.heuristicCost = heuristicCost;
			path = new List<Cell>();
			path.Add(cell);
			totalCost = distanceCost + heuristicCost;
		}

		public CellHolder(Cell cell, float distanceCost, float heuristicCost, CellHolder parent)
		{
			this.cell = cell;
			this.distanceCost = distanceCost;
			this.heuristicCost = heuristicCost;
			path = new List<Cell>(parent.path);
			path.Add(cell);
			totalCost = distanceCost + heuristicCost;
		}

		public int CompareTo(CellHolder other) // we need this method for sorting
		{
			return (int)((totalCost - other.totalCost) * 1e3f);
		}

		public Vector3Int GridPosition => cell.gridPosition;
	}
}
