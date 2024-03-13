using UnityEngine;
using System;

public partial class Player
{
    #region Component References
        Rigidbody rigidBody;
        CapsuleCollider capsuleColider;
        public Weapon sword;
        AudioSource jumpSound;
    #endregion

    #region Basic Variables for (Movements and Jumping)
        // Jumping and Falling
        public float jumpForce = 5f;
        public float jumpToFallTimer = 0.15f;
        [HideInInspector] public float jumpToFallDelta;
        // Moving
        public float walkSpeed = 2f;
        public float runSpeed = 5f;
        public float dashForce  = 2.5f;
        public float speedChangeRate = 5f;
        public float rotationSpeed = 500f;
        [HideInInspector] public Vector2 movement;
        [HideInInspector] public bool isSprinting = false;
    #endregion

    #region Sword Attack and Collison
        public event Action HasAttacked;
        public bool struckAgain;
    #endregion

    #region Player States
        public PlayerBaseState playerState;
        public PlayerIdleState idleState = new();
        public PlayerWalkState walkState = new();
        public PlayerFallState fallState = new();
        public PlayerRunState runState = new();
        public PlayerDashState dashState = new();
        public PlayerStrikeState strikeState = new();
        public PlayerStrike2State strike2State = new();
        public PlayerStrike3State strike3State = new();
    #endregion

    #region Player Animation
    [HideInInspector] public Animator animator;
        [HideInInspector] public float animationBlend;
        // --- Animation parameters IDs --- //
        [HideInInspector] public int animIDSpeed;
        [HideInInspector] public int animIDGrounded;
        [HideInInspector] public int animIDFall;
        [HideInInspector] public int animIDJump;
        [HideInInspector] public int animIDStrike1;
        [HideInInspector] public int animIDStrike2;
        [HideInInspector] public int animIDStrike3;
    #endregion

    private void AssignAnimIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDFall = Animator.StringToHash("Fall");
        animIDJump = Animator.StringToHash("Jump");
        animIDStrike1 = Animator.StringToHash("Strike1");
        animIDStrike2 = Animator.StringToHash("Strike2");
        animIDStrike3 = Animator.StringToHash("Strike3");
    }

    // Start is called before the first frame update
    void Start()
    {
        AssignAnimIDs();
        // References
        sword = GetComponentInChildren<Weapon>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        capsuleColider = GetComponent<CapsuleCollider>();
        // Default state
        playerState = idleState;
        playerState.EnterState(this);
        // Jumping
        jumpSound = GetComponent<AudioSource>();
        jumpToFallDelta = jumpToFallTimer;
    }
}