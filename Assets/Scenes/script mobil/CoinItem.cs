using UnityEngine;

public class CoinItem : MonoBehaviour
{
    public int nilaiKoin = 1;

    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang menyentuh koin adalah Player
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            inventory.TambahKoin(nilaiKoin); // Panggil fungsi tambah koin
            Destroy(gameObject); // Koin menghilang setelah diambil
        }
    }
}