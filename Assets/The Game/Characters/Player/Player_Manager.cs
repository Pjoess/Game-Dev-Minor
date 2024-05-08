using System;
using System.Collections;
using buddy;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
    
public class Player_Manager : MonoBehaviour, IDamageble
{
    #region Variables & References
        private Buddy_Agent buddy;
        [HideInInspector] public Rigidbody rigidBody;
        [HideInInspector] private CapsuleCollider capsuleColider;
        [HideInInspector] public Weapon sword;
        [HideInInspector] public Vector2 movement;
        [HideInInspector] public Vector3 vectorDirection;
        private PlayerInput input;

        [Header("Sound Effects")]
        public AudioSource jumpSound;
        public AudioSource ouchSound;
        public AudioSource rollSound;
        public AudioSource footStepSound;
        public AudioSource pauseSound;

        // --- IDamagable --- //
        [Header("Player Healthpoint")]
        [SerializeField] private int healthPoints;
        [SerializeField] private int maxHealthPoints = 100;
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        [HideInInspector] public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
        
        [Header("Player Move")]
        public float walkSpeed;
        public float runSpeed;
        public float speedChangeRate;
        public float idleToFallTimer = 0.15f;
        public float idleToFallDelta;

        [Header("Player Jump")]
        public bool hasJumped = false;
        public float jumpForce = 5f;
        public float jumpCooldown = 1f;
        public float jumpCooldownDelta;

        [Header("Player Sprinting Keyboard Exclusive")]
        public float lastWKeyPressTime = 0f;
        public float doublePressTimeWindow = 0.2f;
        public bool isSprinting = false;
        public bool sprintToggle = false;

        [Header("Player Rotation")]
        public float rotationSpeed = 600f;
        
        [Header("Player Dashing")]
        public Vector3 dashDirection;
        public float dashForce  = 1.5f;
        public float dashCooldown = 2f;
        public bool isDashing = false;
        [HideInInspector] public float dashCooldownDelta;
        
        [Header("Player Attack")]
        public float attackDistance = 0.15f;
        public bool isStriking = false;
        [SerializeField] int baseMortarIncrease = 10;
        public int clickAmount = 0;
        [HideInInspector] public bool canAttack = false;
        //[HideInInspector] public event Action HasAttacked;
        [HideInInspector] public bool struckAgain;

        [Header("UI Canvas and Buttons")]
        public static bool isPaused = false;
        private PauseMenu pauseMenu;
        private DeathScript deathScript;
        public float buttonCameraOffsetForward = -50f;
        public float buttonCameraOffsetRight = -50f;
        public float buttonCameraOffsetUp = -50f;
        // --- UI CameraFollow --- //
        private CinemachineVirtualCamera virtualCamera;
        private CinemachineOrbitalTransposer transposer;
        [SerializeField] float minCameraZoomZ = -10f;
        [SerializeField] float minCameraZoomY = 2f;
        [SerializeField] float maxCameraZoomZ = -30f;
        [SerializeField] float maxCameraZoomY = 22f;
        [SerializeField] private Vector3 buttonCameraOffset = new(950,100,0); // Adjust this for correct placement

        // --- Buddy --- //
        [Header("Buddy Controls")]
        [SerializeField] private AudioSource buddySwitchMode;

        // --- Player States --- //
        public PlayerBaseState playerState;
        public PlayerIdleState idleState = new();
        public PlayerWalkState walkState = new();
        public PlayerFallState fallState = new();
        public PlayerJumpState jumpState = new();
        public PlayerRunState runState = new();
        public PlayerDashState dashState = new();
        public PlayerStrikeState strikeState = new();
        //public PlayerStrike2State strike2State = new();
        //public PlayerStrike3State strike3State = new();

        // --- Player Animation --- //
        [HideInInspector] public Animator animator;
        [HideInInspector] public float animationBlend;
        // --- Animation parameters IDs --- //
        [HideInInspector] public int animIDSpeed;
        [HideInInspector] public int animIDGrounded;
        [HideInInspector] public int animIDFall;
        [HideInInspector] public int animIDJump;
        [HideInInspector] public int animIDStrike;
        //[HideInInspector] public int animIDStrike1;
        //[HideInInspector] public int animIDStrike2;
        //[HideInInspector] public int animIDStrike3;
        [HideInInspector] public int animIDDash;
        [HideInInspector] public int animIDMoveSpeed;
    #endregion

    private void AssignAnimIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDFall = Animator.StringToHash("Fall");
        animIDJump = Animator.StringToHash("Jump");
        animIDStrike = Animator.StringToHash("Strike");
        //animIDStrike1 = Animator.StringToHash("Strike1");
        //animIDStrike2 = Animator.StringToHash("Strike2");
        //animIDStrike3 = Animator.StringToHash("Strike3");
        animIDDash = Animator.StringToHash("Dash");
        animIDMoveSpeed = Animator.StringToHash("MoveSpeed");
    }

    #region Default Unity Function
        // By Default on Start this will be the stats of the player
        private void StatsOnAwake()
        {
            walkSpeed = 3f;
            runSpeed = 7f;
            speedChangeRate = 5f;
        }

        // Load before starting the Game
        void Awake(){
            StatsOnAwake();
            pauseMenu = FindAnyObjectByType<PauseMenu>();
            // Load Animations
            AssignAnimIDs();
            // References
            sword = GetComponentInChildren<Weapon>();
            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody>();
            capsuleColider = GetComponent<CapsuleCollider>();
            // Get the Cinemachine Virtual Camera component
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            transposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            input = GetComponent<PlayerInput>();
            deathScript = FindObjectOfType<DeathScript>();
            buddy = FindObjectOfType<Buddy_Agent>();
        }
        
        void Start()
        { 
            // Default state
            playerState = idleState;
            playerState.EnterState(this);
            // Jumping
            jumpSound = GetComponent<AudioSource>();
            idleToFallDelta = idleToFallTimer;
            jumpCooldownDelta = 0f;
            // UI
            Time.timeScale = 1;
            isPaused = false;
            // Health
            healthPoints = maxHealthPoints;
            Healthbar healthbar = FindObjectOfType<Healthbar>();
            if (healthbar != null)
            {
                healthbar.OnHealthUpdated += HandleHealthUpdated;
            }
            animator.SetFloat(animIDMoveSpeed, 1);
            sword.onWeaponHit += WeaponHit;
        }

        void Update(){
            playerState.UpdateState(this);
        }
    #endregion

    #region General Functions
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

    #region Player Movement
        public void Movement()
        {
            // Check for double press of the 'W' or other keys depends where you point
            // --- Saved for now need to look for a better solution ---
            if (Input.GetKeyDown(KeyCode.W))
            {
                float currentTime = Time.time;
                if (currentTime - lastWKeyPressTime <= doublePressTimeWindow)
                {
                    isSprinting = !isSprinting;
                }
                lastWKeyPressTime = currentTime;
            }

            if (sprintToggle) isSprinting = true;

            float speed = isSprinting ? runSpeed : walkSpeed;

            animationBlend = Mathf.Lerp(animationBlend, speed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;
            animator.SetFloat(animIDSpeed, animationBlend);

            if (!input.currentControlScheme.Equals("Keyboard&Mouse"))
            {
                animator.SetFloat(animIDMoveSpeed, movement.magnitude);
            }
            else animator.SetFloat(animIDMoveSpeed, 1);

            vectorDirection = new Vector3(movement.x, 0, movement.y);
            // Added Rigid Body Velocity for movement, to fix some bug fixes going through objects
            rigidBody.velocity = vectorDirection * walkSpeed;
            rigidBody.velocity = vectorDirection * runSpeed;

            Vector3 cameraFaceForward = Camera.main.transform.forward;
            Vector3 cameraFaceRight = Camera.main.transform.right;
            cameraFaceForward.y = 0;
            cameraFaceRight.y = 0;
            cameraFaceForward = cameraFaceForward.normalized;
            cameraFaceRight = cameraFaceRight.normalized;

            Vector3 moveDirection = cameraFaceForward * vectorDirection.z + cameraFaceRight * vectorDirection.x;

            transform.Translate(speed * Time.deltaTime * moveDirection, Space.World);

            if (vectorDirection != Vector3.zero)
            {
                Vector3 lookDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * vectorDirection;
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

            Vector3 moveDirection = new(movement.x, 0, movement.y);
            if (moveDirection == Vector3.zero) moveDirection = transform.forward;

            if (isDashing && dashCooldownDelta <= 0)
            {
                if (moveDirection != Vector3.zero)
                {
                    animator.SetBool(animIDDash, true);
                    ChangeState(dashState);
                    Vector3 lookDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * moveDirection;
                    Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                    transform.rotation = rotation;
                    dashDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * moveDirection;
                    dashDirection.y = 0; // Set the vertical component to zero to avoid moving up or down
                                         // Set isDashing to true to indicate the player is currently dashing
                    
                }
            }
            else if (dashCooldownDelta > 0)
            {
                dashCooldownDelta -= Time.deltaTime;
            }  
        }
    #endregion

    #region Player Attack

        public void Attack()
        {
            if(isStriking)
            {
                ChangeState(strikeState);
            }
        }
        public void AttackRotation()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
            {
                Vector3 direction = hit.point - transform.position;
                direction.y = 0;
                direction.Normalize();
                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = rotation;
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Vector3 direction = hit.point - transform.position;
                direction.y = 0;
                direction.Normalize();
                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = rotation;
            }
        }

        public void OnAttackPressed()
        {
            if(canAttack)
            {
                struckAgain = true;
            }
        }
        
        private void WeaponHit()
        {
            AddToMortarBar();
        }

        private void AddToMortarBar()
        {
            if(clickAmount == 0) clickAmount = 1;
            int increaseAmount = baseMortarIncrease / clickAmount;
            Blackboard.instance.AddToMortarBar(increaseAmount);
        }

        public void MoveForwardOnAttack(){
            // Move a little bit forward when attacking
            Vector3 newPosition = transform.position + transform.forward * attackDistance;
            transform.position = newPosition;
        }
    #endregion

    #region Player Animations
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

        public bool IsAnimFinished(float timePlayed)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            return info.normalizedTime >= timePlayed;
        }
    #endregion

    #region New Input System Methods
        void OnMove(InputValue value) => movement = value.Get<Vector2>();

        void OnSprintToggle()
        {
            if (!sprintToggle) sprintToggle = true;
            else
            {
                sprintToggle = false;
                isSprinting = false;
            }
        }

        void OnSprintHold(InputValue value)
        {
            if(value.isPressed) isSprinting = true;
            else isSprinting = false;
        }
        
        // // Removed -- but kept for the future
        // void OnJump(InputValue value)
        // {
        //     if (value.isPressed && IsGrounded()) {
        //         hasJumped = true;
        //     }
        // }

        void OnAttack(InputValue value)
        {
            if(value.isPressed && playerState != dashState){
                AttackRotation();
                clickAmount++;
                if (isStriking) OnAttackPressed();
                else if (!isStriking)
                {
                    isStriking = true;
                }
            }
        }

        void OnDodge(InputValue value)
        {
            if (value.isPressed && !isDashing)  isDashing = true;
        }

        void OnZoom(InputValue value)
       {
            var val = value.Get<float>();
            transposer.m_FollowOffset.z += (val * Time.deltaTime);
            transposer.m_FollowOffset.y -= (val * Time.deltaTime);

            if (transposer.m_FollowOffset.z > minCameraZoomZ) transposer.m_FollowOffset.z = minCameraZoomZ;
            if (transposer.m_FollowOffset.z < maxCameraZoomZ) transposer.m_FollowOffset.z = maxCameraZoomZ;

            if (transposer.m_FollowOffset.y > maxCameraZoomY) transposer.m_FollowOffset.y = maxCameraZoomY;
            if (transposer.m_FollowOffset.y < minCameraZoomY) transposer.m_FollowOffset.y = minCameraZoomY;
    }

        void OnPause(InputValue value)
        {
            pauseSound.Play();
            if (value.isPressed && !isPaused)
            {
                Debug.Log("Game Paused");
                Time.timeScale = 0;
                isPaused = true;
                pauseMenu.EnablePauseCanvas();
            }
            else
            {
                Debug.Log("Game Started");
                Time.timeScale = 1;
                isPaused = false;
                pauseMenu.EnablePauseCanvas();
            }
        }

        void OnDebugTakeDamage()
        {
            if(healthPoints > 0)
            {
                healthPoints -= 30;
                if (healthPoints < 0)   healthPoints = 0;
            }
            if(healthPoints <= 0){
                StartCoroutine(WaitThenEnableDeath(healthPoints));
            }
        }

        void OnBuddyMortarAttack()
        {
            if (buddy != null)
            {
                if(buddy.canShootMortar) buddy.shootMortar = true;
            }
        }

        // --- will be used later --- //
        // void OnToggleBuddyAttack(InputValue value)
        // {
        //     buddySwitchMode.Play();
        //     // Toggle the attack behavior
        //     buddy.ToggleAttackBehaviour();
        // }

    #endregion --- End ---

    #region AttackAnimationEvents
        public void StartAttack()
        {
            if (isStriking)
            {
                struckAgain = false;
                canAttack = true;
                sword.DoSwordAttackEnableCollision();
            }
        }

        public void EndAttack()
        {
            clickAmount = 0;
            if (isStriking)
            {
                sword.SwordToDefault();
                canAttack = false;
                //Check if Dash is queued
                if (isDashing)
                {
                    struckAgain = false;
                    Dash();
                }

                if (!struckAgain)
                {
                    ChangeState(idleState);
                }
            }
            
        }

        public void DisableSwordCollision()
        {
            sword.SwordToDefault();
            clickAmount = 0;
            if (isDashing)
            {
                canAttack = false;
                struckAgain = false;
                Dash();
            }
        }

        public void EndAttackAnim()
        {
            sword.SwordToDefault();
            canAttack = false;
            struckAgain = false;
            if (isDashing)
            {
                Dash();
            }
            ChangeState(idleState);
        }

        public void EndDash()
        {
            ChangeState(idleState);
        }
    #endregion

    #region Player Heal
        public void HealPlayer(int value)
        {
            if(healthPoints < maxHealthPoints)
            {
                healthPoints += value;
                if (healthPoints > maxHealthPoints) healthPoints = maxHealthPoints;
            }
        }
    #endregion

    #region IDamagable
        public void Hit(int damage)
        {
            if(playerState != dashState) 
            {
                ouchSound.Play();
                healthPoints -= damage;
                HandleHealthUpdated(healthPoints);
            }
        }

        public void ApplyKnockback(Vector3 pos)
        {
            if(playerState != dashState)
            {
                Vector3 pushDirection = transform.position - pos;
                pushDirection.Normalize();
                rigidBody.AddForce(pushDirection * 300, ForceMode.Acceleration);
            }
            
        }

        void HandleHealthUpdated(float currentHealth)
        {
            if (currentHealth <= 0)
            {
                // Do something when health reaches zero
                StartCoroutine(WaitThenEnableDeath((int)currentHealth));
            }
        }

        private IEnumerator WaitThenEnableDeath(int health) 
        {
            yield return new WaitForSeconds(0.6f);
            deathScript.EnableDeathCanvas(health);
    }
    #endregion

    

    private void OnFootstep(AnimationEvent animationEvent){}
    private void OnLand(AnimationEvent animationEvent){}
}