using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CharacterController))]

public class TPSController : MonoBehaviour
{
    private PlayerInput playerInput;

    public float speed = 7.5f;
    public float jumpSpeed = 40.0f;
    public float gravity = 9.81f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookYLimit = 60.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [HideInInspector] public bool canMove = true;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpInput;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;

        playerInput = GetComponent<PlayerInput>();
        //playerInput.actions["Move"].performed += context => { moveInput = context.action.ReadValue<Vector2>(); };
        //playerInput.actions["Move"].canceled += _ => { moveInput = new Vector2(0,0); };
        playerInput.actions["Look"].performed += context => { lookInput = context.action.ReadValue<Vector2>() / 10; };
        playerInput.actions["Look"].canceled += _ => { lookInput = new Vector2(0,0); };
        playerInput.actions["Jump"].started += _ => { jumpInput = true; };
        playerInput.actions["Jump"].canceled += _ => { jumpInput = false; };
        moveInput = new Vector2();
        lookInput = new Vector2();
        jumpInput = false;
    }

    void Update()
    {
        if (characterController.isGrounded && canMove)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            moveDirection = (forward * moveInput.y * speed) + (right * moveInput.x * speed);
            if(jumpInput)
                moveDirection.y = jumpSpeed;
        }
        else
            moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotation += lookInput * lookSpeed;
            rotation.y = Mathf.Clamp(rotation.y, -lookYLimit, lookYLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.y, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.x);
        }
    }

    public void Move(CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            moveInput = context.action.ReadValue<Vector2>();
        else
            moveInput = new Vector2(0, 0);

        //moveDirection
    }
}
