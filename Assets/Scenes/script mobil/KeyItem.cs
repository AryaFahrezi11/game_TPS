using UnityEngine;

public class KeyItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Mengecek apakah yang menyentuh adalah Player
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        
        if (inventory != null)
        {
            inventory.GetKey(); // Mengubah status hasKey menjadi true
            Destroy(gameObject); // Kuncinya hilang dari map setelah diambil
        }
    }
}