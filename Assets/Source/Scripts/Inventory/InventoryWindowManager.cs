using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class InventoryWindowManager
{
    public VisualElement m_InventoryWindow;
    private VisualElement m_SlotContainer;
    private Inventory inventory;
    private List<InventorySlotUI> slots = new List<InventorySlotUI>();

    public bool Display
    {
        get => m_InventoryWindow.style.display == DisplayStyle.Flex;
        set => m_InventoryWindow.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;

    }

    public Inventory Inventory
    {
        get => inventory;
        set
        {
            if (inventory != value)
            {
                inventory = value;
                this.updateInventory();
            }
        }
    }

    private void updateInventory()
    {

        var playerInventory = this.inventory;
        if (playerInventory == null)
        {
            //clear everything
            this.slots.Clear();
            m_SlotContainer.Clear();
            return;
        }
        if (playerInventory.items == null)
        {
            playerInventory.items = new List<InventorySlot>();
        }
        if (playerInventory.items.Count < playerInventory.maxSlots)
        {
            var diff = playerInventory.maxSlots - playerInventory.items.Count;
            for (int i = 0; i < diff; i++)
            {
                playerInventory.items.Add(new InventorySlot(null, 0));
            }
        }

        for (int i = 0; i < playerInventory.maxSlots; i++)
        {
            if (playerInventory.items[i] == null)
                playerInventory.items[i] = new InventorySlot(null, 0);
            // m_Root.b
            var slot = new InventorySlotUI(this.m_inventorySlotPrefab.CloneTree(), playerInventory.items[i]);
            // m_Root.On
            slots.Add(slot);
            slot.m_SlotContainer.RegisterCallback<MouseDownEvent>(x =>
            {
                OnSlotClick?.Invoke(slot);
            });
            m_SlotContainer.Add(slot.m_SlotContainer);
        }

    }
    public event System.Action<InventorySlotUI> OnSlotClick;

    public InventoryWindowManager(VisualTreeAsset inventorySlotPrefab, VisualElement InventoryWindow)
    {
        this.m_InventoryWindow = InventoryWindow;
        this.m_inventorySlotPrefab = inventorySlotPrefab;
        m_SlotContainer = m_InventoryWindow.Q<VisualElement>("SlotContainer");

    }
    public InventoryWindowManager(VisualTreeAsset inventorySlotPrefab, VisualElement InventoryWindow, Inventory inventory) : this(inventorySlotPrefab, InventoryWindow)
    {
        this.inventory = inventory;
    }
    private VisualTreeAsset m_inventorySlotPrefab;
}