using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

// create a clas to manage the equiped items
[CreateAssetMenu(fileName = "EquipmentInventory", menuName = "Inventory/EquipmentInventory")]
[System.Serializable]
public class EquipmentInventory : ScriptableObject, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    [SerializeField] private EquipableItem m_Head;
    [SerializeField] private EquipableItem m_Chest;
    [SerializeField] private EquipableItem m_Feet;
    [SerializeField] private EquipableItem m_MainHand;
    [SerializeField] private EquipableItem m_OffHand;

    public EquipableItem Head
    {
        get
        {
            return m_Head;
        }
        set
        {
            if (value != m_Head)
            {
                m_Head = value;
                OnPropertyChanged(nameof(Head));
            }
        }
    }
    public EquipableItem Chest
    {
        get
        {
            return m_Chest;
        }
        set
        {
            if (value != m_Chest)
            {
                m_Chest = value;
                OnPropertyChanged(nameof(Chest));
            }
        }
    }
    public EquipableItem Feet
    {
        get
        {
            return m_Feet;
        }
        set
        {
            if (value != m_Feet)
            {
                m_Feet = value;
                OnPropertyChanged(nameof(Feet));
            }
        }
    }
    public EquipableItem MainHand
    {
        get
        {
            return m_MainHand;
        }
        set
        {
            if (value != m_MainHand)
            {
                m_MainHand = value;
                OnPropertyChanged(nameof(MainHand));
            }
        }
    }
    public EquipableItem OffHand
    {
        get
        {
            return m_OffHand;
        }
        set
        {
            if (value != m_OffHand)
            {
                m_OffHand = value;
                OnPropertyChanged(nameof(OffHand));
            }
        }
    }


    public IEnumerable<EquipableItem> GetAllItems()
    {
        yield return Head;
        yield return Chest;
        yield return Feet;
        yield return MainHand;
        yield return OffHand;
    }
    public void Awake()
    {

    }

    public EquipableItem AddItem(EquipableItem item)
    {
        EquipableItem output;
        switch (item.itemSlot)
        {
            case EquipableItemSlotType.Head:
                output = Head;
                Head = item;
                break;
            case EquipableItemSlotType.Chest:
                output = Chest;
                Chest = item;
                break;
            case EquipableItemSlotType.Feet:
                output = Feet;
                Feet = item;
                break;
            case EquipableItemSlotType.MainHand:
                output = MainHand;
                MainHand = item;
                break;
            case EquipableItemSlotType.OffHand:
                output = OffHand;
                OffHand = item;
                break;
            default:
                Debug.Log("Item slot not found");
                return null;
        }
        return output;
    }
}