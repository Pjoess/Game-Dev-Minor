using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{
    #region Component References
        Rigidbody rb;
        CapsuleCollider cc;
        public Weapon sword;
        AudioSource jumpSound;
    #endregion

    #region Basic Variables for (Movements and Jumping)
        public float jumpForce = 5;
        public float walkSpeed = 2;
        public float runSpeed = 5;
        public float speedChangeRate = 5;
        public float rotationSpeed = 500;
        [HideInInspector] public Vector2 movement;
        [HideInInspector] public bool isSprinting = false;
        public float jumpToFallTimer = 0.15f;
        [HideInInspector] public float jumpToFallDelta;

        public static event Action hasAttacked;
    #endregion

    #region Player States
        public PlayerBaseState playerState;
        public PlayerIdleState idleState = new PlayerIdleState();
        public PlayerWalkState walkState = new PlayerWalkState();
        public PlayerRunState runState = new PlayerRunState();
        public PlayerFallState fallState = new PlayerFallState();
        public PlayerStrikeState strikeState = new PlayerStrikeState();
        public PlayerStrike2State strike2State = new PlayerStrike2State();
        public PlayerStrike3State strike3State = new PlayerStrike3State();
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
        sword = GetComponentInChildren<Weapon>();
        animator = GetComponent<Animator>();
        AssignAnimIDs();
        playerState = idleState;
        playerState.EnterState(this);
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        jumpSound = GetComponent<AudioSource>();
        jumpToFallDelta = jumpToFallTimer;
    }

    // Update is called once per frame
    void Update()
    {
        playerState.UpdateState(this);
    }

    public void Movement()
    {
        float speed = isSprinting ? runSpeed : walkSpeed;
        
        animationBlend = Mathf.Lerp(animationBlend, speed, Time.deltaTime * speedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;
        animator.SetFloat(animIDSpeed, animationBlend);

        Vector3 direction = new(movement.x, 0, movement.y);

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

    #region New Input System Methods
        void OnMove(InputValue value) => movement = value.Get<Vector2>();

        void OnSprint(InputValue value) => isSprinting = value.isPressed;

        void OnJump()
        {
            if (playerState != fallState) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool(animIDJump, true);
            jumpSound.Play();
        }

        void OnAttack()
        {
            if (hasAttacked != null) hasAttacked.Invoke();
            else if (playerState != strikeState && playerState != strike3State) ChangeState(strikeState);
        }
    #endregion --- End ---

    public bool GroundCheck() => Physics.Raycast(transform.position + cc.center, Vector3.down, cc.bounds.extents.y + 0.1f);

    public void FallCheck()
    {
        if(!GroundCheck())
        {
            if(jumpToFallDelta > 0) jumpToFallDelta -= Time.deltaTime;
            else
            {
                animator.SetBool(animIDFall, true);
                ChangeState(fallState);
            }
        }
    }

    public bool IsAnimPlaying(string animStateName)
    {
        bool isAnimPlaying = animator.GetCurrentAnimatorStateInfo(0).length >
           animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
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

    public void ChangeState(PlayerBaseState state)
    {
        playerState.ExitState(this);
        playerState = state ?? playerState;
        playerState.EnterState(this);
    }

    private void OnFootstep(AnimationEvent animationEvent) { }

    private void OnLand(AnimationEvent animationEvent) { }
}