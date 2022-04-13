using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
public class EquipmentSlot : Button
{
    private VisualElement m_imageContainer;
    private Sprite m_defaultImage;
    private EquipableItem m_item;
    private EquipableItemSlotType m_slotType;
    // event handler for on click
    public event System.Action<EquipmentSlot> OnClicked;
    public EquipableItemSlotType slotType
    {
        get => m_slotType;
        set
        {
            m_slotType = value;
            if (m_item == null)
            {
                switch (value)
                {
                    case EquipableItemSlotType.Head:
                    default:
                        m_defaultImage = Resources.Load<Sprite>("Helmet_Icon");
                        break;
                    case EquipableItemSlotType.Chest:
                        m_defaultImage = Resources.Load<Sprite>("Armor_Icon");
                        break;
                    case EquipableItemSlotType.Feet:
                        m_defaultImage = Resources.Load<Sprite>("Boots_Icon");
                        break;
                    case EquipableItemSlotType.MainHand:
                        m_defaultImage = Resources.Load<Sprite>("Sword_Icon");
                        break;
                    case EquipableItemSlotType.OffHand:
                        m_defaultImage = Resources.Load<Sprite>("Shield_Icon");
                        break;
                }
                this.updateImage();

            }
        }
    }
    public EquipableItem item
    {
        get => m_item;
        set
        {
            m_item = value;
            this.updateImage();
        }
    }
    private void updateImage()
    {
        if (item == null)
        {
            m_imageContainer.style.backgroundImage = new StyleBackground(m_defaultImage);
            m_imageContainer.style.opacity = 0.5f;
        }
        else
        {
            m_imageContainer.style.backgroundImage = new StyleBackground(m_item.itemSprite);
            m_imageContainer.style.opacity = 1;
        }
    }
    public EquipmentSlot()
    {
        m_imageContainer = new VisualElement() { name = "image" };
        m_imageContainer.style.flexGrow = 1;
        Add(m_imageContainer);
        style.backgroundImage = new StyleBackground(Resources.Load<Sprite>("Eq_Slot"));
        style.width = style.height = 128;
        style.paddingBottom = style.paddingLeft = style.paddingRight = style.paddingTop = 4;
        style.marginBottom = style.marginLeft = style.marginRight = style.marginTop = 5;
        clicked += OnClick;
    }
    private void OnClick()
    {
        OnClicked?.Invoke(this);
    }
    public new class UxmlFactory : UxmlFactory<EquipmentSlot, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription someNumberAttr = new UxmlIntAttributeDescription { name = "some-number" };
        UxmlEnumAttributeDescription<EquipableItemSlotType> equipmentSlotTypeAttr = new UxmlEnumAttributeDescription<EquipableItemSlotType> { name = "equipment-slot-type" };
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {

            base.Init(ve, bag, cc);
            EquipmentSlot ce = (EquipmentSlot)ve;
            var equipmentType = equipmentSlotTypeAttr.GetValueFromBag(bag, cc);
            ce.slotType = equipmentType;




            // ce.someNumber = someNumberAttr.GetValueFromBag(bag, cc);
        }
    }
}