using System.Linq;
using UnityEngine;

namespace AdventureWorld.Prueba
{
    class ItemDatabase: MonoBehaviour
    {
        public Item[] items;
        
        public  Item GetItem(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            
            return items.FirstOrDefault(item => item.ID == id);
        }
        
        public static ItemDatabase instance;
        private void Awake()
        {
            if(instance != null && instance != this)
            {
                Destroy(this);
                return;
            } 
            instance = this;
            DontDestroyOnLoad(this);
            this.init();

        }

        private void init()
        {
            //Load all items
            items = Resources.LoadAll<Item>("Items");
        }
    }
}