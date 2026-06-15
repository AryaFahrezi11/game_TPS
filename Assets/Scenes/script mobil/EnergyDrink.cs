using UnityEngine;
using System.Collections;

public class EnergyDrink : MonoBehaviour
{
    [SerializeField] private int jumlahPenyembuhan = 30; // Berapa darah yang nambah
    [SerializeField] private float durasiAnimasiMinum = 2.0f; // Sesuaikan dengan panjang durasi animasi Mixamo-mu

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Mulai proses minum
            StartCoroutine(ProsesMinum(other.gameObject));
        }
    }

    private IEnumerator ProsesMinum(GameObject player)
    {
        // 1. Sembunyikan kaleng minuman di map agar kelihatan seperti sudah diambil
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // 2. Ambil komponen script dari player
        PlayerTPS moveSystem = player.GetComponent<PlayerTPS>();
        PlayerHealth healthSystem = player.GetComponent<PlayerHealth>();
        Animator anim = player.GetComponent<Animator>();

        // 3. Kunci pergerakan player & picu animasi minum
        if (moveSystem != null) moveSystem.SetLockActions(true);
        if (anim != null) anim.SetTrigger("drink");

        Debug.Log("Player sedang meminum minuman berenergi...");

        // 4. Jeda/Tunggu sampai animasi minumnya selesai berjalan
        yield return new WaitForSeconds(durasiAnimasiMinum);

        // 5. Tambahkan darah setelah animasi selesai
        if (healthSystem != null)
        {
            healthSystem.TambahDarah(jumlahPenyembuhan);
        }

        // 6. Buka kembali kuncian pergerakan player
        if (moveSystem != null) moveSystem.SetLockActions(false);

        Debug.Log("Selesai minum! Player bisa bergerak kembali.");

        // 7. Hancurkan objek kaleng ini secara total dari memori game
        Destroy(gameObject);
    }
}