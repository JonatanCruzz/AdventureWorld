using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class InventoryWindowManager
{
    public VisualElement m_InventoryWindow;
    private VisualElement m_SlotContainer;
    private Inventory inventory;
    private List<InventorySlotUI> slots = new List<InventorySlotUI>();

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
public class InventoryUI : MonoBehaviour
{
    public VisualTreeAsset inventorySlotPrefab;
    public Player player;


    private VisualElement m_Root;
    private InventoryWindowManager m_mainInventory;
    private InventoryWindowManager m_OtherIventoryWindow;
    private VisualElement m_equipmentWindow;
    private List<InventorySlotUI> slots = new List<InventorySlotUI>();
    private List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();
    private void display()
    {
        m_Root.style.display = DisplayStyle.Flex;
        player.enabled = false;
    }
    public void Show()
    {

        m_OtherIventoryWindow.m_InventoryWindow.style.display = DisplayStyle.None;
        m_mainInventory.m_InventoryWindow.style.display = DisplayStyle.Flex;
        m_equipmentWindow.style.display = DisplayStyle.Flex;

        this.display();
    }
    public void OpenChest(Inventory chestInventory)
    {
        m_OtherIventoryWindow.Inventory = chestInventory;

        m_OtherIventoryWindow.m_InventoryWindow.style.display = DisplayStyle.Flex;
        m_mainInventory.m_InventoryWindow.style.display = DisplayStyle.Flex;
        m_equipmentWindow.style.display = DisplayStyle.None;
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
        }
    }

    #region UI Logic 
    private void OnEnable()
    {
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_Root.style.display = DisplayStyle.None;
        m_mainInventory = new InventoryWindowManager(inventorySlotPrefab, m_Root.Q<VisualElement>("MainInventoryWindow"));
        m_OtherIventoryWindow = new InventoryWindowManager(inventorySlotPrefab, m_Root.Q<VisualElement>("OtherInventoryWindow"));
        m_equipmentWindow = m_Root.Q<VisualElement>("EquipmentWindow");
        m_mainInventory.Inventory = player.inventory;

        // m_Root.Q<EquipmentSlot>("HeadSlot");

        equipmentSlots.Add(m_Root.Q<EquipmentSlot>("HeadSlot"));
        equipmentSlots.Add(m_Root.Q<EquipmentSlot>("ArmorSlot"));
        equipmentSlots.Add(m_Root.Q<EquipmentSlot>("BootSlot"));
        equipmentSlots.Add(m_Root.Q<EquipmentSlot>("MainHandSlot"));
        equipmentSlots.Add(m_Root.Q<EquipmentSlot>("OffHandSlot"));

        foreach (var slot in equipmentSlots)
        {
            slot.OnClicked += OnEquipmentSlotClick;
        }

        // equipmentSlots[0].worldBound.

        m_Root.Q<EquipmentSlot>("HeadSlot").item = player.equipment.Head;
        m_Root.Q<EquipmentSlot>("ArmorSlot").item = player.equipment.Chest;
        m_Root.Q<EquipmentSlot>("BootSlot").item = player.equipment.Feet;
        m_Root.Q<EquipmentSlot>("MainHandSlot").item = player.equipment.MainHand;
        m_Root.Q<EquipmentSlot>("OffHandSlot").item = player.equipment.OffHand;


        player.equipment.PropertyChanged += OnEquipmentChanged;

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
    private void OnSlotClick(InventorySlotUI uiSlot)
    {

        var itemSlot = uiSlot.slot;
        if (itemSlot.item != null)
        {
            if (itemSlot.item is EquipableItem equipable)
            {
                var output = player.equipment.AddItem(equipable);
                itemSlot.item = output;
                itemSlot.numberOfItem = output == null ? 0 : 1;
            }

        }
    }

    private void OnEquipmentChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(player.equipment.Head):
                m_Root.Q<EquipmentSlot>("HeadSlot").item = player.equipment.Head;
                break;
            case nameof(player.equipment.Chest):
                m_Root.Q<EquipmentSlot>("ArmorSlot").item = player.equipment.Chest;
                break;
            case nameof(player.equipment.Feet):
                m_Root.Q<EquipmentSlot>("BootSlot").item = player.equipment.Feet;
                break;
            case nameof(player.equipment.MainHand):
                m_Root.Q<EquipmentSlot>("MainHandSlot").item = player.equipment.MainHand;
                break;
            case nameof(player.equipment.OffHand):
                m_Root.Q<EquipmentSlot>("OffHandSlot").item = player.equipment.OffHand;
                break;

        }
    }
    #endregion
}


public class InventorySlotUI
{
    public InventorySlot slot;
    public TemplateContainer m_SlotContainer;
    private Image image;
    private Label label;
    private Label countLabel;
    public InventorySlotUI(TemplateContainer container, InventorySlot slot)
    {
        this.m_SlotContainer = container;
        image = m_SlotContainer.Q<Image>("SlotIcon");
        label = m_SlotContainer.Q<Label>("ItemLabel");
        countLabel = m_SlotContainer.Q<Label>("CountLabel");

        this.UpdateSlot(slot);

        container.RegisterCallback<MouseDownEvent>(OnMouseDown);
    }

    private void OnMouseDown(MouseDownEvent e)
    {
        if (slot.item != null)
        {
            Debug.Log("Clicked on " + slot.item.itemName);
            slot.AddItem(slot.item, 1, out _);

        }
    }

    public void UpdateSlot(InventorySlot slot)
    {
        if (this.slot != null && this.slot != slot)
        {
            this.slot.PropertyChanged -= OnSlotPropertyChanged;
            if (this.slot.item != null)
            {
                this.slot.item.PropertyChanged -= OnItemPropertyChanged;
            }
        }
        this.slot = slot;
        this.UpdateSlotProperties();
        this.slot.PropertyChanged += OnSlotPropertyChanged;

        this.UpdateItem(this.slot.item);
    }
    private void UpdateSlotProperties()
    {
        this.countLabel.text = slot.numberOfItem.ToString();
    }
    private void UpdateItem(Item item)
    {
        if (item != null)
        {
            item.PropertyChanged += OnItemPropertyChanged;
            image.sprite = item.itemSprite;
            label.text = item.itemName;
        }
        else
        {
            image.sprite = null;
            label.text = "";
        }
    }

    private void OnSlotPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        Debug.Log("Slot property changed " + e.PropertyName);
        if (e.PropertyName == "item")
        {
            if (this.slot.item != null)
            {
                this.slot.item.PropertyChanged -= OnItemPropertyChanged;
            }

            UpdateItem(slot.item);
        }
        else
        {
            this.UpdateSlotProperties();

        }
    }
    private void OnItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        Debug.Log("Item property changed " + e.PropertyName);
        UpdateItem(slot.item);
    }
}