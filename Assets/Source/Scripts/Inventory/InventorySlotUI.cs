using UnityEngine;
using UnityEngine.UIElements;
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
            label.text = item.ItemName;
        }
        else
        {
            image.sprite = null;
            label.text = "";
        }
    }

    private void OnSlotPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
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
        UpdateItem(slot.item);
    }
}
