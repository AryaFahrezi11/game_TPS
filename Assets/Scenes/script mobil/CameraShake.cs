using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Variabel statis supaya bisa dipanggil dari script manapun
    public static CameraShake Instance;

    private Vector3 originalPos;
    private float shakeTimeRemaining = 0f;
    private float shakePower = 0f;
    
    void Awake()
    {
        // Setup singleton sederhana
        Instance = this;
    }

    void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            // Buat posisi kamera random sedikit di sekitar posisi aslinya
            transform.localPosition = originalPos + Random.insideUnitSphere * shakePower;
            
            // Kurangi waktu getar
            shakeTimeRemaining -= Time.deltaTime * 1.0f;
        }
        else
        {
            // Jika waktu getar habis, kembalikan ke posisi semula
            transform.localPosition = originalPos;
        }
    }

    // Fungsi untuk memicu getaran
    public void StartShake(float duration, float power)
    {
        shakeTimeRemaining = duration;
        shakePower = power;
    }
}