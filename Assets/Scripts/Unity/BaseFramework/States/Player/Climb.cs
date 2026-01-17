using UnityEngine;

namespace Unity.Celeste.States.Player
{
    /// <summary>
    /// Climb state for the player - wall climbing/sliding.
    /// Uses Celeste's climb constants (ClimbMaxStamina = 110, WallSlideTime = 1.2).
    /// </summary>
    public class Climb : PlayerState
    {
        private float originalGravityScale;
        private float climbTimer;

        public Climb(UnityPlayerController playerController) : base(playerController)
        {
        }

        public override int GetCelesteStateIndex() => StClimb;

        public override void Enter()
        {
            playerController.GetAnimator()?.Play("PlayerWallSlide");
            playerController.GetRigidbody().gravityScale = 0f;
            climbTimer = playerController.MaxClimbTime; // WallSlideTime from Celeste = 1.2
        }

        public override void Exit()
        {
            playerController.GetRigidbody().gravityScale = playerController.GetBaseGravityScale();
            
            // Start cooldown if climb time ran out
            if (climbTimer <= 0f)
            {
                playerController.StartWallCooldown();
            }
        }

        public override void FixedUpdate()
        {
            climbTimer -= Time.fixedDeltaTime;
            
            // Climb time exhausted - fall off wall
            if (climbTimer <= 0f)
            {
                playerController.SetState(new Fall(playerController));
                return;
            }

            // Handle climb movement (up/down on wall)
            float climbDirection = 0f;
            
            // Holding climb key = climb up (like Celeste's grab + up)
            if (playerController.IsClimbKeyPressed())
            {
                climbDirection = 1f;
            }
            // Pressing down = slide down
            else if (playerController.GetMoveVector().y < 0f)
            {
                climbDirection = playerController.GetMoveVector().y;
            }
            
            // Apply climb velocity (ClimbUpSpeed = -45, ClimbDownSpeed = 80 in Celeste)
            playerController.GetRigidbody().linearVelocityY = 
                playerController.WallClimbSpeed * climbDirection;

            // Handle wall jump
            if (playerController.HandleJump())
            {
                // Apply wall jump force (WallJumpHSpeed = 130 in Celeste)
                playerController.GetRigidbody().linearVelocityX += 
                    playerController.WallJumpForce * -playerController.Direction;
                
                // Flip direction
                playerController.Direction = -playerController.Direction;
                
                playerController.SetState(new Jump(playerController));
                return;
            }

            // Check if no longer touching wall or not pressing toward wall
            if (!playerController.IsTouchingWall() || !playerController.IsPressingTowardWall())
            {
                playerController.SetState(new Fall(playerController));
                return;
            }

            // Lock position when not actively climbing (prevent sliding)
            if (playerController.GetObjectVelocity().y <= 0f && climbDirection <= 0f)
            {
                playerController.SetObjectVelocity(0f, 0f);
            }
        }

        public override void Update()
        {
        }
    }
}
