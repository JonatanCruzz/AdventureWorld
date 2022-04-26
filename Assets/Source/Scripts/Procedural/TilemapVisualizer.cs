using System;
using System.Collections;
using System.Collections.Generic;
using AdventureWorld.Prueba.Procedural;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTopTile, wallBottomTile, wallLeftTile, wallRightTile, wallFullTile;
    [SerializeField] private TileBase wallTopLeftTile, wallTopRightTile, wallBottomLeftTile, wallBottomRightTile;
    [SerializeField] private TileBase wallTopLeftCornerTile, wallTopRightCornerTile, wallBottomLeftCornerTile, wallBottomRightCornerTile;
    [SerializeField] private bool paintFloorbelowWalls = true;

    public void PaintFloorTile(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSigleTile(tilemap, position, tile);
        }
    }

    private void PaintSigleTile(Tilemap tilemap, Vector2Int position, TileBase tile)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int) position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    public void PaintSingleBasicWall(Vector2Int position, string neightboursBinary)
    {
        if(paintFloorbelowWalls)
            PaintSigleTile(floorTilemap, position, floorTile);
        int typeAsInt = Convert.ToInt32(neightboursBinary, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
            tile = wallTopTile;
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
            tile = wallBottomTile;
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
            tile = wallLeftTile;
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
            tile = wallRightTile;
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            if (WallTypesHelper.wallTopLeftInnerCorner.Contains(typeAsInt))
            {
                tile = wallTopLeftCornerTile;
            }
            else
            {
                tile = wallFullTile;
                Debug.Log("full: " + position + "binary: " + neightboursBinary);
       
            }

        }

        //TODO: wallFull, and wallCorner
        if (tile != null)
            PaintSigleTile(wallTilemap, position, tile);
        else
        {
            Debug.Log("no tile for: " + position + "binary: " + neightboursBinary);
        }
    }

    public void PaintSingleCornerWall(Vector2Int position, string neighboursBinaryType)
    {
        int typeAsInt = Convert.ToInt32(neighboursBinaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
            tile = wallTopLeftTile;
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
            tile = wallTopRightTile;
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
            tile = wallBottomLeftTile;
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
            tile = wallBottomRightTile;

        if (tile != null)
            PaintSigleTile(wallTilemap, position, tile);
    }
}