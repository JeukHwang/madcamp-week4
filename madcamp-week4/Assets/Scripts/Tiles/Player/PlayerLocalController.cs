using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocalController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    Vector2 moveDirection = Vector2.zero;
    Rigidbody rb;

    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction fire;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * moveDirection.y * moveSpeed;
        float rotationAngle = moveDirection.x * rotateSpeed * Time.deltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotationAngle, 0f));
    }

    private void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("we fired");
    }
}
