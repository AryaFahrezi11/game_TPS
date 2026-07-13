using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerTPS : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f; // Kecepatan normal
    [SerializeField] private float runSpeed = 8f;  // Kecepatan saat lari
    [SerializeField] private float rotationSpeed = 10f;
    private bool isRunning = false; // Penanda status lari

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;
    private float velocityY;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;

    [Header("Mobile Control (Optional)")]
    [SerializeField] private Joystick joystick; // Menampung komponen Joystick Pack

    private bool isGrounded;
    private CharacterController controller;
    private TPS inputActions;
    private bool isActionsLocked = false;

    private Vector2 moveInput;
    private bool jumpPressed;
    
    // Variabel global baru untuk menampung arah gerakan horizontal
    private Vector3 finalMoveDirection;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new TPS();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Jump.performed -= OnJump;

        inputActions.Disable();
    }

    private void Update()
    {
        CheckGround();
        
        // Reset arah gerakan horizontal di setiap frame
        finalMoveDirection = Vector3.zero;

        // Hanya jalankan input movement & jump jika aksi TIDAK sedang dikunci
        if (!isActionsLocked)
        {
            HandleMovement();
            HandleJump();
        }
        else
        {
            // Jika dikunci, paksa animasi mati
            if (animator != null) 
            {
                animator.SetBool("isWalk", false);
                animator.SetBool("isRun", false);
            }
        }
        
        // Gabungkan gravitasi dan eksekusi movement CUKUP SATU KALI di sini
        ApplyGravityAndExecute();
        UpdateAnimator();
    }

    private void CheckGround()
    {
        isGrounded = controller.isGrounded;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
    }

    private void HandleMovement()
    {
        // 1. Ambil input dari Keyboard (New Input System) sebagai dasarnya
        Vector2 finalInput = moveInput; 

        // 2. TENTUKAN PEMENANG (Joystick vs Keyboard)
        if (joystick != null && joystick.Direction.magnitude > finalInput.magnitude)
        {
            finalInput = joystick.Direction;
        }

        // 3. Masukkan nilai finalInput ke perhitungan physics
        Vector3 move = new Vector3(finalInput.x, 0, finalInput.y);

        if (move.magnitude > 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;

            Vector3 moveDirection = camForward * move.z + camRight * move.x;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // PERBAIKAN LARI: Tentukan kecepatan saat ini berdasarkan tombol lari
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            finalMoveDirection = moveDirection.normalized * currentSpeed;

            // Update Animasi
            if (animator != null) 
            {
                animator.SetBool("isWalk", true);
                animator.SetBool("isRun", isRunning); // Memicu animasi lari
            }
        }
        else
        {
            // Jika finalInput bernilai 0, otomatis IDLE
            if (animator != null) 
            {
                animator.SetBool("isWalk", false);
                animator.SetBool("isRun", false);
            }
        }
    }

    private void HandleJump()
    {
        if (jumpPressed)
        {
            if (isGrounded)
            {
                velocityY = Mathf.Sqrt(jumpForce * -2f * gravity);
                
                if (animator != null)
                {
                    animator.SetTrigger("jump");
                }
            }
            jumpPressed = false; 
        }
    }

    private void ApplyGravityAndExecute()
    {
        if (isGrounded && velocityY < 0)
        {
            velocityY = -2f; 
        }
        else
        {
            velocityY += gravity * Time.deltaTime;
        }

        Vector3 totalMovement = finalMoveDirection;
        totalMovement.y = velocityY;

        controller.Move(totalMovement * Time.deltaTime);
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("yVelocity", velocityY);
        }
    }

    public void SetLockActions(bool lockState)
    {
        isActionsLocked = lockState;
        
        if (lockState && animator != null)
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
        }
    }

    // ================= FITUR JUMP MOBILE =================
    public void TekanTombolJumpMobile()
    {
        if (!isActionsLocked && isGrounded)
        {
            jumpPressed = true;
        }
    }

    // ================= FITUR RUN MOBILE (BARU) =================
    // Dipanggil saat tombol lari DITEKAN TAHAN (PointerDown)
    public void MulaiLariMobile()
    {
        if (!isActionsLocked)
        {
            isRunning = true;
            Debug.Log("Player mulai berlari!");
        }
    }

    // Dipanggil saat tombol lari DILEPAS (PointerUp)
    public void BerhentiLariMobile()
    {
        isRunning = false;
        Debug.Log("Player kembali berjalan.");
    }

    // Tambahkan fungsi Start() baru ini di bawah fungsi Awake() atau sebelum Update()
    private void Start()
    {
        // Cek apakah pemain masuk game melalui tombol Continue
        if (PlayerPrefs.GetInt("GameDikutip", 0) == 1)
        {
            if (PlayerPrefs.HasKey("PlayerX"))
            {
                float x = PlayerPrefs.GetFloat("PlayerX");
                float y = PlayerPrefs.GetFloat("PlayerY");
                float z = PlayerPrefs.GetFloat("PlayerZ");

                controller.enabled = false; 
                
                // =======================================================
                // PERBAIKAN UTAMA: Tambahkan + 0.5f pada koordinat Y 
                // agar player tidak spawn di dalam lantai, tapi di atasnya.
                // =======================================================
                transform.position = new Vector3(x, y + 1.5f, z);
                
                // Reset juga kecepatan gravitasi internal agar tidak ngebut ke bawah
                velocityY = 0f; 
                
                controller.enabled = true; 

                Debug.Log("Posisi karakter aman di atas lantai terowongan!");
            }

            PlayerPrefs.SetInt("GameDikutip", 0);
        }
    }

    // FUNGSI BARU: Panggil fungsi ini setiap kali kamu ingin menyimpan game (misal saat teleport)
    public void SimpanProgressGame()
    {
        // 1. Simpan nama Scene aktif saat ini
        PlayerPrefs.SetString("LevelTerakhir", SceneManager.GetActiveScene().name);

        // 2. Simpan koordinat X, Y, Z posisi Player saat ini
        PlayerPrefs.SetFloat("PlayerX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", transform.position.z);

        // 3. Amankan data ke memori
        PlayerPrefs.Save();
        Debug.Log("Progress koordinat posisi Player berhasil disimpan!");
    }

    // ================= FITUR AUTO-SAVE (BARU) =================

    // 1. Fungsi bawaan Unity yang mendeteksi saat aplikasi/game ditutup paksa (termasuk stop Play di Editor)
    private void OnApplicationQuit()
    {
        SimpanProgressGame();
        Debug.Log("Game ditutup! Posisi terakhir berhasil di-AutoSave.");
    }

    // 2. Fungsi bawaan Unity yang mendeteksi saat game di-minimize (misal: pemain menekan tombol Home di HP)
    private void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            SimpanProgressGame();
            Debug.Log("Game diminimize! Posisi terakhir berhasil di-AutoSave.");
        }
    }
}