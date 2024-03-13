using UnityEngine;
using UnityEngine.InputSystem;
using System;

public partial class Player : MonoBehaviour
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

        public bool IsGrounded() => Physics.Raycast(transform.position + capsuleColider.center, Vector3.down, capsuleColider.bounds.extents.y + 0.1f);

        public void FallCheck()
        {
            if(!IsGrounded())
            {
                if(jumpToFallDelta > 0){
                    jumpToFallDelta -= Time.deltaTime;
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
            if (value.isPressed && IsGrounded() && playerState != fallState) {
                rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                animator.SetBool(animIDJump, true);
                jumpSound.Play();
            }
        }

        void OnAttack(InputValue value)
        {
            if(value.isPressed && IsGrounded())
            {
                if (HasAttacked != null) 
                {
                    HasAttacked.Invoke();
                }
                else if (playerState != strikeState && playerState != strike3State) 
                {
                    ChangeState(strikeState);
                }
            }
        }
    #endregion --- End ---

    private void OnFootstep(AnimationEvent animationEvent){}
    private void OnLand(AnimationEvent animationEvent){}
}