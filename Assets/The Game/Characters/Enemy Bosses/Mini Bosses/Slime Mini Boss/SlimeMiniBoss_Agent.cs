using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    public class SlimeMiniBoss_Agent : MonoBehaviour, IDamageble
    {
        private IBaseNode slimeBT = null;
        public LayerMask attackLayer;

        [Header("NavMesh Agent")]
        private NavMeshAgent miniBossAgent;

        [Header("Object References")]
        private Player_Manager player;
        private GameObject patrolCenterPoint;

        [Header("Chase")]
        private float chaseRange = 15f;
        private bool isChasingPlayer = false;

        [Header("Attack")]
        private float attackRange = 3f;
        private float offsetDistance = 3f;
        private bool isAttacking;
        
        // --- IDamagable --- //
        [Header("Stats")]
        public int healthPoints;
        public int maxHealthPoints = 100;
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
        
        [Header("Slime Damage")]
        private int miniBossDamage = 25;
        
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

    // Mini Boss Receive damage
    public void ApplyDamageToMiniBoss() => healthPoints -= 3; // Do damage to boss (with bullets)

    #region IDamagable
        public void Hit(int damage)
        {
            HealthPoints -= damage;
            Debug.Log("" + healthPoints);

            ApplyDamageToMiniBoss();
            enemyHealthBar.UpdateHealthBar(HealthPoints,MaxHealthPoints);
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
