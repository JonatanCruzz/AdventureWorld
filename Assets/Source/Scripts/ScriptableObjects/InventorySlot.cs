using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class InventorySlot : INotifyPropertyChanged,ISerializable
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
            if (this.numberOfItem != 0) return;
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
        else if (this.item.ItemName == item.ItemName)
        {
            this.addAmount(numberOfItem, out numberOfItemLeft);
        }
        return numberOfItemLeft == 0;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("numberOfItem", numberOfItem);
        info.AddValue("item", item != null?item.ID:"");
        
    }
}