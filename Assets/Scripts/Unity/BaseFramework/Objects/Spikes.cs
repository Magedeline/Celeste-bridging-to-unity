using UnityEngine;

namespace Unity.Celeste
{
    /// <summary>
    /// Spikes hazard - directional hazards that kill the player.
    /// Similar to Celeste's Spikes class.
    /// </summary>
    public class Spikes : MonoBehaviour
    {
        public enum SpikeDirection
        {
            Up,
            Down,
            Left,
            Right
        }
        
        [Header("Spike Settings")]
        [SerializeField] private SpikeDirection direction = SpikeDirection.Up;
        [SerializeField] private bool triggerBlood = true;
        
        private BoxCollider2D spikeCollider;
        
        private void Awake()
        {
            spikeCollider = GetComponent<BoxCollider2D>();
            if (spikeCollider != null)
            {
                spikeCollider.isTrigger = true;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Check if player is approaching from the correct direction
                if (ShouldKillPlayer(other))
                {
                    var player = other.GetComponent<UnityPlayerController>();
                    if (player != null)
                    {
                        if (triggerBlood)
                        {
                            SpawnBloodEffect(other.transform.position);
                        }
                        player.Death();
                    }
                }
            }
        }
        
        private bool ShouldKillPlayer(Collider2D player)
        {
            Vector2 playerCenter = player.bounds.center;
            Vector2 spikeCenter = transform.position;
            
            Vector2 diff = playerCenter - spikeCenter;
            
            switch (direction)
            {
                case SpikeDirection.Up:
                    return diff.y > 0; // Player above spikes
                case SpikeDirection.Down:
                    return diff.y < 0; // Player below spikes
                case SpikeDirection.Left:
                    return diff.x < 0; // Player to the left
                case SpikeDirection.Right:
                    return diff.x > 0; // Player to the right
            }
            
            return true;
        }
        
        private void SpawnBloodEffect(Vector3 position)
        {
            // Simple particle effect for blood/death
            GameObject blood = new GameObject("BloodEffect");
            blood.transform.position = position;
            
            var ps = blood.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startLifetime = 0.5f;
            main.startSpeed = 3f;
            main.startSize = 0.1f;
            main.startColor = new Color(0.8f, 0.1f, 0.1f, 1f);
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.maxParticles = 20;
            main.gravityModifier = 1f;
            
            var emission = ps.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] 
            { 
                new ParticleSystem.Burst(0f, 10, 20) 
            });
            
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.1f;
            
            ps.Play();
            
            Destroy(blood, 1f);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            Vector3 dir = Vector3.up;
            switch (direction)
            {
                case SpikeDirection.Up: dir = Vector3.up; break;
                case SpikeDirection.Down: dir = Vector3.down; break;
                case SpikeDirection.Left: dir = Vector3.left; break;
                case SpikeDirection.Right: dir = Vector3.right; break;
            }
            
            Gizmos.DrawRay(transform.position, dir * 0.5f);
        }
    }
}
