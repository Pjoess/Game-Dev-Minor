using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySlime : EnemyBase
{
    #region Variables
        public bool isChasingPlayer = false;
        public bool attackCollision = false;
        public float chaseRange = 10f;
        [SerializeField]
        public LayerMask playerLayer;
        public Transform centerPoint;
        public float patrolWaitTime = 5f;
        public float patrolRange = 5f;
        private SphereCollider centerCollider;
        public Coroutine idling;
    #endregion

    #region CheckStates
        public override bool CheckAttack()
        {

            if (CheckAttackRange())
            {
                return true;
            }
            else
            {
                return false;
            }


            // if(IsWithinStrikingDistance)
            // {
            //     return true;
            // }
            // else
            // {
            //     return false;
            // }
        }

        public override bool CheckChase()
        {

            if(CheckChaseRange()){
                return true;
            }
            else{
                return false;
            }
            // if(IsAggroed && !IsWithinStrikingDistance)
            // {
            //     return true;
            // }
            // else 
            // {
            //     return false;
            // }
        }

        public override bool CheckFall()
        {
            throw new System.NotImplementedException();
        }

        public override bool CheckHit()
        {
            throw new System.NotImplementedException();
        }

        public override bool CheckIdle()
        {
            if(!CheckChaseRange() && !CheckAttackRange()){
                return true;
            }
            else{
                return false;
            }
            // if (!IsAggroed && !IsWithinStrikingDistance)
            // {
            //     return true;
            // }
            // else
            // {
            //     return false;
            // }
        }
    #endregion

    #region Unity Start Functions
        public void Start()
        {
            InitializeStates();
        }
        public override void Awake()
        {
            centerCollider = centerPoint.GetComponent<SphereCollider>();
            Agent = GetComponent<NavMeshAgent>();
            enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
            Target = GameObject.FindWithTag("Player").transform;
            MaxHealth = 100;
            HealthPoints = MaxHealth;
            MovementSpeed = 10;

            IsAggroed = false;
            IsWithinStrikingDistance = false;
        }
        public override void InitializeStates()
        {
            //STATES
            enemyStateMachine = new EnemyStateMachine();

            enemyChaseState = new EnemyChaseState(this, enemyStateMachine);
            enemyFallState = new EnemyFallState(this, enemyStateMachine);
            enemyHitState = new EnemyHitState(this, enemyStateMachine);
            enemyIdleState = new EnemyIdleState(this, enemyStateMachine);
            enemyAttackState = new EnemyAttackState(this, enemyStateMachine);
            //END STATES

            enemyStateMachine.Initialize(enemyIdleState);
        }
    #endregion

    #region State Logic Functions
        public override void Attack()
        {
            Agent.isStopped = true;
            IsAttacking = true;

            StartCoroutine(AttackCoroutine());
        }

        public override void Chase()
        {
            SetAgentDestination();

            //animations

            //effects

        }

        public override void Hit(float damage)
        {
            HealthPoints -= damage;

            enemyHealthBar.UpdateHealthBar(HealthPoints,MaxHealth);

            if(HealthPoints <= 0)
            {
                Destroy(this.gameObject);
            }


        }

        public override void Idle()
        {
            idling = StartCoroutine(PatrolRoutine());
        }
        public override void ExitIdle(){
            StopCoroutine(idling);
        }
    #endregion

    #region Coroutines
        IEnumerator PatrolRoutine()
        {
            while (true)
            {
                // Check if AI is outside the patrolling area
                if (!centerCollider.bounds.Contains(transform.position))
                {
                    // AI is outside the patrolling area, return to center
                    Agent.SetDestination(centerPoint.position);
                    yield return new WaitForSeconds(patrolWaitTime); // Wait for AI to reach the center
                    continue; // Skip the rest of the loop iteration
                }

                // Randomly select a point within patrol range around center point
                Vector3 randomPoint = centerPoint.position + Random.insideUnitSphere * patrolRange;
                NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRange, 1);
                Vector3 finalPoint = hit.position;

                // Set destination to the selected point
                Agent.SetDestination(finalPoint);

                // Wait for patrolWaitTime seconds
                yield return new WaitForSeconds(patrolWaitTime);
            }
        }
        public IEnumerator AttackCoroutine()
        {

            Vector3 directionToPlayer = Target.position - transform.position;

            Agent.isStopped = true;
            yield return new WaitForSeconds(1f);
            Agent.isStopped = false;

            bool x = true;

            // StartCoroutine(FacePlayer());
            // yield return new WaitForSeconds(1f);
            Vector3 playerPosition = Target.position;
            float distance;
            // Agent.updateRotation = false;
            // Agent.speed = 7f;
            // Agent.SetDestination(directionToPlayer);
            // Agent.Move(directionToPlayer);
            FacePlayer();
            while(x){
                distance = Vector3.Distance(playerPosition, transform.position);
                // Debug.Log(distance);
                Agent.Move(transform.forward * Agent.speed * Time.deltaTime);
                // Agent.Move(directionToPlayer);
                // Agent.SetDestination(playerPosition);

                if(distance <= 1f){
                    Debug.Log("disstance reached");
                    x = false;
                    Agent.isStopped = true;
                    yield return new WaitForSeconds(2f);
                    Agent.isStopped = false;
                    
                    // Agent.velocity = Vector3.zero;
                    // break;
                }
                yield return null;
            }
            // Agent.speed = 3.5f;
            // Agent.isStopped = true;
            // yield return new WaitForSeconds(2f);
            // Agent.isStopped = false;
            Agent.updateRotation = true;
            IsAttacking = false;
            // yield return FacePlayer();


            
            // StartCoroutine(FacePlayer());
            // Agent.Move(directionToPlayer * Time.deltaTime);
            // Agent.Move(directionToPlayer * 0.1f);
            
            // while(!attackCollision){
            //     Debug.Log("Moving dash");
            //     Agent.velocity =new Vector3()* 6f * Time.fixedDeltaTime;
            // }

            // Vector3.MoveTowards(transform.position, Target.position, Time.deltaTime * 10f);

            

        }

        public void FacePlayer()
        {
            Vector3 directionToPlayer = (Target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 200);
            // yield return null;
        }

    #endregion

    #region Triggers/Collisions
        public void OnTriggerEnter(Collider other)
        {
            if (IsWeaponCollisionValid(other))
            {
                float damage = other.GetComponentInParent<Player>().attackDamage;

                Hit(damage);
            }
        }

        public bool IsWeaponCollisionValid(Collider other)
        {
            return other.gameObject.CompareTag("Weapon")&& !isCollisionCooldown;
        }

        public void StartCollisionCooldown()
        {
            isCollisionCooldown = true;
            Invoke(nameof(EndCollisionCooldown), collisionCooldown);
        }

        public void EndCollisionCooldown() => isCollisionCooldown = false;

        public IEnumerator AttackCollisionTimer(){
            attackCollision = true;
            yield return new WaitForSeconds(3);
            attackCollision = false;
        }

        private void OnCollisionEnter(Collision other) {

            if(other.transform.tag == "Player" && IsAttacking && !attackCollision){
                IDamageble damagable = other.collider.GetComponent<IDamageble>();
                damagable.Hit(5);
                StartCoroutine(AttackCollisionTimer());
            }
        }

    #endregion


    private bool CheckAttackRange()
    {
        if(Vector3.Distance(transform.position, Target.position) <= Agent.stoppingDistance){
            return true;
        }
        else{
            return false;
        }
    }



    private bool CheckChaseRange(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, chaseRange, playerLayer);

        if (colliders.Length > 0)
        {
            // Debug.Log("THERE IS");

            foreach (var collider in colliders){
                // Player detected, chase and attack
                // if(collider == Target);
                if(collider.gameObject.tag == "Player"){
                    if (!isChasingPlayer) // If not already chasing
                    {
                        isChasingPlayer = true;
                    }
                    // Transform player = colliders[0].transform;
                    // Agent.SetDestination(player.position);
                }
                // if (!isChasingPlayer) // If not already chasing
                // {
                //     isChasingPlayer = true;
                // }
            }

        }
        else
        {
            if (isChasingPlayer) // If stopped chasing
            {
                isChasingPlayer = false;
            }
        }

        if(isChasingPlayer){
            return true;
        }
        else{
            return false;
        }
    }

    // private void CheckChaseRange()
    // {
    //     // Check if player is within chase range
    //     Collider[] colliders = Physics.OverlapSphere(transform.position, chaseRange, playerLayer);


    //     // if (colliders.Length > 0)
    //     //     {
    //     //         if (colliders[0].gameObject.tag == "Player")
    //     //         {
    //     //             print("COLLIDED WITH ENEMY");
    //     //         }
    //     //     }
    //     if (colliders.Length > 0)
    //     {
    //         Debug.Log("THERE IS");

    //         foreach (var collider in colliders){
    //             // Player detected, chase and attack
    //             if(collider.gameObject.tag == "Player"){
    //                 Transform player = colliders[0].transform;
    //                 Agent.SetDestination(player.position);
    //             }
    //             if (!isChasingPlayer) // If not already chasing
    //             {
    //                 isChasingPlayer = true;
    //             }

    //             // Check if AI is close enough to attack
    //             // if (Vector3.Distance(transform.position, Target.position) <= Agent.stoppingDistance)
    //             // {
    //             //     // Implement attack logic here
    //             //     Attack();
    //             // }
    //         }

    //     }
    //     else
    //     {
    //         if (isChasingPlayer) // If stopped chasing
    //         {
    //             isChasingPlayer = false;
    //         }
    //     }
    // }


}
