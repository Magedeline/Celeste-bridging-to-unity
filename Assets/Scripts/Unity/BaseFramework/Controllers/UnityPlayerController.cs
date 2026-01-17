using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Celeste.States;
using Unity.Celeste.States.Player;

namespace Unity.Celeste
{
    /// <summary>
    /// Unity Player Controller that bridges Celeste's Player physics with Unity 2D.
    /// Based on AdamNbz/celeste-2d-pc-version framework.
    /// 
    /// Celeste Physics Constants (from original Player.cs):
    /// - MaxFall = 160f (max falling speed)
    /// - Gravity = 900f
    /// - MaxRun = 90f (max horizontal speed)
    /// - RunAccel = 1000f (acceleration)
    /// - JumpSpeed = -105f
    /// - VarJumpTime = 0.2f
    /// - DashSpeed = 240f
    /// - DashTime = 0.15f
    /// - WallSlideTime = 1.2f
    /// - ClimbMaxStamina = 110f
    /// - WallJumpHSpeed = 130f
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class UnityPlayerController : MonoBehaviour
    {
        #region Serialized Fields
        
        [Header("Input Actions")]
        [SerializeField] private InputAction moveAction;
        [SerializeField] private InputAction jumpAction;
        [SerializeField] private InputAction dashAction;
        [SerializeField] private InputAction climbAction;
        [SerializeField] private float bufferTime = 0.1f; // JumpGraceTime in Celeste
        
        [Header("Movement Settings (Celeste Values)")]
        [Tooltip("Max horizontal speed (Celeste MaxRun = 90)")]
        [SerializeField] private float movementSpeed = 9f; // Scaled: 90/10
        
        [Tooltip("Jump force (Celeste JumpSpeed = -105)")]
        [SerializeField] private float jumpForce = 10.5f; // Scaled: 105/10
        
        [Tooltip("Base gravity scale")]
        [SerializeField] private float baseGravityScale = 3f;
        
        [Header("Wall Climb Settings (Celeste Values)")]
        [Tooltip("Max climb time (Celeste WallSlideTime = 1.2)")]
        [SerializeField] private float maxClimbTime = 1.2f;
        
        [Tooltip("Wall climb cooldown")]
        [SerializeField] private float wallClimbCooldown = 0.5f;
        
        [Tooltip("Wall jump force (Celeste WallJumpHSpeed = 130)")]
        [SerializeField] private float wallJumpForce = 13f; // Scaled: 130/10
        
        [Tooltip("Wall climb speed (Celeste ClimbUpSpeed = -45)")]
        [SerializeField] private float wallClimbSpeed = 4.5f; // Scaled: 45/10
        
        [Header("Collision Detection")]
        [SerializeField] private Transform footPosition;
        [SerializeField] private Transform handPosition;
        [SerializeField] private float groundCheckRadius = 0.1f;
        [SerializeField] private LayerMask groundLayer;
        
        [Header("Hair Reference")]
        [SerializeField] private HairMovement hairMovement;
        
        [Header("VFX")]
        [SerializeField] private LandingEffect landingEffect;
        
        #endregion
        
        #region Private Fields
        
        private Rigidbody2D rb;
        private Animator animator;
        private Vector2 originalScale;
        
        // State machine
        private PlayerState currentState;
        private PlayerState nextState;
        
        // Direction (1 = right, -1 = left)
        private int _direction = 1;
        
        // Dash system
        private bool dashAble = true;
        
        // Buffer and coyote time
        private float jumpBuffer = 0f;
        private float coyoteTime = 0f;
        private int jumpCount = 0;
        
        // Wall climb
        private float wallCooldownTimer = 0f;
        
        // Death tracking
        private bool isDead = false;
        
        #endregion
        
        #region Properties
        
        public int Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                transform.localScale = new Vector2(_direction * Mathf.Abs(originalScale.x), originalScale.y);
            }
        }
        
        /// <summary>
        /// Dash speed scaled for Unity (Celeste DashSpeed = 240)
        /// </summary>
        public float DashSpeed => movementSpeed * 2.67f; // 240/90 ratio
        
        public float MaxClimbTime => maxClimbTime;
        public float WallClimbCooldown => wallClimbCooldown;
        public float WallJumpForce => wallJumpForce;
        public float WallClimbSpeed => wallClimbSpeed;
        
        #endregion
        
        #region Unity Lifecycle
        
        private void OnEnable()
        {
            moveAction?.Enable();
            jumpAction?.Enable();
            dashAction?.Enable();
            climbAction?.Enable();
        }
        
        private void OnDisable()
        {
            moveAction?.Disable();
            jumpAction?.Disable();
            dashAction?.Disable();
            climbAction?.Disable();
        }
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            
            // Set up default ground layer if not set
            if (groundLayer == 0)
            {
                groundLayer = LayerMask.GetMask("Ground", "Collision");
            }
        }
        
        private void Start()
        {
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D component is missing on UnityPlayerController.");
                return;
            }
            
            originalScale = transform.localScale;
            
            // Set initial state
            SetState(new Idle(this));
        }
        
        private void Update()
        {
            if (currentState != null)
            {
                currentState.Update();
            }
            
            // Handle coyote time and jump buffer
            UpdateJumpBuffers();
            
            // Handle jump input buffering
            if (jumpAction != null && jumpAction.WasPressedThisFrame())
            {
                jumpBuffer = bufferTime;
            }
        }
        
        private void FixedUpdate()
        {
            // Update wall cooldown
            if (wallCooldownTimer > 0)
                wallCooldownTimer -= Time.fixedDeltaTime;
            
            // Process state transition
            ProcessStateTransition();
            
            // Execute current state
            if (currentState != null)
            {
                currentState.FixedUpdate();
            }
            
            // Refresh dash on ground
            if (IsOnTheGround())
            {
                dashAble = true;
            }
        }
        
        #endregion
        
        #region State Machine
        
        private void ProcessStateTransition()
        {
            if (nextState != null && nextState != currentState && 
                (currentState == null || nextState.GetType() != currentState.GetType()))
            {
                currentState?.Exit();
                nextState.prevState = currentState;
                currentState = nextState;
                currentState.Enter();
            }
            else if (nextState == null && currentState == null)
            {
                Debug.LogWarning("Both current and next state are null!");
            }
            
            nextState = null;
        }
        
        public void SetState(PlayerState newState)
        {
            if (currentState == null)
            {
                // First state
                currentState = newState;
                currentState.Enter();
            }
            else
            {
                nextState = newState;
            }
        }
        
        public PlayerState GetState() => currentState;
        
        #endregion
        
        #region Movement Handling
        
        public bool HandleMovement()
        {
            Vector2 inputVector = moveAction?.ReadValue<Vector2>() ?? Vector2.zero;
            
            if (inputVector.x != 0)
            {
                Direction = inputVector.x > 0 ? 1 : -1;
                rb.linearVelocity = new Vector2(movementSpeed * Direction, rb.linearVelocity.y);
                return true;
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
            
            return false;
        }
        
        public bool HandleJump()
        {
            if (jumpBuffer > 0 && jumpCount < 2)
            {
                jumpCount++;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                SetState(new Jump(this));
                jumpBuffer = 0;
                return true;
            }
            return false;
        }
        
        public void HandleDash()
        {
            if (dashAction != null && dashAction.IsPressed() && IsCanDash())
            {
                rb.linearVelocity = new Vector2(Direction * DashSpeed, rb.linearVelocity.y);
                SetState(new Dash(this));
            }
        }
        
        #endregion
        
        #region Collision Detection
        
        public bool IsOnTheGround()
        {
            if (footPosition == null)
            {
                Debug.LogWarning("Foot position is null in IsOnTheGround");
                return Physics2D.Raycast(transform.position, Vector2.down, 0.2f, groundLayer);
            }
            
            return Physics2D.OverlapCircle(footPosition.position, groundCheckRadius, groundLayer);
        }
        
        public bool IsTouchingWall()
        {
            if (handPosition == null)
            {
                return Physics2D.Raycast(transform.position, Vector2.right * Direction, 0.2f, groundLayer);
            }
            
            return Physics2D.OverlapCircle(handPosition.position, groundCheckRadius, groundLayer);
        }
        
        public bool IsPressingTowardWall()
        {
            float inputX = GetMoveInputX();
            if (inputX == 0) return false;
            
            // Pressing toward the direction we're facing
            return Mathf.Sign(inputX) == Direction;
        }
        
        public bool CanClimbWall()
        {
            return wallCooldownTimer <= 0;
        }
        
        public void StartWallCooldown()
        {
            wallCooldownTimer = wallClimbCooldown;
        }
        
        #endregion
        
        #region Input Helpers
        
        public Vector2 GetMoveVector()
        {
            return moveAction?.ReadValue<Vector2>() ?? Vector2.zero;
        }
        
        public float GetMoveInputX()
        {
            return moveAction?.ReadValue<Vector2>().x ?? 0f;
        }
        
        public bool IsClimbKeyPressed()
        {
            return climbAction?.IsPressed() ?? false;
        }
        
        #endregion
        
        #region Velocity/Physics
        
        public Vector2 GetObjectVelocity()
        {
            return rb.linearVelocity;
        }
        
        public void SetObjectVelocity(float x, float y)
        {
            rb.linearVelocity = new Vector2(x, y);
        }
        
        public Rigidbody2D GetRigidbody() => rb;
        
        public float GetBaseGravityScale() => baseGravityScale;
        
        #endregion
        
        #region Dash System
        
        public bool IsCanDash()
        {
            return currentState?.GetStateName() != "Dash" && dashAble;
        }
        
        public void TurnOffDashAble()
        {
            dashAble = false;
        }
        
        #endregion
        
        #region Animation
        
        public Animator GetAnimator() => animator;
        
        #endregion
        
        #region VFX
        
        public void SpawnLandingEffect()
        {
            landingEffect?.SpawnLandingEffect(footPosition?.position ?? transform.position);
        }
        
        #endregion
        
        #region Death/Respawn
        
        public void Death()
        {
            if (isDead) return;
            
            isDead = true;
            SetState(new Death(this));
            UnityGameManager.Instance?.OnPlayerDeath();
        }
        
        public void DisableInput()
        {
            rb.angularVelocity = 0;
            moveAction?.Disable();
            jumpAction?.Disable();
            dashAction?.Disable();
            climbAction?.Disable();
        }
        
        public void EnableInput()
        {
            isDead = false;
            moveAction?.Enable();
            jumpAction?.Enable();
            dashAction?.Enable();
            climbAction?.Enable();
        }
        
        public void SetPlayerPosition(Vector2 newPos)
        {
            transform.position = newPos;
        }
        
        #endregion
        
        #region Private Helpers
        
        private void UpdateJumpBuffers()
        {
            // Reset jump count on ground or wall
            if (IsOnTheGround() || IsTouchingWall())
            {
                jumpCount = 0;
                coyoteTime = bufferTime;
            }
            
            // Coyote time countdown
            if (coyoteTime > 0)
            {
                coyoteTime -= Time.deltaTime;
            }
            else if (coyoteTime < 0)
            {
                coyoteTime = 0;
            }
            
            // Set jump count to 1 if falling without coyote time
            if (jumpCount == 0 && currentState?.GetStateName() == "Fall" && coyoteTime <= 0)
            {
                jumpCount = 1;
            }
            
            // Jump buffer countdown
            if (jumpBuffer > 0)
            {
                jumpBuffer -= Time.deltaTime;
            }
            else if (jumpBuffer < 0)
            {
                jumpBuffer = 0;
            }
        }
        
        #endregion
        
        #region Gizmos
        
        private void OnDrawGizmosSelected()
        {
            // Draw ground check
            if (footPosition != null)
            {
                Gizmos.color = IsOnTheGround() ? Color.green : Color.red;
                Gizmos.DrawWireSphere(footPosition.position, groundCheckRadius);
            }
            
            // Draw wall check
            if (handPosition != null)
            {
                Gizmos.color = IsTouchingWall() ? Color.blue : Color.yellow;
                Gizmos.DrawWireSphere(handPosition.position, groundCheckRadius);
            }
        }
        
        #endregion
    }
}
