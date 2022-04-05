#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace AdventureWorld.Prueba.Editor
{
    public class OdinItemDatabase : OdinMenuEditorWindow
    {
        [MenuItem("My Game/My Editor")]
        private static void OpenWindow()
        {
            GetWindow<OdinItemDatabase>().Show();
        }

        private static string AskForFile(string basePath, string baseName = "InitialName.asset",
            string msg = "Save your prefab")
        {
            var extension = Path.GetExtension(baseName);
            // remove the "." from the extension
            extension = extension.Substring(1);

            var path = EditorUtility.SaveFilePanelInProject(msg, baseName, extension,
                "Please select file name to save prefab to:", basePath);
            if (string.IsNullOrEmpty(path)) return null;
            // get filename without extension
            var name = Path.GetFileNameWithoutExtension(path);
            return name;
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(false);

            var menuInventory = new OdinMenuItem(tree, "Inventories", null);
            menuInventory.OnRightClick += (item) =>
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add"), false, () =>
                {
                    var fileName = AskForFile("Assets/Resources/Inventories", "NewInventory.asset",
                        "Save the inventory");
                    if (string.IsNullOrEmpty(fileName)) return;

                    var newItem = CreateInstance<Inventory>();

                    AssetDatabase.CreateAsset(newItem, $"Assets/Resources/Inventories/{fileName}.asset");
                });
                menu.ShowAsContext();
            };
            var menuItems = new OdinMenuItem(tree, "Items", null);
            menuItems.OnRightClick += (item) =>
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add Equipment"), false, () =>
                {
                    var fileName = AskForFile("Assets/Resources/Items/Equipables", "NewEquipable.asset",
                        "Save the equipable");
                    if (string.IsNullOrEmpty(fileName)) return;

                    var newItem = CreateInstance<EquipableItem>();

                    AssetDatabase.CreateAsset(newItem, $"Assets/Resources/Items/Equipables/{fileName}.asset");
                    this.ForceMenuTreeRebuild();
                });
                menu.AddItem(new GUIContent("Add Consumable"), false, () =>
                {
                    var fileName = AskForFile("Assets/Resources/Items/Consumable", "NewConsumable.asset",
                        "Save the consumable");
                    if (string.IsNullOrEmpty(fileName)) return;

                    var newItem = CreateInstance<ConsumableItem>();

                    AssetDatabase.CreateAsset(newItem, $"Assets/Resources/Items/Consumable/{fileName}.asset");
                    this.ForceMenuTreeRebuild();
                });
                menu.ShowAsContext();
            };
            tree.MenuItems.Insert(0, menuInventory);
            tree.MenuItems.Insert(0, menuItems);
            // tree.Add("Inventories", menuItem);
            tree.AddAllAssetsAtPath("Items/Equipables", ItemDatabase.ITEMS_PATH, typeof(EquipableItem), true, true);
            tree.AddAllAssetsAtPath("Items/Consumables", ItemDatabase.ITEMS_PATH, typeof(ConsumableItem), true, true);
            tree.AddAllAssetsAtPath("Inventories", "Assets/Inventories", typeof(Inventory), true, false);
            tree.EnumerateTree().AddThumbnailIcons();
            // tree.AddAllAssetsAtPath("Equipables", ItemDatabase.ITEMS_PATH + "/Equipable", typeof(EquipableItem));
            return tree;
        }
    }

   
}
#endif