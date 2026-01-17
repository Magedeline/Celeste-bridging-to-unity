using System.Collections.Generic;
using UnityEngine;

namespace Unity.Celeste
{
    /// <summary>
    /// Hair movement system for Celeste-style trailing hair effect.
    /// Based on AdamNbz/celeste-2d-pc-version HairMovement by Symbiosinx.
    /// 
    /// Creates a physics-based hair that follows the player with a trailing effect.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class HairMovement : MonoBehaviour
    {
        #region Serialized Fields
        
        [Header("Hair Settings")]
        [SerializeField, Range(1, 100)] 
        private int stiffness = 30;
        
        [SerializeField, Range(0f, 1f)] 
        private float minSeparation = 0.1f;
        
        [SerializeField, Range(0f, 1f)] 
        private float maxSeparation = 0.7f;
        
        [SerializeField] 
        private Color hairColor = new Color(0.67f, 0.20f, 0.20f); // Celeste red
        
        [SerializeField, Min(1)] 
        private int pixelsPerUnit = 16;
        
        [SerializeField, Min(0)] 
        private int pixelBuffer = 1;
        
        [Header("Hair Structure")]
        [SerializeField]
        private List<HairBlob> hairBlobs = new List<HairBlob>();
        
        [Header("Follow Player Scale")]
        [SerializeField] 
        private Transform playerTransform;
        
        [SerializeField] 
        private bool followPlayerScale = true;
        
        [Header("Debug")]
        [SerializeField] 
        private bool showGizmos = false;
        
        #endregion
        
        #region Private Fields
        
        private SpriteRenderer spriteRenderer;
        private bool flipX = false;
        private float previousScaleSign = 1f;
        
        #endregion
        
        #region Properties
        
        public bool FlipX
        {
            get => flipX;
            set
            {
                if (flipX != value)
                {
                    flipX = value;
                    // Flip all blob offsets
                    for (int i = 0; i < hairBlobs.Count; i++)
                    {
                        var blob = hairBlobs[i];
                        blob.offset.x = -blob.offset.x;
                        hairBlobs[i] = blob;
                    }
                }
            }
        }
        
        public Color HairColor
        {
            get => hairColor;
            set
            {
                hairColor = value;
            }
        }
        
        #endregion
        
        #region Hair Blob Structure
        
        [System.Serializable]
        public struct HairBlob
        {
            public Vector2 offset;
            public float radius;
            [HideInInspector] public Vector3 position;
            [HideInInspector] public Vector3 velocity;
            
            public HairBlob(Vector2 offset, float radius)
            {
                this.offset = offset;
                this.radius = radius;
                this.position = Vector3.zero;
                this.velocity = Vector3.zero;
            }
        }
        
        #endregion
        
        #region Unity Lifecycle
        
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            // Auto-find player transform if not set
            if (playerTransform == null)
            {
                playerTransform = transform.parent;
            }
            
            InitializeBlobs();
        }
        
        private void Update()
        {
            UpdateFlipFromPlayerScale();
            
            if (hairBlobs.Count > 0)
            {
                UpdateBlobPositions();
                UpdateHairVisual();
            }
        }
        
        #endregion
        
        #region Initialization
        
        public void InitializeBlobs()
        {
            // Set up default hair blobs if none exist
            if (hairBlobs.Count == 0)
            {
                SetupDefaultHair();
            }
            
            // Initialize blob positions
            Vector2 offset = Vector2.zero;
            for (int i = 0; i < hairBlobs.Count; i++)
            {
                var blob = hairBlobs[i];
                offset += blob.offset;
                blob.position = (Vector3)offset + transform.position;
                hairBlobs[i] = blob;
            }
        }
        
        private void SetupDefaultHair()
        {
            // Default Celeste-style hair with 5 blobs
            hairBlobs = new List<HairBlob>
            {
                new HairBlob(new Vector2(0f, 0f), 0.25f),
                new HairBlob(new Vector2(-0.1f, -0.15f), 0.22f),
                new HairBlob(new Vector2(-0.1f, -0.12f), 0.19f),
                new HairBlob(new Vector2(-0.08f, -0.1f), 0.16f),
                new HairBlob(new Vector2(-0.06f, -0.08f), 0.13f)
            };
        }
        
        #endregion
        
        #region Update Methods
        
        private void UpdateFlipFromPlayerScale()
        {
            if (!followPlayerScale || playerTransform == null) return;
            
            float parentScaleSign = Mathf.Sign(playerTransform.localScale.x);
            
            // If scale sign changed, flip the hair
            if (parentScaleSign != previousScaleSign)
            {
                FlipX = !FlipX;
                previousScaleSign = parentScaleSign;
            }
        }
        
        private void UpdateBlobPositions()
        {
            // First blob follows the transform directly
            if (hairBlobs.Count > 0)
            {
                var firstBlob = hairBlobs[0];
                firstBlob.position = transform.position + (Vector3)firstBlob.offset;
                hairBlobs[0] = firstBlob;
            }
            
            // Subsequent blobs follow with physics
            for (int i = 1; i < hairBlobs.Count; i++)
            {
                var blob = hairBlobs[i];
                var prevBlob = hairBlobs[i - 1];
                
                // Target position relative to previous blob
                Vector3 targetPos = prevBlob.position + (Vector3)blob.offset;
                
                // Apply spring physics
                Vector3 diff = targetPos - blob.position;
                blob.velocity += diff * stiffness * Time.deltaTime;
                blob.velocity *= 0.9f; // Damping
                blob.position += blob.velocity * Time.deltaTime;
                
                // Constrain distance from previous blob
                Vector3 separation = blob.position - prevBlob.position;
                float distance = separation.magnitude;
                
                if (distance > maxSeparation)
                {
                    blob.position = prevBlob.position + separation.normalized * maxSeparation;
                }
                else if (distance < minSeparation)
                {
                    blob.position = prevBlob.position + separation.normalized * minSeparation;
                }
                
                hairBlobs[i] = blob;
            }
        }
        
        private void UpdateHairVisual()
        {
            // Create a simple visual representation using line renderer or sprites
            // For now, we'll rely on gizmos for visualization
            // A full implementation would generate a sprite texture dynamically
            
            spriteRenderer.color = hairColor;
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Set the hair color (for dash states, etc.)
        /// </summary>
        public void SetColor(Color color)
        {
            hairColor = color;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
            }
        }
        
        /// <summary>
        /// Reset hair positions instantly
        /// </summary>
        public void ResetPositions()
        {
            InitializeBlobs();
        }
        
        /// <summary>
        /// Add a force impulse to the hair
        /// </summary>
        public void AddImpulse(Vector2 force)
        {
            for (int i = 0; i < hairBlobs.Count; i++)
            {
                var blob = hairBlobs[i];
                blob.velocity += (Vector3)force;
                hairBlobs[i] = blob;
            }
        }
        
        #endregion
        
        #region Celeste Hair Colors
        
        /// <summary>
        /// Celeste hair color constants
        /// </summary>
        public static class Colors
        {
            public static readonly Color Normal = new Color(0.67f, 0.20f, 0.20f);      // AC3232
            public static readonly Color NoDash = new Color(0.27f, 0.72f, 1f);          // 44B7FF
            public static readonly Color TwoDash = new Color(1f, 0.43f, 0.94f);         // FF6DEF
            public static readonly Color Feather = new Color(0.95f, 0.92f, 0.43f);      // F2EB6D
            public static readonly Color Flash = Color.white;
        }
        
        #endregion
        
        #region Gizmos
        
        private void OnDrawGizmos()
        {
            if (!showGizmos) return;
            
            Gizmos.color = hairColor;
            
            for (int i = 0; i < hairBlobs.Count; i++)
            {
                var blob = hairBlobs[i];
                Vector3 pos = Application.isPlaying ? blob.position : transform.position;
                
                Gizmos.DrawWireSphere(pos, blob.radius);
                
                if (i > 0)
                {
                    var prevBlob = hairBlobs[i - 1];
                    Vector3 prevPos = Application.isPlaying ? prevBlob.position : transform.position;
                    Gizmos.DrawLine(prevPos, pos);
                }
            }
        }
        
        #endregion
    }
}
