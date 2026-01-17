using UnityEngine;
using Unity.Celeste.States;
using Unity.Celeste.States.Player;

namespace Unity.Celeste
{
    /// <summary>
    /// Bridge connector that links original Celeste code with Unity implementations.
    /// This allows the original Celeste classes to work with Unity components.
    /// </summary>
    public class CelesteBridge : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UnityPlayerController unityPlayer;
        
        /// <summary>
        /// Reference to the original Celeste Player if available
        /// </summary>
        private Celeste.Player celestePlayer;
        
        /// <summary>
        /// Whether to sync state between Unity and Celeste implementations
        /// </summary>
        [Header("Sync Settings")]
        public bool syncStateWithCeleste = false;
        public bool useUnityPhysics = true;
        public bool useUnityInput = true;
        
        private void Start()
        {
            if (unityPlayer == null)
            {
                unityPlayer = GetComponent<UnityPlayerController>();
            }
        }
        
        private void Update()
        {
            if (syncStateWithCeleste && celestePlayer != null && unityPlayer != null)
            {
                SyncCelesteToUnity();
            }
        }
        
        /// <summary>
        /// Syncs Celeste player state to Unity player state
        /// </summary>
        private void SyncCelesteToUnity()
        {
            if (celestePlayer == null || unityPlayer == null) return;
            
            // Sync position
            if (!useUnityPhysics)
            {
                // Convert XNA Vector2 to Unity Vector2
                Vector2 celestePos = new Vector2(
                    celestePlayer.Position.X / 10f,  // Scale factor
                    celestePlayer.Position.Y / 10f
                );
                unityPlayer.transform.position = celestePos;
            }
            
            // Sync state
            int celesteState = celestePlayer.StateMachine.State;
            SyncStateFromCeleste(celesteState);
        }
        
        /// <summary>
        /// Maps Celeste state index to Unity PlayerState
        /// </summary>
        private void SyncStateFromCeleste(int celesteStateIndex)
        {
            PlayerState currentUnityState = unityPlayer.GetState();
            
            // Only change state if necessary
            if (currentUnityState != null && 
                currentUnityState.GetCelesteStateIndex() == celesteStateIndex)
            {
                return;
            }
            
            // Map Celeste states to Unity states
            switch (celesteStateIndex)
            {
                case PlayerState.StNormal:
                    // Could be Idle, Walk, Jump, or Fall - let Unity handle this
                    break;
                    
                case PlayerState.StClimb:
                    if (currentUnityState?.GetStateName() != "Climb")
                    {
                        unityPlayer.SetState(new Climb(unityPlayer));
                    }
                    break;
                    
                case PlayerState.StDash:
                    if (currentUnityState?.GetStateName() != "Dash")
                    {
                        unityPlayer.SetState(new Dash(unityPlayer));
                    }
                    break;
                    
                case PlayerState.StDummy:
                    // Death or cutscene state
                    break;
            }
        }
        
        /// <summary>
        /// Gets the corresponding Celeste state index from Unity state
        /// </summary>
        public int GetCelesteStateFromUnity()
        {
            if (unityPlayer == null) return PlayerState.StNormal;
            
            PlayerState state = unityPlayer.GetState();
            return state?.GetCelesteStateIndex() ?? PlayerState.StNormal;
        }
        
        /// <summary>
        /// Bridges Celeste speed values to Unity
        /// Celeste uses larger scale values, Unity typically uses smaller
        /// </summary>
        public static Vector2 CelesteToUnityVelocity(Microsoft.Xna.Framework.Vector2 celesteVelocity)
        {
            const float scaleFactor = 10f;
            return new Vector2(
                celesteVelocity.X / scaleFactor,
                celesteVelocity.Y / scaleFactor
            );
        }
        
        /// <summary>
        /// Bridges Unity speed values to Celeste
        /// </summary>
        public static Microsoft.Xna.Framework.Vector2 UnityToCelesteVelocity(Vector2 unityVelocity)
        {
            const float scaleFactor = 10f;
            return new Microsoft.Xna.Framework.Vector2(
                unityVelocity.x * scaleFactor,
                unityVelocity.y * scaleFactor
            );
        }
        
        /// <summary>
        /// Bridges Celeste position to Unity position
        /// </summary>
        public static Vector2 CelesteToUnityPosition(Microsoft.Xna.Framework.Vector2 celestePosition)
        {
            const float scaleFactor = 10f;
            return new Vector2(
                celestePosition.X / scaleFactor,
                -celestePosition.Y / scaleFactor  // Y is inverted
            );
        }
        
        /// <summary>
        /// Bridges Unity position to Celeste position
        /// </summary>
        public static Microsoft.Xna.Framework.Vector2 UnityToCelestePosition(Vector2 unityPosition)
        {
            const float scaleFactor = 10f;
            return new Microsoft.Xna.Framework.Vector2(
                unityPosition.x * scaleFactor,
                -unityPosition.y * scaleFactor  // Y is inverted
            );
        }
        
        #region Celeste Constants Access
        
        /// <summary>
        /// Access to Celeste physics constants (from original Player.cs)
        /// </summary>
        public static class CelesteConstants
        {
            // Movement
            public const float MaxFall = 160f;
            public const float Gravity = 900f;
            public const float HalfGravThreshold = 40f;
            public const float FastMaxFall = 240f;
            public const float MaxRun = 90f;
            public const float RunAccel = 1000f;
            public const float RunReduce = 400f;
            public const float AirMult = 0.65f;
            
            // Jump
            public const float JumpGraceTime = 0.1f;
            public const float JumpSpeed = -105f;
            public const float JumpHBoost = 40f;
            public const float VarJumpTime = 0.2f;
            
            // Wall
            public const float WallSlideStartMax = 20f;
            public const float WallSlideTime = 1.2f;
            public const float WallJumpHSpeed = 130f;
            public const float WallJumpForceTime = 0.16f;
            
            // Dash
            public const float DashSpeed = 240f;
            public const float EndDashSpeed = 160f;
            public const float DashTime = 0.15f;
            public const float DashCooldown = 0.2f;
            
            // Climb
            public const float ClimbMaxStamina = 110f;
            public const float ClimbUpSpeed = -45f;
            public const float ClimbDownSpeed = 80f;
            public const float ClimbSlipSpeed = 30f;
            
            // Scale factor for Unity (Celeste uses pixels, Unity uses units)
            public const float ScaleFactor = 10f;
            
            /// <summary>
            /// Get Unity-scaled version of a Celeste value
            /// </summary>
            public static float ToUnity(float celesteValue)
            {
                return celesteValue / ScaleFactor;
            }
        }
        
        #endregion
    }
}
