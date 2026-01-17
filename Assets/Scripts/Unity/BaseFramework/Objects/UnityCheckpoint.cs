using UnityEngine;

namespace Unity.Celeste
{
    /// <summary>
    /// Checkpoint component for save/respawn positions.
    /// Based on AdamNbz/celeste-2d-pc-version framework.
    /// </summary>
    public class UnityCheckpoint : MonoBehaviour
    {
        [Header("Checkpoint Settings")]
        [SerializeField] private int index = 0;
        [SerializeField] private bool isActive = false;
        
        [Header("Visual")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color activeColor = Color.green;
        [SerializeField] private Color inactiveColor = Color.gray;
        
        [Header("Audio")]
        [SerializeField] private AudioClip activateSound;
        
        private AudioSource audioSource;
        
        public int Index => index;
        public bool IsActive => isActive;
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null && activateSound != null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
            
            UpdateVisual();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !isActive)
            {
                UnityGameManager.Instance?.SetCheckpoint(this);
            }
        }
        
        public void Activate()
        {
            if (isActive) return;
            
            isActive = true;
            UpdateVisual();
            
            // Play sound
            if (audioSource != null && activateSound != null)
            {
                audioSource.PlayOneShot(activateSound);
            }
        }
        
        public void Deactivate()
        {
            isActive = false;
            UpdateVisual();
        }
        
        private void UpdateVisual()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = isActive ? activeColor : inactiveColor;
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = isActive ? Color.green : Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            
            // Draw index label
            #if UNITY_EDITOR
            UnityEditor.Handles.Label(transform.position + Vector3.up * 0.7f, $"CP {index}");
            #endif
        }
    }
}
