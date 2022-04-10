using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace AdventureWorld.Prueba.Procedural
{
    public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
    {
        [SerializeField] protected SimpleRandomWalkSO randomWalkParameter;
        
        protected override void RunProceduralGeneration()
        {
            var floorPosition = RunRandomWalk(randomWalkParameter,startPosition);
            tilemapVisualizer.PaintFloorTile(floorPosition);
            WallGenerator.CreateWalls(floorPosition, tilemapVisualizer);
        }
        protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameter, Vector2Int position )
        {
            var currentPosition = position;
            var floorPositions = new HashSet<Vector2Int>();

            for (int i = 0; i < parameter.iterations; i++)
            {
                var path =
                    ProceduralGenerationAlgorithm.SimpleRandomWalk(currentPosition, parameter.walkLength);
                floorPositions.UnionWith(path);

                if (parameter.startRandomlyEveryIteration)
                    currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }

            return floorPositions;
        }

       
    }
}