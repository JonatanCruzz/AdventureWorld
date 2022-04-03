using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureWorld.Prueba.State
{
    
    public abstract class IState
    {
        public abstract void Update(float deltaTime);
        public abstract void FixedUpdate(float deltaTime);
        public abstract void OnEnter();
        public abstract void OnExit();
        
    }
}