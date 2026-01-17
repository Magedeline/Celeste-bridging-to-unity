using UnityEngine;

namespace Unity.Celeste.States.Player
{
    /// <summary>
    /// Walk state for the player - moving on ground.
    /// Uses Celeste's movement constants (MaxRun = 90, RunAccel = 1000).
    /// </summary>
    public class Walk : PlayerState
    {
        public Walk(UnityPlayerController playerController) : base(playerController)
        {
        }

        public override int GetCelesteStateIndex() => StNormal;

        public override void Enter()
        {
            playerController.GetAnimator()?.Play("PlayerWalk");
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
            // Check if player stopped moving
            if (!playerController.HandleMovement())
            {
                playerController.SetState(new Idle(playerController));
                return;
            }

            // Check if player left the ground
            if (!playerController.IsOnTheGround())
            {
                playerController.SetState(new Fall(playerController));
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
