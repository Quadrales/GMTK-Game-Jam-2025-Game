using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int goldCount = 0;

    private float baseMoveSpeed = 6.0f;
    private float lassoDistance = 3.0f;
    private Vector2 moveInput;
    private GameObject activeLasso;
    private LassoArea currentLassoArea;
    private Vector3 lassoSpawnPoint;

    [SerializeField] private GameObject lassoPrefab;
    [SerializeField] private Animator animator;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lassoAction;
    private Rigidbody2D rb;

    private bool isFacingRight = true;

    void Awake()
    {
        // Initialise components once script is loaded
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        // Disable audio listener for any player other than player 1
        GetComponentInChildren<AudioListener>().enabled = (playerInput.playerIndex == 0);


        moveAction = playerInput.actions["Move"];
        lassoAction = playerInput.actions["Lasso"];
    }

    void OnEnable()
    {
        lassoAction.Enable();
        moveAction.Enable();
    }

    private void OnDisable()
    {
        lassoAction.Disable();
        moveAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeLasso == null)
        {
            moveInput = moveAction.ReadValue<Vector2>();
        }

        // Lasso handling
        if (lassoAction.WasPressedThisFrame())
        {
            StartLasso();
        }
        else if (lassoAction.WasReleasedThisFrame())
        {
            ReleaseLasso();
        }

        if (activeLasso != null)
        {
            // Keep lasso slightly in front of where player is facing
            activeLasso.transform.position = lassoSpawnPoint;
        }
    }

    private void StartLasso()
    {
        if (activeLasso == null)
        {
            // Initialise lasso properties
            lassoSpawnPoint = GetLassoSpawnPoint();
            activeLasso = Instantiate(lassoPrefab, lassoSpawnPoint, Quaternion.identity);
            animator.SetBool("ChargingLasso", true);
            currentLassoArea = activeLasso.GetComponent<LassoArea>();
            currentLassoArea.SetOwner(this);
        }
    }

    private void ReleaseLasso()
    {
        if (activeLasso != null)
        {
            // Send animals to barn and increment player gold
            GameManager.Instance.TrySendAnimalsToBarn(activeLasso.GetComponent<Collider2D>(), currentLassoArea.GetOwner());

            Destroy(activeLasso);
            activeLasso = null;
            animator.SetBool("ChargingLasso", false);
        }
    }

    private Vector3 GetLassoSpawnPoint()
    {
        // Changes offset based on facing right or left
        Vector3 lassoOffset = isFacingRight ? Vector3.right : Vector3.left;
        return transform.position + (lassoOffset * lassoDistance);
    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput * baseMoveSpeed;
        animator.SetFloat("Speed", rb.velocity.magnitude);

        CheckToFlipSprite();
    }

    private void CheckToFlipSprite()
    {
        if (isFacingRight && rb.velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            isFacingRight = false;
        }
        else if (!isFacingRight && rb.velocity.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            isFacingRight = true;
        }
    }
}
