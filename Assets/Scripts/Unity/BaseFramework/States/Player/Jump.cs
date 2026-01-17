using UnityEngine;

namespace Unity.Celeste.States.Player
{
    /// <summary>
    /// Jump state for the player - ascending during a jump.
    /// Uses Celeste's jump constants (JumpSpeed = -105, VarJumpTime = 0.2).
    /// </summary>
    public class Jump : PlayerState
    {
        public Jump(UnityPlayerController playerController) : base(playerController)
        {
        }

        public override int GetCelesteStateIndex() => StNormal;

        public override void Enter()
        {
            playerController.GetAnimator()?.Play("PlayerJump");
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
            // Only handle movement if not coming from wall climb (wall jump)
            if (prevState?.GetStateName() != "Climb")
            {
                playerController.HandleMovement();
            }

            // Check for wall grab while ascending
            if (playerController.IsTouchingWall() && 
                playerController.IsPressingTowardWall() && 
                playerController.CanClimbWall())
            {
                playerController.SetState(new Climb(playerController));
                return;
            }

            // Check for jump/dash input (double jump, etc.)
            playerController.HandleJump();
            playerController.HandleDash();

            // Transition to fall when velocity becomes negative (going down)
            if (playerController.GetObjectVelocity().y < 0)
            {
                playerController.SetState(new Fall(playerController));
            }
        }

        public override void Update()
        {
        }
    }
}
