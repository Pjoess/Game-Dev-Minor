using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb;

    [HideInInspector] public Vector2 movement;
    private bool isSprinting = false;

    public float jumpForce = 3;
    public float walkSpeed = 2;
    public float runSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSprinting) Movement(walkSpeed);
        else Movement(runSpeed);
    }
    public void Movement(float speed)
    {
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
}
