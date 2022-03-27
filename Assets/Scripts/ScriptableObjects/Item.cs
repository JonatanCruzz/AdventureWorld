using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum ItemType
{
    Key,
    Consumable,
    Equipable,
    Misc
}

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
    public ItemType itemType
    {
        get
        {
            return _itemType;
        }
        set
        {
            if (value != _itemType)
            {
                _itemType = value;
                OnPropertyChanged(nameof(itemType));
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


    [SerializeField]
    private Sprite _itemSprite;
    [SerializeField]
    private string _itemDescription;
    [SerializeField]
    private string _itemName;
    [SerializeField]
    private ItemType _itemType;
    [SerializeField]
    private bool _stackable;
    [SerializeField]
    private int _stackSize;


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

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable")]
public class ConsumableItem : Item
{
    public void Awake()
    {
        itemType = ItemType.Consumable;
    }
    public int healthRestored;
    public int manaRestored;
    public int staminaRestored;

    public override void AddBuff(Player player)
    {
        player.hp.HP += healthRestored;
        // player.mana += manaRestored;
        // player.stamina += staminaRestored;
    }

}
