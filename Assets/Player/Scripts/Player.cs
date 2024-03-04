using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider cc;

    [HideInInspector] public Vector2 movement;
    [HideInInspector] public bool isSprinting = false;

    public float jumpForce = 3;
    public float walkSpeed = 2;
    public float runSpeed = 5;

    public PlayerBaseState playerState;
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerWalkState walkState = new PlayerWalkState();
    public PlayerRunState runState = new PlayerRunState();
    public PlayerFallState fallState = new PlayerFallState();

    // Start is called before the first frame update
    void Start()
    {
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
        float speed = isSprinting ? runSpeed : walkSpeed;
        transform.Translate(new Vector3(movement.x, 0, movement.y) * speed * Time.deltaTime);
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

}
