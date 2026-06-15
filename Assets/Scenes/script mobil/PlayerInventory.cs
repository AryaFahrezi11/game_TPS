using UnityEngine;
using TMPro; // Wajib untuk UI Teks

public class PlayerInventory : MonoBehaviour
{
    // ==========================================
    // BAGIAN 1: FITUR KUNCI (Untuk buka pintu)
    // ==========================================
    public bool hasKey = false;

    public void GetKey()
    {
        hasKey = true;
        Debug.Log("Kunci didapatkan!");
    }


    // ==========================================
    // BAGIAN 2: FITUR KOIN (Untuk skor & UI)
    // ==========================================
    public TMP_Text textKoin; 
    public static int koinSatuDunia;

    void Start()
    {
        // Langsung update teks UI dengan nilai static saat scene baru terbuka
        if (textKoin != null) 
        {
            textKoin.text = "";
            textKoin.text = koinSatuDunia.ToString(); 
        }
    }

    public void TambahKoin(int jumlah)
    {
        koinSatuDunia += jumlah;
        
        // Update tulisan di layar kalau UI-nya sudah dipasang
        if (textKoin != null) 
        {
            textKoin.text = koinSatuDunia.ToString(); 
        }
        
        Debug.Log("Koin sekarang: " + koinSatuDunia);
    }
}