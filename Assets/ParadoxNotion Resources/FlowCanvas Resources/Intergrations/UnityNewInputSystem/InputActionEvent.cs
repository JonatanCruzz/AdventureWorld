using UnityEngine;
using UnityEngine.InputSystem;
using ParadoxNotion.Design;
using NodeCanvas.Framework;

namespace FlowCanvas.Nodes
{

    [Name("Input Action")]
    [Category("Events/Input (New System)")]
    [Description("Calls respective outputs when the defined Input Action is pressed down, held down or released.\nThis node works with the new Unity Input System only.")]
    public class NewInputEvents : EventNode, IUpdatable
    {
        [RequiredField] public InputActionAsset inputActionAsset;
        [SerializeField] private string selectedActionID;

        private InputAction action;
        private bool isPressed;

        private FlowOutput onActionDown;
        private FlowOutput onActionPress;
        private FlowOutput onActionUp;

        private object actionValue;
        private float timeStarted;

        ///----------------------------------------------------------------------------------------------

        public override string name {
            get { return string.Format("{0} [{1}]", base.name, action != null ? action.name : "NONE"); }
        }

        public override void OnGraphStarted() {
            if ( inputActionAsset == null ) { return; }
            action = inputActionAsset.FindAction(selectedActionID);
            isPressed = false;
            if ( action != null ) {
                action.Enable();
                action.performed += OnActionPerformed;
                action.canceled += OnActionCanceled;
            }
        }

        public override void OnGraphStoped() {
            if ( action != null ) {
                action.performed -= OnActionPerformed;
                action.canceled -= OnActionCanceled;
            }
        }

        protected override void RegisterPorts() {
            if ( inputActionAsset == null ) { return; }
            action = inputActionAsset.FindAction(selectedActionID);
            if ( action != null ) {
                onActionDown = AddFlowOutput("Down");
                onActionPress = AddFlowOutput("Pressed");
                onActionUp = AddFlowOutput("Up");

                if ( action.type == InputActionType.Value ) {
                    var controlType = ParadoxNotion.ReflectionTools.GetType(action.expectedControlType, true);
                    if ( controlType != null ) {
                        AddValueOutput("Value", controlType, () => actionValue);
                    }
                }
                AddValueOutput<float>("Duration", () => Time.time - timeStarted);
            }
        }

        void OnActionPerformed(InputAction.CallbackContext context) {
            isPressed = true;
            actionValue = context.ReadValueAsObject();
            timeStarted = Time.time;
            onActionDown.Call(new Flow());
        }

        void OnActionCanceled(InputAction.CallbackContext context) {
            isPressed = false;
            onActionUp.Call(new Flow());
        }

        void IUpdatable.Update() {
            if ( isPressed ) { onActionPress.Call(new Flow()); }
        }

        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI() {
            base.OnNodeInspectorGUI();
            if ( inputActionAsset != null && GUILayout.Button("Select Input Action") ) {
                var menu = new UnityEditor.GenericMenu();
                InputAction current = null;
                if ( !string.IsNullOrEmpty(selectedActionID) ) { current = inputActionAsset.FindAction(selectedActionID); }
                foreach ( var map in inputActionAsset.actionMaps ) {
                    foreach ( var action in map.actions ) {
                        menu.AddItem(new GUIContent(map.name + "/" + action.name), current == action, () =>
                        {
                            UndoUtility.RecordObject(graph, "Set Input Action");
                            selectedActionID = action.id.ToString();
                            GatherPorts();
                        });
                    }
                }
                menu.ShowAsContext();
            }
        }
#endif
        ///----------------------------------------------------------------------------------------------

    }
}