using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public int width = 20;
    public int height = 20;
    public GameObject gridCellPrefab;

    private GameObject[,] grid;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(x, y); // Use Vector2 for 2D grid
                GameObject gridCell = Instantiate(gridCellPrefab, new Vector2(position.x, position.y), Quaternion.identity); 
                gridCell.transform.parent = transform;
                grid[x, y] = gridCell;
            }
        }
    }

    public void UpdateCell(int x, int y, Color color)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            grid[x, y].GetComponent<Renderer>().material.color = color;
        }
    }
}