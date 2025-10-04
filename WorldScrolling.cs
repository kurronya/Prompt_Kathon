using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScrolling : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    Vector2Int currentTilePosition = new Vector2Int(0, 0);
    [SerializeField] Vector2Int playerTilePosition;
    Vector2Int onTilegridPlayerPosition;
    [SerializeField] float tileSize = 20f;
    GameObject[,] terrainTiles;
    [SerializeField] int terrainTileHorizontalCount = 10;
    [SerializeField] int terrainTileVerticalCount = 10;
    [SerializeField] int fieldOfVisionHeight = 3;
    [SerializeField] int fieldOfVisionWidth = 3;

    private void Awake()
    {
        // Kiểm tra giá trị hợp lệ
        if (terrainTileHorizontalCount <= 0 || terrainTileVerticalCount <= 0)
        {
            Debug.LogError("Terrain tile counts must be greater than 0! Setting default values.");
            terrainTileHorizontalCount = Mathf.Max(1, terrainTileHorizontalCount);
            terrainTileVerticalCount = Mathf.Max(1, terrainTileVerticalCount);
        }

        terrainTiles = new GameObject[terrainTileHorizontalCount, terrainTileVerticalCount];
    }

    private void Start()
    {
        UpdateTilesOnScreen();
    }

    private void Update()
    {
        // Kiểm tra tileSize để tránh chia cho 0
        if (tileSize == 0)
        {
            Debug.LogError("Tile size cannot be 0!");
            return;
        }

        playerTilePosition.x = (int)(playerTransform.position.x / tileSize);
        playerTilePosition.y = (int)(playerTransform.position.y / tileSize);
        playerTilePosition.x -= (playerTransform.position.x < 0) ? 1 : 0;
        playerTilePosition.y -= (playerTransform.position.y < 0) ? 1 : 0;

        if (currentTilePosition != playerTilePosition)
        {
            currentTilePosition = playerTilePosition;
            onTilegridPlayerPosition.x = CalculatePositionOnAxis(playerTilePosition.x, true);
            onTilegridPlayerPosition.y = CalculatePositionOnAxis(playerTilePosition.y, false);
            UpdateTilesOnScreen();
        }
    }

    private void UpdateTilesOnScreen()
    {
        for (int pov_x = -(fieldOfVisionWidth / 2); pov_x <= fieldOfVisionWidth / 2; pov_x++)
        {
            for (int pov_y = -(fieldOfVisionHeight / 2); pov_y <= fieldOfVisionHeight / 2; pov_y++)
            {
                int tileToUpdate_x = CalculatePositionOnAxis(playerTilePosition.x + pov_x, true);
                int tileToUpdate_y = CalculatePositionOnAxis(playerTilePosition.y + pov_y, false);

                GameObject tile = terrainTiles[tileToUpdate_x, tileToUpdate_y];

                if (tile != null)
                {
                    tile.transform.position = CalculateTilePosition(
                        playerTilePosition.x + pov_x,
                        playerTilePosition.y + pov_y
                    );
                }
            }
        }
    }

    private Vector3 CalculateTilePosition(int x, int y)
    {
        return new Vector3(x * tileSize, y * tileSize, 0f);
    }

    private int CalculatePositionOnAxis(int currentValue, bool horizontal)
    {
        int tileCount = horizontal ? terrainTileHorizontalCount : terrainTileVerticalCount;

        // Kiểm tra để tránh chia cho 0
        if (tileCount <= 0)
        {
            Debug.LogError($"Tile count must be greater than 0! Current value: {tileCount}");
            return 0;
        }

        if (currentValue >= 0)
        {
            currentValue = currentValue % tileCount;
        }
        else
        {
            currentValue += 1;
            currentValue = tileCount - 1 + currentValue % tileCount;
        }

        return currentValue;
    }

    public void Add(GameObject tileGameObject, Vector2Int tilePosition)
    {
        // Kiểm tra tọa độ hợp lệ trước khi thêm
        if (tilePosition.x >= 0 && tilePosition.x < terrainTileHorizontalCount &&
            tilePosition.y >= 0 && tilePosition.y < terrainTileVerticalCount)
        {
            terrainTiles[tilePosition.x, tilePosition.y] = tileGameObject;
        }
        else
        {
            Debug.LogError($"Invalid tile position: ({tilePosition.x}, {tilePosition.y})");
        }
    }
}