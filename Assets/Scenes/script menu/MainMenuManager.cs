using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject panelKonfirmasi; // Hubungkan PanelKonfirmasi ke sini
    [SerializeField] private GameObject panelSettings;

    // 1. Fungsi yang dipanggil saat Tombol Exit di Main Menu ditekan
    public void TekanExit()
    {
        if (panelKonfirmasi != null)
        {
            panelKonfirmasi.SetActive(true); // Munculkan panel pertanyaan
        }
    }

    // 2. Fungsi jika pemain memilih "YA" (Benar-benar keluar)
    public void PilihanYa()
    {
        Debug.Log("Game Ditutup!");
        
        // Perintah resmi Unity untuk menutup aplikasi game PC/Android
        Application.Quit(); 

        #if UNITY_EDITOR
        // Trik khusus agar tombol keluar tetap bekerja saat kamu ngetes di dalam Editor Unity
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // 3. Fungsi jika pemain memilih "TIDAK" (Batal keluar)
    public void PilihanTidak()
    {
        if (panelKonfirmasi != null)
        {
            panelKonfirmasi.SetActive(false); // Sembunyikan kembali panel pertanyaan
        }
    }

    // ================= FITUR SETTINGS (BARU) =================

    // 2. Logika Mengubah Kualitas Grafik (Dipanggil oleh Dropdown TMP)
    public void SetKualitasGrafik(int indeksKualitas)
    {
        // Fungsi bawaan Unity untuk mengubah grafik berdasarkan indeks (0 = Low, 1 = Medium, 2 = High, dst)
        QualitySettings.SetQualityLevel(indeksKualitas);
        Debug.Log("Kualitas grafik diubah ke indeks: " + indeksKualitas);
    }

    // 3. Logika Mengubah Volume Suara (Dipanggil oleh Slider)
    public void SetVolume(float nilaiVolume)
    {
        // Mengatur volume master pendengar di dalam game (rentang nilai slider wajib 0 sampai 1)
        AudioListener.volume = nilaiVolume;
        Debug.Log("Volume game sekarang: " + nilaiVolume);
    }
}