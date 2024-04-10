using UnityEngine;
using System;
using System.Collections;
using Cinemachine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public partial class Player
{
    #region Component References
        [SerializeField] private BuddyAI_Controller buddy;
        [HideInInspector] public Rigidbody rigidBody;
        CapsuleCollider capsuleColider;
        [HideInInspector] public Weapon sword;
        [HideInInspector] public AudioSource jumpSound;
    #endregion

    #region Basic Variables for (Movements and Jumping)
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        [SerializeField] private int maxHealthPoints = 3;
        public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
        private int healthPoints;

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
        [HideInInspector] public bool isDashing = false;
        public Vector3 dashDirection;
        public float speedChangeRate = 5f;
        public float rotationSpeed = 600f;
        [HideInInspector] public Vector2 movement;
        [HideInInspector] public Vector3 direction;
        [HideInInspector] public bool isSprinting = false;
        [HideInInspector] public bool hasJumped = false;
        public bool isStriking = false;


        //Dash
        public float dashCooldown = 2f;
        [HideInInspector] public float dashCooldownDelta;

        // UI Buttons
        public bool isPaused = false;
        [SerializeField] GameObject uiButton;
        public float buttonCameraOffsetForward = -50f;
        public float buttonCameraOffsetRight = -50f;
        public float buttonCameraOffsetUp = -50f;

        // UI CameraFollow
        private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Vector3 buttonCameraOffset = new(950,100,0); // Adjust this for correct placement
    #endregion

    #region Sword Attack and Collison
        [HideInInspector] public event Action HasAttacked;
        [HideInInspector] public bool struckAgain;
        public float attackDistance = 0.15f;
        public float attackDamage = 10f;
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
        [HideInInspector] public int animIDDash;
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
        animIDDash = Animator.StringToHash("Dash");
    }
}