
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;
    public float grabDistance = 2f;
    public GameObject torchPrefab;
    public GameObject playerTorch;

    private TorchController torchController;
    private Rigidbody rb;
    private Animator animator;
    private Vector2 moveDirection;
    private bool holdTorch = false;
    public bool isMovable = true;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        torchController = playerTorch.GetComponent<TorchController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
            animator.SetBool("Move", true);
        }
        else if (context.canceled)
        {
            moveDirection = Vector2.zero;
            animator.SetBool("Move", false);
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        animator.SetBool("Shift", context.ReadValueAsButton());
    }

    public void OnPick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (holdTorch)
            {
                animator.SetTrigger("Interact");
                isMovable = false;
                StartCoroutine(WhenAnimationCompleted("Torch Ungrab", () => { isMovable = true; }));
                hidePlayerTorch();
            }
            else
            {
                GameObject closestTorch = FindClosestTorch();
                if (closestTorch != null)
                {
                    float distance = Vector3.Distance(transform.position, closestTorch.transform.position);
                    if (distance <= grabDistance)
                    {
                        animator.SetTrigger("Interact");
                        isMovable = false;
                        StartCoroutine(WhenAnimationCompleted("Torch Grab", () => { isMovable = true; }));
                        showPlayerTorch(closestTorch);
                    }
                }
            }
        }
    }

    public void OnChange(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (holdTorch)
            {
                torchController.on = !torchController.on;
                torchController.applyProperty();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMovable)
        {
            rb.velocity = transform.forward * moveDirection.y * moveSpeed;
            float rotationAngle = moveDirection.x * rotateSpeed * Time.deltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotationAngle, 0f));
        }
    }

    private void showPlayerTorch(GameObject closestTorch)
    {
        TorchController closestTorchController = closestTorch.GetComponent<TorchController>();
        torchController.red = closestTorchController.red;
        torchController.green = closestTorchController.green;
        torchController.blue = closestTorchController.blue;
        torchController.on = closestTorchController.on;
        torchController.applyProperty();

        playerTorch.SetActive(true);
        closestTorch.SetActive(false);
        holdTorch = true;
    }

    private void hidePlayerTorch()
    {
        GameObject newTorch = Instantiate(torchPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        TorchController newTorchController = newTorch.GetComponent<TorchController>();
        newTorchController.red = torchController.red;
        newTorchController.green = torchController.green;
        newTorchController.blue = torchController.blue;
        newTorchController.on = torchController.on;
        newTorchController.applyProperty();

        newTorch.SetActive(true);
        playerTorch.SetActive(false);
        holdTorch = false;
    }

    private GameObject FindClosestTorch()
    {
        GameObject[] torches = GameObject.FindGameObjectsWithTag("Torch");
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject torch in torches)
        {
            float distance = Vector3.Distance(transform.position, torch.transform.position);
            if (distance < closestDistance)
            {
                closest = torch;
                closestDistance = distance;
            }
        }
        return closest;
    }

    private IEnumerator WhenAnimationCompleted(string CurrentAnim, Action Oncomplete)
    {
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(CurrentAnim) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            Debug.Log("PLZ "+animator.GetCurrentAnimatorStateInfo(0).IsName(CurrentAnim));
            yield return null;
        }
        if (Oncomplete != null)
            Oncomplete();
    }
}
