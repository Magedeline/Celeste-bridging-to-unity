using UnityEngine;

namespace Unity.Celeste
{
    /// <summary>
    /// Spring/Bumper that bounces the player.
    /// Similar to Celeste's Spring class.
    /// </summary>
    public class Spring : MonoBehaviour
    {
        public enum SpringOrientation
        {
            Floor,
            Wall,
            Ceiling
        }
        
        [Header("Spring Settings")]
        [SerializeField] private SpringOrientation orientation = SpringOrientation.Floor;
        [SerializeField] private float bounceForce = 15f;
        [SerializeField] private bool refillDash = true;
        
        [Header("Visual")]
        [SerializeField] private Animator animator;
        
        [Header("Audio")]
        [SerializeField] private AudioClip bounceSound;
        private AudioSource audioSource;
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null && bounceSound != null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                BouncePlayer(other);
            }
        }
        
        private void BouncePlayer(Collider2D playerCollider)
        {
            var player = playerCollider.GetComponent<UnityPlayerController>();
            var rb = playerCollider.GetComponent<Rigidbody2D>();
            
            if (rb == null) return;
            
            Vector2 bounceDirection = GetBounceDirection();
            
            // Apply bounce force
            rb.linearVelocity = bounceDirection * bounceForce;
            
            // Refill dash if configured
            if (refillDash && player != null)
            {
                // The dash refill is handled by landing/ground check in most implementations
                // For immediate refill, you'd need to add a RefillDash method to the controller
            }
            
            // Play animation
            if (animator != null)
            {
                animator.SetTrigger("Bounce");
            }
            
            // Play sound
            if (audioSource != null && bounceSound != null)
            {
                audioSource.PlayOneShot(bounceSound);
            }
        }
        
        private Vector2 GetBounceDirection()
        {
            switch (orientation)
            {
                case SpringOrientation.Floor:
                    return Vector2.up;
                case SpringOrientation.Ceiling:
                    return Vector2.down;
                case SpringOrientation.Wall:
                    // Determine direction based on transform scale or facing
                    return transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            }
            return Vector2.up;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            Vector3 dir = GetBounceDirection();
            Gizmos.DrawRay(transform.position, dir);
            
            // Draw spring box
            Gizmos.DrawWireCube(transform.position, new Vector3(0.5f, 0.3f, 0f));
        }
    }
}
