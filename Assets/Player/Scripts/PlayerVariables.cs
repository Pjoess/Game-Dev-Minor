using UnityEngine;
using System;

public partial class Player
{
    #region Component References
        [HideInInspector] public Rigidbody rigidBody;
        CapsuleCollider capsuleColider;
        public Weapon sword;
        public Enemy enemy;
        [HideInInspector] public AudioSource jumpSound;
    #endregion

    #region Basic Variables for (Movements and Jumping)
        // Jumping and Falling
        public float jumpForce = 5f;
        public float jumpCooldown = 1f;
        [HideInInspector] public float jumpCooldownDelta;
        public float idleToFallTimer = 0.15f;
        [HideInInspector] public float idleToFallDelta;
        // Moving
        public float walkSpeed = 2f;
        public float runSpeed = 5f;
        public float dashForce  = 1.5f;
        private bool isDashing = false;
        public Vector3 dashDirection;
        public float speedChangeRate = 5f;
        public float rotationSpeed = 600f;
        [HideInInspector] public Vector2 movement;
        [HideInInspector] public bool isSprinting = false;
        [HideInInspector] public bool hasJumped = false;
        public bool isStriking = false;
    #endregion

    #region Sword Attack and Collison
        [HideInInspector] public event Action HasAttacked;
        [HideInInspector] public bool struckAgain;
        public float attackDistance = 0.15f;
    #endregion

    #region Player States
        public PlayerBaseState playerState;
        public PlayerIdleState idleState = new();
        public PlayerWalkState walkState = new();
        public PlayerFallState fallState = new();
        public PlayerJumpState jumpState = new();
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
        enemy = GetComponentInChildren<Enemy>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        capsuleColider = GetComponent<CapsuleCollider>();
        // Default state
        playerState = idleState;
        playerState.EnterState(this);
        // Jumping
        jumpSound = GetComponent<AudioSource>();
        idleToFallDelta = idleToFallTimer;
        jumpCooldownDelta = 0f;
    }
}