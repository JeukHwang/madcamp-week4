using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationStateController : MonoBehaviour
{
    Vector2 moveDirection = Vector2.zero;
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction run;
    private InputAction interact;
    Animator animator;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        run = playerControls.Player.Run;
        run.Enable();
        interact = playerControls.Player.Interact;
        interact.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        run.Disable();
        interact.Enable();
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        bool isAlreadyWalking = animator.GetBool("isWalking");
        bool isAlreadyRunning = animator.GetBool("isRunning");
        bool isWalking = !moveDirection.Equals(Vector2.zero);
        bool isRunning = isWalking && (run.ReadValue<float>() > 0.5f);
        if (!isAlreadyWalking && isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        if (isAlreadyWalking && !isWalking)
        {
            animator.SetBool("isWalking", false);
        }
        if (!isAlreadyRunning && isRunning)
        {
            animator.SetBool("isRunning", true);
        }
        if (isAlreadyRunning && !isRunning)
        {
            animator.SetBool("isRunning", false);
        }
    }
}
