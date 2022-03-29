using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum EquipableItemSlotType
{
    Head,
    Chest,
    Feet,
    MainHand,
    OffHand
}

[CreateAssetMenu]
public class Item : ScriptableObject, INotifyPropertyChanged
{
    private void setProperty(string propertyName, string localPropertyName, object value)
    {
        var prop = this.GetType().GetProperty(localPropertyName);
        if (prop != null && !prop.GetValue(this).Equals(value))
        {
            prop.SetValue(this, value);
            OnPropertyChanged(propertyName);
        }
    }

    #region Properties
    public string ID = Guid.NewGuid().ToString().ToUpper();

    public int BuyPrice
    {
        get => _buyPrice;
        set => setProperty(nameof(BuyPrice), nameof(_buyPrice), value);
    }
    public float SellPercentage
    {
        get => _sellPercentage;
        set => setProperty(nameof(SellPercentage), nameof(_sellPercentage), value);
    }
    public Sprite itemSprite
    {
        get => _itemSprite;
        set => setProperty(nameof(itemSprite), nameof(_itemSprite), value);
    }
    public string itemDescription
    {
        get => _itemDescription;
        set => setProperty(nameof(itemDescription), nameof(_itemDescription), value);
    }
    public string itemName
    {
        get
        {
            return _itemName;
        }
        set
        {
            if (value != _itemName)
            {
                _itemName = value;
                OnPropertyChanged(nameof(itemName));
            }
        }
    }
    public bool stackable
    {
        get
        {
            return _stackable;
        }
        set
        {
            if (value != _stackable)
            {
                _stackable = value;
                OnPropertyChanged(nameof(stackable));
            }
        }
    }
    public int stackSize
    {
        get
        {
            return _stackSize;
        }
        set
        {
            if (value != _stackSize)
            {
                _stackSize = value;
                OnPropertyChanged(nameof(stackSize));
            }
        }
    }
    #endregion
    #region Private Variables
    [SerializeField]
    private int _buyPrice;
    [SerializeField]
    [Range(0, 1)]
    private float _sellPercentage;
    [SerializeField]
    private Sprite _itemSprite;
    [SerializeField]
    private string _itemDescription;
    [SerializeField]
    private string _itemName;

    [SerializeField]
    private bool _stackable;
    [SerializeField]
    private int _stackSize;
    #endregion

    public void Use()
    {
        Debug.Log("Using " + itemName);


    }

    // method to add the buff to the player
    public virtual void AddBuff(Player player)
    {
        Debug.Log("Adding buff to player");
    }
    public virtual void RemoveBuff(Player player)
    {
        Debug.Log("Removing buff from player");
    }


    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
