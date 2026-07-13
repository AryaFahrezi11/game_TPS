using UnityEngine;
using UnityEngine.UI; // Wajib dipanggil untuk mengontrol UI Image
using System.Collections; // Wajib dipanggil untuk menggunakan Coroutine (Waktu Jeda)

public class LockedDoor : MonoBehaviour
{
    [Header("Teleport Tujuan")]
    public Transform titikMulaiTerowongan;

    [Header("Efek Transisi")]
    public Image layarHitam; // Tarik objek LayarHitamTransisi ke sini
    public float waktuFading = 0.5f; // Kecepatan layar menggelap/menerang (0.5 detik)

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        
        if (inventory != null && inventory.hasKey)
        {
            Debug.Log("Mulai Proses Transisi Teleportasi!");
            
            // ==========================================
            // BARIS BARU: Hapus kunci dari tas Player!
            // ==========================================
            inventory.hasKey = false; 
            
            // Panggil sihir Coroutine untuk menjalankan efek perlahan
            StartCoroutine(ProsesTeleportasi(other.gameObject));
        }
        else if (inventory != null && !inventory.hasKey)
        {
            Debug.Log("Pintu Terkunci! Cari kuncinya dulu.");
        }
    }

    // Ini adalah fungsi spesial (Coroutine) yang bisa berjalan seiring waktu
    private IEnumerator ProsesTeleportasi(GameObject playerObj)
    {
        // ==========================================
        // 1. FADE OUT (Layar perlahan menjadi hitam)
        // ==========================================
        if (layarHitam != null)
        {
            float waktu = 0;
            while (waktu < waktuFading)
            {
                waktu += Time.deltaTime;
                float alpha = Mathf.Lerp(0, 1, waktu / waktuFading);
                layarHitam.color = new Color(0, 0, 0, alpha);
                yield return null; // Tunggu ke frame berikutnya
            }
        }

        // ==========================================
        // 2. PROSES TELEPORTASI INSTAN (Saat layar gelap)
        // ==========================================
        CharacterController cc = playerObj.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        if (titikMulaiTerowongan != null) 
        {
            playerObj.transform.position = titikMulaiTerowongan.position;
            playerObj.transform.rotation = titikMulaiTerowongan.rotation;
            Physics.SyncTransforms(); 
        }

        if (cc != null) cc.enabled = true;

        // Beri jeda 0.2 detik biar kamera & lampu terowongan siap di posisi baru
        yield return new WaitForSeconds(0.2f);

        // ==========================================
        // 3. FADE IN (Layar perlahan kembali transparan)
        // ==========================================
        if (layarHitam != null)
        {
            float waktu = 0;
            while (waktu < waktuFading)
            {
                waktu += Time.deltaTime;
                float alpha = Mathf.Lerp(1, 0, waktu / waktuFading);
                layarHitam.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }
    }
}