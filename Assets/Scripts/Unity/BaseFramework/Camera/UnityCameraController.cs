using UnityEngine;
using UnityEngine.InputSystem;

namespace Unity.Celeste
{
    /// <summary>
    /// Camera Controller with multiple follow modes.
    /// Based on AdamNbz/celeste-2d-pc-version CameraController.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class UnityCameraController : MonoBehaviour
    {
        #region Enums
        
        /// <summary>
        /// Camera movement styles
        /// </summary>
        public enum CameraStyles
        {
            Locked,             // Camera does not move
            Overhead,           // Camera stays directly over target
            DistanceFollow,     // Camera stays within max distance from target
            OffsetFollow,       // Camera follows with an offset
            BetweenTargetAndMouse  // Camera stays between target and mouse
        }
        
        #endregion
        
        #region Serialized Fields
        
        [Header("Target")]
        [Tooltip("The target to follow with this camera")]
        public Transform target;
        
        [Header("Camera Movement")]
        [Tooltip("The way this camera moves")]
        public CameraStyles cameraMovementStyle = CameraStyles.OffsetFollow;
        
        [Header("Distance Settings")]
        [Tooltip("Maximum distance from target before camera moves")]
        [SerializeField] private float maxDistanceFromTarget = 5f;
        
        [Header("Offset Settings")]
        [Tooltip("Camera offset from target")]
        [SerializeField] private Vector2 cameraOffset = new Vector2(0f, 2f);
        
        [Header("Mouse Tracking")]
        [Tooltip("How much the camera tracks the mouse (0-1)")]
        [Range(0f, 1f)]
        [SerializeField] private float mouseTracking = 0.3f;
        
        [Header("Smoothing")]
        [Tooltip("Camera movement smoothing")]
        [SerializeField] private float smoothSpeed = 5f;
        
        [Header("Bounds")]
        [Tooltip("Enable camera bounds")]
        [SerializeField] private bool useBounds = false;
        [SerializeField] private Vector2 boundsMin = new Vector2(-100f, -100f);
        [SerializeField] private Vector2 boundsMax = new Vector2(100f, 100f);
        
        [Header("Input")]
        [SerializeField] private InputAction lookAction;
        
        #endregion
        
        #region Private Fields
        
        private Camera playerCamera;
        private Vector3 velocity = Vector3.zero;
        
        #endregion
        
        #region Unity Lifecycle
        
        private void OnEnable()
        {
            lookAction?.Enable();
        }
        
        private void OnDisable()
        {
            lookAction?.Disable();
        }
        
        private void Start()
        {
            playerCamera = GetComponent<Camera>();
        }
        
        private void Update()
        {
            // Auto-find player if target is null
            if (target == null)
            {
                var player = UnityGameManager.Instance?.GetPlayerController();
                if (player != null)
                {
                    target = player.transform;
                }
            }
            
            SetCameraPosition();
        }
        
        #endregion
        
        #region Camera Position
        
        private void SetCameraPosition()
        {
            if (target == null) return;
            
            Vector3 targetPosition = GetTargetPosition();
            Vector3 mousePosition = GetPlayerMousePosition();
            Vector3 desiredCameraPosition = ComputeCameraPosition(targetPosition, mousePosition);
            
            // Apply bounds
            if (useBounds)
            {
                desiredCameraPosition.x = Mathf.Clamp(desiredCameraPosition.x, boundsMin.x, boundsMax.x);
                desiredCameraPosition.y = Mathf.Clamp(desiredCameraPosition.y, boundsMin.y, boundsMax.y);
            }
            
            // Smooth movement
            if (smoothSpeed > 0)
            {
                transform.position = Vector3.SmoothDamp(
                    transform.position, 
                    desiredCameraPosition, 
                    ref velocity, 
                    1f / smoothSpeed
                );
            }
            else
            {
                transform.position = desiredCameraPosition;
            }
        }
        
        /// <summary>
        /// Gets the follow target's position
        /// </summary>
        public Vector3 GetTargetPosition()
        {
            if (target != null)
            {
                return target.position;
            }
            return transform.position;
        }
        
        /// <summary>
        /// Gets the mouse position in world coordinates
        /// </summary>
        public Vector3 GetPlayerMousePosition()
        {
            if (playerCamera == null) return transform.position;
            
            Vector2 mouseScreenPos = lookAction?.ReadValue<Vector2>() ?? Input.mousePosition;
            Vector3 mouseWorldPos = playerCamera.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = transform.position.z;
            
            return mouseWorldPos;
        }
        
        /// <summary>
        /// Computes the desired camera position based on the movement style
        /// </summary>
        public Vector3 ComputeCameraPosition(Vector3 targetPosition, Vector3 mousePosition)
        {
            Vector3 result = transform.position;
            
            switch (cameraMovementStyle)
            {
                case CameraStyles.Locked:
                    // Camera does not move
                    result = transform.position;
                    break;
                    
                case CameraStyles.Overhead:
                    // Camera stays directly over target
                    result = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
                    break;
                    
                case CameraStyles.DistanceFollow:
                    // Camera stays within max distance from target
                    result = transform.position;
                    if ((targetPosition - result).magnitude > maxDistanceFromTarget)
                    {
                        result = targetPosition + (result - targetPosition).normalized * maxDistanceFromTarget;
                    }
                    result.z = transform.position.z;
                    break;
                    
                case CameraStyles.OffsetFollow:
                    // Camera follows target at an offset
                    result = targetPosition + (Vector3)cameraOffset;
                    result.z = transform.position.z;
                    break;
                    
                case CameraStyles.BetweenTargetAndMouse:
                    // Camera stays between target and mouse position
                    Vector3 desiredPosition = Vector3.Lerp(targetPosition, mousePosition, mouseTracking);
                    Vector3 difference = desiredPosition - targetPosition;
                    difference = Vector3.ClampMagnitude(difference, maxDistanceFromTarget);
                    result = targetPosition + difference;
                    result.z = transform.position.z;
                    break;
            }
            
            return result;
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Set a new target for the camera to follow
        /// </summary>
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
        
        /// <summary>
        /// Set camera bounds
        /// </summary>
        public void SetBounds(Vector2 min, Vector2 max)
        {
            useBounds = true;
            boundsMin = min;
            boundsMax = max;
        }
        
        /// <summary>
        /// Instantly move camera to target position
        /// </summary>
        public void SnapToTarget()
        {
            if (target == null) return;
            
            Vector3 targetPosition = GetTargetPosition();
            Vector3 mousePosition = GetPlayerMousePosition();
            Vector3 desiredPosition = ComputeCameraPosition(targetPosition, mousePosition);
            
            transform.position = desiredPosition;
            velocity = Vector3.zero;
        }
        
        /// <summary>
        /// Shake the camera
        /// </summary>
        public void Shake(float intensity, float duration)
        {
            StartCoroutine(ShakeCoroutine(intensity, duration));
        }
        
        private System.Collections.IEnumerator ShakeCoroutine(float intensity, float duration)
        {
            Vector3 originalPosition = transform.position;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * intensity;
                float y = Random.Range(-1f, 1f) * intensity;
                
                transform.position = new Vector3(
                    originalPosition.x + x,
                    originalPosition.y + y,
                    originalPosition.z
                );
                
                elapsed += Time.deltaTime;
                intensity = Mathf.Lerp(intensity, 0f, elapsed / duration);
                
                yield return null;
            }
            
            transform.position = originalPosition;
        }
        
        #endregion
        
        #region Gizmos
        
        private void OnDrawGizmosSelected()
        {
            // Draw max distance circle
            if (target != null && cameraMovementStyle == CameraStyles.DistanceFollow)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(target.position, maxDistanceFromTarget);
            }
            
            // Draw bounds
            if (useBounds)
            {
                Gizmos.color = Color.cyan;
                Vector3 center = new Vector3(
                    (boundsMin.x + boundsMax.x) / 2f,
                    (boundsMin.y + boundsMax.y) / 2f,
                    transform.position.z
                );
                Vector3 size = new Vector3(
                    boundsMax.x - boundsMin.x,
                    boundsMax.y - boundsMin.y,
                    0.1f
                );
                Gizmos.DrawWireCube(center, size);
            }
        }
        
        #endregion
    }
}
