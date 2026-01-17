using UnityEngine;

namespace Unity.Celeste
{
    /// <summary>
    /// Hazardous object that kills the player on contact.
    /// Based on AdamNbz/celeste-2d-pc-version hazardous.cs
    /// </summary>
    public class Hazardous : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool destroyOnContact = false;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<UnityPlayerController>();
                if (player != null)
                {
                    player.Death();
                }
                else
                {
                    UnityGameManager.Instance?.OnPlayerDeath();
                }
                
                if (destroyOnContact)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
