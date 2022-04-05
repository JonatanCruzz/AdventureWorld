using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureWorld.Prueba.State
{
    public abstract class BaseState
    {
        protected StateMachine stateMachine;

        public BaseState(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Update(float deltaTime)
        {
        }

        public virtual void FixedUpdate(float deltaTime)
        {
        }

        public virtual void OnEnter(BaseState previousBaseState)
        {
        }

        public virtual void OnExit(BaseState nextBaseState)
        {
        }
    }
}