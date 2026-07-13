using UnityEngine;
using UnityEngine.SceneManagement; // Wajib dipanggil untuk membaca nama Level

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        // 1. Baca nama level tempat karakter berada sekarang (misal: "Level 1")
        string sceneSekarang = SceneManager.GetActiveScene().name;
        
        // 2. Simpan nama level itu ke dalam memori HP dengan judul "LevelTerakhir"
        PlayerPrefs.SetString("LevelTerakhir", sceneSekarang);
        
        // 3. Kunci penyimpanannya biar tidak hilang kalau HP dimatikan
        PlayerPrefs.Save(); 
        
        Debug.Log("Pembatas Buku diletakkan di: " + sceneSekarang);
    }
}