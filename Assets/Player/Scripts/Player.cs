using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: 
// Superjump BUG (rennen op een slime en dan space) maybe a bug?
// Collision Fixen vibrating against wall

public partial class Player : MonoBehaviour, IDamageble
{
    // Update is called once per frame
    void Update() => playerState.UpdateState(this);

    #region General Methods
        public void ChangeState(PlayerBaseState state)
        {
            playerState.ExitState(this);
            playerState = state ?? playerState;
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

        public void Dash(){
                Vector3 moveDirection = new Vector3(movement.x, 0, movement.y).normalized;

                if (moveDirection != Vector3.zero)
                {
                    dashDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * moveDirection;
                    dashDirection.y = 0; // Set the vertical component to zero to avoid moving up or down
                    // Set isDashing to true to indicate the player is currently dashing
                    DashCooldown();
                }
        }

        public void DashCooldown()
        {
            StartCoroutine(DashCoroutine(dashDirection.normalized));
            isDashing = true;
        }

        private IEnumerator DashCoroutine(Vector3 dashDirection)
        {
            float elapsed = 0f;
            float duration = 0.4f; // Adjust the duration as needed for dashing

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float currentSpeed = Mathf.Lerp(0, dashForce, elapsed / duration);
                rigidBody.AddForce(currentSpeed * dashDirection, ForceMode.Impulse);
                yield return null;
            }

            // Wait for cooldown after the dash is complete then reset
            yield return new WaitForSeconds(2f);
            isDashing = false;
        }
    #endregion

    #region Attacks Methods
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
                else if (playerState != strikeState && playerState != strike3State) ChangeState(strikeState);
            }
        }

        void OnDash(InputValue value)
        {
            if (value.isPressed && IsGrounded() && !isDashing && !isStriking) ChangeState(dashState);
        }
    #endregion --- End ---

    public void Hit()
    {
        HealthPoints--;
    }

    public void ApplyKnockback(Vector3 pos)
    {
        //Knockback code
    }

    public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }

    private void OnFootstep(AnimationEvent animationEvent){}
    private void OnLand(AnimationEvent animationEvent){}
}
