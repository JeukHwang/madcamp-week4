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

    private PlayerLocalController playerController;

    Animator animator;
    bool hasTorch = false;
    TorchController torchController;

    bool ableToChange = true;
    bool ableToInteract = true;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        torchController = PlayerTorch.GetComponent<TorchController>();
        playerController = GetComponent<PlayerLocalController>();
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
        if (isPicking && ableToInteract)
        {
            GrabOrUngrabTorchIfPossible();
        }
        if (isChanging && hasTorch && ableToChange)
        {
            ableToChange = false;
            StartCoroutine(toggleThrottle());
        }
    }

    IEnumerator toggleThrottle()
    {
        Debug.Log("Change! by L");
        //ableToChange = false;
        torchController.on = !torchController.on;
        //Wait 1 second
        yield return new WaitForSeconds(1.0f);
        ableToChange = true;

        Debug.Log("AbleToChange" + ableToChange);
        //Do process stuff
    }

    void GrabOrUngrabTorchIfPossible()
    {
        if (hasTorch)
        {
            //ableToInteract = false;
            //playerController.movable = false;
            StartCoroutine(CheckAnimationCompleted("Torch Ungrab", () =>
            {
                if (hasTorch)
                {
                    hasTorch = false;
                    animator.ResetTrigger("isInteracting");

                    GameObject newTorch = Instantiate(prefab, GameObject.Find("Player").transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                    TorchController newTorchController = newTorch.GetComponent<TorchController>();
                    newTorch.SetActive(true);

                    TorchController torchController = PlayerTorch.GetComponent<TorchController>();
                    newTorchController.red = torchController.red;
                    newTorchController.green = torchController.green;
                    newTorchController.blue = torchController.blue;
                    newTorchController.on = torchController.on;
                    newTorchController.applyProperty();

                    PlayerTorch.SetActive(false);
                    //ableToInteract = true;
                    //playerController.movable = true;
                }
            }));
            animator.SetTrigger("isInteracting");
        }
        else
        {
            GameObject closestTorch = FindClosestTorch();
            if (closestTorch != null)
            {
                float distance = Vector3.Distance(transform.position, closestTorch.transform.position);
                if (distance <= closeDistance)
                {
                    //ableToInteract = false;
                    //playerController.movable = false;
                    StartCoroutine(CheckAnimationCompleted("Torch Grab", () =>
                        {
                            if (!hasTorch)
                            {
                                hasTorch = true;
                                animator.ResetTrigger("isInteracting");

                                TorchController newTorchController = PlayerTorch.GetComponent<TorchController>();
                                PlayerTorch.SetActive(true);

                                TorchController torchController = closestTorch.GetComponent<TorchController>();
                                newTorchController.red = torchController.red;
                                newTorchController.green = torchController.green;
                                newTorchController.blue = torchController.blue;
                                newTorchController.on = torchController.on;
                                newTorchController.applyProperty();

                                closestTorch.SetActive(false);

                                //Debug.Log("!!!");
                                //Debug.Log(playerController.movable);
                                //ableToInteract = true;
                                //playerController.movable = true;
                            }
                        }));
                    animator.SetTrigger("isInteracting");
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
