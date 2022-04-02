using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
public class EquipmentInventoryUI
{
    public bool Display
    {
        set => m_Root.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;

        get => m_Root.style.display == DisplayStyle.Flex;
    }
    public Dictionary<EquipableItemSlotType, EquipmentSlot> equipmentSlots = new Dictionary<EquipableItemSlotType, EquipmentSlot>();
    public Player player;
    private VisualElement m_Root;
    public EquipmentInventoryUI(VisualElement m_Root, Player player)
    {
        this.m_Root = m_Root;
        this.player = player;

        equipmentSlots.Add(EquipableItemSlotType.Head, m_Root.Q<EquipmentSlot>("HeadSlot"));
        equipmentSlots.Add(EquipableItemSlotType.Chest, m_Root.Q<EquipmentSlot>("ArmorSlot"));
        equipmentSlots.Add(EquipableItemSlotType.Feet, m_Root.Q<EquipmentSlot>("BootSlot"));
        equipmentSlots.Add(EquipableItemSlotType.MainHand, m_Root.Q<EquipmentSlot>("MainHandSlot"));
        equipmentSlots.Add(EquipableItemSlotType.OffHand, m_Root.Q<EquipmentSlot>("OffHandSlot"));

        foreach (var slot in equipmentSlots)
        {
            slot.Value.OnClicked += OnEquipmentSlotClick;
        }
        m_Root.Q<EquipmentSlot>("HeadSlot").item = player.equipment.Head;
        m_Root.Q<EquipmentSlot>("ArmorSlot").item = player.equipment.Chest;
        m_Root.Q<EquipmentSlot>("BootSlot").item = player.equipment.Feet;
        m_Root.Q<EquipmentSlot>("MainHandSlot").item = player.equipment.MainHand;
        m_Root.Q<EquipmentSlot>("OffHandSlot").item = player.equipment.OffHand;

        player.equipment.PropertyChanged += OnEquipmentChanged;

    }

    public void refreshInventory()
    {
    }
    private void OnEquipmentChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(player.equipment.Head):
                equipmentSlots[EquipableItemSlotType.Head].item = player.equipment.Head;
                break;
            case nameof(player.equipment.Chest):
                equipmentSlots[EquipableItemSlotType.Chest].item = player.equipment.Chest;
                break;
            case nameof(player.equipment.Feet):
                equipmentSlots[EquipableItemSlotType.Feet].item = player.equipment.Feet;
                break;
            case nameof(player.equipment.MainHand):
                equipmentSlots[EquipableItemSlotType.MainHand].item = player.equipment.MainHand;
                break;
            case nameof(player.equipment.OffHand):
                equipmentSlots[EquipableItemSlotType.OffHand].item = player.equipment.OffHand;
                break;

        }
    }

    private void OnEquipmentSlotClick(EquipmentSlot slot)
    {
        Debug.Log("Equipment slot clicked " + slot.slotType);
        var item = slot.item;
        if (item == null) return;
        if (this.player.inventory.AddItem(item, 1) == 0)
        {
            switch (slot.slotType)
            {
                case EquipableItemSlotType.Head:
                    this.player.equipment.Head = null;
                    break;
                case EquipableItemSlotType.Chest:
                    this.player.equipment.Chest = null;
                    break;
                case EquipableItemSlotType.Feet:
                    this.player.equipment.Feet = null;
                    break;
                case EquipableItemSlotType.MainHand:
                    this.player.equipment.MainHand = null;
                    break;
                case EquipableItemSlotType.OffHand:
                    this.player.equipment.OffHand = null;
                    break;
            }
        }


    }
}