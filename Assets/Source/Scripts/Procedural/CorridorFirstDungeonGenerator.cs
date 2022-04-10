using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdventureWorld.Prueba.Procedural
{
    public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
    {
        [SerializeField] private int corridoLength = 14;
        [SerializeField] private int corridorCount = 5;

        [SerializeField] [Range(0.1f, 1)] private float roomPercent = 0.8f;

        protected override void RunProceduralGeneration()
        {
            CorridorFirstGeneration();
        }

        private void CorridorFirstGeneration()
        {
            var floorPositions = new HashSet<Vector2Int>();
            var potentialRoomsPositions = new HashSet<Vector2Int>();
            CreateCorridors(floorPositions, potentialRoomsPositions);

            var rooms = CreateRooms(potentialRoomsPositions);
            
            List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);
            CreateRoomsAtDeadEnd(deadEnds, rooms);
            floorPositions.UnionWith(rooms);
            tilemapVisualizer.PaintFloorTile(floorPositions);
            WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
        }

        private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
        {
            var deadEnds = new List<Vector2Int>();
            foreach (var position in floorPositions)
            {
                var neighboursCount = 0;
                foreach (var direction in Direction2D.cardinalDirectionsList)
                {
                    if(floorPositions.Contains(position + direction))
                    {
                        neighboursCount++;
                    }
                }
                if(neighboursCount == 1)
                {
                    deadEnds.Add(position);
                }
            }

            return deadEnds;
        }

        private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomsPositions)
        {
            var rooms = new HashSet<Vector2Int>();
            int roomToCreateCount = Mathf.RoundToInt(potentialRoomsPositions.Count * roomPercent);
            
            var roomToCreate = potentialRoomsPositions.OrderBy((i => Random.value)).Take(roomToCreateCount).ToList();
            foreach (var roomPosition in potentialRoomsPositions)
            {
                var floor = RunRandomWalk(randomWalkParameter, roomPosition);
                rooms.UnionWith(floor);
            }

            // tilemapVisualizer.PaintRoomTile(rooms);
            return rooms;
        }
        
        private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> rooms)
        {
            foreach (var deadEnd in deadEnds)
            {
                if (rooms.Contains(deadEnd)) continue;
                var floor = RunRandomWalk(randomWalkParameter, deadEnd);
                rooms.UnionWith(floor);
            }
        }

        private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomsPositions)
        {
            var currentPosition = startPosition;
            potentialRoomsPositions.Add(currentPosition);
            for (int i = 0; i < corridorCount; i++)
            {
                var corridor = ProceduralGenerationAlgorithm.RandomWalkCorridor(currentPosition, corridoLength);
                currentPosition = corridor[corridor.Count - 1];
                potentialRoomsPositions.Add(currentPosition);
                floorPositions.UnionWith(corridor);
            }
        }
    }
}