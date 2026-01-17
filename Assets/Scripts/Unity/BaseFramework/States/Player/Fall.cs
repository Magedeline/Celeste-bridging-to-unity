using UnityEngine;

namespace Unity.Celeste.States.Player
{
    /// <summary>
    /// Fall state for the player - descending/in air without jumping.
    /// Uses Celeste's fall constants (MaxFall = 160, Gravity = 900).
    /// </summary>
    public class Fall : PlayerState
    {
        public Fall(UnityPlayerController playerController) : base(playerController)
        {
        }

        public override int GetCelesteStateIndex() => StNormal;

        public override void Enter()
        {
            playerController.GetAnimator()?.Play("PlayerFall");
        }

        public override void Exit()
        {
            playerController.SpawnLandingEffect();
        }

        public override void FixedUpdate()
        {
            // Check if landed on ground
            if (playerController.IsOnTheGround())
            {
                playerController.SetState(new Idle(playerController));
                return;
            }

            // Check for wall grab while falling
            if (playerController.IsTouchingWall() && 
                playerController.IsPressingTowardWall() && 
                playerController.CanClimbWall())
            {
                playerController.SetState(new Climb(playerController));
                return;
            }

            // Allow air control
            playerController.HandleMovement();
            
            // Check for jump/dash input (coyote time jump, air dash)
            playerController.HandleJump();
            playerController.HandleDash();
        }

        public override void Update()
        {
        }
    }
}
