using System;
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
    private float closeDistance = 2f;
    Animator animator;
    public GameObject prefab;

    public GameObject PlayerTorch;

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
        interact.Disable();
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        bool isAlreadyWalking = animator.GetBool("isWalking");
        bool isAlreadyRunning = animator.GetBool("isRunning");
        bool isWalking = !moveDirection.Equals(Vector2.zero);
        bool isRunning = isWalking && (run.ReadValue<float>() > 0.5f);
        bool isInteracting = interact.ReadValue<float>() > 0.5f;
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
        if (isInteracting)
        {
            GrabOrUngrabTorchIfPossible();
        }
    }

    void GrabOrUngrabTorchIfPossible()
    {
        if (PlayerTorch.activeSelf)
        {
            animator.SetTrigger("isInteracting");
            StartCoroutine(CheckAnimationCompleted("Torch Ungrab", () =>
            {
                if (PlayerTorch.activeSelf)
                {
                    animator.ResetTrigger("isInteracting");
                    PlayerTorch.SetActive(false);
                    GameObject torch = Instantiate(prefab, GameObject.Find("Player").transform.position + new Vector3(0, 2, 0), Quaternion.identity) as GameObject;
                    torch.tag = "Torch";
             }
            }));
        }
        else
        {
            GameObject closestTorch = FindClosestTorch();
            if (closestTorch != null)
            {
                float distance = Vector3.Distance(transform.position, closestTorch.transform.position);
                if (distance <= closeDistance)
                {
                    animator.SetTrigger("isInteracting");
                    StartCoroutine(CheckAnimationCompleted("Torch Grab", () =>
                    {
                        animator.ResetTrigger("isInteracting");
                        closestTorch.SetActive(false);
                        PlayerTorch.SetActive(true);
                    }
       ));
                }
            }
        }
    }

    GameObject FindClosestTorch()
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

    public IEnumerator CheckAnimationCompleted(string CurrentAnim, Action Oncomplete)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(CurrentAnim))
            yield return null;
        if (Oncomplete != null)
            Oncomplete();
    }
}
