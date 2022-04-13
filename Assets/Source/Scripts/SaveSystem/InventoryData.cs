using System.Collections.Generic;
using System.Linq;

namespace AdventureWorld.Prueba
{
    [System.Serializable]
    internal struct InventoryData
    {
        public List<InventorySlotData> items;
        public int numberOfKey;
        public int maxSlots;

        public InventoryData(Inventory inventory)
        {
            items = inventory.items.Select(item => new InventorySlotData()
            {
                Amount = item.numberOfItem,
                Item = item.item != null ? item.item.ID : ""
            }).ToList();
            numberOfKey = inventory.numberOfKey;
            maxSlots = inventory.maxSlots;
        }
        
        public void toPlayerInventory(Inventory inventory)
        {
            inventory.items = items.Select(item => new InventorySlot()
            {
                item = item.Item != "" ? ItemDatabaseBehaviour.instance.GetItem(item.Item) : null,
                numberOfItem = item.Amount
            }).ToList();
            inventory.numberOfKey = numberOfKey;
            inventory.maxSlots = maxSlots;
        }
    }
}