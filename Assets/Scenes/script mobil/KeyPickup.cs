using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("Koordinat Kembali ke Level 1")]
    [SerializeField] private Vector3 koordinatTujuan; // Tempat awal Player di luar gua/rumah

    [Header("Efek Rotasi (Opsional)")]
    public float kecepatanPutar = 50f;

    [Header("Matikan Pintu Depan")]
    public GameObject pintuLevel1;

    void Update()
    {
        // Membuat kunci berputar pelan di tempat agar terlihat menarik/bisa diambil
        transform.Rotate(Vector3.up * kecepatanPutar * Time.deltaTime);
    }

    // Fungsi sensor tabrakan
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            
            if (cc != null)
            {
                cc.enabled = false;
                other.transform.position = koordinatTujuan;
                cc.enabled = true;

                // ==========================================
                // BARIS BARU: Matikan sensor pintu Level 1 
                // agar tidak menyedot Player lagi!
                // ==========================================
                if (pintuLevel1 != null)
                {
                    // Menonaktifkan komponen Collider pada pintu
                    pintuLevel1.GetComponent<Collider>().enabled = false;
                    Debug.Log("Sensor Pintu Level 1 telah dimatikan permanen!");
                }

                Debug.Log("Kunci berhasil diambil! Player dideportasi.");
                Destroy(gameObject);
            }
        }
    }
}