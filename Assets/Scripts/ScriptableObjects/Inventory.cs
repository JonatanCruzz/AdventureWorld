using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Normal", order = 1)]
public class Inventory : ScriptableObject
{
    public List<InventorySlot> items = new List<InventorySlot>();
    public int numberOfKey;
    public int maxSlots = 10;

    public int AddItem(Item item, int amount = 1)
    {
        int itemsLeft = amount;
        if (item.stackable)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].AddItem(item, itemsLeft, out itemsLeft))
                {
                    break;
                }
            }
            if (itemsLeft > 0)
            {
                // a while to to create a new slot until the item is added 
                // or the inventory is full
                while (itemsLeft > 0)
                {
                    if (items.Count >= maxSlots)
                    {
                        Debug.Log("Inventory is full");
                        break;
                    }
                    else
                    {
                        var slot = new InventorySlot(item, 0);
                        slot.AddItem(item, itemsLeft, out itemsLeft);
                        items.Add(slot);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < maxSlots && itemsLeft > 0; i++)
            {
                if (items[i] == null)
                {
                    items[i] = new InventorySlot(item, 1);
                    itemsLeft--;
                }
                else if (items[i].item == null)
                {
                    items[i].AddItem(item, itemsLeft, out itemsLeft);
                }
            }
            if (itemsLeft > 0)
            {
                Debug.Log("Inventory is full");
            }
        }
        return itemsLeft;
    }




}

[Serializable]
public class InventorySlot : INotifyPropertyChanged
{


    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    [SerializeField]
    private Item _item;
    [SerializeField]
    private int _numberOfItem;
    public Item item
    {
        get => _item;
        set
        {
            if (_item != value)
            {
                _item = value;
                OnPropertyChanged(nameof(item));
            }
        }
    }
    public int numberOfItem
    {
        get => _numberOfItem;
        set
        {
            if (_numberOfItem != value)
            {
                _numberOfItem = value;
                OnPropertyChanged(nameof(numberOfItem));
            }
        }
    }

    public InventorySlot(Item item, int numberOfItem)
    {
        this.item = item;
        this.numberOfItem = numberOfItem;
    }

    public InventorySlot()
    {
        this.item = null;
        this.numberOfItem = 0;
    }
    private void addAmount(int amount, out int numberOfItemLeft)
    {
        var numberOfItem = this.numberOfItem + amount;
        numberOfItemLeft = amount;
        if (!this.item.stackable)
        {
            if (numberOfItem != 0) return;
            numberOfItem = 1;
            numberOfItemLeft = amount - 1;
        }
        else if (this.item.stackSize < numberOfItem)
        {
            numberOfItemLeft = numberOfItem - this.item.stackSize;
            numberOfItem = this.item.stackSize;
        }
        else
        {
            numberOfItemLeft = 0;
        }

        this.numberOfItem = numberOfItem;
    }

    public bool AddItem(Item item, int numberOfItem, out int numberOfItemLeft)
    {
        numberOfItemLeft = numberOfItem;
        if (this.item == null)
        {
            this.item = item;
            this.numberOfItem = 0;
            this.addAmount(numberOfItem, out numberOfItemLeft);
        }
        else if (this.item.itemName == item.itemName)
        {
            this.addAmount(numberOfItem, out numberOfItemLeft);
        }
        return numberOfItemLeft == 0;
    }
}