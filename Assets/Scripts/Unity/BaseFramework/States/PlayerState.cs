using UnityEngine;

namespace Unity.Celeste.States
{
    /// <summary>
    /// Base class for all player states.
    /// Bridges Celeste's state constants with Unity state machine pattern.
    /// Based on AdamNbz/celeste-2d-pc-version framework.
    /// </summary>
    public abstract class PlayerState : State
    {
        protected UnityPlayerController playerController;
        public PlayerState prevState;

        // Celeste state constants for reference
        public const int StNormal = 0;
        public const int StClimb = 1;
        public const int StDash = 2;
        public const int StSwim = 3;
        public const int StBoost = 4;
        public const int StRedDash = 5;
        public const int StHitSquash = 6;
        public const int StLaunch = 7;
        public const int StPickup = 8;
        public const int StDreamDash = 9;
        public const int StSummitLaunch = 10;
        public const int StDummy = 11;
        public const int StIntroWalk = 12;
        public const int StIntroJump = 13;
        public const int StIntroRespawn = 14;
        public const int StIntroWakeUp = 15;
        public const int StBirdDashTutorial = 16;
        public const int StFrozen = 17;
        public const int StReflectionFall = 18;
        public const int StStarFly = 19;
        public const int StTempleFall = 20;
        public const int StCassetteFly = 21;
        public const int StAttract = 22;

        public PlayerState(UnityPlayerController playerController)
        {
            this.playerController = playerController;
        }

        /// <summary>
        /// Gets the name of this state
        /// </summary>
        public string GetStateName()
        {
            return GetType().Name;
        }

        /// <summary>
        /// Gets the corresponding Celeste state constant
        /// </summary>
        public virtual int GetCelesteStateIndex()
        {
            return StNormal;
        }
    }
}
