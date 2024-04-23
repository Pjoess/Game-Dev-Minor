using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySlime : EnemyBase
{
    #region Variables
        // private bool isChasingPlayer = false;
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
            HealthPoints = MaxHealthPoints;
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

            attacking = StartCoroutine(AttackCoroutine());
        }

        public override void Chase()
        {
            SetAgentDestination();

            //animations

            //effects

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
        

        // public void BuddyDamage(){
        //     HealthPoints--;
        // }

        // public override void Hit(int damage)
        // {
        //     if(damage > 0){
        //         HealthPoints -= damage;
        //     }else{
        //         BuddyDamage();
        //     }

        //     enemyHealthBar.UpdateHealthBar(HealthPoints,MaxHealthPoints);

        //     if(HealthPoints <= 0)
        //     {
        //         GetComponent<HealthDropScript>().InstantiateDroppedItem(transform.position);
        //         Destroy(this.gameObject);
        //     }
        // }






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
            timer = StartCoroutine(AttackDashTimer());
            Vector3 playerPosition = Target.position;
            Vector3 directionToPlayer = playerPosition - transform.position;
            Vector3 dashTargetPosition = playerPosition + directionToPlayer.normalized * 1.1f;
            float distance;
            FacePlayer(playerPosition);
            // Vector3 directionToPlayer = Target.position - transform.position;

            Agent.isStopped = true;
            yield return new WaitForSeconds(1f);
            Agent.isStopped = false;


            // StartCoroutine(FacePlayer());
            // yield return new WaitForSeconds(1f);

            // Agent.updateRotation = false;
            // Agent.speed = 7f;
            // Agent.SetDestination(directionToPlayer);
            // Agent.Move(directionToPlayer);
            // NavMeshHit hit;
            // bool y = Agent.Raycast(transform.position, playerPosition, out hit, playerLayer);

            isDashing = true;
            while(isDashing){
                distance = Vector3.Distance(dashTargetPosition, transform.position);
                // Debug.Log(distance);
                Agent.Move(transform.forward * Agent.speed * dashSpeedMultiplier * Time.deltaTime);
                // Agent.Move(directionToPlayer);
                // Agent.SetDestination(playerPosition);

                if(distance <= 1f || Timer >= 2){
                    Timer = 0;
                    StopCoroutine(timer);
                    Debug.Log("disstance reached");
                    Agent.isStopped = true;
                    yield return new WaitForSeconds(2f);
                    Agent.isStopped = false;
                    isDashing = false;
                    // Agent.velocity = Vector3.zero;
                    break;
                }
                yield return null;
            }
            // yield return new WaitForSeconds(0.5f);
            // isDashing = false;
            // Agent.speed = 3.5f;
            // Agent.isStopped = true;
            // yield return new WaitForSeconds(2f);
            // Agent.isStopped = false;
            Agent.updateRotation = true;
            IsAttacking = false;
            // attackCollision = false;
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

        public void FacePlayer(Vector3 playerPosition)
        {
            Vector3 directionToPlayer = (playerPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 400);
            // yield return null;
        }

    #endregion

    #region Triggers/Collisions
        // public void OnTriggerEnter(Collider other)
        // {
        //     if(other.transform.tag == "Player" && IsAttacking && isDashing){
        //         Debug.Log("HIt player");
        //         IDamageble damagable = other.GetComponent<IDamageble>();
        //         damagable.Hit(10);
        //         // StartCoroutine(AttackCollisionTimer());
        //     }
        // }

        // public bool IsWeaponCollisionValid(Collider other)
        // {
        //     return other.gameObject.CompareTag("Weapon")&& !isCollisionCooldown;
        // }

        // public void StartCollisionCooldown()
        // {
        //     isCollisionCooldown = true;
        //     Invoke(nameof(EndCollisionCooldown), collisionCooldown);
        // }

        // public void EndCollisionCooldown() => isCollisionCooldown = false;

        public IEnumerator AttackCollisionTimer(){
            attackCollision = true;
            yield return new WaitForSeconds(3);
            // StopCoroutine(attacking);
            attackCollision = false;
        }

        public IEnumerator AttackDashTimer(){
            while(Timer < 2){
                yield return new WaitForSeconds(1f);
                Timer++;
            }
        }

        private void OnCollisionStay(Collision other) {
            
            if(other.transform.tag != "Player" && isDashing){
                StopCoroutine(attacking);
                isDashing = false;
                Agent.updateRotation = true;
                IsAttacking = false;
                attackCollision = false;
                enemyStateMachine.ChangeState(enemyChaseState);
            }
            if(other.transform.tag == "Player" && IsAttacking && isDashing && !attackCollision && Agent.isStopped == false){
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
        // Collider[] colliders = Physics.OverlapSphere(transform.position, chaseRange, playerLayer);

        // if (colliders.Length > 0)
        // {
        //     // Debug.Log("THERE IS");

        //     foreach (var collider in colliders){
        //         // Player detected, chase and attack
        //         // if(collider == Target);
        //         if(collider.gameObject.tag == "Player"){
        //             if (!isChasingPlayer) // If not already chasing
        //             {
        //                 isChasingPlayer = true;
        //             }
        //             // Transform player = colliders[0].transform;
        //             // Agent.SetDestination(player.position);
        //         }
        //         // if (!isChasingPlayer) // If not already chasing
        //         // {
        //         //     isChasingPlayer = true;
        //         // }
        //     }

        // }
        // else
        // {
        //     if (isChasingPlayer) // If stopped chasing
        //     {
        //         isChasingPlayer = false;
        //     }
        // }

        // if(isChasingPlayer){
        //     return true;
        // }
        // else{
        //     return false;
        // }
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
