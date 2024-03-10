using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Component References
        Rigidbody rb;
        CapsuleCollider cc;
        public Weapon sword;
    #endregion

    #region Basic Variables for (Movements and Jumping)
        public float jumpForce = 3;
        public float walkSpeed = 2;
        public float runSpeed = 5;
        public float speedChangeRate = 5;
        public float rotationSpeed = 500;
        [HideInInspector] public Vector2 movement;
        [HideInInspector] public bool isSprinting = false;
        public float jumpToFallTimer = 0.15f;
    #endregion

    #region Player States
        public PlayerBaseState playerState;
        public PlayerIdleState idleState = new PlayerIdleState();
        public PlayerWalkState walkState = new PlayerWalkState();
        public PlayerRunState runState = new PlayerRunState();
        public PlayerFallState fallState = new PlayerFallState();
        public PlayerHitState hitState = new PlayerHitState();
    #endregion

    #region Player Animation
        [HideInInspector] public Animator animator;
        [HideInInspector] public float animationBlend;
        // --- Animation parameters IDs --- //
        [HideInInspector] public int animIDSpeed;
        [HideInInspector] public int animIDGrounded;
        [HideInInspector] public int animIDFall;
        [HideInInspector] public int animIDJump;
        [HideInInspector] public int animIDStriking;
    #endregion

    private void AssignAnimIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDFall = Animator.StringToHash("Fall");
        animIDJump = Animator.StringToHash("Jump");
        animIDStriking = Animator.StringToHash("Striking");
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

    }

    // Update is called once per frame
    void Update()
    {
        playerState.UpdateState(this);
    }
    public void Movement()
    {
        if (movement == Vector2.zero) ChangeState(idleState);

        float speed = isSprinting ? runSpeed : walkSpeed;
        
        animationBlend = Mathf.Lerp(animationBlend, speed, Time.deltaTime * speedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;
        animator.SetFloat(animIDSpeed, animationBlend);

        Vector3 direction = new Vector3(movement.x, 0, movement.y);

        Vector3 camF = Camera.main.transform.forward;
        Vector3 camR = Camera.main.transform.right;
        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        Vector3 moveDir = camF * direction.z + camR * direction.x;

        transform.Translate(speed * Time.deltaTime * moveDir, Space.World);

        Vector3 lookDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * direction;
        Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

    }

    #region New Input System
        void OnMove(InputValue value)
        {
            movement = value.Get<Vector2>();
        }

        void OnSprint(InputValue value)
        {
            if (value.isPressed) isSprinting = true;
            else isSprinting = false;
        }

        void OnJump()
        {
            if(playerState != fallState) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool(animIDJump, true);
        }

        void OnAttack()
        {
            if (playerState != hitState) ChangeState(hitState);
        }
    #endregion

    public bool GroundCheck()
    {
        return Physics.Raycast(transform.position + cc.center, Vector3.down, cc.bounds.extents.y + 0.1f);
    }

    public void ChangeState(PlayerBaseState state)
    {
        playerState.ExitState(this);
        playerState = state;
        playerState.EnterState(this);
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        
    }

    private void OnLand(AnimationEvent animationEvent)
    {
       
    }

}
