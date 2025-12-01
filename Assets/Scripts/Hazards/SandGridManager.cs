using System.Collections.Generic;
using UnityEngine;

public class SandGridManager : MonoBehaviour
{
    public GameObject sandTilePrefab;
    public float cellSize = 0.2f;

    private HashSet<Vector2Int> filledCells = new HashSet<Vector2Int>();

    public void AddSandAt(Vector3 worldPos)
    {
        Vector2Int cell = WorldToCell(worldPos);

        if (filledCells.Contains(cell))
            return;

        filledCells.Add(cell);

        Vector3 spawnPos = CellToWorld(cell);
        Instantiate(sandTilePrefab, spawnPos, Quaternion.identity);
    }

    private Vector2Int WorldToCell(Vector3 pos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(pos.x / cellSize),
            Mathf.FloorToInt(pos.y / cellSize)
        );
    }

    private Vector3 CellToWorld(Vector2Int cell)
    {
        return new Vector3(
            cell.x * cellSize + cellSize / 2f,
            cell.y * cellSize + cellSize / 2f,
            0
        );
    }
}
