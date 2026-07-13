using UnityEngine;
using UnityEngine.AI;

public class MonsterChaser : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Pengaturan Sensor")]
    public float jarakDeteksi = 20f; 

    private NavMeshAgent agenMonster;
    private Animator anim; // BARIS BARU: Slot untuk komponen Animator

    void Start()
    {
        agenMonster = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>(); // BARIS BARU: Mengambil komponen Animator di badan monster
    }

    void Update()
    {
        if (player != null)
        {
            float jarak = Vector3.Distance(transform.position, player.position);

            if (jarak <= jarakDeteksi)
            {
                agenMonster.SetDestination(player.position);
            }
            else
            {
                if (agenMonster.hasPath)
                {
                    agenMonster.ResetPath(); 
                }
            }

            // =======================================================
            // BARIS BARU: SINKRONISASI FISIKA NAVMESH DENGAN ANIMASI
            // =======================================================
            if (anim != null && agenMonster != null)
            {
                // Mengambil nilai kecepatan gerak asli monster dari NavMesh (magnitude)
                float kecepatanSaatIni = agenMonster.velocity.magnitude;

                // Mengirimkan nilai tersebut ke parameter 'Speed' di Blend Tree
                anim.SetFloat("Speed", kecepatanSaatIni);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, jarakDeteksi);
    }
}