using System;

namespace AdventureWorld.Prueba
{
    [Serializable]
    internal struct EquipmentData
    {
        public string Head;
        public string Chest;
        public string Feet;
        public string MainHand;
        public string OffHand;

        public EquipmentData(EquipmentInventory equipment)
        {
            Head = equipment.Head != null ? equipment.Head.ID : "";
            Chest = equipment.Chest != null ? equipment.Chest.ID : "";
            Feet = equipment.Feet != null ? equipment.Feet.ID : "";
            MainHand = equipment.MainHand != null ? equipment.MainHand.ID : "";
            OffHand = equipment.OffHand != null ? equipment.OffHand.ID : "";
        }

        public void toPlayerEquipment(EquipmentInventory playerEquipment)
        {
            playerEquipment.Head =  (EquipableItem) ItemDatabaseBehaviour.instance.GetItem(Head);
            playerEquipment.Chest =  (EquipableItem) ItemDatabaseBehaviour.instance.GetItem(Chest);
            playerEquipment.Feet =  (EquipableItem) ItemDatabaseBehaviour.instance.GetItem(Feet);
            playerEquipment.MainHand =  (EquipableItem) ItemDatabaseBehaviour.instance.GetItem(MainHand);
            playerEquipment.OffHand =  (EquipableItem) ItemDatabaseBehaviour.instance.GetItem(OffHand);
        }
    }
}