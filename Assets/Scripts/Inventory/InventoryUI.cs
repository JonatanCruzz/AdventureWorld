using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
enum InventoryUIState
{
    equipment,
    other,
    none
}
public class InventoryUI : MonoBehaviour
{
    public VisualTreeAsset inventorySlotPrefab;
    public Player player;


    private VisualElement m_Root;
    private InventoryWindowManager m_mainInventory;
    private InventoryWindowManager m_OtherIventoryWindow;
    private EquipmentInventoryUI m_equipmentWindow;
    private List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();
    public Action OnInventoryClose;

    private InventoryUIState state = InventoryUIState.none;

    private void display()
    {
        m_Root.style.display = DisplayStyle.Flex;
        player.enabled = false;
    }
    public void Show()
    {
        this.state = InventoryUIState.equipment;

        m_OtherIventoryWindow.Display = false;
        m_mainInventory.Display = true;
        m_equipmentWindow.Display = true;

        this.display();
    }
    public void OpenChest(Inventory chestInventory)
    {
        this.state = InventoryUIState.other;
        m_OtherIventoryWindow.Inventory = chestInventory;

        m_OtherIventoryWindow.Display = true;
        m_mainInventory.Display = true;
        m_equipmentWindow.Display = false;
        this.display();
    }

    public void Update()
    {
        if (this.player.enabled) return;

        // if user press Esc, hide the inventory
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_Root.style.display = DisplayStyle.None;
            player.enabled = true;
            this.state = InventoryUIState.none;
            OnInventoryClose?.Invoke();
        }
    }

    #region UI Logic 
    private void OnEnable()
    {
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_Root.style.display = DisplayStyle.None;

        m_mainInventory = new InventoryWindowManager(inventorySlotPrefab, m_Root.Q<VisualElement>("MainInventoryWindow"));
        m_OtherIventoryWindow = new InventoryWindowManager(inventorySlotPrefab, m_Root.Q<VisualElement>("OtherInventoryWindow"));
        m_equipmentWindow = new EquipmentInventoryUI(m_Root.Q<VisualElement>("EquipmentWindow"), player);

        m_mainInventory.Inventory = player.inventory;

        m_mainInventory.OnSlotClick += OnMainInventorySlotClick;
        m_OtherIventoryWindow.OnSlotClick += OnOtherInventorySlotClick;
    }

    private void OnMainInventorySlotClick(InventorySlotUI uiSlot)
    {

        var itemSlot = uiSlot.slot;
        if (itemSlot.item != null)
        {
            switch (this.state)
            {
                case InventoryUIState.equipment:
                    switch (itemSlot.item)
                    {
                        case EquipableItem equipable:
                            var output = player.equipment.AddItem(equipable);
                            itemSlot.item = output;
                            itemSlot.numberOfItem = output == null ? 0 : 1;
                            break;
                        case ConsumableItem consumable:
                            itemSlot.item.AddBuff(player);

                            itemSlot.numberOfItem--;
                            if (itemSlot.numberOfItem <= 0)
                            {
                                itemSlot.item = null;
                            }

                            break;
                    }
                    break;
                case InventoryUIState.other:
                    m_OtherIventoryWindow.Inventory.TryMoveToInv(itemSlot);
                    break;
            }


        }
    }

    private void OnOtherInventorySlotClick(InventorySlotUI uiSlot)
    {
        var itemSlot = uiSlot.slot;
        if (itemSlot.item != null)
        {
            player.inventory.TryMoveToInv(itemSlot);
        }
    }


    #endregion
}

