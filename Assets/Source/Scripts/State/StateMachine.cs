using UnityEngine;
namespace AdventureWorld.Prueba.State
{
   
    public class StateMachine : MonoBehaviour
    {
        public BaseState CurrentBaseState;
        public BaseState PreviousBaseState;

        public void ChangeState(BaseState newBaseState)
        {
            CurrentBaseState?.OnExit(newBaseState);
            
            PreviousBaseState = CurrentBaseState;
            CurrentBaseState = newBaseState;
            CurrentBaseState.OnEnter(PreviousBaseState);
        }

        public void Update()
        {
            CurrentBaseState?.Update(Time.deltaTime);
        }

        public void FixedUpdate()
        {
            CurrentBaseState?.FixedUpdate(Time.deltaTime);
        }
    }
}