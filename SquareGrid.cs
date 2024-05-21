using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGrid : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePf;
    [SerializeField] private Transform camPos;

    private Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();
    private int tileCount;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        tileCount = (width * height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePf, new Vector3(x,y), Quaternion.identity);
                spawnedTile.name = $"tile {x} {y}";

                bool isOffset = (x % 2 != y % 2);
                spawnedTile.Init(isOffset);
                spawnedTile.SetPos(new Vector2(x,y));

                tiles[spawnedTile.GetPos()] = spawnedTile;
            }
        }

        GameAssets.i.unitManager.SetBoard();
        camPos.transform.position = new Vector3((float)width/2 - 0.5f, (float)height/2 - 0.5f, -10);
    }

    public Tile GetTileAtPos(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }

        return null;
    }

    public int GetTileCount()
    {
        return tileCount;
    }

    public Dictionary<Vector2, Tile> GetDictionary()
    {
        return tiles;
    }

    public Vector2 GetGridSize()
    {
        return new Vector2(width,height);
    }
}
