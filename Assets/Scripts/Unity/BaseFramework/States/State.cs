using UnityEngine;

namespace Unity.Celeste.States
{
    /// <summary>
    /// Base abstract class for all states in the state machine pattern.
    /// Based on AdamNbz/celeste-2d-pc-version framework.
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// Called when entering this state
        /// </summary>
        public abstract void Enter();

        /// <summary>
        /// Called when exiting this state
        /// </summary>
        public abstract void Exit();

        /// <summary>
        /// Called every frame (Update)
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Called every fixed update (physics)
        /// </summary>
        public abstract void FixedUpdate();
    }
}
