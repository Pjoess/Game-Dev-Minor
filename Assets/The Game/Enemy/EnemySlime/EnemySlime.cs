using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySlime : EnemyBase
{
    #region Variables
        private bool attackCollision = false;
        private bool isDashing = false;
        private float chaseRange = 15f;
        public LayerMask playerLayer;
        public Transform centerPoint;
        private float patrolWaitTime = 5f;
        private float patrolRange = 5f;
        private SphereCollider centerCollider;
        private Coroutine idling;
        public float dashSpeedMultiplier = 1.5f;
        private Coroutine attacking;
        private int Timer;
        private Coroutine timer;
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
        }

        public override bool CheckChase()
        {

            if(CheckChaseRange()){
                return true;
            }
            else{
                return false;
            }
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
            HealthPoints = MaxHealthPoints;
            MovementSpeed = 10;
            IsAggroed = false;
            IsWithinStrikingDistance = false;
        }

        public override void InitializeStates()
        {
            enemyStateMachine = new EnemyStateMachine();
            enemyChaseState = new EnemyChaseState(this, enemyStateMachine);
            enemyFallState = new EnemyFallState(this, enemyStateMachine);
            enemyHitState = new EnemyHitState(this, enemyStateMachine);
            enemyIdleState = new EnemyIdleState(this, enemyStateMachine);
            enemyAttackState = new EnemyAttackState(this, enemyStateMachine);
            enemyStateMachine.Initialize(enemyIdleState);
        }
    #endregion

    #region State Logic Functions
        public override void Attack()
        {
            Agent.isStopped = true;
            IsAttacking = true;
            attacking = StartCoroutine(AttackCoroutine());
        }

        public override void Chase()
        {
            SetAgentDestination();
        }

        public override void Hit(int damage)
        {
            HealthPoints -= damage;
            enemyHealthBar.UpdateHealthBar(HealthPoints, MaxHealthPoints);
            Debug.Log(damage);

            if(HealthPoints <= 0)
            {
                GetComponent<HealthDropScript>().InstantiateDroppedItem(transform.position);
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
                Vector3 randomPoint = centerPoint.position + Random.insideUnitSphere * patrolRange;
                NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRange, 1);
                Vector3 finalPoint = hit.position;
                Agent.SetDestination(finalPoint);
                yield return new WaitForSeconds(patrolWaitTime);
            }
        }

        public IEnumerator AttackCoroutine()
        {
            timer = StartCoroutine(AttackDashTimer());
            Vector3 playerPosition = Target.position;
            Vector3 directionToPlayer = playerPosition - transform.position;
            Vector3 dashTargetPosition = playerPosition + directionToPlayer.normalized * 1.1f;
            float distance;
            FacePlayer(playerPosition);
            Agent.isStopped = true;
            yield return new WaitForSeconds(1f);
            Agent.isStopped = false;
            isDashing = true;
            while(isDashing){
                distance = Vector3.Distance(dashTargetPosition, transform.position);
                Agent.Move(Agent.speed * dashSpeedMultiplier * Time.deltaTime * transform.forward);
                if(distance <= 1f || Timer >= 2){
                    Timer = 0;
                    StopCoroutine(timer);
                    Debug.Log("disstance reached");
                    Agent.isStopped = true;
                    yield return new WaitForSeconds(2f);
                    Agent.isStopped = false;
                    isDashing = false;
                    break;
                }
                yield return null;
            }
            Agent.updateRotation = true;
            IsAttacking = false;
        }

        public void FacePlayer(Vector3 playerPosition)
        {
            Vector3 directionToPlayer = (playerPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 400);
        }
    #endregion

    #region Triggers/Collisions
        public IEnumerator AttackCollisionTimer(){
            attackCollision = true;
            yield return new WaitForSeconds(3);
            attackCollision = false;
        }

        public IEnumerator AttackDashTimer(){
            while(Timer < 2){
                yield return new WaitForSeconds(1f);
                Timer++;
            }
        }
        
        private void OnCollisionStay(Collision other) 
        {
            if(!other.transform.CompareTag("Player") && isDashing){
                StopCoroutine(attacking);
                isDashing = false;
                Agent.updateRotation = true;
                IsAttacking = false;
                attackCollision = false;
                enemyStateMachine.ChangeState(enemyChaseState);
            }

            if(other.transform.CompareTag("Player") && IsAttacking && isDashing && !attackCollision && Agent.isStopped == false){
                Debug.Log("HIt player");
                IDamageble damagable = other.collider.GetComponent<IDamageble>();
                damagable.Hit(10);
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
        if(Vector3.Distance(transform.position, Target.position) <= chaseRange){
            return true;
        }
        else{
            return false;
        }
    }
}
