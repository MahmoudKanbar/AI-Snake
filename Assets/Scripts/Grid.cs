using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


public partial class Grid : MonoBehaviour
{
    private Cell[,] cells;


    public static Grid Instance { get; private set; }
    public Grid()
    {
        if (Instance == null) Instance = this;
        else throw new System.Exception("There is already a Grid object");
    }


	private Vector3Int Size => Manager.Instance.Size;
	private Cell EmptyCellPrefab => Manager.Instance.EmptyCellPrefab;


    public void Initiate()
    {
        // setting camera position & size
        var camXSize = Size.x;
        var camYSize = Size.y * 1.5f;
        var camYPos = camYSize / 2.0f;
        var CamXPos = camXSize / 10.0f - 1.0f;
        Camera.main.orthographicSize = Mathf.Max(camXSize, camYSize);
        Camera.main.transform.position = Vector3.up * camYPos + Vector3.right * CamXPos;

        cells = new Cell[Size.x, Size.y];

        // generating cells
        var stepSize = new Vector3(2.5f, 2.5f, 0.0f);
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                cells[i, j] = Instantiate(EmptyCellPrefab, transform);
                cells[i, j].gridPosition = new Vector3Int(i, j, 0);
                cells[i, j].transform.position = new Vector3(i * stepSize.x, j * stepSize.y, 0) - Size / 2 + new Vector3(0.5f, 0.5f, 0.0f);
            }
        }

        // finding each cell neighbors
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                var up = j + 1 >= Size.y ? null : cells[i, j + 1];
                var right = i + 1 >= Size.x ? null : cells[i + 1, j];
                var down = j - 1 < 0 ? null : cells[i, j - 1];
                var left = i - 1 < 0 ? null : cells[i - 1, j];
                cells[i, j].neighbors = new Cell[] { up, right, down, left };
            }
        }

        // creating snake
        Snake.Instance.InitiateAt(GetRandomEmptyCell());
    }

	public void ResetAll()
	{
		if (cells == null) return;
		foreach (var cell in cells) Destroy(cell.gameObject);
		Snake.Instance.ResetAll();
	}

    public Cell GetRandomEmptyCell(Cell except = null)
    {
        var emptyCells = new List<Cell>();
        foreach (var cell in cells)
        {
            if (cell.type == CellType.Empty && cell != except)
                emptyCells.Add(cell);
        }
        if (emptyCells.Count == 0) return null;
        return emptyCells[Random.Range(0, emptyCells.Count)];
    }

    public Cell GetRandomFoodCell(Cell except = null)
    {
        var foodCells = new List<Cell>();
        foreach (var cell in cells)
        {
            if (cell.type == CellType.Food && cell != except)
                foodCells.Add(cell);
        }
        if (foodCells.Count == 0) return null;
        return foodCells[Random.Range(0, foodCells.Count)];
    }

    public Cell GetCellAt(int x, int y) => cells[x, y];

    public Cell GetCellAt(Vector3Int position) => cells[position.x, position.y];

    public Cell[] GetShortestPath(Cell source, Cell destination, System.Func<Cell, Cell, float> heuristic)
    {
        if (destination == null) return null;

        var openedSet = new List<CellHolder>();
        var bestHolders = new Dictionary<Vector3Int, CellHolder>();
        var current = new CellHolder(source, 0, 0);

        openedSet.Add(current);
        bestHolders.Add(current.GridPosition, current);

        while (openedSet.Count > 0)
        {
            current = openedSet[0];
            openedSet.RemoveAt(0);

            if (current.cell == destination)
                return current.path.ToArray();

            foreach (var neighbor in current.cell.neighbors)
            {
                if (neighbor == null) continue;
                if (neighbor.type != CellType.Empty && neighbor.type != CellType.Food) continue;

                var neighborPosition = neighbor.gridPosition;
                var distanceCost = current.distanceCost + 1;
                var heuristicCost = heuristic(neighbor, destination);
                var neighborHolder = new CellHolder(neighbor, distanceCost, heuristicCost, current);

                if (bestHolders.ContainsKey(neighborPosition))
                {
                    if (neighborHolder.totalCost < bestHolders[neighborPosition].totalCost)
                    {
                        openedSet.Add(neighborHolder);
                        bestHolders[neighborPosition] = neighborHolder;
                    }
                }
                else
                {
                    openedSet.Add(neighborHolder);
                    bestHolders.Add(neighborPosition, neighborHolder);
                }
            }

            openedSet.Sort();
        }

        return null;
    }
}
