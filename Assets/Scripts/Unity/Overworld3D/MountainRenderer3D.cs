using UnityEngine;
using Unity.Compatibility;

namespace Unity.Celeste.Overworld3D
{
    /// <summary>
    /// Unity 3D renderer for the Celeste mountain overworld
    /// Maintains compatibility with original 2D mountain while adding 3D depth and models
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class MountainRenderer3D : MonoBehaviour
    {
        [Header("3D Mountain Settings")]
        public GameObject mountainModel;
        public GameObject birdModel;
        public GameObject moonModel;
        public GameObject buildingModel;
        public GameObject heartModel;
        
        [Header("Camera Settings")]
        public float cameraDistance = 10f;
        public float cameraHeight = 5f;
        public float rotationSpeed = 0.5f;
        public bool autoRotate = true;
        
        [Header("Lighting")]
        public Light mainLight;
        public Gradient skyboxGradient;
        public Material skyboxMaterial;
        
        [Header("Compatibility")]
        public bool maintain2DCompatibility = true;
        public RenderTexture renderTo2D;
        
        private Camera mountainCamera;
        private float currentRotation = 0f;
        private Vector3 originalCameraPosition;
        
        void Awake()
        {
            mountainCamera = GetComponent<Camera>();
            originalCameraPosition = transform.position;
            
            SetupMountain3D();
            SetupLighting();
        }
        
        void Start()
        {
            if (maintain2DCompatibility && renderTo2D != null)
            {
                mountainCamera.targetTexture = renderTo2D;
            }
        }
        
        void Update()
        {
            if (autoRotate)
            {
                RotateMountain();
            }
            
            UpdateLighting();
            
            if (maintain2DCompatibility)
            {
                UpdateCompatibilityMode();
            }
        }
        
        private void SetupMountain3D()
        {
            // Position 3D models in the scene
            if (mountainModel != null)
            {
                mountainModel.transform.position = Vector3.zero;
                mountainModel.transform.rotation = Quaternion.identity;
            }
            
            if (birdModel != null)
            {
                // Position bird based on game progress
                PositionBird();
            }
            
            if (moonModel != null)
            {
                moonModel.transform.position = new Vector3(0, 20, -50);
            }
            
            // Setup camera position
            UpdateCameraPosition();
        }
        
        private void SetupLighting()
        {
            if (mainLight == null)
            {
                // Create main directional light if not assigned
                GameObject lightGO = new GameObject("Mountain Light");
                mainLight = lightGO.AddComponent<Light>();
                mainLight.type = LightType.Directional;
                mainLight.transform.rotation = Quaternion.Euler(50, -30, 0);
            }
            
            // Setup skybox
            if (skyboxMaterial != null)
            {
                RenderSettings.skybox = skyboxMaterial;
            }
        }
        
        private void RotateMountain()
        {
            currentRotation += rotationSpeed * Time.deltaTime;
            
            // Rotate camera around the mountain
            float x = Mathf.Sin(currentRotation) * cameraDistance;
            float z = Mathf.Cos(currentRotation) * cameraDistance;
            
            transform.position = originalCameraPosition + new Vector3(x, cameraHeight, z);
            transform.LookAt(Vector3.zero);
        }
        
        private void UpdateLighting()
        {
            if (mainLight != null && skyboxGradient != null)
            {
                // Update lighting based on time or game state
                float timeOfDay = Time.time * 0.1f % 1f;
                Color skyColor = skyboxGradient.Evaluate(timeOfDay);
                
                mainLight.color = skyColor;
                RenderSettings.ambientLight = skyColor * 0.3f;
            }
        }
        
        private void UpdateCompatibilityMode()
        {
            // Ensure 2D compatibility by rendering to texture that can be used by original 2D system
            if (renderTo2D != null)
            {
                // The render texture can be read by the original Celeste rendering system
                // This allows seamless integration between 3D mountain and 2D UI/gameplay
            }
        }
        
        private void PositionBird()
        {
            if (birdModel == null) return;
            
            // TODO: Position bird based on current chapter/game progress
            // This would integrate with Celeste's save system to show appropriate bird position
            
            Vector3 birdPosition = Vector3.zero;
            
            // Example positioning for different chapters
            // This would be replaced with actual save data integration
            birdPosition = new Vector3(0, 5, 10);
            
            birdModel.transform.position = birdPosition;
        }
        
        private void UpdateCameraPosition()
        {
            transform.position = originalCameraPosition + new Vector3(0, cameraHeight, -cameraDistance);
            transform.LookAt(Vector3.zero);
        }
        
        // Public methods for integration with Celeste systems
        public void SetChapterProgress(int chapter, bool completed)
        {
            // Update 3D models based on chapter completion
            // This would be called by the main Celeste system
            
            if (completed)
            {
                // Show completion effects, unlock areas, etc.
                Debug.Log($"Chapter {chapter} completed - updating 3D mountain");
            }
        }
        
        public void ShowBird(Vector3 targetPosition)
        {
            if (birdModel != null)
            {
                // Animate bird to target position
                StartCoroutine(MoveBirdTo(targetPosition));
            }
        }
        
        private System.Collections.IEnumerator MoveBirdTo(Vector3 target)
        {
            if (birdModel == null) yield break;
            
            Vector3 startPos = birdModel.transform.position;
            float duration = 2f;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                
                birdModel.transform.position = Vector3.Lerp(startPos, target, t);
                yield return null;
            }
        }
        
        // Integration with original mountain system
        public void SyncWith2DMountain(Vector3 cameraPosition, Vector3 lookDirection)
        {
            if (maintain2DCompatibility)
            {
                // Sync 3D camera with 2D mountain camera state
                transform.position = cameraPosition.ToUnity();
                transform.LookAt(transform.position + lookDirection.ToUnity());
            }
        }
    }
}