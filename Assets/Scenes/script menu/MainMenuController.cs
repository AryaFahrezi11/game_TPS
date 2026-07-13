using UnityEngine;
using UnityEngine.SceneManagement; // Wajib dipanggil untuk pindah scene
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Audio Interaksi")]
    public AudioSource sfxKlikTombol; // Slot untuk suara klik
    public AudioSource bgmAudioSource; // Slot untuk lagu BGM

    [Header("Panel UI")]
    public GameObject panelSetting; // Slot untuk pop-up panel

    // Fungsi Start akan berjalan otomatis tepat saat Main Menu terbuka
    [Header("Tombol Continue")]
    public Button CtnBtn; // TAMBAHKAN BARIS INI DI SINI

    void Start()
    {
        if (PlayerPrefs.HasKey("LevelTerakhir"))
        {
            // Baris 20
            CtnBtn.interactable = true; 
        }
        else
        {
            // Baris 25
            CtnBtn.interactable = false; 
        }
    }

    public void TombolMulaiBaru()
    {
        BunyikanKlik();

        // Bersihkan semua data save lama termasuk koordinat
        PlayerPrefs.DeleteKey("LevelTerakhir");
        PlayerPrefs.DeleteKey("PlayerX");
        PlayerPrefs.DeleteKey("PlayerY");
        PlayerPrefs.DeleteKey("PlayerZ");
        PlayerPrefs.SetInt("GameDikutip", 0); // Set tanda ke 0 (Mulai baru)
        
        SceneManager.LoadScene("Level 1"); 
    }

    public void TombolLanjutkan()
    {
        BunyikanKlik();

        if (PlayerPrefs.HasKey("LevelTerakhir"))
        {
            // BERI TANDA BENDERA: Kasih tahu Unity kalau ini adalah proses Continue!
            PlayerPrefs.SetInt("GameDikutip", 1); 

            string namaScene = PlayerPrefs.GetString("LevelTerakhir");
            SceneManager.LoadScene(namaScene);
        }
    }

    // Fungsi untuk tombol "Setting" atau "Credit"
    public void TombolPengaturan()
    {
        BunyikanKlik();
        Debug.Log("Membuka Panel Pengaturan...");
        // Nanti di sini kita bisa aktifkan objek panel UI Settings
    }
    public void TombolAbout()
    {
        BunyikanKlik();
        Debug.Log("Membuka Panel About...");
        // Nanti di sini kita bisa aktifkan objek panel UI Settings
    }

    // Fungsi untuk tombol "Exit" di pojok kanan atas
    public void TombolKeluar()
    {
        BunyikanKlik();
        Debug.Log("Aplikasi Ditutup!");
        Application.Quit(); // Perintah ini hanya akan berefek saat game di-build ke APK/PC
    }

    public void AturVolume(float nilaiVolume)
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = nilaiVolume; // Mengubah volume lagu sesuai letak slider
        }
    }
    
    // Fungsi internal untuk memutar suara
    private void BunyikanKlik()
    {
        if (sfxKlikTombol != null)
        {
            sfxKlikTombol.Play();
        }
    }
}