using UnityEngine;

namespace Unity.Celeste.States.Player
{
    /// <summary>
    /// Idle state for the player - standing still on ground.
    /// Uses Celeste's movement constants.
    /// </summary>
    public class Idle : PlayerState
    {
        public Idle(UnityPlayerController playerController) : base(playerController)
        {
        }

        public override int GetCelesteStateIndex() => StNormal;

        public override void Enter()
        {
            playerController.GetAnimator()?.Play("PlayerIdle");
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
            // Check if player is still on ground
            if (!playerController.IsOnTheGround())
            {
                playerController.SetState(new Fall(playerController));
                return;
            }

            // Check for movement input
            if (playerController.HandleMovement())
            {
                playerController.SetState(new Walk(playerController));
                return;
            }

            // Check for jump/dash input
            playerController.HandleJump();
            playerController.HandleDash();
        }

        public override void Update()
        {
        }
    }
}
