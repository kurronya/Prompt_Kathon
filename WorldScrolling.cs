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

    // ⭐ THÊM PHẦN NÀY
    [Header("Tile Pattern Settings")]
    [SerializeField] Sprite[] tileSprites; // Array 9 sprites theo thứ tự của bạn
    [SerializeField] int patternWidth = 3;  // Độ rộng pattern (3x3)
    [SerializeField] int patternHeight = 3; // Độ cao pattern (3x3)

    private void Awake()
    {
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
                    // Vị trí world của tile này
                    int worldX = playerTilePosition.x + pov_x;
                    int worldY = playerTilePosition.y + pov_y;

                    // Dịch chuyển tile
                    tile.transform.position = CalculateTilePosition(worldX, worldY);

                    // ⭐ CẬP NHẬT SPRITE THEO PATTERN
                    UpdateTileSprite(tile, worldX, worldY);
                }
            }
        }
    }

    // ⭐ HÀM MỚI: Cập nhật sprite dựa trên vị trí world
    private void UpdateTileSprite(GameObject tile, int worldX, int worldY)
    {
        if (tileSprites == null || tileSprites.Length == 0)
        {
            Debug.LogWarning("Tile sprites array is empty!");
            return;
        }

        // Tính vị trí trong pattern (0-2 cho cả X và Y)
        int patternX = Mod(worldX, patternWidth);
        int patternY = Mod(worldY, patternHeight);

        // Tính index trong mảng sprite theo thứ tự của bạn:
        // 0/2(6) 1/2(7) 2/2(8)
        // 0/1(3) 1/1(4) 2/1(5)
        // 0/0(0) 1/0(1) 2/0(2)
        int spriteIndex = patternY * patternWidth + patternX;

        // Đảm bảo index hợp lệ
        spriteIndex = Mathf.Clamp(spriteIndex, 0, tileSprites.Length - 1);

        // Cập nhật sprite
        SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = tileSprites[spriteIndex];
        }
        else
        {
            Debug.LogError($"Tile {tile.name} doesn't have SpriteRenderer component!");
        }
    }

    // ⭐ Hàm Modulo đúng cho số âm (C# modulo không hoạt động đúng với số âm)
    private int Mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }

    private Vector3 CalculateTilePosition(int x, int y)
    {
        return new Vector3(x * tileSize, y * tileSize, 0f);
    }

    private int CalculatePositionOnAxis(int currentValue, bool horizontal)
    {
        int tileCount = horizontal ? terrainTileHorizontalCount : terrainTileVerticalCount;

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