using System.Collections.Generic;
using UnityEngine;

namespace AdventureWorld.Prueba.Procedural
{
    public class Room
    {
        public int width;
        public int height;
        public Vector2Int position;
    }
  
    public class ProceduralRoomGenerator
    {
        public void createRooms()
        {
            List<Room> rooms = new List<Room>();
            int maxRooms = 10;
            int maxRoomSize = 10;
            int minRoomSize = 5;
            
            for (int i = 0; i < maxRooms; i++)
            {
                int width = Random.Range(minRoomSize, maxRoomSize);
                int height = Random.Range(minRoomSize, maxRoomSize);
                int x = Random.Range(0, 100);
                int y = Random.Range(0, 100);
                // verify if the room is not overlapping
                bool overlapping = false;
                foreach (Room room in rooms)
                {
                    if (room.position.x < x + width && room.position.x + room.width > x && room.position.y < y + height && room.position.y + room.height > y)
                    {
                        overlapping = true;
                        break;
                    }
                }

                if (!overlapping)
                {
                    Room newRoom = new Room();
                    newRoom.width = width;
                    newRoom.height = height;
                    newRoom.position = new Vector2Int(x, y);
                    rooms.Add(newRoom);
                }
            }
            
         
            
            
        }
    }
}