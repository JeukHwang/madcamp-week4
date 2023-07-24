using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationStateController : MonoBehaviour
{
    public GameObject prefab;
    public GameObject PlayerTorch;

    PlayerInputActions playerControls;
    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction run;
    private InputAction pick;
    private InputAction change;
    private float closeDistance = 2f;

    Animator animator;
    bool hasTorch = false;
    TorchController torchController;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        torchController = PlayerTorch.GetComponent<TorchController>();

    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        run = playerControls.Player.Run;
        run.Enable();
        pick = playerControls.Player.Pick;
        pick.Enable();
        change = playerControls.Player.Change;
        change.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        run.Disable();
        pick.Disable();
        change.Disable();
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        bool isAlreadyWalking = animator.GetBool("isWalking");
        bool isAlreadyRunning = animator.GetBool("isRunning");
        bool isWalking = !moveDirection.Equals(Vector2.zero);
        bool isRunning = isWalking && (run.ReadValue<float>() > 0.5f);
        bool isPicking = pick.ReadValue<float>() > 0.5f;
        bool isChanging = change.ReadValue<float>() > 0.5f;
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
        if (isPicking)
        {
            GrabOrUngrabTorchIfPossible();
        }
        if (isChanging && hasTorch)
        {
            // TODO need throttle
            torchController.on = !torchController.on;
        }
    }

    void GrabOrUngrabTorchIfPossible()
    {
        if (hasTorch)
        {
            animator.SetTrigger("isInteracting");
            StartCoroutine(CheckAnimationCompleted("Torch Ungrab", () =>
            {
                if (hasTorch)
                {
                    hasTorch = false;
                    animator.ResetTrigger("isInteracting");
                    GameObject newTorch = Instantiate(prefab, GameObject.Find("Player").transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                    TorchController newTorchController = newTorch.GetComponent<TorchController>();
                    TorchController torchController = PlayerTorch.GetComponent<TorchController>();
                    newTorchController.red = torchController.red;
                    newTorchController.green = torchController.green;
                    newTorchController.blue = torchController.blue;
                    newTorchController.on = torchController.on;
                    PlayerTorch.SetActive(false);
                    newTorch.SetActive(true);
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
                        hasTorch = true;
                        animator.ResetTrigger("isInteracting");
                        TorchController newTorchController = PlayerTorch.GetComponent<TorchController>();
                        TorchController torchController = closestTorch.GetComponent<TorchController>();
                        newTorchController.red = torchController.red;
                        newTorchController.green = torchController.green;
                        newTorchController.blue = torchController.blue;
                        newTorchController.on = torchController.on;
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
