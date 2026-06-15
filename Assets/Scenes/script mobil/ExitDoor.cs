using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    public string namaSceneLuar = "Level1"; 

    private void OnTriggerEnter(Collider other)
    {
        // Baris ini akan muncul di Console setiap kali ada yang menyentuh pintu
        Debug.Log("Ada objek menyentuh pintu: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player terdeteksi! Mencoba pindah ke: " + namaSceneLuar);
            SceneManager.LoadScene(namaSceneLuar);
        }
    }
}