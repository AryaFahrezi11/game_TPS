using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk fungsi restart dan pindah scene

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject panelPause; // Hubungkan PanelPause ke sini

    private bool isPaused = false;

    private void Update()
    {
        // Fitur tambahan: Menekan tombol ESCAPE di keyboard otomatis memicu Pause/Resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // 1. Fungsi untuk melanjutkan permainan
    public void ResumeGame()
    {
        if (panelPause != null) panelPause.SetActive(false); // Sembunyikan panel pause
        
        Time.timeScale = 1f; // Kembalikan jalannya waktu game menjadi normal
        isPaused = false;
        
        Debug.Log("Game Dilanjutkan.");
    }

    // 2. Fungsi untuk menghentikan permainan sementara (Pause)
    public void PauseGame()
    {
        if (panelPause != null) panelPause.SetActive(true); // Munculkan panel pause
        
        Time.timeScale = 0f; // BEKUKAN WAKTU DUNIA GAME TOTAL! (Animasi, gerakan, fisika berhenti)
        isPaused = true;
        
        Debug.Log("Game Di-pause. Waktu membeku!");
    }

    // 3. Fungsi untuk mengulang level dari awal (Restart)
    public void RestartGame()
    {
        // PERINGATAN DOSEN: Wajib kembalikan waktu ke 1f SEBELUM me-load scene!
        // Jika tidak, level baru kamu bakal ikut membeku dan macet dari awal mulai.
        Time.timeScale = 1f; 

        // Mengambil nama scene yang sedang aktif saat ini lalu memuatnya ulang
        string namaSceneSekarang = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(namaSceneSekarang);
        
        Debug.Log("Level di-restart: " + namaSceneSekarang);
    }

    // 4. Fungsi untuk kembali ke Menu Utama
    // 4. Fungsi untuk kembali ke Menu Utama
    public void KembaliKeMainMenu()
    {
        // --- KODE BARU: Simpan posisi player sebelum pindah scene ---
        PlayerTPS playerScript = FindObjectOfType<PlayerTPS>();
        if (playerScript != null)
        {
            playerScript.SimpanProgressGame();
            Debug.Log("Progress otomatis disimpan sebelum kembali ke menu!");
        }
        // -------------------------------------------------------------

        // Wajib kembalikan waktu ke normal dulu
        Time.timeScale = 1f; 

        // PASTIKAN NAMA SCENE BENAR: Cek di Build Settings, apakah namanya "MainMenu" (tanpa spasi) atau "Main Menu" (pakai spasi)
        SceneManager.LoadScene("MainMenu"); 
        
        Debug.Log("Kembali ke Main Menu.");
    }
}