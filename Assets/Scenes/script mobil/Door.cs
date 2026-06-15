using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PlayerInventory inventory = collision.gameObject.GetComponent<PlayerInventory>();
        
        if (inventory != null && inventory.hasKey)
        {
            Debug.Log("Pintu Terbuka! Pindah ke Level 2.");
            SceneManager.LoadScene("Level2"); // Pastikan nama scene sesuai
        }
        else
        {
            Debug.Log("Pintu Terkunci! Cari kuncinya dulu.");
        }
    }
}