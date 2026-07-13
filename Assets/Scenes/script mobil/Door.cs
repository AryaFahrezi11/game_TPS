using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // Variabel harus masuk di dalam kurung kurawal kelas
    [Header("Teleport Tujuan")]
    public Transform titikMulaiTerowongan;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerInventory inventory = collision.gameObject.GetComponent<PlayerInventory>();
        
        if (inventory != null && inventory.hasKey)
        {
            Debug.Log("Pintu Terbuka! Teleportasi ke Terowongan.");
            
            // 1. Ambil objek player yang menabrak pintu
            GameObject playerObj = collision.gameObject;

            // 2. WAJIB: Matikan sementara CharacterController agar Unity mengizinkan perpindahan instan
            CharacterController cc = playerObj.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // 3. Pindahkan posisi dan arah pandang player ke titik terowongan
            if (titikMulaiTerowongan != null) 
            {
                playerObj.transform.position = titikMulaiTerowongan.position;
                playerObj.transform.rotation = titikMulaiTerowongan.rotation;
            }
            else
            {
                Debug.LogWarning("Titik Terowongan belum dimasukkan di Inspector!");
            }

            // 4. Nyalakan lagi pengontrolnya
            if (cc != null) cc.enabled = true;
        }
        else
        {
            Debug.Log("Pintu Terkunci! Cari kuncinya dulu.");
        }
    }
}