using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float baseMoveSpeed = 6.0f;
    private Vector2 moveInput;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private Rigidbody2D rb;

    [SerializeField] private Animator animator;

    private bool isFacingRight = true;

    void Awake()
    {
        // Initialise components once script is loaded
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
    }

    void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
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
