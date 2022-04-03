using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdventureWorld.Prueba
{
    
    class ItemDatabase
    {
        public static readonly string ITEMS_PATH = "Assets/Resources/Items";
        public List<Item> items;
        
        public  Item GetItem(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            
            return items.FirstOrDefault(item => item.ID == id);
        }
        public void init()
        {
            //Load all items
            items = Resources.LoadAll<Item>("Items").ToList();
        }

    }
}