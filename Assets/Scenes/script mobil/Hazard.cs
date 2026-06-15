using UnityEngine;

public class Hazard : MonoBehaviour
{
    public int damageAmount = 20;

    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang menyentuh punya script PlayerHealth
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        
        if (player != null)
        {
            player.TakeDamage(damageAmount);
        }
    }
}