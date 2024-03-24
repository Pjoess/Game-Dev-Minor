using System.Collections;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// TODO:
// Superjump BUG (rennen op een slime en dan space) maybe a bug?
// Collision Fixen vibrating against wall
    
public partial class Player : MonoBehaviour, IDamageble
{
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
        // UI
        uiButton.SetActive(false); // Make the button invisible

        // Get the Cinemachine Virtual Camera component
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        healthPoints = maxHealthPoints;
    }

void LateUpdate()
    {
        if (uiButton != null && virtualCamera != null)
        {
            // Get the position of the virtual camera
            Vector3 cameraPosition = virtualCamera.transform.position;

            // Calculate the position of the button relative to the camera
            Vector3 buttonPosition = cameraPosition + virtualCamera.transform.rotation * buttonCameraOffset;

            // Update the position of the button
            uiButton.transform.position = buttonPosition;
        }
        else
        {
            Debug.LogWarning("UI button or Cinemachine Virtual Camera not assigned.");
        }
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

        void OnSprint(InputValue value) => isSprinting = value.isPressed;

        void OnJump(InputValue value)
        {
            if (value.isPressed && IsGrounded()) {
                hasJumped = true;
            }
        }

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
                // SceneManager.LoadScene("MainMenu"); // stil testing go to scene 
            }
            else
            {
                Debug.Log("Game Started");
                Time.timeScale = 1;
                isPaused = false;
                uiButton.SetActive(false); // Make the button invisible
            }
        }


    #endregion --- End ---



    public void HealPlayer(int value)
    {
        if(healthPoints < maxHealthPoints)
        {
            healthPoints += value;
            if (healthPoints > maxHealthPoints) healthPoints = maxHealthPoints;
        }
    }

    public void Hit()
    {
        HealthPoints--;
    }

    public void ApplyKnockback(Vector3 pos)
    {
        //Knockback code
    }

    private void OnFootstep(AnimationEvent animationEvent){}
    private void OnLand(AnimationEvent animationEvent){}
}
