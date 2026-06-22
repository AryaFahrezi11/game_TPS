using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerTPS : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

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

        // PERBAIKAN: Hanya jalankan input movement & jump jika aksi TIDAK sedang dikunci (misal saat minum)
        if (!isActionsLocked)
        {
            HandleMovement();
            HandleJump();
        }
        else
        {
            // Jika dikunci, paksa animasi jalan mati
            if (animator != null) animator.SetBool("isWalk", false);
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
            Debug.Log("Tombol Space berhasil ditekan!");
        }
    }

    private void HandleMovement()
    {
        // 1. Ambil input dari Keyboard (New Input System) sebagai dasarnya
        Vector2 finalInput = moveInput; 

        // 2. TENTUKAN PEMENANG: Jika joystick ditarik dan kekuatannya lebih besar dari keyboard,
        // maka joystick yang berhak mengontrol karakter.
        if (joystick != null && joystick.Direction.magnitude > finalInput.magnitude)
        {
            finalInput = joystick.Direction;
        }

        // 3. Masukkan nilai finalInput (bukan moveInput lagi) ke perhitungan physics
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

            finalMoveDirection = moveDirection.normalized * moveSpeed;

            if (animator != null) animator.SetBool("isWalk", true);
        }
        else
        {
            // Jika finalInput bernilai 0 (joystick dilepas & keyboard gak dipencet), otomatis IDLE
            if (animator != null) animator.SetBool("isWalk", false);
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
                
                Debug.Log("MANTAP! Karakter berhasil lompat. VelocityY: " + velocityY);
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

        // PERBAIKAN UTAMA: Satukan nilai gerak horizontal (X, Z) dengan gerak vertikal/gravitasi (Y)
        Vector3 totalMovement = finalMoveDirection;
        totalMovement.y = velocityY;

        // Pintu eksekusi tunggal
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
        }
    }

    // ================= FITUR JUMP MOBILE (BARU) =================
    // Fungsi ini akan dipanggil setiap kali tombol JUMP di layar di-klik
    public void TekanTombolJumpMobile()
    {
        // Pastikan karakter tidak sedang dikunci aksinya (misal saat minum energi drink)
        if (!isActionsLocked && isGrounded)
        {
            jumpPressed = true;
            Debug.Log("Tombol Jump Mobile Berhasil Dieksekusi!");
        }
    }
}