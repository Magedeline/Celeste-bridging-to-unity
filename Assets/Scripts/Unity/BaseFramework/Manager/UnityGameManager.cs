using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Celeste
{
    /// <summary>
    /// Unity Game Manager that handles game state, checkpoints, player spawning, and save/load.
    /// Singleton pattern based on AdamNbz/celeste-2d-pc-version framework.
    /// </summary>
    public class UnityGameManager : MonoBehaviour
    {
        #region Singleton
        
        private static UnityGameManager _instance;
        
        public static UnityGameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<UnityGameManager>();
                    
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("UnityGameManager");
                        _instance = go.AddComponent<UnityGameManager>();
                    }
                }
                return _instance;
            }
        }
        
        #endregion
        
        #region Serialized Fields
        
        [Header("Game Manager Settings")]
        [SerializeField] private UnityPlayerController playerPrefab;
        [SerializeField] private float respawnDelay = 0.5f;
        
        [Header("Default Spawn")]
        [SerializeField] private Vector3 defaultSpawnPosition = Vector3.zero;
        
        #endregion
        
        #region Private Fields
        
        private UnityPlayerController currentPlayer;
        private List<UnityCheckpoint> checkpointsList = new List<UnityCheckpoint>();
        private UnityCheckpoint currentCheckpoint;
        private int deathCount = 0;
        private float playTime = 0f;
        private bool isPaused = false;
        
        // Save data
        private SaveSlot currentSaveSlot;
        
        #endregion
        
        #region Properties
        
        public int DeathCount => deathCount;
        public float PlayTime => playTime;
        public bool IsPaused => isPaused;
        
        #endregion
        
        #region Unity Lifecycle
        
        private void Awake()
        {
            // Singleton setup
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void Update()
        {
            if (!isPaused)
            {
                playTime += Time.deltaTime;
            }
        }
        
        #endregion
        
        #region Scene Management
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Refresh checkpoint list
            GetCheckPoints();
            
            // Spawn player if needed
            if (currentPlayer == null && playerPrefab != null)
            {
                SpawnPlayerAtCheckPoint();
            }
        }
        
        public void ChangeScene(string sceneName)
        {
            StartCoroutine(ChangeSceneCoroutine(sceneName));
        }
        
        private IEnumerator ChangeSceneCoroutine(string sceneName)
        {
            // Optional: Add fade out effect here
            yield return new WaitForSeconds(0.1f);
            
            SceneManager.LoadScene(sceneName);
        }
        
        public IEnumerator ChangeSceneAfterDelay(string sceneName, float delay)
        {
            yield return new WaitForSeconds(delay);
            ChangeScene(sceneName);
        }
        
        #endregion
        
        #region Player Management
        
        public UnityPlayerController GetPlayerController()
        {
            return currentPlayer;
        }
        
        public void SpawnPlayerAtCheckPoint()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("Player prefab is not assigned!");
                return;
            }
            
            // Destroy old player if exists
            if (currentPlayer != null)
            {
                Destroy(currentPlayer.gameObject);
            }
            
            // Instantiate new player
            currentPlayer = Instantiate(playerPrefab);
            
            // Set position to checkpoint or default
            if (currentCheckpoint != null)
            {
                currentPlayer.transform.position = currentCheckpoint.transform.position;
            }
            else
            {
                currentPlayer.transform.position = defaultSpawnPosition;
            }
            
            // Enable input
            currentPlayer.EnableInput();
        }
        
        #endregion
        
        #region Checkpoint System
        
        public void GetCheckPoints()
        {
            checkpointsList.Clear();
            
            var checkpoints = FindObjectsByType<UnityCheckpoint>(FindObjectsSortMode.None);
            checkpointsList.AddRange(checkpoints);
            
            // Sort by index
            checkpointsList.Sort((a, b) => a.Index.CompareTo(b.Index));
        }
        
        public void SetCheckpoint(UnityCheckpoint checkpoint)
        {
            if (currentCheckpoint == checkpoint) return;
            
            currentCheckpoint = checkpoint;
            
            // Activate all checkpoints up to this one
            foreach (var cp in checkpointsList)
            {
                if (cp.Index <= checkpoint.Index)
                {
                    cp.Activate();
                }
            }
            
            Debug.Log($"Checkpoint activated: {checkpoint.name}");
        }
        
        #endregion
        
        #region Death/Respawn
        
        public void OnPlayerDeath()
        {
            deathCount++;
            Debug.Log($"Player died! Total deaths: {deathCount}");
            
            StartCoroutine(RespawnAfterDelay());
        }
        
        private IEnumerator RespawnAfterDelay()
        {
            yield return new WaitForSeconds(respawnDelay);
            
            SpawnPlayerAtCheckPoint();
        }
        
        #endregion
        
        #region Save/Load System
        
        [System.Serializable]
        public class SaveSlot
        {
            public string checkpointName;
            public int deathCount;
            public float playTime;
            public string currentScene;
        }
        
        public void SaveGame(int slotId = 0)
        {
            SaveSlot slot = new SaveSlot
            {
                checkpointName = currentCheckpoint?.name ?? "",
                deathCount = deathCount,
                playTime = playTime,
                currentScene = SceneManager.GetActiveScene().name
            };
            
            string json = JsonUtility.ToJson(slot);
            PlayerPrefs.SetString($"SaveSlot_{slotId}", json);
            PlayerPrefs.Save();
            
            Debug.Log($"Game saved to slot {slotId}");
        }
        
        public void LoadGame(int slotId = 0)
        {
            string json = PlayerPrefs.GetString($"SaveSlot_{slotId}", "");
            
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning($"No save data in slot {slotId}");
                return;
            }
            
            SaveSlot slot = JsonUtility.FromJson<SaveSlot>(json);
            
            deathCount = slot.deathCount;
            playTime = slot.playTime;
            
            // Load scene if different
            if (slot.currentScene != SceneManager.GetActiveScene().name)
            {
                StartCoroutine(LoadSceneAndRestoreCheckpoint(slot));
            }
            else
            {
                RestoreCheckpoint(slot.checkpointName);
            }
            
            Debug.Log($"Game loaded from slot {slotId}");
        }
        
        private IEnumerator LoadSceneAndRestoreCheckpoint(SaveSlot slot)
        {
            SceneManager.LoadScene(slot.currentScene);
            yield return null; // Wait for scene to load
            
            RestoreCheckpoint(slot.checkpointName);
        }
        
        private void RestoreCheckpoint(string checkpointName)
        {
            if (string.IsNullOrEmpty(checkpointName)) return;
            
            GetCheckPoints();
            
            var checkpoint = checkpointsList.Find(cp => cp.name == checkpointName);
            if (checkpoint != null)
            {
                SetCheckpoint(checkpoint);
            }
        }
        
        #endregion
        
        #region Pause System
        
        public void PauseGame()
        {
            isPaused = true;
            Time.timeScale = 0f;
        }
        
        public void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f;
        }
        
        public void TogglePause()
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
        
        #endregion
    }
}
