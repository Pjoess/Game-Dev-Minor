using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

namespace BasicEnemySlime
{
    public class BasicEnemySlime : MonoBehaviour, IDamageble
    {
        private IBaseNode basicSlimeBT = null;
        public LayerMask attackLayer; // Player
        public NavMeshAgent agent;
        private Rigidbody rigidBody; // Important for the bullets damage received
        public static float originalSpeed;

        private DecalProjector projector;
        [SerializeField] private Material neutralFace, hitFace;

        [Header("Patrol Center Point")]
        public GameObject patrolCenterPoint;

        [Header("Patrol Settings")]
        private float patrolRadius = 20f;
        public float stopDistance = 4f;

        [Header("Chase")]
        public float chaseRange = 15f;

        [Header("Attack")]
        public float attackRange = 6f;
        public bool hasAttacked = false;
        
        // --- IDamagable --- //
        [Header("Stats")]
        private EnemyHealthBar enemyHealthBar;
        public int healthPoints;
        public int maxHealthPoints = 15;
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
    
        [Header("Cone Settings")]
        public float ConeOffset = 1f;
        public float coneWidth = 30f;
        public float coneLength = 6f;
        public float thickness = 1f;

        public Animator animator;
        public int animIDAnticipate;
        public int animIDAttack;
        public int animIDWalking;

        private Renderer slimeRenderer;
        private Color originalColor;

        private void AssignAnimIDs()
        {
            animIDAnticipate = Animator.StringToHash("isAnticipating");
            animIDAttack = Animator.StringToHash("isAttacking");
            animIDWalking = Animator.StringToHash("isWalking");
        }

        private void SetRandomColor()
        {
            List<Color> colors = new()
            {
                Color.blue,
                Color.green,
                Color.yellow,
            };

            Color randomColor = colors[Random.Range(0, colors.Count)];
            Transform slimeTransform = transform.Find("slime");
            if (slimeTransform != null)
            {
                if (slimeTransform.TryGetComponent<Renderer>(out slimeRenderer))
                {
                    slimeRenderer.material.color = randomColor;
                    originalColor = randomColor;
                }
            }
        }

        private void Awake()
        {
            AssignAnimIDs();
            HealthPoints = MaxHealthPoints;
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
            rigidBody = GetComponent<Rigidbody>();
            projector = GetComponentInChildren<DecalProjector>();
        }

        void Start()
        {
            originalSpeed = agent.speed;
            BehaviourTree();
            SetRandomColor();
        }

        void Update()
        {
            basicSlimeBT?.Update(); // Update the boss behavior tree
        }

        #region Behaviour Tree
        private void BehaviourTree()
        {
            //ChaseAttackPatrol();
            AttackAndReturn();
        }

        private void AttackAndReturn()
        {
            List<IBaseNode> IsPlayerInLineOfSight = new()
            {
                new AttackPlayerNode(this),
                new ChasePlayerNode(this)
            };

            List<IBaseNode> Root = new()
            {
                new SelectorNode(IsPlayerInLineOfSight),
                new ReturnBackToPosition(agent,agent.transform.position,stopDistance,chaseRange,attackRange,animator,animIDWalking),
            };

            basicSlimeBT = new SelectorNode(Root);
        }

        // Removed for now, changed plan to slime goes back to its spawned point.
        //private void ChaseAttackPatrol()
        //{
        //    List<IBaseNode> IsPlayerInLineOfSight = new()
        //    {
        //        new ChasePlayerNode(this),
        //        new AttackPlayerNode(agent, attackRange, coneWidth, coneLength, animator, animIDAnticipate, animIDAttack),
        //    };

        //    List<IBaseNode> IsPlayerNotInLineOfSight = new()
        //    {
        //        new PatrolNode(agent, patrolCenterPoint, patrolRadius, stopDistance, chaseRange, attackRange,animator,animIDWalking),
        //    };

        //    List<IBaseNode> Root = new()
        //    {
        //        new SequenceNode(IsPlayerInLineOfSight),
        //        new SequenceNode(IsPlayerNotInLineOfSight),
        //    };

        //    basicSlimeBT = new SelectorNode(Root);
        //}
        #endregion

        #region Cone Raycast
        private void OnDrawGizmos()
        {
            // Draw cone shape in Gizmos
            DrawCone(transform.position, transform.forward, coneWidth, coneLength, thickness);

            // Draw chase range sphere
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRange);

            // Draw chase range sphere
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, stopDistance);

            if(patrolCenterPoint != null){
                // Draw patrol radius
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(patrolCenterPoint.transform.position, patrolRadius);
            }

            // Draw stop distance
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, stopDistance);
        }

        // Draw cone shape in Gizmos
        private void DrawCone(Vector3 origin, Vector3 direction, float coneWidth, float coneLength, float thickness)
        {
            // Store the current Gizmos color
            Color previousColor = Gizmos.color;

            // Set the desired color
            Gizmos.color = Color.red;

            // Calculate half width
            float halfWidth = coneWidth / 2f;

            // Calculate offset for thickness
            Vector3 offset = Vector3.up * thickness;

            // Calculate the position of the cone from the middle point
            Vector3 coneStart = origin + (-direction * ConeOffset);

            // Draw cone base with thickness
            Gizmos.DrawLine(coneStart - offset, coneStart + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength - offset);
            Gizmos.DrawLine(coneStart - offset, coneStart + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength - offset);
            Gizmos.DrawLine(coneStart + offset, coneStart + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength + offset);
            Gizmos.DrawLine(coneStart + offset, coneStart + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength + offset);

            // Draw cone sides with thickness
            Gizmos.DrawLine(coneStart - offset, coneStart + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength / Mathf.Cos(Mathf.Deg2Rad * halfWidth) - offset);
            Gizmos.DrawLine(coneStart - offset, coneStart + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength / Mathf.Cos(Mathf.Deg2Rad * halfWidth) - offset);
            Gizmos.DrawLine(coneStart + offset, coneStart + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength / Mathf.Cos(Mathf.Deg2Rad * halfWidth) + offset);
            Gizmos.DrawLine(coneStart + offset, coneStart + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength / Mathf.Cos(Mathf.Deg2Rad * halfWidth) + offset);

            // Draw lines to connect cone sides to cone base with thickness
            Gizmos.DrawLine(coneStart - offset, coneStart - offset);
            Gizmos.DrawLine(coneStart + offset, coneStart + offset);
            Gizmos.DrawLine(coneStart + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength - offset, coneStart + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength + offset);
            Gizmos.DrawLine(coneStart + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength - offset, coneStart + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength + offset);

            // Restore the previous Gizmos color
            Gizmos.color = previousColor;
        }
        #endregion
        
        #region IDamagable
        public void Hit(int damage)
        {
            HealthPoints -= damage;
            enemyHealthBar.UpdateHealthBar(HealthPoints, MaxHealthPoints);
            CheckDeath();
            StartCoroutine(ChangeColorOnHit());
        }

        private IEnumerator ChangeColorOnHit()
        {
            if (slimeRenderer != null)
            {
                Color lightRed = new(255 / 255f, 51 / 255f, 51 / 255f, 1f);
                slimeRenderer.material.color = lightRed;
                SetHitFace();
                yield return new WaitForSeconds(0.2f);
                SetNeutralFace();
                slimeRenderer.material.color = originalColor;
            }
        }

        private void CheckDeath()
        {
            if (HealthPoints <= 0)
            {
                GetComponent<HealthDropScript>().InstantiateDroppedItem(transform.position);
                Destroy(gameObject);
            }
        }
        #endregion

        private void OnCollisionEnter(Collision collision)
        {
            if(hasAttacked)
            {
                if(collision.gameObject.CompareTag("Player"))
                {
                    Blackboard.instance.HitPlayer(10, collision.contacts[0].point);
                    animator.SetBool(animIDAnticipate, false);
                    animator.SetBool(animIDAttack, false);
                    hasAttacked = false;
                    agent.updateRotation = true;
                    agent.isStopped = false;
                }
            }
        }

        #region Animator
        public void EndWalk()
            {
                animator.SetBool(animIDWalking, false);
            }

            public void EndAnticipate()
            {
                animator.SetBool(animIDAnticipate, false);
                animator.SetBool(animIDAttack, true);
            }

            public void DoAttack()
            {
                hasAttacked = true;
            }

            public void EndAttack()
            {   
                animator.SetBool(animIDAnticipate, false);
                hasAttacked = false;
            }

            public void EndAttackAnim()
            {
                animator.SetBool(animIDAttack, false);
                agent.updateRotation = true;
                agent.isStopped = false;
            }
        #endregion


        private void SetNeutralFace()
        {
            projector.material = neutralFace;
        }

        private void SetHitFace()
        {
            projector.material = hitFace;
        }
    }
}