using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GrassGridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tilemap grassTilemap;
    [SerializeField] private Tile[] grassTiles;

    public int Width => width;
    public int Height => height;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int randomNum = Random.Range(0, 2); // Only 3 tiles total
                Tile randomTile = grassTiles[randomNum];

                grassTilemap.SetTile(new Vector3Int(x, y, 0), randomTile);
            }
        }
    }
}
