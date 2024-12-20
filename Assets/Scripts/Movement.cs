using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ParkourPlayer : MonoBehaviour
{
    // Movement Variables
    public float walkSpeed = 2f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    // Wall Run Variables
    public float wallRunSpeed = 7f;
    public float wallRunTime = 5f;
    private float currentWallRunTime = 0f;
    public float wallRunForce = 12f;
    private bool isWallRunning = false;
    public float wallRunTiltAngle = 10f;

    // Camera Look
    public float lookSpeedX = 2f;
    public float lookSpeedY = 2f;
    private float xRotation = 0f;
    private Camera playerCamera;

    private Vector3 velocity;
    private CharacterController characterController;
    private Transform orientation;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        orientation = transform;

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
        HandleWallRun();
    }

    void HandleMovement()
    {
        float moveDirectionX = Input.GetAxisRaw("Horizontal");
        float moveDirectionZ = Input.GetAxisRaw("Vertical");

        Vector3 move = orientation.right * moveDirectionX + orientation.forward * moveDirectionZ;

        // Apply movement speed
        move *= walkSpeed;

        // Jumping
        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Apply jump force
            }
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move the player
        characterController.Move(move * Time.deltaTime);
        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        orientation.Rotate(Vector3.up * mouseX);
    }

    void HandleWallRun()
    {
        RaycastHit hit;

        // Check for walls to wall run
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 2f) && !isWallRunning)
        {
            if (hit.collider.CompareTag("Wall") && !characterController.isGrounded)
            {
                StartWallRun();
            }
        }
        else if (isWallRunning)
        {
            StopWallRun();
        }
    }

    void StartWallRun()
    {
        isWallRunning = true;
        currentWallRunTime = wallRunTime;

        // Zero out vertical velocity to prevent falling while wall-running
        velocity.y = 0f;

        // Apply wall-run force to push the player along the wall
        velocity += transform.up * wallRunForce;

        // Move the player along the wall
        characterController.Move(velocity * Time.deltaTime);

        // Apply camera tilt
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, wallRunTiltAngle);
    }

    void StopWallRun()
    {
        isWallRunning = false;

        // Reset camera tilt
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
