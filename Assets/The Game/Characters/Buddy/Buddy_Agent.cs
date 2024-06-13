using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections.Generic;

namespace buddy
{
    public class Buddy_Agent : MonoBehaviour
    {
        private IBaseNode buddyBT = null;
        private Rigidbody rigidBody; // this is important otherwise the Bullets don't work
        public NavMeshAgent agent;
        public LayerMask attackLayer;
        public AudioSource shootSound;
        public GameObject bulletPrefab;
        public GameObject mortarPrefab;

        [Header("Attack")]
        public GameObject targetedEnemy;
        public int shotsFired = 0;
        public float bulletSpeed = 8f;
        public float bulletLifetime = 5f;
        public float bulletShootHeight = 1f;
        public float mortarSpeed = 5f;
        public float mortarSpawnHeight = 8f;
        public bool canShootMortar = false;
        public bool shootMortar = false;

        [Header("Ranges")]
        public float shootingRange = 20f;
        public float maxAgentToPlayerDistance = 5f;

        [Header("Cooldown")]
        public TMP_Text buddyCooldownText;
        [SerializeField] private float mortarCooldownTime = 3f;

        public Animator animator;
        public int animIDWalk;
        public int animIDShooting;
        public int animIDShootingMortar;

        public void AssignAnimIDs()
        {
            animIDWalk = Animator.StringToHash("isWalking");
            animIDShooting = Animator.StringToHash("isShooting");
            animIDShootingMortar = Animator.StringToHash("isShootingMortar");
        }

        private void Awake()
        {
            AssignAnimIDs();
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            rigidBody = GetComponent<Rigidbody>();
        }

        void Start()
        {
            BuddyBehaviourTree();
        }

        void Update()
        {
            buddyBT?.Update();
        }

        private void BuddyBehaviourTree()
        {
            List<IBaseNode> buddyMovement = new()
            {
                new FollowNode(agent, maxAgentToPlayerDistance, animator, animIDWalk),
                new IdleNode(agent),
            };

            List<IBaseNode> enemyInLineOfSight = new()
            {
                new EnemyTargetingNode(this),
                //new ShootBulletNode(this),
                new ShootMortarNode(this)
            };

            List<IBaseNode> selectNode = new()
            {
                new SequenceNode(buddyMovement),
                new SequenceNode(enemyInLineOfSight),
            };

            buddyBT = new SelectorNode(selectNode);
        }

        public void PlayerHitTarget(GameObject enemy)
        {
            if (targetedEnemy != null)
            {
                if (targetedEnemy != enemy)
                {
                    targetedEnemy.GetComponentInParent<IEnemyMaterialChanger>().UnTargetSlime();
                    targetedEnemy = enemy;
                    targetedEnemy.GetComponentInParent<IEnemyMaterialChanger>().TargetSlime();
                }
            }
            else
            {
                targetedEnemy = enemy;
                targetedEnemy.GetComponentInParent<IEnemyMaterialChanger>().TargetSlime();
            }
        }

        void OnDrawGizmosSelected()
        {
            // Draw a wire sphere to represent the shooting range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shootingRange);

            // Draw a wire sphere to represent the distance to move
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, maxAgentToPlayerDistance);
        }
    }
}
