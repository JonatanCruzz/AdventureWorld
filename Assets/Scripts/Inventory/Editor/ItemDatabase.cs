#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.IO;
using System;
using System.Linq;

class SavePrefabWindow : EditorWindow
{

    string prefabName;

    void OnGUI()
    {
        prefabName = EditorGUILayout.TextField("Prefab Name", prefabName);

        if (GUILayout.Button("Save Prefab"))
        {
            OnClickSavePrefab();
            GUIUtility.ExitGUI();
        }
    }

    void OnClickSavePrefab()
    {
        prefabName = prefabName.Trim();

        if (string.IsNullOrEmpty(prefabName))
        {
            EditorUtility.DisplayDialog("Unable to save prefab", "Please specify a valid prefab name.", "Close");
            return;
        }

        // You may also want to check for illegal characters :)

        // Save your prefab

        Close();
    }

}
public class ItemDatabase : EditorWindow
{
    private Sprite m_DefaultItemIcon;
    private static List<Item> m_ItemDatabase = new List<Item>();

    private VisualElement m_ItemsTab;
    private static VisualTreeAsset m_ItemRowTemplate;
    private ListView m_ItemListView;
    private float m_ItemHeight = 40;

    private ScrollView m_DetailSection;
    private VisualElement m_LargeDisplayIcon;
    private Item m_activeItem;
    private static readonly string ItemsFolder = "Assets/Items/";

    [MenuItem("WUG/Item Database")]
    public static void Init()
    {
        ItemDatabase wnd = GetWindow<ItemDatabase>();
        wnd.titleContent = new GUIContent("Item Database");
        Vector2 size = new Vector2(800, 475);
        wnd.minSize = size;
        wnd.maxSize = size;
    }
    public void CreateGUI()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
            ("Assets/UI/Inventory/Editor/InventoryDatabase.uxml");
        VisualElement rootFromUXML = visualTree.Instantiate();
        rootVisualElement.Add(rootFromUXML);
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>
            ("Assets/UI/Inventory/Editor/InventoryDatabase.uss");
        rootVisualElement.styleSheets.Add(styleSheet);
        m_DefaultItemIcon = (Sprite)AssetDatabase.LoadAssetAtPath(
            "Assets/Sprites/UI/Inventory/UnknownIcon.png", typeof(Sprite));

        m_ItemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
            ("Assets/UI/Inventory/Editor/ItemRow.uxml");


        LoadAllItems();

        m_ItemsTab = rootVisualElement.Q<VisualElement>("ItemsTab");
        GenerateListView();

        m_DetailSection = rootVisualElement.Q<ScrollView>("ScrollView_Details");
        m_DetailSection.style.visibility = Visibility.Hidden;
        m_LargeDisplayIcon = m_DetailSection.Q<VisualElement>("Icon");

        m_DetailSection.Q<TextField>("ItemName")
             .RegisterValueChangedCallback(evt =>
             {
                 m_activeItem.name = evt.newValue;
                 m_ItemListView.Refresh();

             });
        m_DetailSection.Q<ObjectField>("IconPicker")
            .RegisterValueChangedCallback(evt =>
            {
                Sprite newSprite = evt.newValue as Sprite;
                m_activeItem.itemSprite = newSprite == null ? m_DefaultItemIcon : newSprite;
                m_LargeDisplayIcon.style.backgroundImage = newSprite ==
                    null ? m_DefaultItemIcon.texture : newSprite.texture;
                m_ItemListView.Refresh();
            });
        rootVisualElement.Q<Button>("Btn_DeleteItem").clicked += DeleteItem_OnClick;
        var btnAddItem = rootVisualElement.Q<Button>("Btn_AddItem");
        btnAddItem.clicked += () =>
        {
            GenericDropdownMenu dropdownMenu = new GenericDropdownMenu();
            dropdownMenu.AddItem("Equipable", false, () => { AddItem<EquipableItem>(); });
            dropdownMenu.AddItem("Consumable", false, () => { AddItem<ConsumableItem>(); });
            // dropdownMenu.AddItem("Key", false, () => { AddItem<ConsumableItem>(); });

            dropdownMenu.DropDown(btnAddItem.worldBound, btnAddItem);
            // dropdownMenu.DoDisplayEditorMenu(btnAddItem.worldBound);
            // dropdownMenu.DoDisplayEditorMenu(btnAddItem.worldBound);
        };


    }

    private void AddItem<T>() where T : Item
    {
        //Create an instance of the scriptable object and set the default parameters
        Item newItem = CreateInstance<T>();
        var itemType = newItem.GetType().Name;
        // remove the "Item" from the end of the type name
        itemType = itemType.Substring(0, itemType.Length - 4);
        newItem.itemName = $"New {itemType}";
        newItem.itemSprite = m_DefaultItemIcon;
        var baseItemPath = $"{ItemsFolder}{itemType}/";
        // if folder doesn't exist, create it
        if (!Directory.Exists(baseItemPath))
        {
            Directory.CreateDirectory(baseItemPath);
        }
        //Create the asset, using the unique ID for the name
        string path = EditorUtility.SaveFilePanelInProject("Save Your Prefab", "InitialName.asset", "asset", "Please select file name to save prefab to:", baseItemPath);
        if (!string.IsNullOrEmpty(path))
        {
            Debug.Log($"Saving prefab to {path}");
            // get filename from path
            string filename = Path.GetFileName(path);
            // remove the ".asset" from the end of the filename
            newItem.itemName = filename.Substring(0, filename.Length - 6);

            // // Actually save your prefab!
            AssetDatabase.CreateAsset(newItem, $"{ItemsFolder}{itemType}/{filename}");
            // //Add it to the item list
            m_ItemDatabase.Add(newItem);
            // //Refresh the ListView so everything is redrawn again
            m_ItemListView.Refresh();
            m_ItemListView.style.height = m_ItemDatabase.Count * m_ItemHeight;
        }

    }
    private void DeleteItem_OnClick()
    {
        //Get the path of the fie and delete it through AssetDatabase
        string path = AssetDatabase.GetAssetPath(m_activeItem);
        AssetDatabase.DeleteAsset(path);
        //Purge the reference from the list and refresh the ListView
        m_ItemDatabase.Remove(m_activeItem);
        m_ItemListView.Refresh();
        //Nothing is selected, so hide the details section
        m_DetailSection.style.visibility = Visibility.Hidden;
    }
    private void GenerateListView()
    {
        Func<VisualElement> makeItem = () => m_ItemRowTemplate.CloneTree();

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            e.Q<VisualElement>("Icon").style.backgroundImage = m_ItemDatabase[i] == null ? m_DefaultItemIcon.texture : m_ItemDatabase[i].itemSprite.texture;
            e.Q<Label>("Name").text = m_ItemDatabase[i].itemName + " (" + m_ItemDatabase[i].GetType().Name + ")";
        };

        m_ItemListView = new ListView(m_ItemDatabase, 35, makeItem, bindItem);
        m_ItemListView.selectionType = SelectionType.Single;
        m_ItemListView.style.height = m_ItemDatabase.Count * m_ItemHeight;
        m_ItemsTab.Add(m_ItemListView);

        m_ItemListView.onSelectionChange += ListView_onSelectionChange;
    }

    private void ListView_onSelectionChange(IEnumerable<object> selectedItems)
    {

        m_activeItem = (Item)selectedItems.First();
        SerializedObject so = new SerializedObject(m_activeItem);
        m_DetailSection.Bind(so);

        if (m_activeItem.itemSprite != null)
        {
            m_LargeDisplayIcon.style.backgroundImage = m_activeItem.itemSprite.texture;
        }
        m_DetailSection.style.visibility = Visibility.Visible;
        m_DetailSection.Q<VisualElement>("EquipmentSection").style.display = DisplayStyle.None;
        m_DetailSection.Q<VisualElement>("ConsumableSection").style.display = DisplayStyle.None;

        switch (m_activeItem)
        {
            case EquipableItem item:
                m_DetailSection.Q<VisualElement>("EquipmentSection").style.display = DisplayStyle.Flex;
                break;
            case ConsumableItem item:
                m_DetailSection.Q<VisualElement>("ConsumableSection").style.display = DisplayStyle.Flex;
                break;
        }
    }

    private void LoadAllItems()
    {
        m_ItemDatabase.Clear();

        string[] allPaths = Directory.GetFiles("Assets/Items", "*.asset", SearchOption.AllDirectories);
        foreach (string path in allPaths)
        {
            string cleanedPath = path.Replace("\\", "/");
            Item item = AssetDatabase.LoadAssetAtPath<Item>(cleanedPath);
            if (item != null)
            {
                m_ItemDatabase.Add(item);
            }
        }
    }
}
#endif