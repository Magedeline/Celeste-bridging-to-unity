// Celeste Player stub for bridging with Unity
// This is a compatibility shim for the original Celeste.Player class

using Microsoft.Xna.Framework;

namespace Celeste
{
    /// <summary>
    /// Stub class representing the original Celeste Player
    /// This bridges XNA/MonoGame Celeste code with Unity
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Player position in Celeste coordinate space
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Player velocity
        /// </summary>
        public Vector2 Speed { get; set; }

        /// <summary>
        /// State machine for player state management
        /// </summary>
        public StateMachine StateMachine { get; private set; }

        public Player()
        {
            StateMachine = new StateMachine();
            Position = Vector2.Zero;
            Speed = Vector2.Zero;
        }
    }

    /// <summary>
    /// State machine for managing player states
    /// </summary>
    public class StateMachine
    {
        /// <summary>
        /// Current state index
        /// </summary>
        public int State { get; set; }

        public StateMachine()
        {
            State = 0; // Default to normal state
        }
    }
}
