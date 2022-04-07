using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPosition = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        var cornerWallPosition = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionsList);
        CreateBasicWall(tilemapVisualizer, basicWallPosition, floorPositions);
        CreateCornerWall(tilemapVisualizer, cornerWallPosition, floorPositions);
    }

    private static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPosition,
        HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in basicWallPosition)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(position + direction))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }

            tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinaryType);
        }
    }
    private static void CreateCornerWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPosition,
        HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in cornerWallPosition)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                if (floorPositions.Contains(position + direction))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }

            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryType);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions,
        List<Vector2Int> directionsList)
    {
        var wallPositions = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            foreach (var direction in directionsList)
            {
                var neightbourPosition = position + direction;
                if (!floorPositions.Contains(neightbourPosition))
                {
                    wallPositions.Add(neightbourPosition);
                }
            }
        }

        return wallPositions;
    }
}