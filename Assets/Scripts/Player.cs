using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var hit = Physics2D.Raycast(ray.origin, ray.direction, 20.0f);

			if (hit.collider == null) return;
			var cell = hit.collider.GetComponent<Cell>();
			if (cell == null) return;

			switch (cell.type)
			{
				case CellType.Empty:
					cell.ChangeTypeTo(CellType.Block);
					break;
				case CellType.Block:
					cell.ChangeTypeTo(CellType.Food);
					break;
				case CellType.Food:
					cell.ChangeTypeTo(CellType.Empty);
					break;
			}
		}
	}
}
