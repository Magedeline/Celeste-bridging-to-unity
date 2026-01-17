using UnityEngine;

namespace Unity.Celeste
{
    /// <summary>
    /// Conveyor block that moves and carries the player.
    /// Based on AdamNbz/celeste-2d-pc-version ConveyorBlock.
    /// </summary>
    public class ConveyorBlock : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float speed = 1f;
        [SerializeField] private float boostForce = 2f;
        
        [Header("Waypoints")]
        [SerializeField] private Transform startWaypoint;
        [SerializeField] private Transform endWaypoint;
        
        private float direction = 1f;
        private bool isMoving = false;
        private bool isCollidingWithPlayer = false;
        private Rigidbody2D rb;
        private Transform minWaypoint;
        private Transform maxWaypoint;
        
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            
            if (startWaypoint != null && endWaypoint != null)
            {
                // Determine which waypoint is min/max based on x position
                if (startWaypoint.position.x <= endWaypoint.position.x)
                {
                    minWaypoint = startWaypoint;
                    maxWaypoint = endWaypoint;
                }
                else
                {
                    minWaypoint = endWaypoint;
                    maxWaypoint = startWaypoint;
                }
            }
        }
        
        private float CalculateMovedRatio()
        {
            if (minWaypoint == null || maxWaypoint == null) return 0f;
            
            float totalDistance = maxWaypoint.position.x - minWaypoint.position.x;
            if (totalDistance <= 0) return 0f;
            
            float currentDistance = transform.position.x - minWaypoint.position.x;
            return Mathf.Clamp01(currentDistance / totalDistance);
        }
        
        private float CalculateBoost()
        {
            return boostForce * direction;
        }
        
        private void FixedUpdate()
        {
            if (startWaypoint == null || endWaypoint == null) return;
            
            // Move the block
            if (isMoving)
            {
                transform.position += new Vector3(direction * speed * Time.fixedDeltaTime, 0f, 0f);
            }
            
            // Check bounds and reverse direction
            if (direction > 0)
            {
                if (transform.position.x >= maxWaypoint.position.x)
                {
                    isMoving = false;
                    direction *= -1f;
                    transform.position = new Vector3(maxWaypoint.position.x, transform.position.y, transform.position.z);
                }
            }
            else
            {
                if (transform.position.x <= minWaypoint.position.x)
                {
                    isMoving = false;
                    direction *= -1f;
                    transform.position = new Vector3(minWaypoint.position.x, transform.position.y, transform.position.z);
                }
            }
            
            // Move player with block
            if (isCollidingWithPlayer && isMoving)
            {
                var player = UnityGameManager.Instance?.GetPlayerController();
                if (player != null)
                {
                    player.SetPlayerPosition(
                        player.transform.position + new Vector3(direction * speed * Time.fixedDeltaTime, 0f, 0f)
                    );
                }
            }
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Only activate if player is on top of the block
                var spriteRenderer = GetComponent<SpriteRenderer>();
                var playerSpriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
                
                if (spriteRenderer != null && playerSpriteRenderer != null)
                {
                    if (spriteRenderer.bounds.min.y + 0.2f <= playerSpriteRenderer.bounds.max.y)
                    {
                        isMoving = true;
                        isCollidingWithPlayer = true;
                    }
                }
                else
                {
                    isMoving = true;
                    isCollidingWithPlayer = true;
                }
            }
        }
        
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Apply boost when player leaves
                if (collision.rigidbody != null)
                {
                    collision.rigidbody.linearVelocityX += CalculateBoost();
                }
                isCollidingWithPlayer = false;
            }
        }
        
        private void OnDrawGizmos()
        {
            if (startWaypoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(startWaypoint.position, 0.3f);
            }
            
            if (endWaypoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(endWaypoint.position, 0.3f);
            }
            
            if (startWaypoint != null && endWaypoint != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(startWaypoint.position, endWaypoint.position);
            }
        }
    }
}
