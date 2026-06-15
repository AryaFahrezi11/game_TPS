using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab; // Masukkan Prefab koin di sini
    public int jumlahKoin = 10;   // Berapa banyak koin yang mau dimunculkan

    [Header("Area Spawning")]
    public float xRange = 10f; // Jarak ke kanan-kiri
    public float zRange = 10f; // Jarak ke depan-belakang
    public float yPos = 1f;    // Ketinggian koin dari lantai

    void Start()
    {
        SpawnCoins();
    }

    void SpawnCoins()
    {
        for (int i = 0; i < jumlahKoin; i++)
        {
            // Menghasilkan posisi random berdasarkan jangkauan yang kita set
            float randomX = Random.Range(-xRange, xRange);
            float randomZ = Random.Range(-zRange, zRange);

            // Posisi relatif terhadap objek Spawner ini
            Vector3 spawnPos = transform.position + new Vector3(randomX, yPos, randomZ);

            // Memunculkan koin di posisi tersebut
            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        }
    }

    // Biar kita bisa lihat area spawn-nya di Scene View (opsional tapi membantu)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, yPos, 0), new Vector3(xRange * 2, 0.5f, zRange * 2));
    }
}