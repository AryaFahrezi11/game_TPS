using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ditambahkan untuk pindah scene!

public class LockedDoor : MonoBehaviour
{
    // Variabel ini akan muncul di Inspector agar kamu gampang ganti nama scene-nya
    public string namaSceneTujuan = "sceneGudang"; 

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        
        if (inventory != null)
        {
            if (inventory.hasKey == true)
            {
                Debug.Log("Pintu Terbuka! Pindah ke dalam rumah...");
                
                // Memuat scene baru berdasarkan nama yang diketik di Inspector
                SceneManager.LoadScene(namaSceneTujuan); 
            }
            else
            {
                Debug.Log("Pintu Terkunci! Cari kunci dulu.");
            }
        }
    }
}