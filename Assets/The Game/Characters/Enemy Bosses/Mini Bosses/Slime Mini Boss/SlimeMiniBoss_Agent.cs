using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

namespace SlimeMiniBoss
{
    public class SlimeMiniBoss_Agent : MonoBehaviour
    {
        private IBaseNode slimeBT = null;
        public LayerMask attackLayer;

        [Header("NavMesh Agent")]
        [HideInInspector] private NavMeshAgent miniBossAgent;

        [Header("Object References")]
        private Player_Manager player;
        [SerializeField] private GameObject patrolCenterPoint;

        [Header("Movement")]
        [SerializeField] private float movementSpeed = 2f;

        [Header("Patrol")]
        [SerializeField] private float patrolWaitTime = 4f;
        [SerializeField] private float patrolRange = 10f;
        [SerializeField] private bool isPatrolling;

        [Header("Chase")]
        [SerializeField] private float chaseRange = 15f;
        [SerializeField] private bool isChasingPlayer = false;

        [Header("Attack")]
        [SerializeField] private float attackRange = 3f;
        [SerializeField] private float offsetDistance = 3f;
        [SerializeField] private bool isAttacking;
        
        // --- IDamagable --- //
        [Header("Stats")]
        [SerializeField] private int healthPoints;
        [SerializeField] private int maxHealthPoints = 40;
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        [HideInInspector] public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
        
        [Header("Slime Damage")]
        [SerializeField] private int miniBossDamage = 25;
        
        public EnemyHealthBar enemyHealthBar;

        private void Awake()
        {
            miniBossAgent = GetComponent<NavMeshAgent>();
            player = FindObjectOfType<Player_Manager>();
            enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        }

        void Start()
        {
            HealthPoints = MaxHealthPoints;
            miniBossAgent.speed = 2f;
            MiniBossSlimeBehaviourTree();
        }

        void Update()
        {
            slimeBT?.Update(); // Update the boss behavior tree
        }

        private void MiniBossSlimeBehaviourTree()
        {
            List<IBaseNode> bossNodes = new()
            {
                new ChasePlayerNode(miniBossAgent, player.transform, chaseRange),
                new AttackPlayerNode(miniBossAgent, player.transform, attackRange, offsetDistance, miniBossDamage, attackLayer),
            };

            slimeBT = new SelectorNode(bossNodes);
        }

    #region IDamagable
        // Mini Boss Receive damage
        public void ApplyDamageToMiniBoss(int damage) => HealthPoints -= damage; // Do damage to boss (with bullets)

        public void Hit(int damage)
        {
            ApplyDamageToMiniBoss(damage);
            enemyHealthBar.UpdateHealthBar(HealthPoints, MaxHealthPoints);
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (HealthPoints <= 0)
            {
                GetComponent<MemoryDropScipt>().DropItem(transform.position);
                Destroy(gameObject);
            }
        }
    #endregion
    }
}
