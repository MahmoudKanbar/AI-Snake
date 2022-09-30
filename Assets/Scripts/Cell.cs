using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType { Empty, Block, Snake, Food }

public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite empty;
    [SerializeField] private Sprite block;
	[SerializeField] private Sprite snake;
    [SerializeField] private Sprite food;

	public CellType type;
    public Vector3Int gridPosition;
    public Cell[] neighbors;

    public Cell[] GetEmptyNeighbors()
    {
        var emptyNeighbors = new List<Cell>(4);
        for (int  i = 0; i < neighbors.Length; i++)
        {
            var neighbor = neighbors[i];
            if (neighbor != null && neighbor.type == CellType.Empty)
                emptyNeighbors.Add(neighbor);
        }
        return emptyNeighbors.ToArray();
    }

    public void ChangeTypeTo(CellType cellType)
    {
        type = cellType;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (type == CellType.Empty) spriteRenderer.sprite = empty;
        if (type == CellType.Block) spriteRenderer.sprite = block;
		if (type == CellType.Snake) spriteRenderer.sprite = snake;
        if (type == CellType.Food) spriteRenderer.sprite = food;
	}
}
