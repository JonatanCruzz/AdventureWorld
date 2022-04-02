using UnityEngine;
namespace AdventureWorld.Prueba.State
{
   
    public class StateMachine : MonoBehaviour
    {
        public IState currentState;
        public IState previousState;
        public IState globalState;

        public void ChangeState(IState newState)
        {
            if (currentState != null)
            {
                currentState.OnExit();
            }
            previousState = currentState;
            currentState = newState;
            currentState.OnEnter();
        }

        public void Update()
        {
            if (currentState != null)
            {
                currentState.Update(Time.deltaTime);
            }
        }

        public void FixedUpdate()
        {
            if (currentState != null)
            {
                currentState.FixedUpdate(Time.deltaTime);
            }
        }
    }
}