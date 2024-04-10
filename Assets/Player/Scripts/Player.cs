using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
    
public class Player : MonoBehaviour, IDamageble
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
        idleToFallDelta = idleToFallTimer;
        jumpCooldownDelta = 0f;
        // UI
        uiButton.SetActive(false); // Make the button invisible
        Time.timeScale = 1; // start game unPaused
        isPaused = false;

        // Get the Cinemachine Virtual Camera component
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        healthPoints = maxHealthPoints;
    }

    // Update is called once per frame
    void Update(){
        playerState.UpdateState(this);
    }

    #region General Methods
        public void ChangeState(PlayerBaseState state)
        {
            playerState.ExitState(this);
            playerState = state ?? playerState; // If state is null -> stay on the playerState
            playerState.EnterState(this);
        }

        public bool IsGrounded()
        {
            return Physics.Raycast(transform.position + capsuleColider.center + new Vector3(0.4f, 0.0f, -0.4f), Vector3.down, capsuleColider.bounds.extents.y + 0.1f) ||
                Physics.Raycast(transform.position + capsuleColider.center + new Vector3(-0.4f, 0.0f, 0.4f), Vector3.down, capsuleColider.bounds.extents.y + 0.1f) ||
                Physics.Raycast(transform.position + capsuleColider.center + new Vector3(-0.4f, 0.0f, -0.4f), Vector3.down, capsuleColider.bounds.extents.y + 0.1f) ||
                Physics.Raycast(transform.position + capsuleColider.center + new Vector3(0.4f, 0.0f, 0.4f), Vector3.down, capsuleColider.bounds.extents.y + 0.1f);
        }
        public void FallCheck()
        {
            if(!IsGrounded())
            {
                if(idleToFallDelta > 0){
                    idleToFallDelta -= Time.deltaTime;
                } else {
                    animator.SetBool(animIDFall, true);
                    ChangeState(fallState);
                }
            }
        }
    #endregion

    #region Movements and Facing Direction
        public void Movement()
        {
            float speed = isSprinting ? runSpeed : walkSpeed;
            
            animationBlend = Mathf.Lerp(animationBlend, speed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;
            animator.SetFloat(animIDSpeed, animationBlend);

            direction = new(movement.x, 0, movement.y);

            Vector3 cameraFaceForward = Camera.main.transform.forward;
            Vector3 cameraFaceRight = Camera.main.transform.right;
            cameraFaceForward.y = 0;
            cameraFaceRight.y = 0;
            cameraFaceForward = cameraFaceForward.normalized;
            cameraFaceRight = cameraFaceRight.normalized;

            Vector3 moveDirection = cameraFaceForward * direction.z + cameraFaceRight * direction.x;

            transform.Translate(speed * Time.deltaTime * moveDirection, Space.World);

            if(direction != Vector3.zero) 
            {
                Vector3 lookDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * direction;
                Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
        }

        public void Jump()
        {
            if(jumpCooldownDelta > 0)
            {
                jumpCooldownDelta -= Time.deltaTime;
            }
            else if(hasJumped)
            {
                ChangeState(jumpState);
            }
        }

        public void Dash()
        {
        //When dashing while attacking, the dash is queued and happens immediately after finising the attack. Bug or feature?
        //You can dash through small spaces.

            Vector3 moveDirection = new Vector3(movement.x, 0, movement.y);

            if (isDashing && dashCooldownDelta <= 0)
            {
                if (moveDirection != Vector3.zero)
                {
                    Vector3 lookDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * moveDirection;
                    Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                    transform.rotation = rotation;
                    dashDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * moveDirection;
                    dashDirection.y = 0; // Set the vertical component to zero to avoid moving up or down
                                         // Set isDashing to true to indicate the player is currently dashing
                    ChangeState(dashState);
                }
            }
            else if (dashCooldownDelta > 0)
            {
                dashCooldownDelta -= Time.deltaTime;
            }

            if(moveDirection == Vector3.zero) 
            {
                isDashing = false;
            }    
        }
    #endregion

    #region Attacks Methods

    public void Attack()
    {
        if(isStriking)
        {
            ChangeState(strikeState);
        }
    }
        public void AttackRotation()
        {
            Vector3 direction = new(movement.x, 0, movement.y);

            if (direction != Vector3.zero)
            {
                Vector3 lookDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * direction;
                Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * 0.5f * Time.deltaTime);
            }
        }

        public void OnAttackStruck() => struckAgain = true;

        public void MoveForwardOnAttack(){
            // Move a little bit forward when attacking
            Vector3 newPosition = transform.position + transform.forward * attackDistance;
            transform.position = newPosition;
        }
    #endregion

    #region Animation of Player
        public bool IsAnimPlaying(string animStateName)
        {
            bool isAnimPlaying = animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            return isAnimPlaying && animator.GetCurrentAnimatorStateInfo(0).IsName(animStateName);
        }

        public bool IsAnimFinished(string animStateName)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(animStateName))
            {
                AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
                return info.normalizedTime > 1.0f;
            }
            else return false;
        }
    #endregion

    #region New Input System Methods
        void OnMove(InputValue value) => movement = value.Get<Vector2>();

        void OnSprint(InputValue value)
        {
            if(!isSprinting) isSprinting = true;
            else isSprinting = false;
        }

        // void OnJump(InputValue value)
        // {
        //     if (value.isPressed && IsGrounded()) {
        //         hasJumped = true;
        //     }
        // }

        void OnAttack(InputValue value)
        {
            if(value.isPressed && IsGrounded()){
                if (HasAttacked != null) HasAttacked.Invoke();
                else if (!isStriking)
                {
                    isStriking = true;
                }
            }
        }

        void OnDash(InputValue value)
        {
            if (value.isPressed && !isDashing)  isDashing = true;
        }

        void OnPause(InputValue value)
        {
            if (value.isPressed && !isPaused)
            {
                Debug.Log("Game Paused");
                Time.timeScale = 0;
                isPaused = true;
                uiButton.SetActive(true); // Make the button visible
            }
            else
            {
                Debug.Log("Game Started");
                Time.timeScale = 1;
                isPaused = false;
                uiButton.SetActive(false); // Make the button invisible
            }
        }

        void OnDebugTakeDamage()
        {
            if(healthPoints > 0)
            {
                healthPoints -= 30;
                if (healthPoints < 0)   healthPoints = 0;
            }
        }

        void OnToggleBuddyAttack(InputValue value)
        {
            buddy.ToggleAttackBehaviour();
        }


    #endregion --- End ---


    public void EnableSwordCollision()
    {
        sword.DoSwordAttackEnableCollision();
    }

    public void DisableSwordCollision()
    {
        sword.SwordToDefault();
    }


    public void HealPlayer(int value)
    {
        if(healthPoints < maxHealthPoints)
        {
            healthPoints += value;
            if (healthPoints > maxHealthPoints) healthPoints = maxHealthPoints;
        }
    }

    public void Hit(int damage)
    {
        // int random = Random.Range(1,2);

        // if(random == 1){
        //     healthPoints -= maxHealthPoints;
        // }
        if(playerState != dashState) healthPoints -= damage;
    }

    public void ApplyKnockback(Vector3 pos)
    {
        //Knockback code
    }

    private void OnFootstep(AnimationEvent animationEvent){}
    private void OnLand(AnimationEvent animationEvent){}
}
