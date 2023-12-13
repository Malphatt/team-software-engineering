using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    private Vector3 cameraOffset;

    private float walkingspeed = 10.0f;
    private float runningspeed = 20.0f;
    private float sneakingSpeed = 2.5f;
    private float groundAcceleration = 2.5f;
    private float airAcceleration = 0.5f;
    private float jumpHeight = 2.0f;
    private float doubleJumpHeight = 1.5f;

    private float gravity = -9.81f;
    private float friction = 0.8f;
    private float slideFriction = 0.9875f;
    private float speed;
    private Vector3 velocity;
    
    private CharacterController characterController;
    private Vector3 inputDirection = Vector3.zero;
    private float mouseSensitivity = 0.05f;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private bool isGrounded;
    private bool isSprinting = false;
    private bool isSneakSliding = false;
    private bool isSneaking = false;
    private bool isSliding = false;
    private bool isJumping = false;
    private int jumpCount = 0;
    private int maxJumpCount = 2;

    private float originalHeight;
    private Vector3 originalCenter;
    private float sneakHeight = 1.5f;
    private float slideHeight = 0.75f;
    private float crouchSpeed = 5.0f;

    void Awake()
    {
        // Set up references
        characterController = GetComponent<CharacterController>();
        originalHeight = characterController.height;
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
        Vector3 moveDirection = forward * inputDirection.z + right * inputDirection.x;

        // Check if the player is moving (pressing WASD)
        if (moveDirection.magnitude == 0f)
        {
            // If the player is not moving and is grounded, apply friction to slow them down
            if (isGrounded && isSliding)
            {
                // If the player is sneak sliding and has a higher velocity than walking speed, apply a smaller friction
                velocity.x = velocity.x * slideFriction;
                velocity.z = velocity.z * slideFriction;
            }
            else if (isGrounded)
            {
                // If the player is grounded and not moving, apply normal friction
                velocity.x = velocity.x * friction;
                velocity.z = velocity.z * friction;
                
            }
        }
        else
        {
            // If the player is moving and is grounded, apply acceleration to speed them up
            if (isGrounded)
            {
                velocity.x = Mathf.Lerp(velocity.x, moveDirection.x * speed, groundAcceleration * Time.deltaTime);
                velocity.z = Mathf.Lerp(velocity.z, moveDirection.z * speed, groundAcceleration * Time.deltaTime);
            }
            // If the player is moving and is not grounded, apply a smaller acceleration but only if the player is moving in the opposite direction of their velocity
            else
            {
                float moveDirectionRadians = Mathf.Atan2(moveDirection.x, moveDirection.z);
                float velocityRadians = Mathf.Atan2(velocity.x, velocity.z);

                float angle = Mathf.DeltaAngle(moveDirectionRadians, velocityRadians);
                float pi = Mathf.PI;

                // If the angle between the move direction and the velocity is greater than 90 degrees, slow down the player
                if (angle > pi / 2 || angle < -pi / 2)
                {
                    // Slow down the velocity in the current direction
                    velocity.x = Mathf.Lerp(velocity.x, moveDirection.x * speed, airAcceleration * Time.deltaTime);
                    velocity.z = Mathf.Lerp(velocity.z, moveDirection.z * speed, airAcceleration * Time.deltaTime);
                }
                // If the angle between the move direction and the velocity is less than 90 degrees, change the direction of the player
                else
                {
                    Vector3 newVelocity = new Vector3(moveDirection.x * speed, velocity.y, moveDirection.z * speed);
                    velocity.x = Mathf.Lerp(velocity.x, newVelocity.x, airAcceleration * Time.deltaTime);
                    velocity.z = Mathf.Lerp(velocity.z, newVelocity.z, airAcceleration * Time.deltaTime);
                }
            }

        }

        // Alter the player's velocity based on whether they are sprinting or not and if they are grounded
        if (isGrounded)
        {
            if (isSprinting)
            {
                speed = runningspeed;
            }
            else if (isSneakSliding)
            {
                speed = sneakingSpeed;
            }
            else
            {
                speed = walkingspeed;
            }
        }

        // Check if the player is sneaking or sliding
        if (isSneakSliding)
        {
            // Alter the player's hitbox to be smaller

            // If the player's speed is greater than walkingspeed, slide
            if (velocity.magnitude > walkingspeed)
            {
                isSliding = true;
                characterController.height = Mathf.Lerp(characterController.height, slideHeight, crouchSpeed * Time.deltaTime);
                characterController.center = Vector3.Lerp(characterController.center, new Vector3(0f, 1 - slideHeight / 2, 0f), crouchSpeed * Time.deltaTime);
            }
            // If the player's speed is less than or equal to walkingspeed and they aren't sliding, sneak
            else if (!isSliding)
            {
                isSneaking = true;
                characterController.height = Mathf.Lerp(characterController.height, sneakHeight, crouchSpeed * Time.deltaTime);
                characterController.center = Vector3.Lerp(characterController.center, new Vector3(0f, 1 - sneakHeight / 2, 0f), crouchSpeed * Time.deltaTime);
            }
            // If the player's speed is slow enough, stop sliding
            else if (velocity.magnitude <= 1.5f)
            {
                isSliding = false;
            }
        }
        else
        {
            // Reset the player's hitbox to its original size
            characterController.height = Mathf.Lerp(characterController.height, originalHeight, crouchSpeed * Time.deltaTime);
            characterController.center = Vector3.Lerp(characterController.center, new Vector3(0f, 1 - originalHeight / 2, 0f), crouchSpeed * Time.deltaTime);
            isSneaking = false;
            isSliding = false;
        }

        // If the player is holding the jump button, jump
        if (isJumping && !isSneakSliding)
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

    void Grapple() // You might not need this method
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

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isSprinting = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isSprinting = false;
        }
    }

    public void OnSneakSlide(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isSneakSliding = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isSneakSliding = false;
        }
    }
}