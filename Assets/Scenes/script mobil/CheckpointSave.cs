using UnityEngine;

public class CheckpointSave : MonoBehaviour
{
    // Fungsi ini akan aktif otomatis saat ada benda yang menyentuh sensor
    private void OnTriggerEnter(Collider other)
    {
        // Mengecek apakah yang menyentuh sensor ini adalah Player
        if (other.CompareTag("Player"))
        {
            // Mencari script PlayerTPS di badan Player
            PlayerTPS playerScript = other.GetComponent<PlayerTPS>();
            
            if (playerScript != null)
            {
                // Eksekusi fungsi simpan posisi yang sudah kita buat sebelumnya!
                playerScript.SimpanProgressGame();
                Debug.Log("Checkpoint Tersentuh! Posisi di terowongan berhasil disimpan.");

                // Mematikan sensor ini agar game tidak nyangkut nyimpen terus-terusan
                // setiap kali pemain mondar-mandir di area ini
                gameObject.SetActive(false); 
            }
        }
    }
}