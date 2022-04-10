using System.Collections.Generic;
using Noise;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AdventureWorld.Prueba.Procedural
{
    public class PerlinNoiseGenerator : MonoBehaviour
    {
        public Tilemap baseTilemap;
        public List<Tilemap> tilemaps;
        public TileBase[] tiles;
        public int width = 100;
        public int height = 100;
        public float scale = 1;
        public float offsetX = 100;
        public float offsetY = 100;
        public float perlinScale = 1;
        public int seed = 100;

        private void Start()
        {
        }

        [Button(ButtonSizes.Large)]
        public void Generate()
        {
            // 2d vector with the id of the tile
            var noiseMap = new int[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    noiseMap[x, y] = PerlinNoise(x, y);
                }
            }

            for (int i = 0; i < tiles.Length; i++)
            {
                var tile = tiles[i];
                // create a new tilemap instance
                if (tilemaps.Count <= i)
                {
                    tilemaps.Add(Instantiate(baseTilemap));
                    tilemaps[i].transform.parent = transform;
                }

                var tilemapInstance = tilemaps[i];
                tilemapInstance.ClearAllTiles();

                // filter the noise map to only show the tiles that have the id of the tile
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (noiseMap[x, y] == i)
                        {
                            tilemapInstance.SetTile(new Vector3Int(x, y, 0), tile);
                        }
                    }
                }
            }
        }

        private int PerlinNoise(int x, int y)
        {
            var perlin = OpenSimplex2S.Noise2_ImproveXAxis(seed, (x + offsetX) / scale, (y + offsetY) / scale);
            float clamp = Mathf.Clamp(perlin, 0, 1);
            var scaled = Mathf.FloorToInt(clamp * (tiles.Length));
            if (scaled == tiles.Length)
            {
                scaled--;
            }

            return scaled;
        }
    }
}