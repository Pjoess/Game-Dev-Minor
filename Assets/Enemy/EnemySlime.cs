using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;

public class EnemySlime : MonoBehaviour, IChaseTriggerCheckable
{
    #region General variables
        public int HealthPoints { get; set; }
        public int MaxHealth { get; set; } = 3;
        public float MoveSpeed { get; set; }
        
        Coroutine coroutine { get; set; }
        public Vector3 JumpPosition { get; set; }
    #endregion

    #region Movement variables
        public Rigidbody RB { get; set; }
        public bool IsAggroed { get; set; }

    #endregion


    #region AI variables
        public NavMeshAgent Agent { get; set; }
        public Transform Target { get; set; }
    #endregion



    public float elapsed = 0f;
    public float duration = 5f;

    #region Standard methods for enemy
        public void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Target = GameObject.FindWithTag("Player").transform;
            RB = GetComponent<Rigidbody>();
        }
        void Start()
        {
            
        }
        void Update()
        {
            if(IsAggroed && IsGrounded()){
                WhenJump();
            }
            

        }

        public void Chase()
        {
            if(IsAggroed){
                
            }
        }

        public void WhenJump(){
            if (Agent.enabled)
            {
                Agent.SetDestination(transform.position);

                JumpPosition = transform.position;
                // disable the agent
                Agent.updatePosition = false;
                Agent.updateRotation = false;
                Agent.isStopped = true;
            }
            RB.isKinematic = false;
            RB.useGravity = true;

            // this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(this.transform.position - Target.transform.position), 5 * Time.deltaTime);
            RB.AddRelativeForce(new Vector3(0, 5f, 0), ForceMode.Impulse);
            RB.AddRelativeForce(this.transform.forward * 4, ForceMode.Impulse);
        }
        public bool IsGrounded()
        {
            float groundCheckDistance = (GetComponent<BoxCollider>().size.y /2) + 0.1f;

            RaycastHit hit;

            if(Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance))
            {
                Debug.Log("IsGrounded");
                Agent.updatePosition = true;
                Agent.updateRotation = true;
                Agent.isStopped = false;
                RB.isKinematic = true;
                RB.useGravity = false;
                return true;
            } else {
                return false;
            }
        }

    #endregion

    #region Enemy States functions

    #endregion

    #region Coroutines

        IEnumerator waiter()
        {
            float counter = 0;
            //Wait for 4 seconds
            float waitTime = 2;
            while (counter < waitTime)
            {
                //Increment Timer until counter >= waitTime
                counter += Time.deltaTime;
                Debug.Log("We have waited for: " + counter + " seconds");
                //Check if we want to quit this function
                if (counter >= waitTime)
                {
                    yield break;
                }
                yield return null;
            }
        }
    #endregion

    #region Attacked 

        public void Hit()
        {
            
        }

    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }
    #endregion
}
