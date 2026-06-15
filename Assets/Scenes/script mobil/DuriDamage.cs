using UnityEngine;

public class DuriDamage : MonoBehaviour
{
    public int jumlahDamage = 20;

    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang menyentuh duri adalah Player
        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(jumlahDamage); // Darah berkurang
            Debug.Log("Aduh! Kena duri.");
        }
    }
}