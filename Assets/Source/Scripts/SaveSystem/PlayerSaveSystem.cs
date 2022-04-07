using System.Linq;
using PixelCrushers;
using UnityEngine;

namespace AdventureWorld.Prueba
{
    [RequireComponent(typeof(global::Player))]
    public class PlayerSaveSystem : Saver
    {
        private global::Player _player;


        public override string RecordData()
        {
            _player = GetComponent<global::Player>();

            var playerData = new PlayerData
            {
                hp = new HealthData(_player.hp),
                inventory = new InventoryData(_player.inventory),
                equipment = new EquipmentData(_player.equipment)
            };
            return SaveSystem.Serialize(playerData);
        }

        public override void ApplyData(string s)
        {
            Debug.Log("Apply Player Data: " + s);

            var playerData = SaveSystem.Deserialize<PlayerData>(s);
            if (playerData == null) return;
            _player = GetComponent<global::Player>();

            playerData.hp.toHealthUnit(_player.hp);
         
            playerData.inventory.toPlayerInventory(_player.inventory);
            playerData.equipment.toPlayerEquipment(_player.equipment);
            
            if(_player.hp.HP > _player.hp.Max_HP)
                _player.hp.HP = _player.hp.Max_HP;
            //TODO: _player.UpdateEquipment();
        }
    }
}