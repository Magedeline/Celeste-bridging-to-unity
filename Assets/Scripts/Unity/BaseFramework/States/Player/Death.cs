using UnityEngine;

namespace Unity.Celeste.States.Player
{
    /// <summary>
    /// Death state for the player.
    /// Handles player death behavior and input disabling.
    /// </summary>
    public class Death : PlayerState
    {
        public Death(UnityPlayerController playerController) : base(playerController)
        {
        }

        public override int GetCelesteStateIndex() => StDummy;

        public override void Enter()
        {
            playerController.GetAnimator()?.Play("PlayerDeath");
            playerController.DisableInput();
            
            // Stop all movement
            playerController.SetObjectVelocity(0f, 0f);
            playerController.GetRigidbody().gravityScale = 0f;
        }

        public override void Exit()
        {
            playerController.GetRigidbody().gravityScale = playerController.GetBaseGravityScale();
        }

        public override void FixedUpdate()
        {
            // Death state does nothing - wait for respawn
        }

        public override void Update()
        {
        }
    }
}
