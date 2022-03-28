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

    #region Properties
    public string ID = Guid.NewGuid().ToString().ToUpper();

    public int BuyPrice
    {
        get => _buyPrice;
        set
        {
            if (value != _buyPrice)
            {
                _buyPrice = value;
                OnPropertyChanged(nameof(BuyPrice));
            }
        }
    }
    public float SellPercentage
    {
        get => _sellPercentage;
        set
        {
            if (value != _sellPercentage)
            {
                _sellPercentage = value;
                OnPropertyChanged(nameof(SellPercentage));
            }
        }
    }
    public Sprite itemSprite
    {
        get
        {
            return _itemSprite;
        }
        set
        {
            if (value != _itemSprite)
            {
                _itemSprite = value;
                OnPropertyChanged(nameof(itemSprite));
            }
        }
    }
    public string itemDescription
    {
        get
        {
            return _itemDescription;
        }
        set
        {
            if (value != _itemDescription)
            {
                _itemDescription = value;
                OnPropertyChanged(nameof(itemDescription));
            }
        }
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


    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
