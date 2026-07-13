using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [Header("Komponen Wajib")]
    public CharacterController cc; 
    public AudioSource audioLangkah; 

    [Header("Pengaturan Jeda Suara")]
    public float jedaJalan = 0.4f;  // Jeda saat jalan biasa
    public float jedaLari = 0.25f;  // Jeda lebih cepat saat lari
    
    // Angka penentu (karena walkSpeed kamu 5 dan runSpeed 8, batas amannya kita taruh di 6)
    public float batasKecepatanLari = 6f; 

    private float timer; 
    private float jedaSaatIni;

    void Start()
    {
        // Di awal game, siapkan jeda default
        jedaSaatIni = jedaJalan;
        timer = jedaSaatIni; 
    }

    void Update()
    {
        // Baca kecepatan asli karakter saat ini
        float kecepatan = cc.velocity.magnitude;

        if (cc.isGrounded == true && kecepatan > 0.1f)
        {
            // 1. Tentukan jeda suara berdasarkan laju karakter
            if (kecepatan > batasKecepatanLari)
            {
                jedaSaatIni = jedaLari; // Pakai tempo cepat
            }
            else
            {
                jedaSaatIni = jedaJalan; // Pakai tempo santai
            }

            // 2. Eksekusi bunyi
            if (timer >= jedaSaatIni)
            {
                audioLangkah.Play();
                timer = 0f; 
            }

            timer += Time.deltaTime;
        }
        else
        {
            // Reset timer agar siap berbunyi instan saat melangkah lagi
            timer = jedaSaatIni;
        }
    }
}