using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
	private float lastStepTime;
	private Cell source, destination;
	private List<Cell> body = new List<Cell>();


	public static Snake Instance { get; private set; }
	public Snake()
	{
		if (Instance == null) Instance = this;
		else throw new System.Exception("There is already a Snake object");
	}


	public float StepDeltaTime => Manager.Instance.StepDeltaTime;
	public SpriteRenderer Marker => Manager.Instance.Marker;
	public bool UseMarker => Manager.Instance.UseMarker;


	public void AddNewCellToBody()
	{
		var latestCellEmptyNeighbors = body[body.Count - 1].GetEmptyNeighbors();
		var randomIndex = Random.Range(0, latestCellEmptyNeighbors.Length);
		var newCell = latestCellEmptyNeighbors[randomIndex];
		newCell.ChangeTypeTo(CellType.Snake);
		body.Add(newCell);
	}

	public void InitiateAt(Cell cell)
	{
		source = cell;
		cell.ChangeTypeTo(CellType.Snake);
		body.Add(cell);
	}

	public void ResetAll()
	{
		lastStepTime = 0;
		source = destination = null;
		body = new List<Cell>();
		Marker.transform.position = Vector3.one * 10;
	}

	public void MoveOneStep(Vector3Int direction)
	{
		var oldCellPosition = body[0].gridPosition;
		var nextCellPosition = oldCellPosition + direction;

		for (int i = 0; i < body.Count; i++)
		{
			oldCellPosition = body[i].gridPosition;
			body[i].ChangeTypeTo(CellType.Empty);
			body[i] = Grid.Instance.GetCellAt(nextCellPosition);
			body[i].ChangeTypeTo(CellType.Snake);
			nextCellPosition = oldCellPosition;
		}

		source = body[0];
	}

	public void SetNewRandomDestination()
	{
		destination = Grid.Instance.GetRandomFoodCell(destination);
		if (destination == null) destination = Grid.Instance.GetRandomEmptyCell(destination);
		if (UseMarker) Marker.transform.position = destination.transform.position;
		else Marker.transform.position = Vector3.one * 1e10f;
	}

	private void Update()
	{
		if (Time.time - lastStepTime >= StepDeltaTime)
		{
			var path = Grid.Instance.GetShortestPath(source, destination, Heuristics.GetEuclideanDistance);
			if (path == null || destination == path[0])
			{
				SetNewRandomDestination();
				return;
			}
			var nextCell = path[1];
			var direction = nextCell.gridPosition - source.gridPosition;
			if (nextCell.type == CellType.Food) AddNewCellToBody();
			MoveOneStep(direction);
			lastStepTime = Time.time;
		}
	}
}
