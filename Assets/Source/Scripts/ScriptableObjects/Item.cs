using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject, INotifyPropertyChanged
{
    private void SetProperty(string propertyName, string localPropertyName, object value)
    {
        var prop = this.GetType().GetProperty(localPropertyName);
        if (prop == null || prop.GetValue(this).Equals(value)) return;
        prop.SetValue(this, value);
        OnPropertyChanged(propertyName);
    }

    #region Properties
    [PropertyOrder(-1)]
    public string ID = Guid.NewGuid().ToString().ToUpper();

    public int BuyPrice
    {
        get => _buyPrice;
        set => SetProperty(nameof(BuyPrice), nameof(_buyPrice), value);
    }
    public float SellPercentage
    {
        get => _sellPercentage;
        set => SetProperty(nameof(SellPercentage), nameof(_sellPercentage), value);
    }
    public Sprite itemSprite
    {
        get => _itemSprite;
        set => SetProperty(nameof(itemSprite), nameof(_itemSprite), value);
    }
    public string ItemDescription
    {
        get => _itemDescription;
        set => SetProperty(nameof(ItemDescription), nameof(_itemDescription), value);
    }
    public string ItemName
    {
        get => _itemName;
        set
        {
            if (value != _itemName)
            {
                _itemName = value;
                OnPropertyChanged(nameof(ItemName));
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
    [ProgressBar(0, 100)]
    [Range(0, 100)]
    private float _sellPercentage;
    [SerializeField]
    [PreviewField, Required, AssetsOnly, HideLabel, PropertyOrder(-1), HorizontalGroup("Split", Width=50)]
    private Sprite _itemSprite;
    [SerializeField]
    [TextArea(3, 5)]
    private string _itemDescription;
    [SerializeField]
    [VerticalGroup("Split/Properties")]
    private string _itemName;

    [SerializeField]
    [VerticalGroup("Split/Properties")]
    private bool _stackable;
    [SerializeField]
    [VerticalGroup("Split/Properties"), ShowIf("@this._stackable")]
    private int _stackSize;
    #endregion

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
