using UnityEngine;

public class StoryIntroManager : MonoBehaviour
{
    [Header("UI Element")]
    [SerializeField] private GameObject panelCeritaAwal; // Hubungkan PanelCeritaAwal ke sini

    private void Start()
    {
        // Otomatis aktifkan panel cerita di awal game
        if (panelCeritaAwal != null)
        {
            panelCeritaAwal.SetActive(true);
            
            // BEKUKAN WAKTU: Biar player gak bisa jalan dan gak bisa kena duri saat baca
            Time.timeScale = 0f; 
            Debug.Log("Cerita dimulai. Waktu dibekukan.");
        }
    }

    // Fungsi yang dipanggil saat Tombol Mulai diklik
    public void TutupCeritaDanMulai()
    {
        if (panelCeritaAwal != null)
        {
            panelCeritaAwal.SetActive(false); // Sembunyikan panel cerita
            
            // JALANKAN WAKTU: Kembalikan waktu ke normal agar game bisa dimainkan
            Time.timeScale = 1f; 
            Debug.Log("Petualangan dimulai! Waktu berjalan normal.");
        }
    }
}