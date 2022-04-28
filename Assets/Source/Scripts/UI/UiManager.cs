using System;
using NodeCanvas.Tasks.Actions;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AdventureWorld.Prueba
{
    public enum UIState
    {
        None,
        Teleport,
        Inventory,
        Close
    }

    public class UiManager : MonoBehaviour
    {
        [SerializeField] private TeleportManager teleportUI;
        [SerializeField] private InventoryUI inventoryUI;
        [SerializeField] private CloseUIManager closeUI;
        [SerializeField] private PlayerInput input;

        private Player _player;

        public Player player
        {
            get => _player;
            set
            {
                _player = value;
                inventoryUI.player = _player;
            }
        }

        [OdinSerialize] public UIState CurrentState { get; set; } = UIState.None;

        public void ShowTeleportUI()
        {
            if (CurrentState != UIState.None) return;
            teleportUI.Display = true;
            CurrentState = UIState.Teleport;
        }

        public void HideTeleportUI()
        {
            if (CurrentState != UIState.Teleport) return;
            teleportUI.Display = false;
            CurrentState = UIState.None;
        }

        public void ShowInventoryUI()
        {
            if (CurrentState != UIState.None) return;
            inventoryUI.Show();
            CurrentState = UIState.Inventory;
            // inventoryUI.OpenChest();
        }

        public void OpenChest(Inventory inv)
        {
            if (CurrentState != UIState.None) return;
            inventoryUI.OpenChest(inv);
            CurrentState = UIState.Inventory;
        }

        public void HideInventoryUI()
        {
            if (CurrentState != UIState.Inventory) return;
            inventoryUI.Hide();
            CurrentState = UIState.None;
        }

        public void ShowCloseUI()
        {
            if (CurrentState != UIState.None) return;
            closeUI.ShowCanvas();
            CurrentState = UIState.Close;
        }

        public void HideCloseUI()
        {
            if (CurrentState != UIState.Close) return;
            closeUI.HideCanvas();
            CurrentState = UIState.None;
        }

        public void OnEnable()
        {
            input.onActionTriggered += OnActionTriggered;
        }

        public void OnDisable()
        {
            input.onActionTriggered -= OnActionTriggered;
        }

        private void OnActionTriggered(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            var isCancel = ctx.action.name == "Cancel" || ctx.action.name == "Escape";
            switch (CurrentState)
            {
                case UIState.Teleport:
                    if (isCancel)
                        HideTeleportUI();
                    break;
                case UIState.Inventory:
                    if (isCancel)
                        HideInventoryUI();
                    break;
                case UIState.Close:
                    if (isCancel)
                        HideCloseUI();
                    break;
                case UIState.None:
                    switch (ctx.action.name)
                    {
                        case "Teleport":
                            ShowTeleportUI();
                            break;
                        case "Inventory":
                            ShowInventoryUI();
                            break;
                        default:
                            if (isCancel)
                                ShowCloseUI();
                            break;
                    }

                    break;
            }
        }

        public void Start()
        {
            teleportUI.enabled = true;
            this.inventoryUI.gameObject.SetActive(true);
        }
    }
}