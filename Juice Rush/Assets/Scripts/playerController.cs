using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public Camera playerCamera;
    Vector3 cameraOffset;

    [SerializeField] float walkingspeed = 10.0f;
    [SerializeField] float runningspeed = 20.0f;
    [SerializeField] float jumpHeight = 2.0f;
    [SerializeField] float doubleJumpHeight = 1.5f;

    float gravity = -9.81f;
    float speed;
    Vector3 velocity;
    
    CharacterController characterController;
    Vector3 inputDirection = Vector3.zero;
    float mouseSensitivity = 0.05f;
    float rotationX = 0f;
    float rotationY = 0f;

    bool isGrounded;
    bool isJumping = false;
    int jumpCount = 0;
    int maxJumpCount = 2;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        cameraOffset = playerCamera.transform.position - transform.position;
        speed = walkingspeed;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;
        if (characterController.isGrounded && velocity.y < 0f)
        {
            velocity.y = 0f;
            jumpCount = 0;
        }

        // Move camera
        playerCamera.transform.position = transform.position + cameraOffset;
    }

    void FixedUpdate()
    {
        // Move player
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 moveDirection = (forward * inputDirection.z + right * inputDirection.x) * speed;

        characterController.Move(moveDirection * Time.deltaTime);

        if (isJumping)
        {
            Jump();
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void Jump(bool pressedJump = false)
    {
        if (isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            jumpCount++;
        }
        else if (jumpCount < maxJumpCount && pressedJump)
        {
            velocity.y += Mathf.Sqrt(doubleJumpHeight * -3.0f * gravity);
            jumpCount++;
        }
    }

    void Slide()
    {

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Move player
        Vector2 inputVector = context.ReadValue<Vector2>();
        inputDirection = new Vector3(inputVector.x, 0f, inputVector.y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // Camera
        rotationX += context.ReadValue<Vector2>().x * mouseSensitivity;
        rotationY += context.ReadValue<Vector2>().y * mouseSensitivity;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);

        // Rotate player
        transform.localRotation = Quaternion.Euler(0f, rotationX, 0f);
        // Rotate camera
        playerCamera.transform.localRotation = Quaternion.Euler(-rotationY, rotationX, 0f);
    }

//TODO: Implement
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Attack");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped Attacking");
        }
    }

//TODO: Implement
    public void OnADS(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("ADS");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped ADS");
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isJumping = true;

            if (!isGrounded)
            {
                Jump(true);
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isJumping = false;
        }

    }

//TODO: Implement
    public void OnGrapple(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Grapple");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped Grappling");
        }
    }

//TODO: Implement
    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Reload");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped Reloading");
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            speed = runningspeed;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            speed = walkingspeed;
        }
    }

//TODO: Implement
    public void OnSneakSlide(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Sneak/Slide");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped Sneaking/Sliding");
        }
    }

//TODO: Implement
    public void OnSwapWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Swap Weapon");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped Swapping Weapon");
        }
    }

//TODO: Implement
    public void OnPrimaryWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Primary Weapon");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped Primary Weapon");
        }
    }

//TODO: Implement
    public void OnSecondaryWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Secondary Weapon");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped Secondary Weapon");
        }
    }
}
