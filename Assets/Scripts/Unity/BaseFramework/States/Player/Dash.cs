using UnityEngine;

namespace Unity.Celeste.States.Player
{
    /// <summary>
    /// Dash state for the player - quick burst of movement.
    /// Uses Celeste's dash constants (DashSpeed = 240, DashTime = 0.15).
    /// </summary>
    public class Dash : PlayerState
    {
        // Celeste dash constants
        private const float DashDuration = 0.15f; // DashTime from Celeste
        private float dashTimer = 0f;
        private float previousSpeed;

        public Dash(UnityPlayerController playerController) : base(playerController)
        {
        }

        public override int GetCelesteStateIndex() => StDash;

        public override void Enter()
        {
            playerController.GetAnimator()?.Play("PlayerDash");
            dashTimer = DashDuration;
            playerController.TurnOffDashAble();
            
            // Store previous speed for returning after dash
            previousSpeed = Mathf.Abs(playerController.GetObjectVelocity().x);
            
            // Apply dash speed (DashSpeed = 240 in Celeste, scaled for Unity)
            playerController.SetObjectVelocity(
                playerController.DashSpeed * playerController.Direction, 
                0f
            );
            
            // Disable gravity during dash
            playerController.GetRigidbody().gravityScale = 0f;
        }

        public override void Exit()
        {
            // Restore gravity
            playerController.GetRigidbody().gravityScale = playerController.GetBaseGravityScale();
        }

        public override void FixedUpdate()
        {
            dashTimer -= Time.fixedDeltaTime;
            
            // Dash complete - return to previous state
            if (dashTimer <= 0f)
            {
                playerController.SetState(prevState ?? new Fall(playerController));
                playerController.SetObjectVelocity(
                    previousSpeed * playerController.Direction, 
                    0f
                );
                return;
            }

            // Check for wall grab during dash
            if (playerController.IsTouchingWall() && !playerController.IsOnTheGround())
            {
                playerController.SetState(new Climb(playerController));
                return;
            }
        }

        public override void Update()
        {
        }
    }
}
