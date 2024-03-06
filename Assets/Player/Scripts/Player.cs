using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider cc;

    [HideInInspector] public Vector2 movement;
    [HideInInspector] public bool isSprinting = false;
    public float jumpToFallTimer = 0.15f;
    [HideInInspector] public float _animationBlend;

    public float jumpForce = 3;
    public float walkSpeed = 2;
    public float runSpeed = 5;

    public PlayerBaseState playerState;
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerWalkState walkState = new PlayerWalkState();
    public PlayerRunState runState = new PlayerRunState();
    public PlayerFallState fallState = new PlayerFallState();

    [HideInInspector] public Animator animator;

    //Animation param IDs
    [HideInInspector] public int animIDSpeed;
    [HideInInspector] public int animIDGrounded;
    [HideInInspector] public int animIDFall;
    [HideInInspector] public int animIDJump;
    [HideInInspector] public int animIDMotionMagnitude;

    private void AssignAnimIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDFall = Animator.StringToHash("Fall");
        animIDJump = Animator.StringToHash("Jump");
        animIDMotionMagnitude = Animator.StringToHash("MotionMagnitude");
    }

    // Start is called before the first frame update
    void Start()
    {
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
        //animator.SetFloat(animIDMotionMagnitude, 1f);
        if (movement == Vector2.zero) ChangeState(idleState);

        float speed = isSprinting ? runSpeed : walkSpeed;
        transform.Translate(new Vector3(movement.x, 0, movement.y) * speed * Time.deltaTime);

        _animationBlend = Mathf.Lerp(_animationBlend, speed, Time.deltaTime * 5f);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        animator.SetFloat(animIDSpeed, _animationBlend);

    }

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
