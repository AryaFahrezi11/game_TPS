using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // Wajib ditambahkan untuk menggunakan Coroutine

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public Slider healthBar;
    public static int healthSatuDunia = 100;

    [Header("Efek Damage")]
    public Image damageScreenImage; // Tarik objek DamageScreenEffect ke sini
    public float flashDuration = 0.1f; // Berapa lama merahnya muncul
    public Color flashColor = new Color(1f, 0f, 0f, 0.3f); // Warna merah transparansinya

    void Start()
    {
        if(healthBar != null) {
            healthBar.maxValue = maxHealth;
            healthBar.value = healthSatuDunia; // Ambil dari static
        }
        
        // Pastikan awalnya transparansinya 0
        if(damageScreenImage != null)
        {
            damageScreenImage.color = Color.clear;
        }
    }

    public void TakeDamage(int damage)
    {
        healthSatuDunia -= damage;
        
        if(healthBar != null) {
            healthBar.value = healthSatuDunia;
        }

        // --- TAMBAHAN: Panggil fungsi efek kedip ---
        if(damageScreenImage != null)
        {
            StopAllCoroutines(); // Hentikan kalau ada kedipan sebelumnya
            StartCoroutine(FlashDamageScreen());
        }

        if (CameraShake.Instance != null)
        {
            // Durasi: 0.15 detik, Kekuatan: 0.2
            CameraShake.Instance.StartShake(0.15f, 0.2f);
        }

        if (healthSatuDunia <= 0)
        {
            Die();
        }
    }

    // PASTIKAN ADA KATA 'public' DI DEPANNYA YA, AMRULL!
    public void TambahDarah(int jumlahPulih)
    {
        // healthSatuDunia adalah variabel static darah kamu yang kemarin
        healthSatuDunia += jumlahPulih;

        // Batasi agar tidak melebihi darah maksimal (100)
        if (healthSatuDunia > maxHealth)
        {
            healthSatuDunia = maxHealth;
        }

        // Update slider UI darah jika ada di scene
        if (healthBar != null)
        {
            healthBar.value = healthSatuDunia;
        }

        Debug.Log("Nyawa bertambah! Nyawa sekarang: " + healthSatuDunia);
    }

    // Fungsi Coroutine untuk membuat layar berkedip
    IEnumerator FlashDamageScreen()
    {
        damageScreenImage.color = flashColor; // Set ke warna merah
        yield return new WaitForSeconds(flashDuration); // Tunggu sebentar
        damageScreenImage.color = Color.clear; // Set kembali ke transparan
    }

    void Die()
    {
        healthSatuDunia = maxHealth; // Reset nyawa buat restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}