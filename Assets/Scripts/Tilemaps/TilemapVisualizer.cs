using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using Pixeye.Unity;


public class TilemapVisualizer : MonoBehaviour
{
    [Foldout("Tilemaps", true)]
    [SerializeField] Tilemap floorTilemap;
    [SerializeField] Tilemap wallTilemap;
    [SerializeField] Tilemap wallTilemapTop;
    [SerializeField] Tilemap wallTilemapBottom;
    [SerializeField] Tilemap wallTilemapLeft;
    [SerializeField] Tilemap wallTilemapRight;
    [SerializeField] Tilemap corridorTilemap;

    [Foldout("Tiles to use", true)]
    [SerializeField] TileBase[] floorTile;
    [SerializeField] TileBase[] corridorTile;
    [SerializeField] TileBase[] wallTopArray;
    [SerializeField] TileBase[] wallBottomArray;

    [SerializeField] TileBase wallTop;
    [SerializeField] TileBase wallSideRight;
    [SerializeField] TileBase wallSideLeft;
    [SerializeField] TileBase wallBottom;
    [SerializeField] TileBase wallFull;
    [SerializeField] TileBase wallInnerCornerDownLeft;
    [SerializeField] TileBase wallInnerCornerDownRight;
    [SerializeField] TileBase wallDiagonalCornerDownRight;
    [SerializeField] TileBase wallDiagonalCornerDownLeft;
    [SerializeField] TileBase wallDiagonalCornerUpRight;
    [SerializeField] TileBase wallDiagonalCornerUpLeft;

    public void PaintFloorTilesRandomly(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTilesRandomly(floorPositions, floorTilemap, floorTile);
    }

    public void PaintCorridorTiles(IEnumerable<Vector2Int> corridorPositions)
    {
        PaintTilesRandomly(corridorPositions, corridorTilemap, corridorTile);
    }

    private void PaintTilesRandomly(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase[] tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTileRandomly(tilemap, tile, position);
        }
    }

    internal void PaintTopWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTopArray[Random.Range(0, wallTopArray.Length)];
        }
        if (tile != null)
        {
            PaintSingleTile(wallTilemapTop, tile, position);
        }
    }

    internal void PaintBottomWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottomArray[Random.Range(0, wallBottomArray.Length)];
        }
        if (tile != null)
            PaintSingleTile(wallTilemapBottom, tile, position);
    }
    internal void PaintLeftWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        if (tile != null)
            PaintSingleTile(wallTilemapLeft, tile, position);
    }
    internal void PaintRightWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        if (tile != null)
            PaintSingleTile(wallTilemapRight, tile, position);
    }

    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }

        if (tile != null)
            PaintSingleTile(wallTilemap, tile, position);
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    private void PaintSingleTileRandomly(Tilemap tilemap, TileBase[] tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        for (int i = 0; i < tile.Length; i++)
        {
            tilemap.SetTile(tilePosition, tile[Random.Range(0, tile.Length)]);
        }
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeASInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeASInt))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeASInt))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeASInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeASInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeASInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeASInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeASInt))
        {
            tile = wallFull;
        }
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeASInt))
        {
            tile = wallBottom;
        }

        if (tile != null)
            PaintSingleTile(wallTilemap, tile, position);
    }

    public void Clear()
    {
        corridorTilemap.ClearAllTiles();
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        wallTilemapTop.ClearAllTiles();
        wallTilemapBottom.ClearAllTiles();
        wallTilemapLeft.ClearAllTiles();
        wallTilemapRight.ClearAllTiles();
    }
}
