using UnityEngine;

namespace AdventureWorld.Prueba
{
    class ItemDatabaseBehaviour: MonoBehaviour
    {
        private ItemDatabase _database;      
        public static ItemDatabaseBehaviour instance;
        public Item GetItem(string id) => _database.GetItem(id);
        private void Awake()
        {
            if(instance != null && instance != this)
            {
                Destroy(this);
                return;
            } 
            instance = this;
            DontDestroyOnLoad(this);
            _database = new ItemDatabase();
            _database.init();

        }

       
    }
}