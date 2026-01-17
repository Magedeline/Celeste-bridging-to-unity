using UnityEngine;

namespace Unity.Celeste
{
    /// <summary>
    /// Landing effect spawner for when player lands on ground.
    /// </summary>
    public class LandingEffect : MonoBehaviour
    {
        [Header("Effect Settings")]
        [SerializeField] private GameObject landingEffectPrefab;
        [SerializeField] private float effectDuration = 0.3f;
        [SerializeField] private bool useParticleSystem = true;
        
        [Header("Audio")]
        [SerializeField] private AudioClip landingSound;
        private AudioSource audioSource;
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null && landingSound != null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        
        public void SpawnLandingEffect(Vector3 position)
        {
            // Spawn visual effect
            if (landingEffectPrefab != null)
            {
                GameObject effect = Instantiate(landingEffectPrefab, position, Quaternion.identity);
                Destroy(effect, effectDuration);
            }
            else if (useParticleSystem)
            {
                // Create a simple particle burst
                CreateParticleBurst(position);
            }
            
            // Play sound
            if (audioSource != null && landingSound != null)
            {
                audioSource.PlayOneShot(landingSound);
            }
        }
        
        private void CreateParticleBurst(Vector3 position)
        {
            // Create a simple dust particle effect
            GameObject particleObj = new GameObject("LandingDust");
            particleObj.transform.position = position;
            
            var ps = particleObj.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startLifetime = 0.3f;
            main.startSpeed = 2f;
            main.startSize = 0.1f;
            main.startColor = new Color(0.8f, 0.8f, 0.8f, 0.5f);
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.maxParticles = 10;
            
            var emission = ps.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] 
            { 
                new ParticleSystem.Burst(0f, 5, 10) 
            });
            
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Hemisphere;
            shape.radius = 0.1f;
            
            ps.Play();
            
            Destroy(particleObj, effectDuration);
        }
    }
}
